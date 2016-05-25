using JetBrains.Annotations;

namespace Svg.Contrib.Render.ZPL
{
  [PublicAPI]
  public class ZplContainer : CompoundContainer<ZplStream>
  {
    public ZplContainer([NotNull] ZplStream header,
                        [NotNull] ZplStream body,
                        [NotNull] ZplStream footer)
      : base(header,
             body,
             footer) {}

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public ZplStream Combine()
    {
      var result = new ZplStream
                   {
                     this.Header,
                     this.Body,
                     this.Footer
                   };

      return result;
    }
  }
}