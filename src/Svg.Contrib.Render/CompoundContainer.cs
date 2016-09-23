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

    protected CompoundContainer([NotNull] ICollection<object> header,
                                [NotNull] ICollection<object> body,
                                [NotNull] ICollection<object> footer)
    {
      this.Header = header;
      this.Body = body;
      this.Footer = footer;
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

    [NotNull]
    [Pure]
    [CollectionAccess(CollectionAccessType.Read)]
    public override IEnumerable<byte> ToByteStream([NotNull] Encoding encoding)
    {
      foreach (var line in this)
      {
        byte[] array;
        var s = line as string;
        if (s != null)
        {
          array = encoding.GetBytes(s);
        }
        else
        {
          array = line as byte[];
        }

        if (array == null)
        {
          continue;
        }
        if (!array.Any())
        {
          continue;
        }

        foreach (var @byte in array)
        {
          yield return @byte;
        }
        foreach (var @byte in encoding.GetBytes(Environment.NewLine))
        {
          yield return @byte;
        }
      }
    }
  }
}