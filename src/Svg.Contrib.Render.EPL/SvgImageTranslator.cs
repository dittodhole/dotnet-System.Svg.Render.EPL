using System.Drawing;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

// ReSharper disable NonLocalizedString
// ReSharper disable VirtualMemberNeverOverriden.Global

namespace Svg.Contrib.Render.EPL
{
  [PublicAPI]
  public class SvgImageTranslator : SvgImageTranslatorBase<EplContainer>
  {
    public SvgImageTranslator([NotNull] EplTransformer eplTransformer,
                              [NotNull] EplCommands eplCommands)
      : base(eplTransformer)
    {
      this.EplTransformer = eplTransformer;
      this.EplCommands = eplCommands;
    }

    [NotNull]
    protected EplTransformer EplTransformer { get; }

    [NotNull]
    protected EplCommands EplCommands { get; }

    protected override void StoreGraphics([NotNull] string variableName,
                                          [NotNull] Bitmap bitmap,
                                          [NotNull] EplContainer container)
    {
      var pcxByteArray = this.EplTransformer.ConvertToPcx(bitmap);

      container.Header.Add(this.EplCommands.DeleteGraphics(variableName));
      container.Header.Add(this.EplCommands.DeleteGraphics(variableName));
      container.Header.Add(this.EplCommands.StoreGraphics(variableName,
                                                          pcxByteArray.Length));
      container.Header.Add(pcxByteArray);
    }

    protected override void GraphicDirectWrite([NotNull] SvgImage svgElement,
                                               [NotNull] Matrix matrix,
                                               float sourceAlignmentWidth,
                                               float sourceAlignmentHeight,
                                               int horizontalStart,
                                               int verticalStart,
                                               [NotNull] EplContainer container)
    {
      using (var bitmap = this.EplTransformer.ConvertToBitmap(svgElement,
                                                              matrix,
                                                              (int) sourceAlignmentWidth,
                                                              (int) sourceAlignmentHeight))
      {
        if (bitmap == null)
        {
          return;
        }

        int numberOfBytesPerRow;
        var rawBinaryData = this.EplTransformer.GetRawBinaryData(bitmap,
                                                                 true,
                                                                 out numberOfBytesPerRow);
        var rows = bitmap.Height;

        container.Body.Add(this.EplCommands.GraphicDirectWrite(horizontalStart,
                                                               verticalStart,
                                                               numberOfBytesPerRow,
                                                               rows));
        container.Body.Add(rawBinaryData);
      }
    }

    protected override void PrintGraphics([NotNull] SvgImage svgElement,
                                          [NotNull] Matrix matrix,
                                          int horizontalStart,
                                          int verticalStart,
                                          int sector,
                                          [NotNull] string variableName,
                                          [NotNull] EplContainer container)
    {
      container.Body.Add(this.EplCommands.PrintGraphics(horizontalStart,
                                                        verticalStart,
                                                        variableName));
    }
  }
}