using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable NonLocalizedString

namespace System.Svg.Render.EPL
{
  public class EplRenderer : RendererBase<EplStream>
  {
    public EplRenderer([NotNull] Matrix viewMatrix,
                       [NotNull] EplCommands eplCommands,
                       PrinterCodepage printerCodepage,
                       int countryCode)
    {
      this.ViewMatrix = viewMatrix;
      this.EplCommands = eplCommands;
      this.PrinterCodepage = printerCodepage;
      this.CountryCode = countryCode;
    }

    [NotNull]
    protected Matrix ViewMatrix { get; }

    [NotNull]
    protected EplCommands EplCommands { get; }

    protected PrinterCodepage PrinterCodepage { get; }

    protected int CountryCode { get; }

    [NotNull]
    public Encoding GetEncoding()
    {
      var codepage = (int) this.PrinterCodepage;
      var encoding = Encoding.GetEncoding(codepage);

      return encoding;
    }

    [NotNull]
    public virtual IEnumerable<EplStream> GetInternalMemoryTranslation([NotNull] SvgDocument svgDocument)
    {
      var parentMatrix = this.CreateParentMatrix();
      var translations = this.TranslaveSvgElementAndChildrenForStoring(svgDocument,
                                                                       parentMatrix,
                                                                       this.ViewMatrix);

      return translations;
    }

    [NotNull]
    protected virtual IEnumerable<EplStream> TranslaveSvgElementAndChildrenForStoring([NotNull] SvgElement svgElement,
                                                                                      [NotNull] Matrix parentMatrix,
                                                                                      [NotNull] Matrix viewMatrix)
    {
      var matrix = this.MultiplyTransformationsIntoNewMatrix(svgElement,
                                                             parentMatrix);

      {
        var eplStream = this.TranslateSvgElementForStoring(svgElement,
                                                           matrix,
                                                           viewMatrix);
        if (eplStream.Any())
        {
          yield return eplStream;
        }
      }

      foreach (var child in svgElement.Children)
      {
        var translations = this.TranslaveSvgElementAndChildrenForStoring(child,
                                                                         matrix,
                                                                         viewMatrix);

        foreach (var eplStream in translations)
        {
          if (eplStream == null)
          {
            continue;
          }
          if (eplStream.Any())
          {
            yield return eplStream;
          }
        }
      }
    }

    [NotNull]
    protected virtual EplStream CreateEplStream() => new EplStream();

    [NotNull]
    protected virtual Matrix CreateParentMatrix() => new Matrix();

    [NotNull]
    protected virtual EplStream TranslateSvgElementForStoring([NotNull] SvgElement svgElement,
                                                              [NotNull] Matrix matrix,
                                                              [NotNull] Matrix viewMatrix)
    {
      var container = this.CreateEplStream();

      var type = svgElement.GetType();

      var svgElementToInternalMemoryTranslator = this.GetTranslator(type) as ISvgElementToInternalMemoryTranslator;
      if (svgElementToInternalMemoryTranslator == null)
      {
        return container;
      }

      matrix = matrix.Clone();
      matrix.Multiply(viewMatrix,
                      MatrixOrder.Append);

      svgElementToInternalMemoryTranslator.TranslateForStoring(svgElement,
                                                               matrix,
                                                               container);

      return container;
    }

    [NotNull]
    public override EplStream GetTranslation([NotNull] SvgDocument svgDocument)
    {
      var parentMatrix = this.CreateParentMatrix();
      var eplStream = this.CreateEplStream();

      eplStream.Add(this.EplCommands.SetReferencePoint(0,
                                                       0));
      eplStream.Add(this.EplCommands.PrintDirection(PrintOrientation.Top));
      eplStream.Add(this.EplCommands.CharacterSetSelection(8,
                                                           this.PrinterCodepage,
                                                           this.CountryCode));

      this.TranslateSvgElementAndChildren(svgDocument,
                                          parentMatrix,
                                          this.ViewMatrix,
                                          eplStream);

      eplStream.Add(this.EplCommands.Print(1));

      return eplStream;
    }
  }
}