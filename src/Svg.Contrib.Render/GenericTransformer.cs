using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using ImageMagick;
using JetBrains.Annotations;

// ReSharper disable NonLocalizedString
// ReSharper disable VirtualMemberNeverOverriden.Global

namespace Svg.Contrib.Render
{
  [PublicAPI]
  public class GenericTransformer
  {
    public GenericTransformer([NotNull] SvgUnitReader svgUnitReader,
                              int outputWidth,
                              int outputHeight)
    {
      this.SvgUnitReader = svgUnitReader;
      this.OutputWidth = outputWidth;
      this.OutputHeight = outputHeight;
    }

    protected int OutputHeight { get; }
    protected int OutputWidth { get; }

    [NotNull]
    protected SvgUnitReader SvgUnitReader { get; }

    [Pure]
    [MustUseReturnValue]
    protected virtual float GetLineHeightFactor([NotNull] SvgTextBase svgTextBase)
    {
      var svgText = svgTextBase as SvgText ?? svgTextBase.Parent as SvgText;
      if (svgText == null)
      {
        return 1f;
      }

      var result = 1f;
      if (svgText.HasNonEmptyCustomAttribute("linespacing"))
      {
        var linespacing = svgText.CustomAttributes["linespacing"];
        var percentage = new Percentage(linespacing);
        result = percentage.Value;
      }

      if (svgText.HasNonEmptyCustomAttribute("line-height"))
      {
        var lineHeight = svgText.CustomAttributes["line-height"];
        var percentage = new Percentage(lineHeight);
        result = percentage.Value;
      }

      return result;
    }

    [Pure]
    protected virtual void ApplyMatrixOnPoint(float x,
                                              float y,
                                              [NotNull] Matrix matrix,
                                              out float newX,
                                              out float newY)
    {
      var originalPoint = new PointF(x,
                                     y);

      var points = new[]
                   {
                     originalPoint
                   };
      matrix.TransformPoints(points);

      var transformedPoint = points[0];
      newX = transformedPoint.X;
      newY = transformedPoint.Y;
    }

    [Pure]
    [MustUseReturnValue]
    protected virtual float ApplyMatrixOnLength(float length,
                                                [NotNull] Matrix matrix)
    {
      var vector = new PointF(length,
                              0f);

      vector = this.ApplyMatrixOnVector(vector,
                                        matrix);

      var result = this.GetLengthOfVector(vector);

      return result;
    }

    [Pure]
    [MustUseReturnValue]
    protected virtual PointF ApplyMatrixOnVector(PointF vector,
                                                 [NotNull] Matrix matrix)
    {
      var vectors = new[]
                    {
                      vector
                    };

      matrix.TransformVectors(vectors);

      var result = vectors[0];

      return result;
    }

    [Pure]
    [MustUseReturnValue]
    protected virtual float GetLengthOfVector(PointF vector)
    {
      var result = Math.Sqrt(Math.Pow(vector.X,
                                      2) + Math.Pow(vector.Y,
                                                    2));

      return (int) result;
    }

    [Pure]
    public virtual void Transform([NotNull] SvgLine svgLine,
                                  [NotNull] Matrix matrix,
                                  out float startX,
                                  out float startY,
                                  out float endX,
                                  out float endY,
                                  out float strokeWidth)
    {
      startX = this.SvgUnitReader.GetValue(svgLine,
                                           svgLine.StartX);
      startY = this.SvgUnitReader.GetValue(svgLine,
                                           svgLine.StartY);
      endX = this.SvgUnitReader.GetValue(svgLine,
                                         svgLine.EndX);
      endY = this.SvgUnitReader.GetValue(svgLine,
                                         svgLine.EndY);
      strokeWidth = this.SvgUnitReader.GetValue(svgLine,
                                                svgLine.StrokeWidth);

      this.ApplyMatrixOnPoint(startX,
                              startY,
                              matrix,
                              out startX,
                              out startY);

      this.ApplyMatrixOnPoint(endX,
                              endY,
                              matrix,
                              out endX,
                              out endY);

      if (endX < startX)
      {
        var temp = startX;
        startX = endX;
        endX = temp;
      }

      if (endY < startY)
      {
        var temp = startY;
        startY = endY;
        endY = temp;
      }

      strokeWidth = this.ApplyMatrixOnLength(strokeWidth,
                                             matrix);
    }

