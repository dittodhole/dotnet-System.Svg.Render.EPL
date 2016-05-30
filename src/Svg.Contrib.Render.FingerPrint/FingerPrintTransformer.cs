using System.Drawing.Drawing2D;
using System.Linq;
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

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public override Matrix CreateViewMatrix(float sourceDpi,
                                            float destinationDpi,
                                            ViewRotation viewRotation)
    {
      var magnificationFactor = destinationDpi / sourceDpi;

      // TODO test this shit!

      var matrix = new Matrix(1,
                              0,
                              0,
                              -1,
                              0,
                              0);
      matrix.Scale(magnificationFactor,
                   magnificationFactor);
      if (viewRotation == ViewRotation.Normal)
      {
        // TODO test this orientation!
        matrix.Translate(0,
                         this.OutputHeight,
                         MatrixOrder.Append);
      }
      else if (viewRotation == ViewRotation.RotateBy90Degrees)
      {
        matrix.Rotate(270f);
      }
      else if (viewRotation == ViewRotation.RotateBy180Degrees)
      {
        // TODO test this orientation!
        matrix.Rotate(180f);
        matrix.Translate(-this.OutputWidth,
                         this.OutputHeight,
                         MatrixOrder.Append);
      }
      else if (viewRotation == ViewRotation.RotateBy270Degress)
      {
        // TODO test this orientation!
        matrix.Rotate(90f);
        matrix.Translate(0,
                         this.OutputHeight,
                         MatrixOrder.Append);
      }

      return matrix;
    }

    [Pure]
    public override void Transform([NotNull] SvgRectangle svgRectangle,
                                   [NotNull] Matrix matrix,
                                   out float startX,
                                   out float startY,
                                   out float endX,
                                   out float endY,
                                   out float strokeWidth)
    {
      base.Transform(svgRectangle,
                     matrix,
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
                          [NotNull] Matrix matrix,
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

      direction = this.GetDirection(matrix);

      if ((int) direction % 2 == 0)
      {
        startX -= fontSize / this.GetLineHeightFactor(svgTextBase);
      }
      else
      {
        startY -= fontSize / this.GetLineHeightFactor(svgTextBase);
      }

      this.ApplyMatrixOnPoint(startX,
                              startY,
                              matrix,
                              out startX,
                              out startY);
    }

    [MustUseReturnValue]
    [Pure]
    public virtual Direction GetDirection([NotNull] Matrix matrix)
    {
      var sector = this.GetRotationSector(matrix);
      var direction = (Direction) ((4 - sector) % 4 + 1);

      return direction;
    }

    [Pure]
    public void GetFontSelection([NotNull] SvgTextBase svgTextBase,
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
  }
}