using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest;

namespace System.Svg.Render.EPL.Tests
{
  public static class SvgLineTranslatorSpecs
  {
    public class SvgLineTranslatorSpecsContext : ContextSpecification
    {
      public SvgLineTranslatorSpecsContext()
      {
        var svgUnitCalculator = new SvgUnitCalculator();

        this.SvgLineTranslator = new SvgLineTranslator(svgUnitCalculator);
      }

      protected SvgLineTranslator SvgLineTranslator { get; }
      protected SvgLine SvgLine { get; set; }
      protected object Actual { get; set; }

      protected override void BecauseOf()
      {
        base.BecauseOf();

        this.Actual = this.SvgLineTranslator.Translate(this.SvgLine);
      }
    }

    [TestClass]
    public class when_horizontal_sepcs_given : SvgLineTranslatorSpecsContext
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
      public void return_valid_EPL_code()
      {
        Assert.AreEqual("LO50,200,400,20",
                        this.Actual);
      }
    }

    [TestClass]
    public class when_vertical_sepcs_given : SvgLineTranslatorSpecsContext
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
      public void return_valid_EPL_code()
      {
        Assert.AreEqual("LO200,50,20,400",
                        this.Actual);
      }
    }

    [TestClass]
    public class when_diagonal_specs_given : SvgLineTranslatorSpecsContext
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
      public void return_invalid_EPL_code()
      {
        Assert.AreEqual("LS10,10,20,200,200",
                        this.Actual);
      }
    }

    [TestClass]
    public class when_stroke_is_white : SvgLineTranslatorSpecsContext
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
      public void return_invalid_EPL_code()
      {
        Assert.AreEqual("LW10,10,20,200",
                        this.Actual);
      }
    }

    [TestClass]
    public class when_partially_swapped_coordinates_are_given : SvgLineTranslatorSpecsContext
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
      public void returns_invalid_epl_code()
      {
        Assert.AreEqual(string.Empty,
                        this.Actual);
      }
    }

    [TestClass]
    public class when_completely_swapped_coordinates_are_given : SvgLineTranslatorSpecsContext
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
      public void returns_invalid_epl_code()
      {
        Assert.AreEqual(string.Empty,
                        this.Actual);
      }
    }
  }
}