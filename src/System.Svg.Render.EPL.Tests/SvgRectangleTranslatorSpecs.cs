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
      protected object ActualUpperLine { get; set; }
      protected object ActualRightLine { get; set; }
      protected object ActualLowerLine { get; set; }
      protected object ActualLeftLine { get; set; }
      protected object ActualInnerLine { get; set; }
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
                              FillOpacity = 0f
                            };
      }

      protected override void BecauseOf()
      {
        base.BecauseOf();

        var translation = (string) this.SvgRectangleTranslator.Translate(this.SvgRectangle,
                                                                         this.SvgUnitCalculator.SourceDpi);
        var lines = translation.Split(new[]
                                      {
                                        Environment.NewLine
                                      },
                                      StringSplitOptions.None);

        this.ActualUpperLine = lines.ElementAt(0);
        this.ActualRightLine = lines.ElementAt(1);
        this.ActualLowerLine = lines.ElementAt(2);
        this.ActualLeftLine = lines.ElementAt(3);
      }

      [TestMethod]
      public void return_valid_epl_code()
      {
        Assert.AreEqual("LO10,10,100,20",
                        this.ActualUpperLine);
        Assert.AreEqual("LO110,10,20,100",
                        this.ActualRightLine);
        Assert.AreEqual("LO10,110,100,20",
                        this.ActualLowerLine);
        Assert.AreEqual("LO10,10,20,100",
                        this.ActualLeftLine);
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
                              FillOpacity = 0f
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
                              Fill = new SvgColourServer(Color.Black),
                              FillOpacity = 1f
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
                              Fill = new SvgColourServer(Color.White),
                              FillOpacity = 1f
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