using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  [PublicAPI]
  public class EplStream : MixedStream
  {
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    public virtual void Add([NotNull] EplStream eplStream)
    {
      foreach (var line in eplStream)
      {
        this.AddElement(line);
      }
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
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