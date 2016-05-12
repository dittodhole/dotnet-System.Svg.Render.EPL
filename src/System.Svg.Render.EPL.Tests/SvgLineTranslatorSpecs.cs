using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest;

// ReSharper disable InconsistentNaming
// ReSharper disable ExceptionNotDocumented

namespace System.Svg.Render.EPL.Tests
{
  public static class SvgLineTranslatorSpecs
  {
    public abstract class SvgLineTranslatorSpecsContext : ContextSpecification
    {
      protected SvgLineTranslatorSpecsContext()
      {
        this.Matrix = new Matrix();
        this.SvgUnitCalculator = new SvgUnitCalculator(PrintDirection.None);
        this.SvgLineTranslator = new SvgLineTranslator(this.SvgUnitCalculator,
                                                       Encoding.Default);
      }

      [NotNull]
      private Matrix Matrix { get; }

      [NotNull]
      private SvgUnitCalculator SvgUnitCalculator { get; }

      [NotNull]
      private SvgLineTranslator SvgLineTranslator { get; }

      protected SvgLine SvgLine { get; set; }
      protected object Actual { get; set; }

      protected override void BecauseOf()
      {
        base.BecauseOf();

        var translation = this.SvgLineTranslator.Translate(this.SvgLine,
                                                           this.Matrix);

        this.Actual = this.SvgLineTranslator.GetString(translation);
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
                         Stroke = new SvgColourServer(Color.White)
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
                         StartX = new SvgUnit(10f),
                         StartY = new SvgUnit(20f),
                         EndX = new SvgUnit(60f),
                         EndY = new SvgUnit(30f),
                         StrokeWidth = new SvgUnit(20f)
                       };
      }

      [TestMethod]
      public void return_valid_epl_code()
      {
        Assert.AreEqual("LS10,20,20,60,30",
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
                         StartX = new SvgUnit(90f),
                         StartY = new SvgUnit(60f),
                         EndX = new SvgUnit(30f),
                         EndY = new SvgUnit(50f),
                         StrokeWidth = new SvgUnit(20f)
                       };
      }

      [TestMethod]
      public void return_valid_epl_code()
      {
        Assert.AreEqual("LS90,60,20,30,50",
                        this.Actual);
      }
    }
  }
}