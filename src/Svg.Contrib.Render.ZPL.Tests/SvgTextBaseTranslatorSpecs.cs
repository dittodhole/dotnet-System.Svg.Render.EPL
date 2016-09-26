using System.Drawing;
using JetBrains.Annotations;
using NUnit.Framework;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable InconsistentNaming

namespace Svg.Contrib.Render.ZPL.Tests
{
  public static class SvgTextBaseTranslatorSpecs
  {
    public abstract class SvgTextTranslatorSpecsContext : SvgElementBaseTranslatorContext
    {
      protected SvgTextTranslatorSpecsContext()
      {
        this.SvgTextTranslator = new SvgTextBaseTranslator<SvgText>(this.ZplTransformer,
                                                                    this.ZplCommands);
      }

      [NotNull]
      private SvgTextBaseTranslator<SvgText> SvgTextTranslator { get; }

      protected SvgText SvgText { get; set; }

      protected override void BecauseOf()
      {
        this.SvgTextTranslator.Translate(this.SvgText,
                                         this.Matrix,
                                         this.Matrix,
                                         this.ZplContainer);

        base.BecauseOf();
      }
    }

    [TestFixture]
    public class when_svg_text_is_translated : SvgTextTranslatorSpecsContext
    {
      protected override void Context()
      {
        base.Context();

        this.SvgText = new SvgText("hello")
                       {
                         X = new SvgUnitCollection
                             {
                               new SvgUnit(50f)
                             },
                         Y = new SvgUnitCollection
                             {
                               new SvgUnit(70f)
                             },
                         FontSize = new SvgUnit(12f)
                       };
      }

      [Test]
      public void return_valid_zpl_code()
      {
        Assert.AreEqual(@"^FT50,46
^A0N,12,0^FDhello^FS",
                        this.Actual);
      }
    }

    [TestFixture]
    public class when_svg_text_with_black_fill_is_translated : SvgTextTranslatorSpecsContext
    {
      protected override void Context()
      {
        base.Context();

        this.SvgText = new SvgText("hello")
                       {
                         X = new SvgUnitCollection
                             {
                               new SvgUnit(50f)
                             },
                         Y = new SvgUnitCollection
                             {
                               new SvgUnit(70f)
                             },
                         Fill = new SvgColourServer(Color.White),
                         FontSize = new SvgUnit(12f)
                       };
      }

      [Test]
      public void return_valid_zpl_code()
      {
        Assert.AreEqual(@"^FT50,46
^A0N,12,0^FDhello^FS",
                        this.Actual);
      }
    }
  }
}
