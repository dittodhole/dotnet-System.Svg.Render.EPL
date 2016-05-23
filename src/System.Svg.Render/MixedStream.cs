using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace System.Svg.Render
{
  [PublicAPI]
  public abstract class MixedStream : IEnumerable<object>
  {
    [NotNull]
    [ItemNotNull]
    private ICollection<object> InternalStream { get; } = new LinkedList<object>();

    [NotNull]
    [ItemNotNull]
    [Pure]
    [MustUseReturnValue]
    [CollectionAccess(CollectionAccessType.Read)]
    public virtual IEnumerator<object> GetEnumerator() => this.InternalStream.GetEnumerator();

    [NotNull]
    [ItemNotNull]
    [Pure]
    [MustUseReturnValue]
    [CollectionAccess(CollectionAccessType.Read)]
    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    public void Add([NotNull] string s) => this.AddElement(s);

    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    // ReSharper disable ExceptionNotDocumentedOptional
    public void Add([NotNull] IEnumerable<byte> buffer) => this.AddElement(buffer.ToArray());
    // ReSharper restore ExceptionNotDocumentedOptional

    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    public void Add([NotNull] byte[] array) => this.AddElement(array);

    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    // ReSharper disable ExceptionNotDocumentedOptional
    protected virtual void AddElement([NotNull] object obj) => this.InternalStream.Add(obj);
    // ReSharper restore ExceptionNotDocumentedOptional

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    [CollectionAccess(CollectionAccessType.Read)]
    // ReSharper disable ExceptionNotDocumentedOptional
    public byte[] ToByteArray([NotNull] Encoding encoding) => this.ToByteStream(encoding)
                                                                  .ToArray();
    // ReSharper restore ExceptionNotDocumentedOptional

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    [CollectionAccess(CollectionAccessType.Read)]
    public abstract IEnumerable<byte> ToByteStream([NotNull] Encoding encoding);

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    [CollectionAccess(CollectionAccessType.Read)]
    public override string ToString()
    {
      // ReSharper disable ExceptionNotDocumentedOptional
      var result = string.Join(Environment.NewLine,
                               this.InternalStream.OfType<string>());
      // ReSharper restore ExceptionNotDocumentedOptional

      return result;
    }
  }
}