using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest;

// ReSharper disable InconsistentNaming
// ReSharper disable ExceptionNotDocumented

namespace System.Svg.Render.EPL.Tests
{
  public static class SvgRectangleTranslatorSpecs
  {
    public abstract class SvgRectangleTranslatorSpecsContext : SvgElementBaseTranslatorContext
    {
      protected SvgRectangleTranslatorSpecsContext()
      {
        this.SvgUnitCalculator = new SvgUnitCalculator(PrintDirection.None);
        this.SvgLineTranslator = new SvgLineTranslator(this.SvgUnitCalculator,
                                                       this.EplCommands);
        this.SvgRectangleTranslator = new SvgRectangleTranslator(this.SvgUnitCalculator,
                                                                 this.SvgLineTranslator,
                                                                 Encoding.Default);
      }

      [NotNull]
      private SvgUnitCalculator SvgUnitCalculator { get; }

      [NotNull]
      private SvgLineTranslator SvgLineTranslator { get; }

      [NotNull]
      private SvgRectangleTranslator SvgRectangleTranslator { get; }

      protected SvgRectangle SvgRectangle { get; set; }
      protected object Actual { get; set; }

      protected override void BecauseOf()
      {
        base.BecauseOf();

        var translation = this.SvgRectangleTranslator.Translate(this.SvgRectangle,
                                                                this.Matrix);

        this.Actual = this.SvgRectangleTranslator.GetString(translation);
      }
    }

    [TestClass]
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

      [TestMethod]
      public void returns_valid_epl_code()
      {
        Assert.AreEqual("X60,30,20,180,180",
                        this.Actual);
      }
    }

    [TestClass]
    public class when_svg_rectangle_which_is_a_point_is_translated : SvgRectangleTranslatorSpecsContext
    {
      protected override void Context()
      {
        base.Context();

        this.SvgRectangle = new SvgRectangle
                            {
                              X = new SvgUnit(50f),
                              Y = new SvgUnit(90f),
                              Width = new SvgUnit(0f),
                              Height = new SvgUnit(0f),
                              StrokeWidth = new SvgUnit(20f),
                              Fill = SvgPaintServer.None
                            };
      }

      [TestMethod]
      public void return_valid_epl_code()
      {
        Assert.AreEqual("X40,80,20,60,100",
                        this.Actual);
      }
    }

    [TestClass]
    public class when_svg_rectangle_with_black_fill_is_translated : SvgRectangleTranslatorSpecsContext
    {
      protected override void Context()
      {
        base.Context();

        this.SvgRectangle = new SvgRectangle
                            {
                              X = new SvgUnit(30f),
                              Y = new SvgUnit(50f),
                              Width = new SvgUnit(100f),
                              Height = new SvgUnit(50f),
                              Stroke = SvgPaintServer.None,
                              Fill = new SvgColourServer(Color.Black)
                            };
      }

      [TestMethod]
      public void return_valid_epl_code()
      {
        Assert.AreEqual("LO30,100,100,50",
                        this.Actual);
      }
    }
  }
}