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

    public abstract class SvgUnitCalculatorTransformationSepcsContext : ContextSpecification
    {
      protected SvgUnitCalculatorTransformationSepcsContext()
      {
        this.SvgUnitCalculator = new SvgUnitCalculator();
      }

      protected SvgUnitCalculator SvgUnitCalculator { get; }
      protected SvgUnit X { get; set; }
      protected SvgUnit Y { get; set; }
      protected object RotationTranslation { get; set; }
      protected object Actual { get; set; }
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
        this.Actual = svgUnit3.Value;
      }

      [TestMethod]
      public void the_result_should_be_correct()
      {
        Assert.AreEqual(this.Actual,
                        400f);
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
        this.Actual = svgUnit3.Value;
      }

      [TestMethod]
      public void the_result_should_be_correct()
      {
        Assert.AreEqual(this.Actual,
                        200f);
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

        int devicePoints;
        if (this.SvgUnitCalculator.TryGetDevicePoints(svgUnit,
                                                      203,
                                                      out devicePoints))
        {
          this.Actual = devicePoints;
        }
      }

      [TestMethod]
      public void result_should_be_correct()
      {
        Assert.AreEqual(609,
                        this.Actual);
      }
    }

    [TestClass]
    public class when_user_value_is_mapped_to_inches_and_converted_to_device_points : SvgUnitCalculatorSpecsContext
    {
      protected override void Context()
      {
        base.Context();

        this.SvgUnitCalculator.UserUnitTypeSubstitution = SvgUnitType.Inch;
      }

      protected override void BecauseOf()
      {
        base.BecauseOf();

        var svgUnit = new SvgUnit(3f);

        int devicePoints;
        if (this.SvgUnitCalculator.TryGetDevicePoints(svgUnit,
                                                      203,
                                                      out devicePoints))
        {
          this.Actual = devicePoints;
        }
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

        int devicePoints;
        if (this.SvgUnitCalculator.TryGetDevicePoints(svgUnit,
                                                      203,
                                                      out devicePoints))
        {
          this.Actual = devicePoints;
        }
      }

      [TestMethod]
      public void result_should_be_correct()
      {
        Assert.AreEqual(799,
                        this.Actual);
      }
    }

    [TestClass]
    public class when_user_value_is_mapped_to_centimeters_and_converted_to_device_points : SvgUnitCalculatorSpecsContext
    {
      protected override void Context()
      {
        base.Context();

        this.SvgUnitCalculator.UserUnitTypeSubstitution = SvgUnitType.Centimeter;
      }

      protected override void BecauseOf()
      {
        base.BecauseOf();

        var svgUnit = new SvgUnit(10f);

        int devicePoints;
        if (this.SvgUnitCalculator.TryGetDevicePoints(svgUnit,
                                                      203,
                                                      out devicePoints))
        {
          this.Actual = devicePoints;
        }
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

        int devicePoints;
        if (this.SvgUnitCalculator.TryGetDevicePoints(svgUnit,
                                                      203,
                                                      out devicePoints))
        {
          this.Actual = devicePoints;
        }
      }

      [TestMethod]
      public void result_should_be_correct()
      {
        Assert.AreEqual(67397,
                        this.Actual);
      }
    }

    [TestClass]
    public class when_user_value_is_mapped_to_millimeters_and_converted_to_device_points : SvgUnitCalculatorSpecsContext
    {
      protected override void Context()
      {
        base.Context();

        this.SvgUnitCalculator.UserUnitTypeSubstitution = SvgUnitType.Millimeter;
      }

      protected override void BecauseOf()
      {
        base.BecauseOf();

        var svgUnit = new SvgUnit(8433f);

        int devicePoints;
        if (this.SvgUnitCalculator.TryGetDevicePoints(svgUnit,
                                                      203,
                                                      out devicePoints))
        {
          this.Actual = devicePoints;
        }
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

        int devicePoints;
        if (this.SvgUnitCalculator.TryGetDevicePoints(svgUnit,
                                                      203,
                                                      out devicePoints))
        {
          this.Actual = devicePoints;
        }
      }

      [TestMethod]
      public void result_should_be_correct()
      {
        Assert.AreEqual(281,
                        this.Actual);
      }
    }

    [TestClass]
    public class when_user_value_is_mapped_to_pixels_and_converted_to_device_points : SvgUnitCalculatorSpecsContext
    {
      protected override void Context()
      {
        base.Context();

        this.SvgUnitCalculator.UserUnitTypeSubstitution = SvgUnitType.Pixel;
      }

      protected override void BecauseOf()
      {
        base.BecauseOf();

        var svgUnit = new SvgUnit(100f);

        int devicePoints;
        if (this.SvgUnitCalculator.TryGetDevicePoints(svgUnit,
                                                      203,
                                                      out devicePoints))
        {
          this.Actual = devicePoints;
        }
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

        int devicePoints;
        if (this.SvgUnitCalculator.TryGetDevicePoints(svgUnit,
                                                      203,
                                                      out devicePoints))
        {
          this.Actual = devicePoints;
        }
      }

      [TestMethod]
      public void result_should_be_correct()
      {
        Assert.AreEqual(36325,
                        this.Actual);
      }
    }

    [TestClass]
    public class when_user_value_is_mapped_to_pica_and_converted_to_device_points_and_translated : SvgUnitCalculatorSpecsContext
    {
      protected override void Context()
      {
        base.Context();

        this.SvgUnitCalculator.UserUnitTypeSubstitution = SvgUnitType.Pica;
      }

      protected override void BecauseOf()
      {
        base.BecauseOf();

        var svgUnit = new SvgUnit(128840f);
        int devicePoints;
        if (this.SvgUnitCalculator.TryGetDevicePoints(svgUnit,
                                                      203,
                                                      out devicePoints))
        {
          this.Actual = devicePoints;
        }
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

        int devicePoints;
        if (this.SvgUnitCalculator.TryGetDevicePoints(svgUnit,
                                                      203,
                                                      out devicePoints))
        {
          this.Actual = devicePoints;
        }
      }

      [TestMethod]
      public void should_get_not_implemented_exception()
      {
        Assert.IsNull(this.Actual);
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

        int devicePoints;
        if (this.SvgUnitCalculator.TryGetDevicePoints(svgUnit,
                                                      203,
                                                      out devicePoints))
        {
          this.Actual = devicePoints;
        }
      }

      [TestMethod]
      public void should_get_not_implemented_exception()
      {
        Assert.IsNull(this.Actual);
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

        int devicePoints;
        if (this.SvgUnitCalculator.TryGetDevicePoints(svgUnit,
                                                      203,
                                                      out devicePoints))
        {
          this.Actual = devicePoints;
        }
      }

      [TestMethod]
      public void should_get_not_implemented_exception()
      {
        Assert.IsNull(this.Actual);
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

        int devicePoints;
        if (this.SvgUnitCalculator.TryGetDevicePoints(svgUnit,
                                                      203,
                                                      out devicePoints))
        {
          this.Actual = devicePoints;
        }
      }

      [TestMethod]
      public void should_get_not_implemented_exception()
      {
        Assert.IsNull(this.Actual);
      }
    }
  }
}