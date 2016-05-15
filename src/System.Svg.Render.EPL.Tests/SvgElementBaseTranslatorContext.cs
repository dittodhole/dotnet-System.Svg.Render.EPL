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
      this.Container = new EplStream();
      this.SvgUnitReader = new SvgUnitReader();
      this.EplCommands = new EplCommands();
      this.EplTransformer = new EplTransformer(this.SvgUnitReader,
                                               PrintDirection.None)
                            {
                              LineHeightFactor = 1f
                            };
      this.Matrix = this.EplTransformer.CreateViewMatrix();
    }

    [NotNull]
    protected EplStream Container { get; }

    [NotNull]
    protected SvgUnitReader SvgUnitReader { get; }

    [NotNull]
    protected EplCommands EplCommands { get; }

    [NotNull]
    protected EplTransformer EplTransformer { get; set; }

    [NotNull]
    protected Matrix Matrix { get; }

    protected object Actual { get; private set; }

    protected override void BecauseOf()
    {
      base.BecauseOf();

      this.Actual = this.Container.ToString();
    }
  }
}