using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using ImageMagick;
using JetBrains.Annotations;

// ReSharper disable NonLocalizedString
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace Svg.Contrib.Render.FingerPrint
{
  [PublicAPI]
  public class FingerPrintTransformer : GenericTransformer
  {
    public const int DefaultOutputHeight = 1296;
    public const int DefaultOutputWidth = 816;

    public FingerPrintTransformer([NotNull] SvgUnitReader svgUnitReader)
      : base(svgUnitReader,
             FingerPrintTransformer.DefaultOutputWidth,
             FingerPrintTransformer.DefaultOutputHeight) {}

    public FingerPrintTransformer([NotNull] SvgUnitReader svgUnitReader,
                                  int outputWidth,
                                  int outputHeight)
      : base(svgUnitReader,
             outputWidth,
             outputHeight) {}

    [Pure]
    [NotNull]
    protected override Matrix CreateDeviceMatrix()
    {
      var deviceMatrix = new Matrix(1,
                                    0,
                                    0,
                                    -1,
                                    0,
                                    0);
      return deviceMatrix;
    }

    [Pure]
    [NotNull]
    protected override Matrix ApplyViewRotationOnDeviceMatrix([NotNull] Matrix deviceMatrix,
                                                              float magnificationFactor,
                                                              ViewRotation viewRotation = ViewRotation.Normal)
    {
      var viewMatrix = deviceMatrix.Clone();

      if (viewRotation == ViewRotation.Normal)
      {
        viewMatrix.Scale(magnificationFactor,
                         magnificationFactor,
                         MatrixOrder.Prepend);
        viewMatrix.Translate(0,
                             this.OutputHeight,
                             MatrixOrder.Append);
      }
      else if (viewRotation == ViewRotation.RotateBy90Degrees)
      {
        viewMatrix.Scale(magnificationFactor,
                         magnificationFactor,
                         MatrixOrder.Prepend);
        viewMatrix.Rotate(270f,
                          MatrixOrder.Prepend);
      }
      else if (viewRotation == ViewRotation.RotateBy180Degrees)
      {
        viewMatrix.Scale(magnificationFactor,
                         magnificationFactor,
                         MatrixOrder.Prepend);
        viewMatrix.Rotate(180f,
                          MatrixOrder.Prepend);
        viewMatrix.Translate(this.OutputWidth,
                             0,
                             MatrixOrder.Append);
      }
      else if (viewRotation == ViewRotation.RotateBy270Degress)
      {
        viewMatrix.Scale(magnificationFactor,
                         magnificationFactor,
                         MatrixOrder.Prepend);
        viewMatrix.Rotate(90f,
                          MatrixOrder.Prepend);
        viewMatrix.Translate(this.OutputWidth,
                             this.OutputHeight,
                             MatrixOrder.Append);
      }

      return viewMatrix;
    }

    [Pure]
    public override void Transform([NotNull] SvgRectangle svgRectangle,
                                   [NotNull] Matrix sourceMatrix,
                                   [NotNull] Matrix viewMatrix,
                                   out float startX,
                                   out float startY,
                                   out float endX,
                                   out float endY,
                                   out float strokeWidth)
    {
      base.Transform(svgRectangle,
                     sourceMatrix,
                     viewMatrix,
                     out startX,
                     out startY,
                     out endX,
                     out endY,
                     out strokeWidth);

      startX -= strokeWidth / 2f;
      endX += strokeWidth / 2f;
      startY -= strokeWidth / 2f;
      endY += strokeWidth / 2f;
    }

    [Pure]
    public void Transform([NotNull] SvgTextBase svgTextBase,
                          [NotNull] Matrix sourceMatrix,
                          [NotNull] Matrix viewMatrix,
                          out float startX,
                          out float startY,
                          out float fontSize,
                          out Direction direction)
    {
      startX = this.SvgUnitReader.GetValue(svgTextBase,
                                           svgTextBase.X.FirstOrDefault());
      startY = this.SvgUnitReader.GetValue(svgTextBase,
                                           svgTextBase.Y.FirstOrDefault());
      fontSize = this.SvgUnitReader.GetValue(svgTextBase,
                                             svgTextBase.FontSize);

      direction = this.GetDirection(sourceMatrix,
                                    viewMatrix);

      if ((int) direction % 2 > 0)
      {
        startX -= fontSize / this.GetLineHeightFactor(svgTextBase);
      }

      this.ApplyMatrixOnPoint(startX,
                              startY,
                              sourceMatrix,
                              viewMatrix,
                              out startX,
                              out startY);
    }

    [Pure]
    public virtual Direction GetDirection([NotNull] Matrix sourceMatrix,
                                          [NotNull] Matrix viewMatrix)
    {
      var sector = this.GetRotationSector(sourceMatrix,
                                          viewMatrix);
      var direction = (Direction) ((4 - sector) % 4 + 1);

      return direction;
    }

    [Pure]
    public virtual void GetFontSelection([NotNull] SvgTextBase svgTextBase,
                                         float fontSize,
                                         out string fontName,
                                         out int characterHeight,
                                         out int slant)
    {
      if (svgTextBase.FontWeight > SvgFontWeight.Normal)
      {
        fontName = "Swiss 721 Bold BT";
      }
      else
      {
        fontName = "Swiss 721 BT";
      }

      characterHeight = (int) fontSize;

      if ((svgTextBase.FontStyle & SvgFontStyle.Italic) != 0)
      {
        slant = 20;
      }
      else
      {
        slant = 0;
      }
    }

    [NotNull]
    [Pure]
    public override IEnumerable<byte> GetRawBinaryData([NotNull] Bitmap bitmap,
                                                       bool invertBytes,
                                                       int numberOfBytesPerRow)
    {
      var result = new byte[]
                   {
                     0x40,
                     0x00
                   }.Concat(base.GetRawBinaryData(bitmap,
                                                  invertBytes,
                                                  numberOfBytesPerRow));

      return result;
    }

    [NotNull]
    [Pure]
    public virtual byte[] ConvertToPcx([NotNull] Bitmap bitmap)
    {
      var width = bitmap.Width;
      var mod = width % 8;
      if (mod > 0)
      {
        width += 8 - mod;
      }
      var height = bitmap.Height;

      using (var magickImage = new MagickImage(bitmap))
      {
        if (mod > 0)
        {
          var magickGeometry = new MagickGeometry
                               {
                                 Width = width,
                                 Height = height,
                                 IgnoreAspectRatio = true
                               };
          magickImage.Resize(magickGeometry);
        }

        magickImage.ColorAlpha(MagickColors.White);

        var quantizeSettings = new QuantizeSettings
                               {
                                 Colors = 2,
                                 DitherMethod = DitherMethod.No
                               };
        magickImage.Quantize(quantizeSettings);

        magickImage.Format = MagickFormat.Pcx;
        magickImage.ColorType = ColorType.Palette;
        magickImage.ColorSpace = ColorSpace.Gray;

        var array = magickImage.ToByteArray();

        return array;
      }
    }
  }
}