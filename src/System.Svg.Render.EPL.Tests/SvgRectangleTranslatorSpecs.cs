using System.Drawing;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Svg.Render.EPL.ExtensionMethods;

// ReSharper disable InconsistentNaming
// ReSharper disable ExceptionNotDocumented

namespace System.Svg.Render.EPL.Tests
{
  public static class SvgRectangleTranslatorSpecs
  {
    public abstract class SvgRectangleTranslatorSpecsContext : SvgElementBaseTranslatorContext
    {
      protected SvgRectangleTranslatorSpecsContext()
      {
        this.SvgRectangleTranslator = new SvgRectangleTranslator(this.EplTransformer,
                                                                 this.EplCommands);
      }

      [NotNull]
      private SvgRectangleTranslator SvgRectangleTranslator { get; }

      protected SvgRectangle SvgRectangle { get; set; }
      protected object Actual { get; set; }

      protected override void BecauseOf()
      {
        base.BecauseOf();

        var translation = this.SvgRectangleTranslator.Translate(this.SvgRectangle,
                                                                this.Matrix);

        this.Actual = this.Encoding.GetString(translation);
      }
    }

    [TestClass]
    public class when_svg_rectangle_without_fill_is_translated : SvgRectangleTranslatorSpecsContext
    {
      protected override void Context()
      {
        base.Context();

        this.SvgRectangle = new SvgRectangle
                            {
                              X = new SvgUnit(70f),
                              Y = new SvgUnit(40f),
                              Width = new SvgUnit(100f),
                              Height = new SvgUnit(130f),
                              StrokeWidth = new SvgUnit(20f),
                              Stroke = new SvgColourServer(Color.Black),
                              Fill = SvgPaintServer.None
                            };
      }

      [TestMethod]
      public void returns_valid_epl_code()
      {
        Assert.AreEqual("X60,30,20,180,180",
                        this.Actual);
      }
    }


    [TestClass]
    public class when_svg_rectangle_with_black_fill_is_translated : SvgRectangleTranslatorSpecsContext
    {
      protected override void Context()
      {
        base.Context();

        this.SvgRectangle = new SvgRectangle
                            {
                              X = new SvgUnit(30f),
                              Y = new SvgUnit(70f),
                              Width = new SvgUnit(100f),
                              Height = new SvgUnit(50f),
                              Stroke = SvgPaintServer.None,
                              Fill = new SvgColourServer(Color.Black)
                            };
      }

      [TestMethod]
      public void return_valid_epl_code()
      {
        Assert.AreEqual("LO30,70,100,50",
                        this.Actual);
      }
    }
  }
}