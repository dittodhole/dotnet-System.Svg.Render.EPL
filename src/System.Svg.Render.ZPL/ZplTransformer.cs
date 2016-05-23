using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using JetBrains.Annotations;

// ReSharper disable NonLocalizedString

namespace System.Svg.Render.ZPL
{
  [PublicAPI]
  public class ZplTransformer : GenericTransformer
  {
    public const int DefaultOutputHeight = 1296;
    public const int DefaultOutputWidth = 816;

    public ZplTransformer([NotNull] SvgUnitReader svgUnitReader)
      : base(svgUnitReader,
             ZplTransformer.DefaultOutputWidth,
             ZplTransformer.DefaultOutputHeight) {}

    public ZplTransformer([NotNull] SvgUnitReader svgUnitReader,
                          int outputWidth,
                          int outputHeight)
      : base(svgUnitReader,
             outputWidth,
             outputHeight) {}

    protected virtual int MaximumUpperFontSizeOverlap { get; } = 2;

    [NotNull]
    [ItemNotNull]
    private IDictionary<int, FieldOrientation> FieldOrientationMappings { get; } = new Dictionary<int, FieldOrientation>
                                                                                   {
                                                                                     {
                                                                                       0, FieldOrientation.Normal
                                                                                     },
                                                                                     {
                                                                                       1, FieldOrientation.RotatedBy90Degrees
                                                                                     },
                                                                                     {
                                                                                       2, FieldOrientation.RotatedBy180Degrees
                                                                                     },
                                                                                     {
                                                                                       3, FieldOrientation.RotatedBy270Degrees
                                                                                     }
                                                                                   };

    [Pure]
    [MustUseReturnValue]
    public virtual FieldOrientation GetRotation([NotNull] Matrix matrix)
    {
      var vector = new PointF(10f,
                              0f);

      vector = this.ApplyMatrixOnVector(vector,
                                        matrix);

      // ReSharper disable ExceptionNotDocumentedOptional
      var fieldOrientations = this.FieldOrientationMappings.Count();
      // ReSharper restore ExceptionNotDocumentedOptional

      var key = (int) Math.Abs(Math.Atan2(vector.Y,
                                          vector.X) / (2 * Math.PI) * fieldOrientations) % fieldOrientations;

      // ReSharper disable ExceptionNotDocumentedOptional
      var fieldOrientation = this.FieldOrientationMappings[key];
      // ReSharper restore ExceptionNotDocumentedOptional

      return fieldOrientation;
    }

    //public override void Transform(SvgImage svgImage,
    //                               Matrix matrix,
    //                               out float startX,
    //                               out float startY,
    //                               out float endX,
    //                               out float endY,
    //                               out float sourceAlignmentWidth,
    //                               out float sourceAlignmentHeight)
    //{
    //  base.Transform(svgImage,
    //                 matrix,
    //                 out startX,
    //                 out startY,
    //                 out endX,
    //                 out endY,
    //                 out sourceAlignmentWidth,
    //                 out sourceAlignmentHeight);

    //  var rotation = this.GetRotation(matrix);
    //  if (rotation % 2 > 0)
    //  {
    //    var width = Math.Abs(startX - endX);

    //    startX -= width;
    //    endX -= width;
    //  }
    //}

    public override void Transform([NotNull] SvgLine svgLine,
                                   [NotNull] Matrix matrix,
                                   out float startX,
                                   out float startY,
                                   out float endX,
                                   out float endY,
                                   out float strokeWidth)
    {
      base.Transform(svgLine,
                     matrix,
                     out startX,
                     out startY,
                     out endX,
                     out endY,
                     out strokeWidth);

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
    }

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

      startX -= strokeWidth / 2f;
      endX += strokeWidth / 2f;
      startY -= strokeWidth / 2f;
      endY += strokeWidth / 2f;
    }

    public virtual void GetFontSelection([NotNull] SvgTextBase svgTextBase,
                                         float fontSize,
                                         out string fontName,
                                         out int characterHeight,
                                         out int width)
    {
      fontName = "0";
      characterHeight = (int) fontSize;
      width = characterHeight / 15 * 12;
    }
  }
}