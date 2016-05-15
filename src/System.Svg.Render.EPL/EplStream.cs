using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class EplStream
  {
    protected ICollection<object> InternalStream { get; } = new LinkedList<object>();

    public bool IsEmpty => this.InternalStream.Any();

    public void Add([NotNull] string s)
    {
      this.InternalStream.Add(s);
    }

    public void Add([NotNull] IEnumerable<byte> buffer)
    {
      var array = buffer.ToArray();

      this.Add(array);
    }

    public void Add([NotNull] byte[] array)
    {
      this.InternalStream.Add(array);
    }

    public void Add([NotNull] EplStream eplStream)
    {
      foreach (var line in eplStream.InternalStream)
      {
        this.InternalStream.Add(line);
      }
    }

    public byte[] ToByteArray([NotNull] Encoding encoding) => this.ToByteStream(encoding)
                                                                  .ToArray();

    public IEnumerable<byte> ToByteStream([NotNull] Encoding encoding)
    {
      foreach (var line in this.InternalStream)
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

    public override string ToString()
    {
      var result = string.Join(Environment.NewLine,
                               this.InternalStream.OfType<string>());

      return result;
    }
  }
}