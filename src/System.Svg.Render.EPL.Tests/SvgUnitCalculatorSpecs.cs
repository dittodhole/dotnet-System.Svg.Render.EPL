using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest;
// ReSharper disable InconsistentNaming

namespace System.Svg.Render.EPL.Tests
{
  public static class SvgUnitCalculatorSpecs
  {
    public abstract class SvgUnitCalculatorFailingSpecsContext : ContextSpecification
    {
      protected SvgUnitCalculatorFailingSpecsContext()
      {
        this.SvgUnitCalculator = new SvgUnitCalculator();
      }

      protected SvgUnitCalculator SvgUnitCalculator { get; }
      protected object Actual { get; set; }
    }

    public abstract class SvgUnitCalculatorSpecsContext : ContextSpecification
    {
      protected SvgUnitCalculatorSpecsContext()
      {
        this.SvgUnitCalculator = new SvgUnitCalculator();
      }

      protected SvgUnitCalculator SvgUnitCalculator { get; }
      protected object Actual { get; set; }
    }

    [TestClass]
    public class when_two_svg_unit_instances_with_different_types_are_the_basis_of_math_methods : SvgUnitCalculatorFailingSpecsContext
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
    public class when_two_svg_unit_instances_are_added : SvgUnitCalculatorSpecsContext
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
      public void the_result_should_be_correct()
      {
        Assert.AreEqual(this.Actual,
                        400);
      }
    }

    [TestClass]
    public class when_two_svg_unit_instances_are_substracted : SvgUnitCalculatorSpecsContext
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
      public void the_result_should_be_correct()
      {
        Assert.AreEqual(this.Actual,
                        200);
      }
    }
  }
}