using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace Svg.Contrib.Render
{
  [PublicAPI]
  public abstract class CompoundContainer : Container,
                                            IEnumerable<object>
  {
    protected CompoundContainer()
    {
      this.Header = new LinkedList<object>();
      this.Body = new LinkedList<object>();
      this.Footer = new LinkedList<object>();
    }

    /// <exception cref="ArgumentNullException"><paramref name="header" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="body" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="footer" /> is <see langword="null" />.</exception>
    protected CompoundContainer([NotNull] ICollection<object> header,
                                [NotNull] ICollection<object> body,
                                [NotNull] ICollection<object> footer)
    {
      this.Header = header ?? throw new ArgumentNullException(nameof(header));
      this.Body = body ?? throw new ArgumentNullException(nameof(body));
      this.Footer = footer ?? throw new ArgumentNullException(nameof(footer));
    }

    [NotNull]
    public ICollection<object> Header { get; }

    [NotNull]
    public ICollection<object> Body { get; }

    [NotNull]
    public ICollection<object> Footer { get; }

    public IEnumerator<object> GetEnumerator()
    {
      // TODO so many allocations ... darmn :beers:
      return this.Header.Concat(this.Body)
                 .Concat(this.Footer)
                 .GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return this.GetEnumerator();
    }

    public override string ToString()
    {
      return string.Join(Environment.NewLine,
                         this.OfType<string>()
                             .ToArray());
    }

    /// <exception cref="ArgumentNullException"><paramref name="encoding" /> is <see langword="null" />.</exception>
    [Pure]
    [CollectionAccess(CollectionAccessType.Read)]
    public override IEnumerable<byte> ToByteStream(Encoding encoding)
    {
      if (encoding == null)
      {
        throw new ArgumentNullException(nameof(encoding));
      }

      foreach (var line in this)
      {
        byte[] array;
        if (line is string s)
        {
          array = encoding.GetBytes(s);
        }
        else
        {
          array = line as byte[];
        }

        if (array != null)
        {
          foreach (var @byte in array)
          {
            yield return @byte;
          }
        }

        foreach (var @byte in encoding.GetBytes(Environment.NewLine))
        {
          yield return @byte;
        }
      }
    }
  }
}
