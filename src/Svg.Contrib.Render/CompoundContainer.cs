using System;
using System.Linq;
using JetBrains.Annotations;

namespace Svg.Contrib.Render
{
  [PublicAPI]
  public abstract class CompoundContainer<T> : Container
  {
    protected CompoundContainer([NotNull] T header,
                                [NotNull] T body,
                                [NotNull] T footer)
    {
      this.Header = header;
      this.Body = body;
      this.Footer = footer;
    }

    [NotNull]
    public T Header { get; }

    [NotNull]
    public T Body { get; }

    [NotNull]
    public T Footer { get; }

    public override string ToString()
    {
      var translations = new[]
                         {
                           this.Header.ToString(),
                           this.Body.ToString(),
                           this.Footer.ToString()
                         }.Where(s => !string.IsNullOrEmpty(s));
      return string.Join(Environment.NewLine,
                         translations);
    }
  }
}