using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace Svg.Contrib.Render.EPL
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
          // ReSharper disable ExceptionNotDocumentedOptional
          array = encoding.GetBytes(s);
          // ReSharper restore ExceptionNotDocumentedOptional
        }
        else
        {
          array = line as byte[];
        }

        if (array == null)
        {
          continue;
        }
        // ReSharper disable ExceptionNotDocumentedOptional
        if (!array.Any())
          // ReSharper restore ExceptionNotDocumentedOptional
        {
          continue;
        }

        foreach (var @byte in array)
        {
          yield return @byte;
        }

        // ReSharper disable ExceptionNotDocumentedOptional
        foreach (var @byte in encoding.GetBytes(Environment.NewLine))
          // ReSharper restore ExceptionNotDocumentedOptional
        {
          yield return @byte;
        }
      }
    }
  }
}