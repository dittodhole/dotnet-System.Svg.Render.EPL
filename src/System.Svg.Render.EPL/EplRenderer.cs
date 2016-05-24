using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

// ReSharper disable NonLocalizedString

namespace System.Svg.Render.EPL
{
  [PublicAPI]
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
    [Pure]
    [MustUseReturnValue]
    public Encoding GetEncoding()
    {
      var codepage = (int) this.PrinterCodepage;
      // ReSharper disable ExceptionNotDocumentedOptional
      var encoding = Encoding.GetEncoding(codepage);
      // ReSharper restore ExceptionNotDocumentedOptional

      return encoding;
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual IEnumerable<EplStream> GetInternalMemoryTranslation([NotNull] SvgDocument svgDocument)
    {
      var parentMatrix = this.CreateParentMatrix();
      var translations = this.TranslaveSvgElementAndChildrenForStoring(svgDocument,
                                                                       parentMatrix,
                                                                       this.ViewMatrix);

      return translations;
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
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
        // ReSharper disable ExceptionNotDocumentedOptional
        if (eplStream.Any())
          // ReSharper restore ExceptionNotDocumentedOptional
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
          // ReSharper disable ExceptionNotDocumentedOptional
          if (eplStream.Any())
            // ReSharper restore ExceptionNotDocumentedOptional
          {
            yield return eplStream;
          }
        }
      }
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected virtual Matrix CreateParentMatrix() => new Matrix();

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected virtual EplStream TranslateSvgElementForStoring([NotNull] SvgElement svgElement,
                                                              [NotNull] Matrix matrix,
                                                              [NotNull] Matrix viewMatrix)
    {
      var container = this.EplCommands.CreateEplStream();

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
    [Pure]
    [MustUseReturnValue]
    public override EplStream GetTranslation([NotNull] SvgDocument svgDocument)
    {
      var parentMatrix = this.CreateParentMatrix();
      var eplStream = this.EplCommands.CreateEplStream();

      this.AddHeaderToTranslation(eplStream);
      this.AddBodyToTranslation(svgDocument,
                                parentMatrix,
                                eplStream);
      this.AddFooterToTranslation(eplStream);

      return eplStream;
    }

    protected virtual void AddHeaderToTranslation([NotNull] EplStream eplStream)
    {
      eplStream.Add(this.EplCommands.SetReferencePoint(0,
                                                       0));
      eplStream.Add(this.EplCommands.PrintDirection(PrintOrientation.Top));
      eplStream.Add(this.EplCommands.CharacterSetSelection(8,
                                                           this.PrinterCodepage,
                                                           this.CountryCode));
    }

    protected virtual void AddBodyToTranslation([NotNull] SvgDocument svgDocument,
                                                [NotNull] Matrix parentMatrix,
                                                [NotNull] EplStream eplStream)
    {
      this.TranslateSvgElementAndChildren(svgDocument,
                                          parentMatrix,
                                          this.ViewMatrix,
                                          eplStream);
    }

    protected virtual void AddFooterToTranslation([NotNull] EplStream eplStream)
    {
      eplStream.Add(this.EplCommands.Print(1));
      eplStream.Add(string.Empty);
    }
  }
}