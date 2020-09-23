using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using ImageMagick;
using JetBrains.Annotations;

#if NETSTANDARD2_0
using System.IO;
#endif

namespace Svg.Contrib.Render.FingerPrint
{
  [PublicAPI]
  public class FingerPrintTransformer : GenericTransformer
  {
    public const int DefaultOutputHeight = 1296;
    public const int DefaultOutputWidth = 816;

    /// <exception cref="ArgumentNullException"><paramref name="svgUnitReader" /> is <see langword="null" />.</exception>
    public FingerPrintTransformer([NotNull] SvgUnitReader svgUnitReader,
                                  int outputWidth = FingerPrintTransformer.DefaultOutputWidth,
                                  int outputHeight = FingerPrintTransformer.DefaultOutputHeight)
      : base(svgUnitReader,
             outputWidth,
             outputHeight) { }

    [Pure]
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

    /// <exception cref="ArgumentNullException"><paramref name="deviceMatrix" /> is <see langword="null" />.</exception>
    [Pure]
    protected override Matrix ApplyViewRotationOnDeviceMatrix(Matrix deviceMatrix,
                                                              float magnificationFactor,
                                                              ViewRotation viewRotation = ViewRotation.Normal)
    {
      if (deviceMatrix == null)
      {
        throw new ArgumentNullException(nameof(deviceMatrix));
      }

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
      else if (viewRotation == ViewRotation.RotateBy270Degrees)
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

    /// <exception cref="ArgumentNullException"><paramref name="svgRectangle" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    [Pure]
    public override void Transform(SvgRectangle svgRectangle,
                                   Matrix sourceMatrix,
                                   Matrix viewMatrix,
                                   out float startX,
                                   out float startY,
                                   out float endX,
                                   out float endY,
                                   out float strokeWidth)
    {
      if (svgRectangle == null)
      {
        throw new ArgumentNullException(nameof(svgRectangle));
      }
      if (sourceMatrix == null)
      {
        throw new ArgumentNullException(nameof(sourceMatrix));
      }
      if (viewMatrix == null)
      {
        throw new ArgumentNullException(nameof(viewMatrix));
      }

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

    /// <exception cref="ArgumentNullException"><paramref name="svgTextBase" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    [Pure]
    public void Transform([NotNull] SvgTextBase svgTextBase,
                          [NotNull] Matrix sourceMatrix,
                          [NotNull] Matrix viewMatrix,
                          out float startX,
                          out float startY,
                          out float fontSize,
                          out Direction direction)
    {
      if (svgTextBase == null)
      {
        throw new ArgumentNullException(nameof(svgTextBase));
      }
      if (sourceMatrix == null)
      {
        throw new ArgumentNullException(nameof(sourceMatrix));
      }
      if (viewMatrix == null)
      {
        throw new ArgumentNullException(nameof(viewMatrix));
      }

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
        var lineHeightFactor = this.GetLineHeightFactor(svgTextBase);
        if (lineHeightFactor >= float.Epsilon)
        {
          startX -= fontSize / lineHeightFactor;
        }
        else
        {
          startX -= fontSize;
        }
      }

      this.ApplyMatrixOnPoint(startX,
                              startY,
                              sourceMatrix,
                              viewMatrix,
                              out startX,
                              out startY);
    }

    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    [Pure]
    public virtual Direction GetDirection([NotNull] Matrix sourceMatrix,
                                          [NotNull] Matrix viewMatrix)
    {
      if (sourceMatrix == null)
      {
        throw new ArgumentNullException(nameof(sourceMatrix));
      }
      if (viewMatrix == null)
      {
        throw new ArgumentNullException(nameof(viewMatrix));
      }

      var sector = this.GetRotationSector(sourceMatrix,
                                          viewMatrix);
      var direction = (Direction) ((4 - sector) % 4 + 1);

      return direction;
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgTextBase" /> is <see langword="null" />.</exception>
    [Pure]
    public virtual void GetFontSelection([NotNull] SvgTextBase svgTextBase,
                                         float fontSize,
                                         out string fontName,
                                         out int characterHeight,
                                         out int slant)
    {
      if (svgTextBase == null)
      {
        throw new ArgumentNullException(nameof(svgTextBase));
      }

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

    /// <exception cref="ArgumentNullException"><paramref name="bitmap" /> is <see langword="null" />.</exception>
    [Pure]
    public override IEnumerable<byte> GetRawBinaryData(Bitmap bitmap,
                                                       bool invertBytes,
                                                       int numberOfBytesPerRow)
    {
      if (bitmap == null)
      {
        throw new ArgumentNullException(nameof(bitmap));
      }

      var result = new byte[]
                   {
                     0x40,
                     0x00
                   }.Concat(base.GetRawBinaryData(bitmap,
                                                  invertBytes,
                                                  numberOfBytesPerRow));

      return result;
    }

    /// <exception cref="ArgumentNullException"><paramref name="bitmap" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    public virtual byte[] ConvertToPcx([NotNull] Bitmap bitmap)
    {
      if (bitmap == null)
      {
        throw new ArgumentNullException(nameof(bitmap));
      }

      // TODO merge with Svg.Contrib.Render.EPL.EplTransformer.ConvertToPcx, Svg.Contrib.Render.EPL

      var width = bitmap.Width;
      var mod = width % 8;
      if (mod > 0)
      {
        width += 8 - mod;
      }
      var height = bitmap.Height;

      MagickImage magickImage;
#if NETSTANDARD2_0
      using (var memoryStream = new MemoryStream())
      {
        bitmap.Save(memoryStream,
                    bitmap.RawFormat);

        magickImage = new MagickImage(memoryStream);
      }
#else
      magickImage = new MagickImage(bitmap);
#endif

      using (magickImage)
      {
        if (mod > 0)
        {
          var magickGeometry = new MagickGeometry(width,
                                                  height)
                               {
                                 IgnoreAspectRatio = true
                               };
          magickImage.Resize(magickGeometry);
        }

        if (magickImage.HasAlpha)
        {
          magickImage.ColorAlpha(MagickColors.White);
        }

        var quantizeSettings = new QuantizeSettings
                               {
                                 Colors = 2,
                                 DitherMethod = DitherMethod.No
                               };
        magickImage.Quantize(quantizeSettings);

        magickImage.ColorType = ColorType.Bilevel;
        magickImage.Depth = 1;
        magickImage.Format = MagickFormat.Pcx;

        magickImage.Density = new Density(bitmap.HorizontalResolution,
                                          bitmap.VerticalResolution);

        magickImage.Negate(); // TODO see https://github.com/dlemstra/Magick.NET/issues/569

        var array = magickImage.ToByteArray();

        array = this.StripExtendedColorPaletteFromPcx(array);

        return array;
      }
    }
  }
}
