using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using JetBrains.Annotations;

namespace Svg.Contrib.Render.FingerPrint
{
  [PublicAPI]
  public class SvgImageTranslator : SvgImageTranslatorBase<FingerPrintContainer>
  {
    public SvgImageTranslator([NotNull] FingerPrintTransformer fingerPrintTransformer,
                              [NotNull] FingerPrintCommands fingerPrintCommands)
      : base(fingerPrintTransformer)
    {
      this.FingerPrintTransformer = fingerPrintTransformer;
      this.FingerPrintCommands = fingerPrintCommands;
    }

    [NotNull]
    protected FingerPrintCommands FingerPrintCommands { get; }

    [NotNull]
    protected FingerPrintTransformer FingerPrintTransformer { get; }

    protected override void StoreGraphics([NotNull] string variableName,
                                          [NotNull] Bitmap bitmap,
                                          [NotNull] FingerPrintContainer container)
    {
      var pcxByteArray = this.FingerPrintTransformer.ConvertToPcx(bitmap);

      container.Header.Add(this.FingerPrintCommands.RemoveImage(variableName));
      container.Header.Add(this.FingerPrintCommands.ImageLoad(variableName,
                                                              pcxByteArray.Length));
      container.Header.Add(pcxByteArray);
    }

    protected override void GraphicDirectWrite([NotNull] SvgImage svgElement,
                                               [NotNull] Matrix matrix,
                                               float sourceAlignmentWidth,
                                               float sourceAlignmentHeight,
                                               int horizontalStart,
                                               int verticalStart,
                                               [NotNull] FingerPrintContainer container)
    {
      throw new NotImplementedException();
      using (var bitmap = this.FingerPrintTransformer.ConvertToBitmap(svgElement,
                                                                      matrix,
                                                                      (int) sourceAlignmentWidth,
                                                                      (int) sourceAlignmentHeight))
      {
        if (bitmap == null)
        {
          return;
        }

        int numberOfBytesPerRow;
        var rawBinaryData = this.FingerPrintTransformer.GetRawBinaryData(bitmap,
                                                                         false,
                                                                         out numberOfBytesPerRow);

        container.Body.Add(this.FingerPrintCommands.Position(horizontalStart,
                                                             verticalStart));
        container.Body.Add(this.FingerPrintCommands.Direction(Direction.Direction3));
        container.Body.Add(this.FingerPrintCommands.Align(Alignment.TopLeft));
        container.Body.Add(this.FingerPrintCommands.NormalImage());
        container.Body.Add(this.FingerPrintCommands.PrintBuffer(rawBinaryData.Count()));
        container.Body.Add(rawBinaryData);
      }
    }

    protected override void PrintGraphics([NotNull] SvgImage svgElement,
                                          [NotNull] Matrix matrix,
                                          int horizontalStart,
                                          int verticalStart,
                                          int sector,
                                          [NotNull] string variableName,
                                          [NotNull] FingerPrintContainer container)
    {
      container.Body.Add(this.FingerPrintCommands.Magnify(1,
                                                          1));
      container.Body.Add(this.FingerPrintCommands.Position(horizontalStart,
                                                           verticalStart));
      container.Body.Add(this.FingerPrintCommands.Direction(Direction.Direction3));
      container.Body.Add(this.FingerPrintCommands.Align(Alignment.TopLeft));
      container.Body.Add(this.FingerPrintCommands.NormalImage());
      container.Body.Add(this.FingerPrintCommands.PrintImage(variableName));
    }

    [Pure]
    protected override void GetPosition([NotNull] SvgImage svgElement,
                                        [NotNull] Matrix matrix,
                                        out float sourceAlignmentWidth,
                                        out float sourceAlignmentHeight,
                                        out int horizontalStart,
                                        out int verticalStart,
                                        out int sector)
    {
      base.GetPosition(svgElement,
                       matrix,
                       out sourceAlignmentWidth,
                       out sourceAlignmentHeight,
                       out horizontalStart,
                       out verticalStart,
                       out sector);

      if (sector % 2 > 0)
      {
        horizontalStart += (int) sourceAlignmentHeight;
      }
    }
  }
}