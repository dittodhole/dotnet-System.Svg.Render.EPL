using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;

namespace Svg.Contrib.Render
{
  [PublicAPI]
  public abstract class Container
  {
    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public abstract IEnumerable<byte> ToByteStream([NotNull] Encoding encoding);
  }
}