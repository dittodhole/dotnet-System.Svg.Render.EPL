using JetBrains.Annotations;

namespace Svg.Contrib.Render.EPL
{
  [PublicAPI]
  public class EplContainer : CompoundContainer<EplStream>
  {
    public EplContainer([NotNull] EplStream header,
                        [NotNull] EplStream body,
                        [NotNull] EplStream footer)
      : base(header,
             body,
             footer) {}

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public EplStream Combine()
    {
      var result = new EplStream
                   {
                     this.Header,
                     this.Body,
                     this.Footer
                   };

      return result;
    }
  }
}