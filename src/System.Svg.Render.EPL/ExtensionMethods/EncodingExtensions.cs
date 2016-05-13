using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL.ExtensionMethods
{
  public static class EncodingExtensions
  {
    public static string GetString([NotNull] this Encoding encoding,
                                   IEnumerable<byte> buffer)
    {
      var bytes = buffer.ToArray();
      var result = encoding.GetString(bytes);

      return result;
    }
  }
}