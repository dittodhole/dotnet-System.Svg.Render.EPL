using System.Drawing.Drawing2D;
using JetBrains.Annotations;
using UnitTest;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Svg.Contrib.Render.EPL.Tests
{
  public abstract class SvgElementBaseTranslatorContext : ContextSpecification
  {
    protected SvgElementBaseTranslatorContext()
    {
      this.EplContainer = new EplContainer(new EplStream(),
                                           new EplStream(),
                                           new EplStream());
      this.SvgUnitReader = new SvgUnitReader();
      this.EplCommands = new EplCommands();
      this.EplTransformer = new EplTransformer(this.SvgUnitReader);
      this.Matrix = new Matrix();
    }

    [NotNull]
    protected EplContainer EplContainer { get; }

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

      this.Actual = this.EplContainer.ToString();
    }
  }
}