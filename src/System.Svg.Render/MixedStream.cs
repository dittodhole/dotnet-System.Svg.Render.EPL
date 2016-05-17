using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace System.Svg.Render
{
  public abstract class MixedStream : IEnumerable<object>
  {
    private ICollection<object> InternalStream { get; } = new LinkedList<object>();

    public virtual bool IsEmpty => !this.InternalStream.Any();

    public virtual IEnumerator<object> GetEnumerator()
    {
      return this.InternalStream.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return this.GetEnumerator();
    }

    public void Add([NotNull] string s)
    {
      this.AddElement(s);
    }

    public void Add([NotNull] IEnumerable<byte> buffer)
    {
      var array = buffer.ToArray();

      this.AddElement(array);
    }

    public void Add([NotNull] byte[] array)
    {
      this.AddElement(array);
    }

    protected virtual void AddElement([NotNull] object obj)
    {
      this.InternalStream.Add(obj);
    }

    [NotNull]
    public byte[] ToByteArray([NotNull] Encoding encoding) => this.ToByteStream(encoding)
                                                                  .ToArray();

    [NotNull]
    public abstract IEnumerable<byte> ToByteStream([NotNull] Encoding encoding);

    [NotNull]
    public override string ToString()
    {
      var result = string.Join(Environment.NewLine,
                               this.InternalStream.OfType<string>());

      return result;
    }
  }
}