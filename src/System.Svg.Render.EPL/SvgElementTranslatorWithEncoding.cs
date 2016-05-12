using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public abstract class SvgElementTranslatorWithEncoding<T> : SvgElementTranslatorBase<T>
    where T : SvgElement
  {
    protected SvgElementTranslatorWithEncoding([NotNull] Encoding encoding)
    {
      this.Encoding = encoding;
    }

    private Encoding Encoding { get; }

    protected IEnumerable<byte> GetBytes([NotNull] string s)
    {
      var result = this.Encoding.GetBytes(s);

      return result;
    }

    public string GetString([NotNull] IEnumerable<byte> buffer)
    {
      var bytes = buffer.ToArray();
      var result = this.Encoding.GetString(bytes);

      return result;
    }
  }
}