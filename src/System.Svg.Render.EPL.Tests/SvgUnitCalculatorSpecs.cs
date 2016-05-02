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

    [TestClass]
    public class when_inch_is_converted_to_device_points : SvgUnitCalculatorSpecsContext
    {
      protected override void BecauseOf()
      {
        base.BecauseOf();

        var svgUnit = new SvgUnit(SvgUnitType.Inch,
                                  3f);
        this.Actual = this.SvgUnitCalculator.GetDevicePoints(svgUnit,
                                                             203);
      }

      [TestMethod]
      public void result_should_be_correct()
      {
        Assert.AreEqual(609,
                        this.Actual);
      }
    }

    [TestClass]
    public class when_untyped_value_is_mapped_to_inches_and_converted_to_device_points : SvgUnitCalculatorSpecsContext
    {
      protected override void Context()
      {
        base.Context();

        this.SvgUnitCalculator.NoneSubstution = SvgUnitType.Inch;
      }

      protected override void BecauseOf()
      {
        base.BecauseOf();

        var svgUnit = new SvgUnit(SvgUnitType.None,
                                  3f);
        this.Actual = this.SvgUnitCalculator.GetDevicePoints(svgUnit,
                                                             203);
      }

      [TestMethod]
      public void result_should_be_correct()
      {
        Assert.AreEqual(609,
                        this.Actual);
      }
    }

    [TestClass]
    public class when_centimeter_is_converted_to_device_points : SvgUnitCalculatorSpecsContext
    {
      protected override void BecauseOf()
      {
        base.BecauseOf();

        var svgUnit = new SvgUnit(SvgUnitType.Centimeter,
                                  10f);
        this.Actual = this.SvgUnitCalculator.GetDevicePoints(svgUnit,
                                                             203);
      }

      [TestMethod]
      public void result_should_be_correct()
      {
        Assert.AreEqual(799,
                        this.Actual);
      }
    }

    [TestClass]
    public class when_untyped_value_is_mapped_to_centimeters_and_converted_to_device_points : SvgUnitCalculatorSpecsContext
    {
      protected override void Context()
      {
        base.Context();

        this.SvgUnitCalculator.NoneSubstution = SvgUnitType.Centimeter;
      }

      protected override void BecauseOf()
      {
        base.BecauseOf();

        var svgUnit = new SvgUnit(SvgUnitType.Centimeter,
                                  10f);
        this.Actual = this.SvgUnitCalculator.GetDevicePoints(svgUnit,
                                                             203);
      }

      [TestMethod]
      public void result_should_be_correct()
      {
        Assert.AreEqual(799,
                        this.Actual);
      }
    }

    [TestClass]
    public class when_millimeter_is_converted_to_device_points : SvgUnitCalculatorSpecsContext
    {
      protected override void BecauseOf()
      {
        base.BecauseOf();

        var svgUnit = new SvgUnit(SvgUnitType.Millimeter,
                                  8433f);
        this.Actual = this.SvgUnitCalculator.GetDevicePoints(svgUnit,
                                                             203);
      }

      [TestMethod]
      public void result_should_be_correct()
      {
        Assert.AreEqual(67397,
                        this.Actual);
      }
    }

    [TestClass]
    public class when_untyped_value_is_mapped_to_millimeters_and_converted_to_device_points : SvgUnitCalculatorSpecsContext
    {
      protected override void Context()
      {
        base.Context();

        this.SvgUnitCalculator.NoneSubstution = SvgUnitType.Millimeter;
      }

      protected override void BecauseOf()
      {
        base.BecauseOf();

        var svgUnit = new SvgUnit(SvgUnitType.None,
                                  8433f);
        this.Actual = this.SvgUnitCalculator.GetDevicePoints(svgUnit,
                                                             203);
      }

      [TestMethod]
      public void result_should_be_correct()
      {
        Assert.AreEqual(67397,
                        this.Actual);
      }
    }

    [TestClass]
    public class when_pixel_is_converted_to_device_points : SvgUnitCalculatorSpecsContext
    {
      protected override void BecauseOf()
      {
        base.BecauseOf();

        var svgUnit = new SvgUnit(SvgUnitType.Pixel,
                                  100f);
        this.Actual = this.SvgUnitCalculator.GetDevicePoints(svgUnit,
                                                             203);
      }

      [TestMethod]
      public void result_should_be_correct()
      {
        Assert.AreEqual(281,
                        this.Actual);
      }
    }

    [TestClass]
    public class when_untyped_value_is_mapped_to_pixels_and_converted_to_device_points : SvgUnitCalculatorSpecsContext
    {
      protected override void Context()
      {
        base.Context();

        this.SvgUnitCalculator.NoneSubstution = SvgUnitType.Pixel;
      }

      protected override void BecauseOf()
      {
        base.BecauseOf();

        var svgUnit = new SvgUnit(SvgUnitType.None,
                                  100f);
        this.Actual = this.SvgUnitCalculator.GetDevicePoints(svgUnit,
                                                             203);
      }

      [TestMethod]
      public void result_should_be_correct()
      {
        Assert.AreEqual(281,
                        this.Actual);
      }
    }

    [TestClass]
    public class when_pica_is_converted_to_device_points : SvgUnitCalculatorSpecsContext
    {
      protected override void BecauseOf()
      {
        base.BecauseOf();

        var svgUnit = new SvgUnit(SvgUnitType.Pica,
                                  128840f);
        this.Actual = this.SvgUnitCalculator.GetDevicePoints(svgUnit,
                                                             203);
      }

      [TestMethod]
      public void result_should_be_correct()
      {
        Assert.AreEqual(36325,
                        this.Actual);
      }
    }

    [TestClass]
    public class when_untyped_value_is_mapped_to_pica_and_converted_to_device_points_and_translated : SvgUnitCalculatorSpecsContext
    {
      protected override void Context()
      {
        base.Context();

        this.SvgUnitCalculator.NoneSubstution = SvgUnitType.Pica;
      }

      protected override void BecauseOf()
      {
        base.BecauseOf();

        var svgUnit = new SvgUnit(SvgUnitType.None,
                                  128840f);
        this.Actual = this.SvgUnitCalculator.GetDevicePoints(svgUnit,
                                                             203);
      }

      [TestMethod]
      public void result_should_be_correct()
      {
        Assert.AreEqual(36325,
                        this.Actual);
      }
    }

    [TestClass]
    public class when_em_is_converted_to_device_points : SvgUnitCalculatorFailingSpecsContext
    {
      protected override void BecauseOf()
      {
        base.BecauseOf();

        var svgUnit = new SvgUnit(SvgUnitType.Em,
                                  100f);

        try
        {
          this.SvgUnitCalculator.GetDevicePoints(svgUnit,
                                                 203);
        }
        catch (NotImplementedException notImplementedException)
        {
          this.Actual = notImplementedException;
        }
      }

      [TestMethod]
      public void should_get_not_implemented_exception()
      {
        Assert.IsInstanceOfType(this.Actual,
                                typeof(NotImplementedException));
      }
    }

    [TestClass]
    public class when_ex_is_converted_to_device_points : SvgUnitCalculatorFailingSpecsContext
    {
      protected override void BecauseOf()
      {
        base.BecauseOf();

        var svgUnit = new SvgUnit(SvgUnitType.Ex,
                                  100f);

        try
        {
          this.SvgUnitCalculator.GetDevicePoints(svgUnit,
                                                 203);
        }
        catch (NotImplementedException notImplementedException)
        {
          this.Actual = notImplementedException;
        }
      }

      [TestMethod]
      public void should_get_not_implemented_exception()
      {
        Assert.IsInstanceOfType(this.Actual,
                                typeof(NotImplementedException));
      }
    }

    [TestClass]
    public class when_percentage_is_converted_to_device_points : SvgUnitCalculatorFailingSpecsContext
    {
      protected override void BecauseOf()
      {
        base.BecauseOf();

        var svgUnit = new SvgUnit(SvgUnitType.Percentage,
                                  100f);

        try
        {
          this.SvgUnitCalculator.GetDevicePoints(svgUnit,
                                                 203);
        }
        catch (NotImplementedException notImplementedException)
        {
          this.Actual = notImplementedException;
        }
      }

      [TestMethod]
      public void should_get_not_implemented_exception()
      {
        Assert.IsInstanceOfType(this.Actual,
                                typeof(NotImplementedException));
      }
    }

    [TestClass]
    public class when_user_value_is_converted_to_device_points : SvgUnitCalculatorFailingSpecsContext
    {
      protected override void BecauseOf()
      {
        base.BecauseOf();

        var svgUnit = new SvgUnit(SvgUnitType.User,
                                  100f);

        try
        {
          this.SvgUnitCalculator.GetDevicePoints(svgUnit,
                                                 203);
        }
        catch (NotImplementedException notImplementedException)
        {
          this.Actual = notImplementedException;
        }
      }

      [TestMethod]
      public void should_get_not_implemented_exception()
      {
        Assert.IsInstanceOfType(this.Actual,
                                typeof(NotImplementedException));
      }
    }

    [TestClass]
    public class when_untyped_value_is_converted_to_device_points : SvgUnitCalculatorFailingSpecsContext
    {
      protected override void BecauseOf()
      {
        base.BecauseOf();

        var svgUnit = new SvgUnit(SvgUnitType.None,
                                  100f);

        try
        {
          this.SvgUnitCalculator.GetDevicePoints(svgUnit,
                                                 203);
        }
        catch (NotImplementedException notImplementedException)
        {
          this.Actual = notImplementedException;
        }
      }

      [TestMethod]
      public void should_get_not_implemented_exception()
      {
        Assert.IsInstanceOfType(this.Actual,
                                typeof(NotImplementedException));
      }
    }
  }
}