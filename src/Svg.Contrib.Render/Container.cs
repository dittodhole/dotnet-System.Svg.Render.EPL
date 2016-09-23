using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;

namespace Svg.Contrib.Render
{
  [PublicAPI]
  public abstract class Container
  {
    /// <exception cref="ArgumentNullException"><paramref name="encoding" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    public abstract IEnumerable<byte> ToByteStream([NotNull] Encoding encoding);
  }
}
