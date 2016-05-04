using System.Drawing;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest;

// ReSharper disable InconsistentNaming
// ReSharper disable ExceptionNotDocumented

namespace System.Svg.Render.EPL.Tests
{
  public static class SvgRectangleTranslatorSpecs
  {
    public abstract class SvgRectangleTranslatorSpecsContext : ContextSpecification
    {
      protected SvgRectangleTranslatorSpecsContext()
      {
        this.SvgUnitCalculator = new SvgUnitCalculator
                                 {
                                   UserUnitTypeSubstitution = SvgUnitType.Pixel
                                 };
        this.SvgLineTranslator = new SvgLineTranslator(this.SvgUnitCalculator);
        this.SvgRectangleTranslator = new SvgRectangleTranslator(this.SvgLineTranslator,
                                                                 this.SvgUnitCalculator);
      }

      protected SvgUnitCalculator SvgUnitCalculator { get; }
      protected SvgRectangleTranslator SvgRectangleTranslator { get; }
      protected SvgLineTranslator SvgLineTranslator { get; }
      protected SvgRectangle SvgRectangle { get; set; }
      protected object Actual { get; set; }
    }

    [TestClass]
    public class when_svg_rectangle_without_fill_is_translated : SvgRectangleTranslatorSpecsContext
    {
      protected override void Context()
      {
        base.Context();

        this.SvgRectangle = new SvgRectangle
                            {
                              X = new SvgUnit(10f),
                              Y = new SvgUnit(10f),
                              Width = new SvgUnit(100f),
                              Height = new SvgUnit(100f),
                              StrokeWidth = new SvgUnit(20f),
                              Stroke = new SvgColourServer(Color.Black),
                              Fill = new SvgColourServer(Color.Empty)
                            };
      }

      protected override void BecauseOf()
      {
        base.BecauseOf();

        this.Actual = this.SvgRectangleTranslator.Translate(this.SvgRectangle,
                                                            this.SvgUnitCalculator.SourceDpi);
      }

      [TestMethod]
      public void returns_four_epl_commands()
      {
        Assert.AreEqual(4,
                        this.GetActualLines()
                            .Count());
      }

      [TestMethod]
      public void returns_valid_epl_code_for_upper_line()
      {
        Assert.AreEqual("LO10,10,100,20",
                        this.GetActualLines()
                            .ElementAt(0));
      }

      [TestMethod]
      public void returns_valid_epl_code_for_right_line()
      {
        Assert.AreEqual("LO110,10,20,100",
                        this.GetActualLines()
                            .ElementAt(1));
      }

      [TestMethod]
      public void returns_valid_epl_code_for_lower_line()
      {
        Assert.AreEqual("LO10,110,100,20",
                        this.GetActualLines()
                            .ElementAt(2));
      }

      [TestMethod]
      public void returns_valid_epl_code_for_left_line()
      {
        Assert.AreEqual("LO10,10,20,100",
                        this.GetActualLines()
                            .ElementAt(3));
      }

      private string[] GetActualLines()
      {
        var lines = (string) this.Actual;
        var linesArray = lines.Split(new[]
                                     {
                                       Environment.NewLine
                                     },
                                     StringSplitOptions.None);

        return linesArray;
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
                              X = new SvgUnit(10f),
                              Y = new SvgUnit(10f),
                              Width = new SvgUnit(0f),
                              Height = new SvgUnit(0f),
                              StrokeWidth = new SvgUnit(20f),
                              Fill = new SvgColourServer(Color.Empty)
                            };
      }

      protected override void BecauseOf()
      {
        base.BecauseOf();

        this.Actual = this.SvgRectangleTranslator.Translate(this.SvgRectangle,
                                                            this.SvgUnitCalculator.SourceDpi);
      }

      [TestMethod]
      public void return_valid_epl_code()
      {
        Assert.AreEqual("LO10,10,20,20",
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
                              X = new SvgUnit(10f),
                              Y = new SvgUnit(10f),
                              Width = new SvgUnit(100f),
                              Height = new SvgUnit(50f),
                              //StrokeWidth = new SvgUnit(20f),
                              Stroke = new SvgColourServer(Color.Empty),
                              Fill = new SvgColourServer(Color.Black)
                            };
      }

      protected override void BecauseOf()
      {
        base.BecauseOf();

        this.Actual = this.SvgRectangleTranslator.Translate(this.SvgRectangle,
                                                            this.SvgUnitCalculator.SourceDpi);
      }

      [TestMethod]
      public void return_valid_epl_code()
      {
        Assert.AreEqual("LO10,10,100,50",
                        this.Actual);
      }
    }

    [TestClass]
    public class when_svg_rectangle_with_white_fill_is_translated : SvgRectangleTranslatorSpecsContext
    {
      protected override void Context()
      {
        base.Context();

        this.SvgRectangle = new SvgRectangle
                            {
                              X = new SvgUnit(10f),
                              Y = new SvgUnit(10f),
                              Width = new SvgUnit(100f),
                              Height = new SvgUnit(50f),
                              //StrokeWidth = new SvgUnit(20f),
                              Stroke = new SvgColourServer(Color.Empty),
                              Fill = new SvgColourServer(Color.White)
                            };
      }

      protected override void BecauseOf()
      {
        base.BecauseOf();

        this.Actual = this.SvgRectangleTranslator.Translate(this.SvgRectangle,
                                                            this.SvgUnitCalculator.SourceDpi);
      }

      [TestMethod]
      public void return_valid_epl_code()
      {
        Assert.AreEqual("LW10,10,100,50",
                        this.Actual);
      }
    }
  }
}