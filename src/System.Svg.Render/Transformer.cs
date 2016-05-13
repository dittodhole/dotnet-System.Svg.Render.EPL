﻿using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using JetBrains.Annotations;

namespace System.Svg.Render
{
  public class Transformer
  {
    public Transformer([NotNull] SvgUnitReader svgUnitReader)
    {
      this.SvgUnitReader = svgUnitReader;
    }

    [NotNull]
    protected SvgUnitReader SvgUnitReader { get; }

    public float LineHeightFactor { get; set; } = 1.25f;

    protected virtual PointF AdaptPoint(PointF point)
    {
      return point;
    }

    protected void ApplyMatrixAndAdaptPoint(float x,
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
      transformedPoint = this.AdaptPoint(transformedPoint);
      newX = transformedPoint.X;
      newY = transformedPoint.Y;
    }

    protected float GetLengthOfVector(PointF vector)
    {
      var result = Math.Sqrt(Math.Pow(vector.X,
                                      2) + Math.Pow(vector.Y,
                                                    2));

      return (int) result;
    }

    protected void ApplyMatrix(float length,
                               [NotNull] Matrix matrix,
                               out float newLength)
    {
      var vector = new PointF(length,
                              0f);

      this.ApplyMatrix(vector,
                       matrix,
                       out vector);

      newLength = this.GetLengthOfVector(vector);
    }

    protected void ApplyMatrix(PointF vector,
                               [NotNull] Matrix matrix,
                               out PointF newVector)
    {
      var vectors = new[]
                    {
                      vector
                    };

      matrix.TransformVectors(vectors);

      newVector = vectors[0];
    }

    public void Transform([NotNull] SvgLine svgLine,
                          [NotNull] Matrix matrix,
                          out float startX,
                          out float startY,
                          out float endX,
                          out float endY,
                          out float strokeWidth)
    {
      startX = this.SvgUnitReader.GetValue(svgLine.StartX);
      startY = this.SvgUnitReader.GetValue(svgLine.StartY);
      endX = this.SvgUnitReader.GetValue(svgLine.EndX);
      endY = this.SvgUnitReader.GetValue(svgLine.EndY);
      strokeWidth = this.SvgUnitReader.GetValue(svgLine.StrokeWidth);

      this.ApplyMatrixAndAdaptPoint(startX,
                                    startY,
                                    matrix,
                                    out startX,
                                    out startY);

      this.ApplyMatrixAndAdaptPoint(endX,
                                    endY,
                                    matrix,
                                    out endX,
                                    out endY);

      this.ApplyMatrix(strokeWidth,
                       matrix,
                       out strokeWidth);
    }

    public virtual void Transform([NotNull] SvgImage svgImage,
                                  [NotNull] Matrix matrix,
                                  out float startX,
                                  out float startY,
                                  out float endX,
                                  out float endY,
                                  out float sourceAlignmentWidth,
                                  out float sourceAlignmentHeight)
    {
      startX = this.SvgUnitReader.GetValue(svgImage.X);
      startY = this.SvgUnitReader.GetValue(svgImage.Y);
      sourceAlignmentWidth = this.SvgUnitReader.GetValue(svgImage.Width);
      sourceAlignmentHeight = this.SvgUnitReader.GetValue(svgImage.Height);
      endX = startX + sourceAlignmentWidth;
      endY = startY + sourceAlignmentHeight;

      this.ApplyMatrixAndAdaptPoint(startX,
                                    startY,
                                    matrix,
                                    out startX,
                                    out startY);

      this.ApplyMatrix(sourceAlignmentWidth,
                       matrix,
                       out sourceAlignmentWidth);
      this.ApplyMatrix(sourceAlignmentHeight,
                       matrix,
                       out sourceAlignmentHeight);

      this.ApplyMatrixAndAdaptPoint(endX,
                                    endY,
                                    matrix,
                                    out endX,
                                    out endY);
    }

    public void Transform([NotNull] SvgRectangle svgRectangle,
                          [NotNull] Matrix matrix,
                          bool outerPoints,
                          out float startX,
                          out float startY,
                          out float endX,
                          out float endY,
                          out float strokeWidth)
    {
      startX = this.SvgUnitReader.GetValue(svgRectangle.X);
      endX = startX + this.SvgUnitReader.GetValue(svgRectangle.Width);
      startY = this.SvgUnitReader.GetValue(svgRectangle.Y);
      endY = startY + this.SvgUnitReader.GetValue(svgRectangle.Height);
      strokeWidth = this.SvgUnitReader.GetValue(svgRectangle.StrokeWidth);

      if (outerPoints)
      {
        startX -= strokeWidth / 2f;
        startY -= strokeWidth / 2f;
        endX += strokeWidth / 2f;
        endY += strokeWidth / 2f;
      }

      this.ApplyMatrixAndAdaptPoint(startX,
                                    startY,
                                    matrix,
                                    out startX,
                                    out startY);

      this.ApplyMatrixAndAdaptPoint(endX,
                                    endY,
                                    matrix,
                                    out endX,
                                    out endY);

      this.ApplyMatrix(strokeWidth,
                       matrix,
                       out strokeWidth);
    }

    public void Transform([NotNull] SvgTextBase svgTextBase,
                          [NotNull] Matrix matrix,
                          out float x,
                          out float y,
                          out float fontSize)
    {
      x = this.SvgUnitReader.GetValue(svgTextBase.X.First());
      y = this.SvgUnitReader.GetValue(svgTextBase.Y.First());
      fontSize = this.SvgUnitReader.GetValue(svgTextBase.FontSize);

      y -= fontSize / this.LineHeightFactor;

      this.ApplyMatrixAndAdaptPoint(x,
                                    y,
                                    matrix,
                                    out x,
                                    out y);
    }
  }
}