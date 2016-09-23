using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

// ReSharper disable NonLocalizedString
// ReSharper disable VirtualMemberNeverOverriden.Global

namespace Svg.Contrib.Render.ZPL
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

    [NotNull]
    [ItemNotNull]
    private IDictionary<int, FieldOrientation> SectorMappings { get; } = new Dictionary<int, FieldOrientation>
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
    public virtual FieldOrientation GetFieldOrientation([NotNull] Matrix sourceMatrix,
                                                        [NotNull] Matrix viewMatrix)
    {
      var sector = this.GetRotationSector(sourceMatrix,
                                          viewMatrix);

      // ReSharper disable ExceptionNotDocumentedOptional
      var fieldOrientation = this.SectorMappings[sector];
      // ReSharper restore ExceptionNotDocumentedOptional

      return fieldOrientation;
    }

    [Pure]
    public override void Transform([NotNull] SvgImage svgImage,
                                   [NotNull] Matrix sourceMatrix,
                                   [NotNull] Matrix viewMatrix,
                                   out float startX,
                                   out float startY,
                                   out float endX,
                                   out float endY,
                                   out float sourceAlignmentWidth,
                                   out float sourceAlignmentHeight)
    {
      base.Transform(svgImage,
                     sourceMatrix,
                     viewMatrix,
                     out startX,
                     out startY,
                     out endX,
                     out endY,
                     out sourceAlignmentWidth,
                     out sourceAlignmentHeight);

      var height = endY - startY;

      startY += height;
      endY += height;
    }

    [Pure]
    public virtual void GetFontSelection([NotNull] SvgTextBase svgTextBase,
                                         float fontSize,
                                         [NotNull] out string fontName,
                                         out int characterHeight,
                                         out int width)
    {
      fontName = "0";
      characterHeight = (int) Math.Max(fontSize,
                                       10f);
      width = 0;
    }

    [Pure]
    public override void Transform([NotNull] SvgTextBase svgTextBase,
                                   [NotNull] Matrix sourceMatrix,
                                   [NotNull] Matrix viewMatrix,
                                   out float startX,
                                   out float startY,
                                   out float fontSize)
    {
      base.Transform(svgTextBase,
                     sourceMatrix,
                     viewMatrix,
                     out startX,
                     out startY,
                     out fontSize);

      if (this.GetRotationSector(sourceMatrix,
                                 viewMatrix) % 2 == 0)
      {
        startY -= fontSize / this.GetLineHeightFactor(svgTextBase);
      }
      else
      {
        startX += fontSize / this.GetLineHeightFactor(svgTextBase);
      }
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
  }
}