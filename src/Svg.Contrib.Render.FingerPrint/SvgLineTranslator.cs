using System;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace Svg.Contrib.Render.FingerPrint
{
  [PublicAPI]
  public class SvgLineTranslator : SvgElementTranslatorBase<SvgLine>
  {
    public SvgLineTranslator([NotNull] FingerPrintTransformer fingerPrintTransformer,
                             [NotNull] FingerPrintCommands fingerPrintCommands)
    {
      this.FingerPrintTransformer = fingerPrintTransformer;
      this.FingerPrintCommands = fingerPrintCommands;
    }

    [NotNull]
    protected FingerPrintCommands FingerPrintCommands { get; }

    [NotNull]
    protected FingerPrintTransformer FingerPrintTransformer { get; }

    public override void Translate([NotNull] SvgLine svgElement,
                                   [NotNull] Matrix matrix,
                                   [NotNull] FingerPrintContainer container)
    {
      int x;
      int y;
      int length;
      int lineWeight;
      int verticalEnd;
      float strokeWidth;
      this.GetPosition(svgElement,
                       matrix,
                       out x,
                       out y,
                       out length,
                       out lineWeight,
                       out verticalEnd,
                       out strokeWidth);

      this.AddTranslationToContainer(svgElement,
                                     x,
                                     y,
                                     verticalEnd,
                                     length,
                                     lineWeight,
                                     strokeWidth,
                                     container);
    }

    protected virtual void GetPosition([NotNull] SvgLine svgElement,
                                       [NotNull] Matrix matrix,
                                       out int x,
                                       out int y,
                                       out int horizontalLength,
                                       out int verticalLength,
                                       out int verticalEnd,
                                       out float strokeWidth)
    {
      float startX;
      float startY;
      float endX;
      float endY;
      this.FingerPrintTransformer.Transform(svgElement,
                                            matrix,
                                            out startX,
                                            out startY,
                                            out endX,
                                            out endY,
                                            out strokeWidth);

      // TODO find a good TOLERANCE
      if (Math.Abs(startY - endY) < 0.5f
          || Math.Abs(startX - endX) < 0.5f)
      {
        x = (int) startX;
        y = (int) startY;
        horizontalLength = (int) (endX - startX);
        verticalLength = (int) (endY - startY);
        verticalEnd = (int) endY;
      }
      else
      {
        throw new NotImplementedException();
        x = (int) startX;
        y = (int) startY;
        horizontalLength = (int) strokeWidth;
        verticalLength = (int) endX;
        verticalEnd = (int) endY;
      }
    }

    protected virtual void AddTranslationToContainer([NotNull] SvgLine svgElement,
                                                     int x,
                                                     int y,
                                                     int verticalEnd,
                                                     int length,
                                                     int lineWeight,
                                                     float strokeWidth,
                                                     [NotNull] FingerPrintContainer container)
    {
      if (length == 0
          || lineWeight == 0)
      {
        if (length == 0)
        {
          length = (int) strokeWidth;
        }
        if (lineWeight == 0)
        {
          lineWeight = (int) strokeWidth;
        }

        container.Body.Add(this.FingerPrintCommands.Position(x,
                                                             y));
        container.Body.Add(this.FingerPrintCommands.Line(length,
                                                         lineWeight));
      }
      else
      {
        throw new NotImplementedException();
      }
    }
  }
}