    [Pure]
    public virtual void Transform([NotNull] SvgImage svgImage,
                                  [NotNull] Matrix matrix,
                                  out float startX,
                                  out float startY,
                                  out float endX,
                                  out float endY,
                                  out float sourceAlignmentWidth,
                                  out float sourceAlignmentHeight)
    {
      startX = this.SvgUnitReader.GetValue(svgImage,
                                           svgImage.X);
      startY = this.SvgUnitReader.GetValue(svgImage,
                                           svgImage.Y);
      sourceAlignmentWidth = this.SvgUnitReader.GetValue(svgImage,
                                                         svgImage.Width);
      sourceAlignmentHeight = this.SvgUnitReader.GetValue(svgImage,
                                                          svgImage.Height);
      endX = startX + sourceAlignmentWidth;
      endY = startY + sourceAlignmentHeight;

      this.ApplyMatrixOnPoint(startX,
                              startY,
                              matrix,
                              out startX,
                              out startY);

      this.ApplyMatrixOnPoint(endX,
                              endY,
                              matrix,
                              out endX,
                              out endY);

      if (endY < startY)
      {
        var temp = startY;
        startY = endY;
        endY = temp;
      }

      if (endX < startX)
      {
        var temp = startX;
        startX = endX;
        endX = temp;
      }

      sourceAlignmentWidth = this.ApplyMatrixOnLength(sourceAlignmentWidth,
                                                      matrix);
      sourceAlignmentHeight = this.ApplyMatrixOnLength(sourceAlignmentHeight,
                                                       matrix);
    }

    [Pure]
    public virtual void Transform([NotNull] SvgRectangle svgRectangle,
                                  [NotNull] Matrix matrix,
                                  out float startX,
                                  out float startY,
                                  out float endX,
                                  out float endY,
                                  out float strokeWidth)
    {
      startX = this.SvgUnitReader.GetValue(svgRectangle,
                                           svgRectangle.X);
      endX = startX + this.SvgUnitReader.GetValue(svgRectangle,
                                                  svgRectangle.Width);
      startY = this.SvgUnitReader.GetValue(svgRectangle,
                                           svgRectangle.Y);
      endY = startY + this.SvgUnitReader.GetValue(svgRectangle,
                                                  svgRectangle.Height);
      strokeWidth = this.SvgUnitReader.GetValue(svgRectangle,
                                                svgRectangle.StrokeWidth);

      this.ApplyMatrixOnPoint(startX,
                              startY,
                              matrix,
                              out startX,
                              out startY);

      this.ApplyMatrixOnPoint(endX,
                              endY,
                              matrix,
                              out endX,
                              out endY);

      if (endY < startY)
      {
        var temp = startY;
        startY = endY;
        endY = temp;
      }

      if (endX < startX)
      {
        var temp = startX;
        startX = endX;
        endX = temp;
      }

      strokeWidth = this.ApplyMatrixOnLength(strokeWidth,
                                             matrix);
    }

