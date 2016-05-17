using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class EplStream : MixedStream
  {
    public virtual void Add([NotNull] EplStream eplStream)
    {
      foreach (var line in eplStream)
      {
        this.AddElement(line);
      }
    }

    [NotNull]
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