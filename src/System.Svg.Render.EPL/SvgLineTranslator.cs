namespace System.Svg.Render.EPL
{
  public class SvgLineTranslator : SvgElementTranslator<SvgLine>
  {
    public override object Translate(SvgLine instance)
    {
      // TODO fix if upper/lower bound are swapped

      var startX = (int) instance.StartX.Value;
      var startY = (int) instance.StartY.Value;
      var endX = (int) instance.EndX.Value;
      var endY = (int) instance.EndY.Value;
      var strokeWidth = (int) instance.StrokeWidth.Value;

      string translation;
      if (startY == endY
          && startX < endX)
      {
        // horizontal
        var horizontalStart = startX;
        var verticalStart = startY;
        var horizontalLength = endX - endY;
        var verticalLength = strokeWidth;

        translation = $"LO{horizontalStart},{verticalStart},{horizontalLength},{verticalLength}";
      }
      else if (startX == endX
               && startY < endY)
      {
        // vertical
        var horizontalStart = startX;
        var verticalStart = startY;
        var horizontalLength = strokeWidth;
        var verticalLength = endY - startY;

        translation = $"LO{horizontalStart},{verticalStart},{horizontalLength},{verticalLength}";
      }
      else if (startX < endX
               && startY < endY)
      {
        // diagonal
        var horizontalStart = startX;
        var verticalStart = startY;
        var horizontalLength = strokeWidth;
        var verticalLength = endX;
        var verticalEnd = endY;

        translation = $"LS{horizontalStart},{verticalStart},{horizontalLength},{verticalLength},{verticalEnd}";
      }
      else
      {
        throw new NotImplementedException();
      }

      return translation;
    }
  }
}