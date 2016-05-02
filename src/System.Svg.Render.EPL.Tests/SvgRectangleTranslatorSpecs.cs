using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest;

// ReSharper disable InconsistentNaming

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
    }

    [TestClass]
    public class when_svg_rectangle_is_translated : SvgRectangleTranslatorSpecsContext
    {
      private SvgLine UpperLine { get; set; }
      private SvgLine RightLine { get; set; }
      private SvgLine LowerLine { get; set; }
      private SvgLine LeftLine { get; set; }
      private object ActualUpperLine { get; set; }
      private object ActualRightLine { get; set; }
      private object ActualLowerLine { get; set; }
      private object ActualLeftLine { get; set; }

      protected override void Context()
      {
        base.Context();

        this.SvgRectangle = new SvgRectangle
                            {
                              X = new SvgUnit(10f),
                              Y = new SvgUnit(10f),
                              Width = new SvgUnit(100f),
                              Height = new SvgUnit(100f),
                              StrokeWidth = new SvgUnit(20f)
                            };
      }

      protected override void BecauseOf()
      {
        base.BecauseOf();

        this.UpperLine = this.SvgRectangleTranslator.GetUpperLine(this.SvgRectangle);
        this.RightLine = this.SvgRectangleTranslator.GetRightLine(this.SvgRectangle);
        this.LowerLine = this.SvgRectangleTranslator.GetLowerLine(this.SvgRectangle);
        this.LeftLine = this.SvgRectangleTranslator.GetLeftLine(this.SvgRectangle);

        this.ActualUpperLine = this.SvgLineTranslator.Translate(this.UpperLine,
                                                                this.SvgUnitCalculator.SourceDpi);
        this.ActualRightLine = this.SvgLineTranslator.Translate(this.RightLine,
                                                                this.SvgUnitCalculator.SourceDpi);
        this.ActualLowerLine = this.SvgLineTranslator.Translate(this.LowerLine,
                                                                this.SvgUnitCalculator.SourceDpi);
        this.ActualLeftLine = this.SvgLineTranslator.Translate(this.LeftLine,
                                                               this.SvgUnitCalculator.SourceDpi);
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
      private object Actual { get; set; }

      protected override void Context()
      {
        base.Context();

        this.SvgRectangle = new SvgRectangle
                            {
                              X = new SvgUnit(10f),
                              Y = new SvgUnit(10f),
                              Width = new SvgUnit(0f),
                              Height = new SvgUnit(0f),
                              StrokeWidth = new SvgUnit(20f)
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
    public class when_horizontal_svg_rectangle_with_black_fill_is_translated : SvgRectangleTranslatorSpecsContext
    {
      private object Actual { get; set; }

      protected override void Context()
      {
        base.Context();

        this.SvgRectangle = new SvgRectangle
                            {
                              X = new SvgUnit(10f),
                              Y = new SvgUnit(10f),
                              Width = new SvgUnit(100f),
                              Height = new SvgUnit(50f),
                              StrokeWidth = new SvgUnit(20f),
                              Stroke = new SvgColourServer(Color.Black)
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
    public class when_vertical_svg_rectangle_with_black_fill_is_translated : SvgRectangleTranslatorSpecsContext
    {
      private object Actual { get; set; }

      protected override void Context()
      {
        base.Context();

        this.SvgRectangle = new SvgRectangle
                            {
                              X = new SvgUnit(10f),
                              Y = new SvgUnit(10f),
                              Width = new SvgUnit(50f),
                              Height = new SvgUnit(100f),
                              StrokeWidth = new SvgUnit(20f),
                              Stroke = new SvgColourServer(Color.Black)
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
        Assert.AreEqual("LO10,10,50,100",
                        this.Actual);
      }
    }
  }
}