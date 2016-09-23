using System.Drawing;
using JetBrains.Annotations;
using NUnit.Framework;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable InconsistentNaming

namespace Svg.Contrib.Render.ZPL.Tests
{
  public static class SvgLineTranslatorSpecs
  {
    public abstract class SvgLineTranslatorSpecsContext : SvgElementBaseTranslatorContext
    {
      protected SvgLineTranslatorSpecsContext()
      {
        this.SvgLineTranslator = new SvgLineTranslator(this.ZplTransformer,
                                                       this.ZplCommands);
      }

      [NotNull]
      private SvgLineTranslator SvgLineTranslator { get; }

      protected SvgLine SvgLine { get; set; }

      protected override void BecauseOf()
      {
        this.SvgLineTranslator.Translate(this.SvgLine,
                                         this.Matrix,
                                         this.Matrix,
                                         this.ZplContainer);

        base.BecauseOf();
      }
    }

    [TestFixture]
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

      [Test]
      public void return_valid_zpl_code()
      {
        Assert.AreEqual(@"^FT50,200
^GB400,0,20,B^FS",
                        this.Actual);
      }
    }

    [TestFixture]
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

      [Test]
      public void return_valid_zpl_code()
      {
        Assert.AreEqual(@"^FT200,50
^GB0,400,20,B^FS",
                        this.Actual);
      }
    }

    //[TestFixture]
    //public class when_diagonal_svg_line_is_translated : SvgLineTranslatorSpecsContext
    //{
    //  protected override void Context()
    //  {
    //    base.Context();

    //    this.SvgLine = new SvgLine
    //                   {
    //                     StartX = new SvgUnit(10f),
    //                     StartY = new SvgUnit(10f),
    //                     EndX = new SvgUnit(200f),
    //                     EndY = new SvgUnit(200f),
    //                     StrokeWidth = new SvgUnit(20f)
    //                   };
    //  }

    //  [Test]
    //  public void return_valid_zpl_code()
    //  {
    //    Assert.AreEqual("LS10,10,20,200,200",
    //                    this.Actual);
    //  }
    //}

    [TestFixture]
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

      [Test]
      public void return_valid_zpl_code()
      {
        Assert.AreEqual(@"^FT10,10
^GB0,200,20,W^FS",
                        this.Actual);
      }
    }

    //  protected override void Context()
    //{
    //public class when_svg_line_with_partly_swapped_coordinates_is_translated : SvgLineTranslatorSpecsContext

    //[TestFixture]
    //  {
    //    base.Context();

    //    this.SvgLine = new SvgLine
    //                   {
    //                     StartX = new SvgUnit(10f),
    //                     StartY = new SvgUnit(20f),
    //                     EndX = new SvgUnit(60f),
    //                     EndY = new SvgUnit(30f),
    //                     StrokeWidth = new SvgUnit(20f)
    //                   };
    //  }

    //  [Test]
    //  public void return_valid_zpl_code()
    //  {
    //    Assert.AreEqual("LS10,20,20,60,30",
    //                    this.Actual);
    //  }
    //}

    //[TestFixture]
    //public class when_svg_line_with_completely_swapped_coordinates_is_translated : SvgLineTranslatorSpecsContext
    //{
    //  protected override void Context()
    //  {
    //    base.Context();

    //    this.SvgLine = new SvgLine
    //                   {
    //                     StartX = new SvgUnit(90f),
    //                     StartY = new SvgUnit(60f),
    //                     EndX = new SvgUnit(30f),
    //                     EndY = new SvgUnit(50f),
    //                     StrokeWidth = new SvgUnit(20f)
    //                   };
    //  }

    //  [Test]
    //  public void return_valid_zpl_code()
    //  {
    //    Assert.AreEqual("LS30,50,20,90,60",
    //                    this.Actual);
    //  }
    //}
  }
}
