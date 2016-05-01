using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest;

namespace System.Svg.Render.EPL.Tests
{
  public static class SvgUnitCalculatorSpecs
  {
    public class SvgUnitCalculatorFailingSpecsContext : ContextSpecification
    {
      public SvgUnitCalculatorFailingSpecsContext()
      {
        this.SvgUnitCalculator = new SvgUnitCalculator();
      }

      protected SvgUnitCalculator SvgUnitCalculator { get; }
      protected Exception Actual { get; set; }
    }

    public class SvgUnitCalculatorSpecsContext : ContextSpecification
    {
      public SvgUnitCalculatorSpecsContext()
      {
        this.SvgUnitCalculator = new SvgUnitCalculator();
      }

      protected SvgUnitCalculator SvgUnitCalculator { get; }
      protected float Actual { get; set; }
    }

    [TestClass]
    public class when_doing_something_with_different_svg_unit_types : SvgUnitCalculatorFailingSpecsContext
    {
      protected override void BecauseOf()
      {
        base.BecauseOf();

        try
        {
          this.SvgUnitCalculator.CheckSvgUnitType(new SvgUnit(SvgUnitType.Centimeter,
                                                              0f),
                                                  new SvgUnit(SvgUnitType.Em,
                                                              0f));
        }
        catch (ArgumentException argumentException)
        {
          this.Actual = argumentException;
        }
      }

      [TestMethod]
      public void no_svg_unit_should_be_returned()
      {
        Assert.IsInstanceOfType(this.Actual,
                                typeof(ArgumentException));
      }
    }

    [TestClass]
    public class when_adding_svg_units : SvgUnitCalculatorSpecsContext
    {
      protected override void BecauseOf()
      {
        base.BecauseOf();

        var svgUnit1 = new SvgUnit(100f);
        var svgUnit2 = new SvgUnit(300f);
        var svgUnit3 = this.SvgUnitCalculator.Add(svgUnit1,
                                                  svgUnit2);
        this.Actual = this.SvgUnitCalculator.GetValue(svgUnit3);
      }

      [TestMethod]
      public void the_values_should_be_added_correctly()
      {
        Assert.AreEqual(this.Actual,
                        400f);
      }
    }

    [TestClass]
    public class when_substracting_svg_units : SvgUnitCalculatorSpecsContext
    {
      protected override void BecauseOf()
      {
        base.BecauseOf();

        var svgUnit1 = new SvgUnit(300f);
        var svgUnit2 = new SvgUnit(100f);
        var svgUnit3 = this.SvgUnitCalculator.Substract(svgUnit1,
                                                        svgUnit2);
        this.Actual = this.SvgUnitCalculator.GetValue(svgUnit3);
      }

      [TestMethod]
      public void the_values_should_be_substracted_correctly()
      {
        Assert.AreEqual(this.Actual,
                        200f);
      }
    }
  }
}