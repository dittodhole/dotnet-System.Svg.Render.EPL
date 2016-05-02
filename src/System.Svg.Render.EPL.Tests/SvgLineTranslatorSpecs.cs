using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest;

// ReSharper disable InconsistentNaming

namespace System.Svg.Render.EPL.Tests
{
  public static class SvgLineTranslatorSpecs
  {
    public abstract class SvgLineTranslatorSpecsContext : ContextSpecification
    {
      protected SvgLineTranslatorSpecsContext()
      {
        this.SvgUnitCalculator = new SvgUnitCalculator
                                 {
                                   UserUnitTypeSubstitution = SvgUnitType.Pixel
                                 };
        this.SvgLineTranslator = new SvgLineTranslator(this.SvgUnitCalculator);
      }

      private SvgUnitCalculator SvgUnitCalculator { get; }
      private SvgLineTranslator SvgLineTranslator { get; }
      protected SvgLine SvgLine { get; set; }
      protected object Actual { get; set; }

      protected override void BecauseOf()
      {
        base.BecauseOf();

        this.Actual = this.SvgLineTranslator.Translate(this.SvgLine,
                                                       this.SvgUnitCalculator.SourceDpi);
      }
    }

    [TestClass]
    public class when_horizontal_svg_line_is_translated : SvgLineTranslatorSpecsContext
    {
      protected override void Context()
      {
        base.Context();

        this.SvgLine = new SvgLine
                       {
                         StartX = new SvgUnit(50f),
                         StartY = new SvgUnit(200f),
                         EndX = new SvgUnit(450f),
                         EndY = new SvgUnit(200f),
                         StrokeWidth = new SvgUnit(20f)
                       };
      }

      [TestMethod]
      public void return_valid_epl_code()
      {
        Assert.AreEqual("LO50,200,400,20",
                        this.Actual);
      }
    }

    [TestClass]
    public class when_vertical_svg_line_is_translated : SvgLineTranslatorSpecsContext
    {
      protected override void Context()
      {
        base.Context();

        this.SvgLine = new SvgLine
                       {
                         StartX = new SvgUnit(200f),
                         StartY = new SvgUnit(50f),
                         EndX = new SvgUnit(200f),
                         EndY = new SvgUnit(450f),
                         StrokeWidth = new SvgUnit(20f)
                       };
      }

      [TestMethod]
      public void return_valid_epl_code()
      {
        Assert.AreEqual("LO200,50,20,400",
                        this.Actual);
      }
    }

    [TestClass]
    public class when_diagonal_svg_line_is_translated : SvgLineTranslatorSpecsContext
    {
      protected override void Context()
      {
        base.Context();

        this.SvgLine = new SvgLine
                       {
                         StartX = new SvgUnit(10f),
                         StartY = new SvgUnit(10f),
                         EndX = new SvgUnit(200f),
                         EndY = new SvgUnit(200f),
                         StrokeWidth = new SvgUnit(20f)
                       };
      }

      [TestMethod]
      public void return_valid_epl_code()
      {
        Assert.AreEqual("LS10,10,20,200,200",
                        this.Actual);
      }
    }

    [TestClass]
    public class when_svg_line_with_white_stroke_color_is_translated : SvgLineTranslatorSpecsContext
    {
      protected override void Context()
      {
        base.Context();

        this.SvgLine = new SvgLine
                       {
                         StartX = new SvgUnit(10f),
                         StartY = new SvgUnit(10f),
                         EndX = new SvgUnit(10f),
                         EndY = new SvgUnit(210f),
                         StrokeWidth = new SvgUnit(20f),
                         Color = new SvgColourServer(Color.White)
                       };
      }

      [TestMethod]
      public void return_valid_epl_code()
      {
        Assert.AreEqual("LW10,10,20,200",
                        this.Actual);
      }
    }

    [TestClass]
    public class when_svg_line_with_partly_swapped_coordinates_is_translated : SvgLineTranslatorSpecsContext
    {
      protected override void Context()
      {
        base.Context();

        this.SvgLine = new SvgLine
                       {
                         StartX = new SvgUnit(0f),
                         StartY = new SvgUnit(0f),
                         EndX = new SvgUnit(-10f),
                         EndY = new SvgUnit(0f)
                       };
      }

      [TestMethod]
      public void return_no_epl_code()
      {
        Assert.AreEqual(string.Empty,
                        this.Actual);
      }
    }

    [TestClass]
    public class when_svg_line_with_completely_swapped_coordinates_is_translated : SvgLineTranslatorSpecsContext
    {
      protected override void Context()
      {
        base.Context();

        this.SvgLine = new SvgLine
                       {
                         StartX = new SvgUnit(0f),
                         StartY = new SvgUnit(-10f),
                         EndX = new SvgUnit(-10f),
                         EndY = new SvgUnit(0f)
                       };
      }

      [TestMethod]
      public void return_no_epl_code()
      {
        Assert.AreEqual(string.Empty,
                        this.Actual);
      }
    }

    // TODO add test for TranslateHorizontalLine
    // TODO add test for TranslateVerticalLine
    // TODO add test for TranslateDiagonal
  }
}