using System.Drawing;
using JetBrains.Annotations;
using NUnit.Framework;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable InconsistentNaming

namespace Svg.Contrib.Render.ZPL.Tests
{
  public static class SvgRectangleTranslatorSpecs
  {
    public abstract class SvgRectangleTranslatorSpecsContext : SvgElementBaseTranslatorContext
    {
      protected SvgRectangleTranslatorSpecsContext()
      {
        this.SvgRectangleTranslator = new SvgRectangleTranslator(this.ZplTransformer,
                                                                 this.ZplCommands,
                                                                 this.SvgUnitReader);
      }

      [NotNull]
      private SvgRectangleTranslator SvgRectangleTranslator { get; }

      protected SvgRectangle SvgRectangle { get; set; }

      protected override void BecauseOf()
      {
        this.SvgRectangleTranslator.Translate(this.SvgRectangle,
                                              this.Matrix,
                                              this.Matrix,
                                              this.ZplContainer);

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
      public void returns_valid_zpl_code()
      {
        Assert.AreEqual(@"^FT60,180
^GB120,150,20,B^FS",
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
      public void return_valid_zpl_code()
      {
        Assert.AreEqual(@"^FT30,120
^GB100,0,50,B^FS",
                        this.Actual);
      }
    }
  }
}