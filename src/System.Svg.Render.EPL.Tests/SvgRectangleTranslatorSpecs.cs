using System.Drawing;
using System.Drawing.Drawing2D;
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
        this.SvgUnitCalculator = new SvgUnitCalculator(PrintDirection.None)
                                 {
                                   UserUnitTypeSubstitution = SvgUnitType.Pixel
                                 };
        this.SvgRectangleTranslator = new SvgRectangleTranslator(this.SvgUnitCalculator);
      }

      protected SvgUnitCalculator SvgUnitCalculator { get; }
      protected SvgRectangleTranslator SvgRectangleTranslator { get; }
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

        object translation;
        if (this.SvgRectangleTranslator.TryTranslate(this.SvgRectangle,
                                                     new Matrix(),
                                                     this.SvgUnitCalculator.SourceDpi,
                                                     out translation))
        {
          this.Actual = translation;
        }
      }

      [TestMethod]
      public void returns_valid_epl_code()
      {
        Assert.AreEqual("X10,10,20,110,110",
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

        object translation;
        if (this.SvgRectangleTranslator.TryTranslate(this.SvgRectangle,
                                                     new Matrix(),
                                                     this.SvgUnitCalculator.SourceDpi,
                                                     out translation))
        {
          this.Actual = translation;
        }
      }

      [TestMethod]
      public void return_valid_epl_code()
      {
        Assert.AreEqual("X10,10,20,10,10",
                        this.Actual);
      }
    }

    /*
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

        object translation;
        if (this.SvgRectangleTranslator.TryTranslate(this.SvgRectangle,
                                                     new Matrix(),
                                                     this.SvgUnitCalculator.SourceDpi,
                                                     out translation))
        {
          this.Actual = translation;
        }
      }

      [TestMethod]
      public void return_valid_epl_code()
      {
        Assert.AreEqual("LO10,10,100,50",
                        this.Actual);
      }
    }
    */
  }
}