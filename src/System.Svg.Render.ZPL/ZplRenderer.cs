using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Text;
using JetBrains.Annotations;

namespace System.Svg.Render.ZPL
{
  [PublicAPI]
  public class ZplRenderer : RendererBase<ZplStream>
  {
    public ZplRenderer([NotNull] Matrix viewMatrix,
                       [NotNull] ZplCommands zplCommands,
                       CharacterSet characterSet)
    {
      this.ViewMatrix = viewMatrix;
      this.ZplCommands = zplCommands;
      this.CharacterSet = characterSet;
    }

    [NotNull]
    protected Matrix ViewMatrix { get; }

    [NotNull]
    protected ZplCommands ZplCommands { get; }

    protected CharacterSet CharacterSet { get; }

    [NotNull]
    [ItemNotNull]
    private IDictionary<CharacterSet, int> CharacterSetMappings { get; } = new Dictionary<CharacterSet, int>
                                                                           {
                                                                             {
                                                                               CharacterSet.ZebraCodePage1252, 1252
                                                                             },
                                                                             {
                                                                               CharacterSet.ZebraCodePage850, 850
                                                                             }
                                                                           };

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual Encoding GetEncoding()
    {
      // ReSharper disable ExceptionNotDocumentedOptional
      var codepage = this.CharacterSetMappings[this.CharacterSet];
      // ReSharper restore ExceptionNotDocumentedOptional
      // ReSharper disable ExceptionNotDocumentedOptional
      var encoding = Encoding.GetEncoding(codepage);
      // ReSharper restore ExceptionNotDocumentedOptional

      return encoding;
    }

    //[NotNull]
    //[Pure]
    //[MustUseReturnValue]
    //public virtual IEnumerable<ZplStream> GetInternalMemoryTranslation([NotNull] SvgDocument svgDocument)
    //{
    //  var parentMatrix = this.CreateParentMatrix();
    //  var translations = this.TranslaveSvgElementAndChildrenForStoring(svgDocument,
    //                                                                   parentMatrix,
    //                                                                   this.ViewMatrix);

    //  return translations;
    //}

    //[NotNull]
    //[Pure]
    //[MustUseReturnValue]
    //protected virtual IEnumerable<ZplStream> TranslaveSvgElementAndChildrenForStoring([NotNull] SvgElement svgElement,
    //                                                                                  [NotNull] Matrix parentMatrix,
    //                                                                                  [NotNull] Matrix viewMatrix)
    //{
    //  var matrix = this.MultiplyTransformationsIntoNewMatrix(svgElement,
    //                                                         parentMatrix);

    //  {
    //    var zplStream = this.TranslateSvgElementForStoring(svgElement,
    //                                                       matrix,
    //                                                       viewMatrix);
    //    if (zplStream.Any())
    //    {
    //      yield return zplStream;
    //    }
    //  }

    //  foreach (var child in svgElement.Children)
    //  {
    //    var translations = this.TranslaveSvgElementAndChildrenForStoring(child,
    //                                                                     matrix,
    //                                                                     viewMatrix);

    //    foreach (var zplStream in translations)
    //    {
    //      if (zplStream == null)
    //      {
    //        continue;
    //      }
    //      if (zplStream.Any())
    //      {
    //        yield return zplStream;
    //      }
    //    }
    //  }
    //}

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected virtual Matrix CreateParentMatrix() => new Matrix();

    //[NotNull]
    //[Pure]
    //[MustUseReturnValue]
    //protected virtual ZplStream TranslateSvgElementForStoring([NotNull] SvgElement svgElement,
    //                                                          [NotNull] Matrix matrix,
    //                                                          [NotNull] Matrix viewMatrix)
    //{
    //  var container = this.CreateZplStream();

    //  var type = svgElement.GetType();

    //  var svgElementToInternalMemoryTranslator = this.GetTranslator(type) as ISvgElementToInternalMemoryTranslator;
    //  if (svgElementToInternalMemoryTranslator == null)
    //  {
    //    return container;
    //  }

    //  matrix = matrix.Clone();
    //  matrix.Multiply(viewMatrix,
    //                  MatrixOrder.Append);

    //  svgElementToInternalMemoryTranslator.TranslateForStoring(svgElement,
    //                                                           matrix,
    //                                                           container);

    //  return container;
    //}

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public override ZplStream GetTranslation([NotNull] SvgDocument svgDocument)
    {
      var parentMatrix = this.CreateParentMatrix();
      var result = this.ZplCommands.CreateZplStream();

      var streamContainer = new Container<ZplStream>(this.ZplCommands.CreateZplStream(),
                                                     this.ZplCommands.CreateZplStream(),
                                                     this.ZplCommands.CreateZplStream());
      this.AddBodyToTranslation(svgDocument,
                                parentMatrix,
                                streamContainer);
      this.AddHeaderToTranslation(svgDocument,
                                  parentMatrix,
                                  streamContainer);
      this.AddFooterToTranslation(svgDocument,
                                  parentMatrix,
                                  streamContainer);

      result.Add(streamContainer.Header);
      result.Add(streamContainer.Body);
      result.Add(streamContainer.Footer);

      return result;
    }

    protected virtual void AddHeaderToTranslation([NotNull] SvgDocument svgDocument,
                                                  [NotNull] Matrix parentMatrix,
                                                  [NotNull] Container<ZplStream> container)
    {
      container.Header.Add(this.ZplCommands.LabelHome(0,
                                                      0));
      container.Header.Add(this.ZplCommands.PrintOrientation(PrintOrientation.Normal));
      container.Header.Add(this.ZplCommands.ChangeInternationalFont(this.CharacterSet));
    }

    protected virtual void AddBodyToTranslation([NotNull] SvgDocument svgDocument,
                                                [NotNull] Matrix parentMatrix,
                                                [NotNull] Container<ZplStream> container)
    {
      container.Body.Add(this.ZplCommands.StartFormat());
      this.TranslateSvgElementAndChildren(svgDocument,
                                          parentMatrix,
                                          this.ViewMatrix,
                                          container);
    }

    protected virtual void AddFooterToTranslation([NotNull] SvgDocument svgDocument,
                                                  [NotNull] Matrix parentMatrix,
                                                  [NotNull] Container<ZplStream> container)
    {
      container.Footer.Add(this.ZplCommands.EndFormat());
    }
  }
}