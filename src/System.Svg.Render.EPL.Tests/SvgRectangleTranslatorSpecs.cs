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
        var svgUnitCalculator = new SvgUnitCalculator();

        this.SvgLineTranslator = new SvgLineTranslator(svgUnitCalculator);
        this.SvgRectangleTranslator = new SvgRectangleTranslator(this.SvgLineTranslator,
                                                                 svgUnitCalculator);
      }

      private SvgRectangleTranslator SvgRectangleTranslator { get; }
      private SvgLineTranslator SvgLineTranslator { get; }
      protected SvgRectangle SvgRectangle { get; set; }
      private SvgLine UpperLine { get; set; }
      private SvgLine RightLine { get; set; }
      private SvgLine LowerLine { get; set; }
      private SvgLine LeftLine { get; set; }
      protected object ActualUpperLine { get; set; }
      protected object ActualRightLine { get; set; }
      protected object ActualLowerLine { get; set; }
      protected object ActualLeftLine { get; set; }

      protected override void BecauseOf()
      {
        base.BecauseOf();

        this.UpperLine = this.SvgRectangleTranslator.GetUpperLine(this.SvgRectangle);
        this.RightLine = this.SvgRectangleTranslator.GetRightLine(this.SvgRectangle);
        this.LowerLine = this.SvgRectangleTranslator.GetLowerLine(this.SvgRectangle);
        this.LeftLine = this.SvgRectangleTranslator.GetLeftLine(this.SvgRectangle);

        this.ActualUpperLine = this.SvgLineTranslator.Translate(this.UpperLine);
        this.ActualRightLine = this.SvgLineTranslator.Translate(this.RightLine);
        this.ActualLowerLine = this.SvgLineTranslator.Translate(this.LowerLine);
        this.ActualLeftLine = this.SvgLineTranslator.Translate(this.LeftLine);
      }
    }

    [TestClass]
    public class when_svg_rectangle_is_translated : SvgRectangleTranslatorSpecsContext
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
                              StrokeWidth = new SvgUnit(20f)
                            };
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
  }
}