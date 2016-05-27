using System.Drawing;
using JetBrains.Annotations;
using NUnit.Framework;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable InconsistentNaming

namespace Svg.Contrib.Render.EPL.Tests
{
  public static class SvgRectangleTranslatorSpecs
  {
    public abstract class SvgRectangleTranslatorSpecsContext : SvgElementBaseTranslatorContext
    {
      protected SvgRectangleTranslatorSpecsContext()
      {
        this.SvgRectangleTranslator = new SvgRectangleTranslator(this.EplTransformer,
                                                                 this.EplCommands,
                                                                 this.SvgUnitReader);
      }

      [NotNull]
      private SvgRectangleTranslator SvgRectangleTranslator { get; }

      protected SvgRectangle SvgRectangle { get; set; }

      protected override void BecauseOf()
      {
        this.SvgRectangleTranslator.Translate(this.SvgRectangle,
                                              this.Matrix,
                                              this.EplContainer);

        base.BecauseOf();
      }
    }

    [TestFixture]
    public class when_svg_rectangle_without_fill_is_translated : SvgRectangleTranslatorSpecsContext
    {
      protected override void Context()
      {
        base.Context();

        this.SvgRectangle = new SvgRectangle
                            {
                              X = new SvgUnit(70f),
                              Y = new SvgUnit(40f),
                              Width = new SvgUnit(100f),
                              Height = new SvgUnit(130f),
                              StrokeWidth = new SvgUnit(20f),
                              Stroke = new SvgColourServer(Color.Black),
                              Fill = SvgPaintServer.None
                            };
      }

      [Test]
      public void returns_valid_epl_code()
      {
        Assert.AreEqual("X65,35,20,175,175",
                        this.Actual);
      }
    }

    [TestFixture]
    public class when_svg_rectangle_with_black_fill_is_translated : SvgRectangleTranslatorSpecsContext
    {
      protected override void Context()
      {
        base.Context();

        this.SvgRectangle = new SvgRectangle
                            {
                              X = new SvgUnit(30f),
                              Y = new SvgUnit(70f),
                              Width = new SvgUnit(100f),
                              Height = new SvgUnit(50f),
                              Stroke = SvgPaintServer.None,
                              Fill = new SvgColourServer(Color.Black)
                            };
      }

      [Test]
      public void return_valid_epl_code()
      {
        Assert.AreEqual("LO29,69,101,51",
                        this.Actual);
      }
    }
  }
}