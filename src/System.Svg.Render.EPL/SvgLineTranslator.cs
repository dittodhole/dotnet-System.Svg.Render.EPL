using System.Drawing;

namespace System.Svg.Render.EPL
{
  public class SvgLineTranslator : SvgElementTranslator<SvgLine>
  {
    public SvgLineTranslator(SvgUnitCalculator svgUnitCalculator)
    {
      if (svgUnitCalculator == null)
      {
        // TODO add documentation
        throw new ArgumentNullException(nameof(svgUnitCalculator));
      }

      this.SvgUnitCalculator = svgUnitCalculator;
    }

    private SvgUnitCalculator SvgUnitCalculator { get; }

    public override object Translate(SvgLine instance)
    {
      // TODO fix if upper/lower bound are swapped
      // TODO implement device-specific values

      var startX = this.SvgUnitCalculator.GetValue(instance.StartX);
      var startY = this.SvgUnitCalculator.GetValue(instance.StartY);
      var endX = this.SvgUnitCalculator.GetValue(instance.EndX);
      var endY = this.SvgUnitCalculator.GetValue(instance.EndY);
      var strokeWidth = this.SvgUnitCalculator.GetValue(instance.StrokeWidth);

      string translation;
      if (startY == endY
          || startX == endX)
      {
        // horizontal or vertical
        string command;

        var stroke = instance.Stroke as SvgColourServer;
        if (stroke?.Colour == Color.White)
        {
          command = "LW";
        }
        else
        {
          command = "LO";
        }

        if (startX < endX)
        {
          // horizontal
          var horizontalStart = startX;
          var verticalStart = startY;
          var horizontalLength = endX - startX;
          if (horizontalLength == 0)
          {
            horizontalLength = strokeWidth;
          }
          var verticalLength = strokeWidth;

          translation = $"{command}{horizontalStart},{verticalStart},{horizontalLength},{verticalLength}";
        }
        else if (startY < endY)
        {
          // vertical
          var horizontalStart = startX;
          var verticalStart = startY;
          var horizontalLength = strokeWidth;
          var verticalLength = endY - startY;
          if (verticalLength == 0)
          {
            verticalLength = strokeWidth;
          }

          translation = $"{command}{horizontalStart},{verticalStart},{horizontalLength},{verticalLength}";
        }
        else
        {
          // TODO add documentation
          throw new NotImplementedException();
        }
      }
      else if (startX <= endX
               && startY <= endY)
      {
        // diagonal
        var command = "LS";

        var horizontalStart = startX;
        var verticalStart = startY;
        var horizontalLength = strokeWidth;
        var verticalLength = endX;
        var verticalEnd = endY;

        translation = $"{command}{horizontalStart},{verticalStart},{horizontalLength},{verticalLength},{verticalEnd}";
      }
      else
      {
        // TODO add documentation
        throw new NotImplementedException();
      }

      return translation;
    }
  }
}