using System.Drawing.Drawing2D;
using JetBrains.Annotations;
using UnitTest;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global

namespace System.Svg.Render.EPL.Tests
{
  public abstract class SvgElementBaseTranslatorContext : ContextSpecification
  {
    protected SvgElementBaseTranslatorContext()
    {
      this.Container = new Container<EplStream>(new EplStream(),
                                                new EplStream(),
                                                new EplStream());
      this.SvgUnitReader = new SvgUnitReader();
      this.EplCommands = new EplCommands();
      this.EplTransformer = new EplTransformer(this.SvgUnitReader);
      this.Matrix = new Matrix();
    }

    [NotNull]
    protected Container<EplStream> Container { get; }

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