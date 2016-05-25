using Svg;
using JetBrains.Annotations;

namespace Svg.Contrib.Render
{
  [PublicAPI]
  public class SvgUnitReader
  {
    public SvgUnitReader(float sourceDpi)
    {
      this.SourceDpi = sourceDpi;
    }

    protected float SourceDpi { get; }

    [Pure]
    [MustUseReturnValue]
    public virtual float GetValue([NotNull] SvgElement svgElement,
                                  SvgUnit svgUnit)
    {
      var result = svgUnit.Value;

      return result;
    }
  }
}