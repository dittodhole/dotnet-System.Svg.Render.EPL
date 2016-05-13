using System.Drawing.Drawing2D;
using System.Text;
using JetBrains.Annotations;
using UnitTest;

namespace System.Svg.Render.EPL.Tests
{
  public abstract class SvgElementBaseTranslatorContext : ContextSpecification
  {
    protected SvgElementBaseTranslatorContext()
    {
      this.Encoding = Encoding.Default;
      this.EplCommands = new EplCommands(this.Encoding);
      this.Matrix = new Matrix();
    }

    [NotNull]
    protected Encoding Encoding { get; }

    [NotNull]
    protected EplCommands EplCommands { get; }

    [NotNull]
    protected Matrix Matrix { get; }
  }
}