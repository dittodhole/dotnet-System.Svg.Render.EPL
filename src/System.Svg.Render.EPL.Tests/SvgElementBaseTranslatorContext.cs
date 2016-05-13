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
      this.SvgUnitReader = new SvgUnitReader();
      this.Encoding = Encoding.Default;
      this.EplCommands = new EplCommands(this.Encoding);
      this.EplTransformer = new EplTransformer(this.SvgUnitReader,
                                               PrintDirection.None)
                            {
                              LineHeightFactor = 1f
                            };
      this.Matrix = this.EplTransformer.CreateViewMatrix();
    }

    [NotNull]
    protected SvgUnitReader SvgUnitReader { get; }

    [NotNull]
    protected Encoding Encoding { get; }

    [NotNull]
    protected EplCommands EplCommands { get; }

    [NotNull]
    protected EplTransformer EplTransformer { get; set; }

    [NotNull]
    protected Matrix Matrix { get; }
  }
}