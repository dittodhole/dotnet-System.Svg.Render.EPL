﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace Svg.Contrib.Render.ZPL
{
  [PublicAPI]
  public class SvgRectangleTranslator : SvgElementTranslatorBase<ZplContainer, SvgRectangle>
  {
    /// <exception cref="ArgumentNullException"><paramref name="zplTransformer" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="zplCommands" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="svgUnitReader" /> is <see langword="null" />.</exception>
    public SvgRectangleTranslator([NotNull] ZplTransformer zplTransformer,
                                  [NotNull] ZplCommands zplCommands,
                                  [NotNull] SvgUnitReader svgUnitReader)
    {
      if (zplTransformer == null)
      {
        throw new ArgumentNullException(nameof(zplTransformer));
      }
      if (zplCommands == null)
      {
        throw new ArgumentNullException(nameof(zplCommands));
      }
      if (svgUnitReader == null)
      {
        throw new ArgumentNullException(nameof(svgUnitReader));
      }
      this.ZplTransformer = zplTransformer;
      this.ZplCommands = zplCommands;
      this.SvgUnitReader = svgUnitReader;
    }

    [NotNull]
    protected ZplTransformer ZplTransformer { get; }

    [NotNull]
    protected ZplCommands ZplCommands { get; }

    [NotNull]
    protected SvgUnitReader SvgUnitReader { get; }

    /// <exception cref="ArgumentNullException"><paramref name="svgRectangle" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="zplContainer" /> is <see langword="null" />.</exception>
    public override void Translate([NotNull] SvgRectangle svgRectangle,
                                   [NotNull] Matrix sourceMatrix,
                                   [NotNull] Matrix viewMatrix,
                                   [NotNull] ZplContainer zplContainer)
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
      if (zplContainer == null)
      {
        throw new ArgumentNullException(nameof(zplContainer));
      }

      if (svgRectangle.Fill != SvgPaintServer.None
          && (svgRectangle.Fill as SvgColourServer)?.Colour != Color.White)
      {
        this.TranslateFilledBox(svgRectangle,
                                sourceMatrix,
                                viewMatrix,
                                zplContainer);
      }
      else if (svgRectangle.Stroke != SvgPaintServer.None)
      {
        this.TranslateBox(svgRectangle,
                          sourceMatrix,
                          viewMatrix,
                          zplContainer);
      }
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgRectangle" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="zplContainer" /> is <see langword="null" />.</exception>
    protected virtual void TranslateFilledBox([NotNull] SvgRectangle svgRectangle,
                                              [NotNull] Matrix sourceMatrix,
                                              [NotNull] Matrix viewMatrix,
                                              [NotNull] ZplContainer zplContainer)
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
      if (zplContainer == null)
      {
        throw new ArgumentNullException(nameof(zplContainer));
      }

      // TODO fix this! square gets rendered ...

      var startX = this.SvgUnitReader.GetValue(svgRectangle,
                                               svgRectangle.X);
      var startY = this.SvgUnitReader.GetValue(svgRectangle,
                                               svgRectangle.Y);
      var endX = startX + this.SvgUnitReader.GetValue(svgRectangle,
                                                      svgRectangle.Width);
      var endY = startY + this.SvgUnitReader.GetValue(svgRectangle,
                                                      svgRectangle.Height);

      var svgLine = new SvgLine
                    {
                      Color = svgRectangle.Color,
                      Stroke = SvgPaintServer.None,
                      StrokeWidth = svgRectangle.StrokeWidth,
                      StartX = startX,
                      StartY = startY,
                      EndX = endX,
                      EndY = endY
                    };

      float strokeWidth;
      this.ZplTransformer.Transform(svgLine,
                                    sourceMatrix,
                                    viewMatrix,
                                    out startX,
                                    out startY,
                                    out endX,
                                    out endY,
                                    out strokeWidth);

      var horizontalStart = (int) startX;
      var verticalStart = (int) endY;
      int width;
      int height;
      int thickness;

      var sector = this.ZplTransformer.GetRotationSector(sourceMatrix,
                                                         viewMatrix);
      if (sector % 2 == 0)
      {
        width = (int) (endX - startX);
        height = 0;
        thickness = (int) (endY - startY);
      }
      else
      {
        width = 0;
        height = (int) (endY - startY);
        thickness = (int) (endX - startX);
      }

      zplContainer.Body.Add(this.ZplCommands.FieldTypeset(horizontalStart,
                                                          verticalStart));
      zplContainer.Body.Add(this.ZplCommands.GraphicBox(width,
                                                        height,
                                                        thickness,
                                                        LineColor.Black));
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgRectangle" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="zplContainer" /> is <see langword="null" />.</exception>
    protected virtual void TranslateBox([NotNull] SvgRectangle svgRectangle,
                                        [NotNull] Matrix sourceMatrix,
                                        [NotNull] Matrix viewMatrix,
                                        [NotNull] ZplContainer zplContainer)
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
      if (zplContainer == null)
      {
        throw new ArgumentNullException(nameof(zplContainer));
      }

      float startX;
      float endX;
      float startY;
      float endY;
      float strokeWidth;
      this.ZplTransformer.Transform(svgRectangle,
                                    sourceMatrix,
                                    viewMatrix,
                                    out startX,
                                    out startY,
                                    out endX,
                                    out endY,
                                    out strokeWidth);

      var horizontalStart = (int) startX;
      var verticalStart = (int) endY;
      var width = (int) (endX - startX);
      var height = (int) (endY - startY);
      var thickness = (int) strokeWidth;

      zplContainer.Body.Add(this.ZplCommands.FieldTypeset(horizontalStart,
                                                          verticalStart));
      zplContainer.Body.Add(this.ZplCommands.GraphicBox(width,
                                                        height,
                                                        thickness,
                                                        LineColor.Black));
    }
  }
}
