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
        this.SvgLineTranslator = new SvgLineTranslator();
      }

      protected SvgLineTranslator SvgLineTranslator { get; set; }
      protected SvgLine SvgLine { get; set; }
      protected object Actual { get; set; }

      protected override void BecauseOf()
      {
        this.Actual = this.SvgLineTranslator.Translate(this.SvgLine);
      }
    }

    [TestClass]
    public class when_horizontal_sepcs_given : SvgLineTranslatorSpecsContext
    {
      protected override void Context()
      {
        this.SvgLine = new SvgLine
                       {
                         StartX = new SvgUnit(50f),
                         StartY = new SvgUnit(200f),
                         EndX = new SvgUnit(600f),
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
  }
}