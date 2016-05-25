using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Text;
using JetBrains.Annotations;

namespace Svg.Contrib.Render.ZPL
{
  [PublicAPI]
  public class ZplRenderer : RendererBase<ZplContainer>
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

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected virtual Matrix CreateParentMatrix() => new Matrix();

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public override ZplContainer GetTranslation([NotNull] SvgDocument svgDocument)
    {
      var parentMatrix = this.CreateParentMatrix();
      var zplContainer = new ZplContainer(this.ZplCommands.CreateZplStream(),
                                          this.ZplCommands.CreateZplStream(),
                                          this.ZplCommands.CreateZplStream());
      this.AddBodyToTranslation(svgDocument,
                                parentMatrix,
                                zplContainer);
      this.AddHeaderToTranslation(svgDocument,
                                  parentMatrix,
                                  zplContainer);
      this.AddFooterToTranslation(svgDocument,
                                  parentMatrix,
                                  zplContainer);

      return zplContainer;
    }

    protected virtual void AddHeaderToTranslation([NotNull] SvgDocument svgDocument,
                                                  [NotNull] Matrix parentMatrix,
                                                  [NotNull] ZplContainer container)
    {
      container.Header.Add(this.ZplCommands.LabelHome(0,
                                                      0));
      container.Header.Add(this.ZplCommands.PrintOrientation(PrintOrientation.Normal));
      container.Header.Add(this.ZplCommands.ChangeInternationalFont(this.CharacterSet));
    }

    protected virtual void AddBodyToTranslation([NotNull] SvgDocument svgDocument,
                                                [NotNull] Matrix parentMatrix,
                                                [NotNull] ZplContainer container)
    {
      container.Body.Add(this.ZplCommands.StartFormat());
      this.TranslateSvgElementAndChildren(svgDocument,
                                          parentMatrix,
                                          this.ViewMatrix,
                                          container);
    }

    protected virtual void AddFooterToTranslation([NotNull] SvgDocument svgDocument,
                                                  [NotNull] Matrix parentMatrix,
                                                  [NotNull] ZplContainer container)
    {
      container.Footer.Add(this.ZplCommands.EndFormat());
    }
  }
}