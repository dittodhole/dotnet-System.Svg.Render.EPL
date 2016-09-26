using System.Drawing.Drawing2D;
using JetBrains.Annotations;
using UnitTest;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Svg.Contrib.Render.ZPL.Tests
{
  public abstract class SvgElementBaseTranslatorContext : ContextSpecification
  {
    protected SvgElementBaseTranslatorContext()
    {
      this.ZplContainer = new ZplContainer();
      this.SvgUnitReader = new SvgUnitReader();
      this.ZplCommands = new ZplCommands();
      this.ZplTransformer = new ZplTransformer(this.SvgUnitReader);
      this.Matrix = new Matrix();
    }

    [NotNull]
    protected ZplContainer ZplContainer { get; }

    [NotNull]
    protected SvgUnitReader SvgUnitReader { get; }

    [NotNull]
    protected ZplCommands ZplCommands { get; }

    [NotNull]
    protected ZplTransformer ZplTransformer { get; set; }

    [NotNull]
    protected Matrix Matrix { get; }

    protected object Actual { get; private set; }

    protected override void BecauseOf()
    {
      base.BecauseOf();

      this.Actual = this.ZplContainer.ToString();
    }
  }
}