    [Pure]
    public virtual void Transform([NotNull] SvgTextBase svgTextBase,
                                  [NotNull] Matrix matrix,
                                  out float startX,
                                  out float startY,
                                  out float fontSize)
    {
      startX = this.SvgUnitReader.GetValue(svgTextBase,
                                           svgTextBase.X.FirstOrDefault());
      startY = this.SvgUnitReader.GetValue(svgTextBase,
                                           svgTextBase.Y.FirstOrDefault());
      fontSize = this.SvgUnitReader.GetValue(svgTextBase,
                                             svgTextBase.FontSize);

      startY -= fontSize / this.GetLineHeightFactor(svgTextBase);

      this.ApplyMatrixOnPoint(startX,
                              startY,
                              matrix,
                              out startX,
                              out startY);

      fontSize = this.ApplyMatrixOnLength(fontSize,
                                          matrix);
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual Matrix CreateViewMatrix(float sourceDpi,
                                           float destinationDpi,
                                           ViewRotation viewRotation)
    {
      var magnificationFactor = destinationDpi / sourceDpi;

      // TODO test this shit!

      var matrix = new Matrix();
      matrix.Scale(magnificationFactor,
                   magnificationFactor);
      if (viewRotation == ViewRotation.RotateBy90Degrees)
      {
        matrix.Rotate(90f);
        matrix.Translate(this.OutputWidth,
                         0,
                         MatrixOrder.Append);
      }
      else if (viewRotation == ViewRotation.RotateBy180Degrees)
      {
        // TODO test this orientation!
        matrix.Rotate(180f);
        matrix.Translate(-this.OutputWidth,
                         -this.OutputHeight,
                         MatrixOrder.Append);
      }
      else if (viewRotation == ViewRotation.RotateBy270Degress)
      {
        matrix.Rotate(270f);
        matrix.Translate(0,
                         this.OutputHeight,
                         MatrixOrder.Append);
      }

      return matrix;
    }

    [Pure]
    [MustUseReturnValue]
    public virtual int GetRotationSector([NotNull] Matrix matrix)
    {
      var vector = new PointF(10f,
                              0f);

      vector = this.ApplyMatrixOnVector(vector,
                                        matrix);

      var radians = Math.Atan2(vector.Y,
                               vector.X);
      var degrees = radians * (180d / Math.PI);
      if (degrees < 0)
      {
        degrees = 360 + degrees;
      }

      var sector = (int) Math.Round(degrees / 90d) % 4;

      return sector;
    }

    [CanBeNull]
    [Pure]
    [MustUseReturnValue]
    public virtual Bitmap ConvertToBitmap([NotNull] SvgImage svgElement,
                                          [NotNull] Matrix matrix,
                                          int sourceAlignmentWidth,
                                          int sourceAlignmentHeight)
    {
      var stretchImage = this.StretchImage(svgElement);

      using (var image = svgElement.GetImage() as Image)
      {
        if (image == null)
        {
          return null;
        }

        Bitmap bitmap;
        if (stretchImage)
        {
          bitmap = new Bitmap(image,
                              sourceAlignmentWidth,
                              sourceAlignmentHeight);
        }
        else
        {
          var sourceRatio = (float) sourceAlignmentWidth / sourceAlignmentHeight;
          var destinationRatio = (float) image.Width / image.Height;

          // TODO find a good TOLERANCE
          if (Math.Abs(sourceRatio - destinationRatio) < 0.5f)
          {
            bitmap = new Bitmap(image,
                                sourceAlignmentWidth,
                                sourceAlignmentHeight);
          }
          else
          {
            int destinationWidth;
            int destinationHeight;

            if (sourceRatio < destinationRatio)
            {
              destinationWidth = sourceAlignmentWidth;
              destinationHeight = (int) (sourceAlignmentWidth / destinationRatio);
            }
            else
            {
              destinationWidth = (int) (sourceAlignmentHeight * destinationRatio);
              destinationHeight = sourceAlignmentHeight;
            }

            var x = (sourceAlignmentWidth - destinationWidth) / 2;
            var y = (sourceAlignmentHeight - destinationHeight) / 2;

            bitmap = new Bitmap(sourceAlignmentWidth,
                                sourceAlignmentHeight);
            using (var graphics = Graphics.FromImage(bitmap))
            {
              var rect = new Rectangle(x,
                                       y,
                                       destinationWidth,
                                       destinationHeight);
              graphics.DrawImage(image,
                                 rect);
            }
          }
        }

        var rotateFlipType = (RotateFlipType) this.GetRotationSector(matrix);
        bitmap.RotateFlip(rotateFlipType);

        return bitmap;
      }
    }

    [Pure]
    [MustUseReturnValue]
    protected virtual bool StretchImage([NotNull] SvgImage svgImage) => false;

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual IEnumerable<byte> GetRawBinaryData([NotNull] Bitmap bitmap,
                                                      bool invertByte,
                                                      out int numberOfBytesPerRow)
    {
      // TODO merge with MagickImage, as we are having different thresholds here

      numberOfBytesPerRow = (int) Math.Ceiling(bitmap.Width / 8f);

      var rawBinaryData = this.GetRawBinaryData(bitmap,
                                                invertByte,
                                                numberOfBytesPerRow);

      return rawBinaryData;
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual IEnumerable<byte> GetRawBinaryData([NotNull] Bitmap bitmap,
                                                      bool invertByte,
                                                      int numberOfBytesPerRow)
    {
      var height = bitmap.Height;
      var width = bitmap.Width;

      for (var y = 0;
           y < height;
           y++)
      {
        for (var octett = 0;
             octett < numberOfBytesPerRow;
             octett++)
        {
          var value = (int) byte.MinValue;

          for (var i = 0;
               i < 8;
               i++)
          {
            var x = octett * 8 + i;
            var bitIndex = 7 - i;
            if (x < width)
            {
              var color = bitmap.GetPixel(x,
                                          y);
              if (color.A > 0x32
                  || color.R > 0x96 && color.G > 0x96 && color.B > 0x96)
              {
                value |= 1 << bitIndex;
              }
            }
          }

          if (invertByte)
          {
            value = ~value;
          }

          yield return (byte) value;
        }
      }
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual byte[] ConvertToPcx([NotNull] Bitmap bitmap,
                                       bool invert)
    {
      MagickImage magickImage;

      var width = bitmap.Width;
      var mod = width % 8;
      var height = bitmap.Height;
      if (mod > 0)
      {
        width += 8 - mod;
        using (var resizedBitmap = new Bitmap(width,
                                              height))
        {
          using (var graphics = Graphics.FromImage(resizedBitmap))
          {
            graphics.DrawImageUnscaled(bitmap,
                                       0,
                                       0);
            graphics.Save();
          }

          magickImage = new MagickImage(resizedBitmap);
        }
      }
      else
      {
        magickImage = new MagickImage(bitmap);
      }

      byte[] array;
      using (magickImage)
      {
        // TODO threshold
        magickImage.Format = MagickFormat.Pcx;
        magickImage.ColorType = ColorType.Bilevel;
        if (invert)
        {
          magickImage.Negate();
        }

        array = magickImage.ToByteArray();
      }

      return array;
    }
  }
}