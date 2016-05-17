using System.Drawing;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable InconsistentNaming

namespace System.Svg.Render.EPL.Tests
{
  public static class SvgTextBaseTranslatorSpecs
  {
    public abstract class SvgTextTranslatorSpecsContext : SvgElementBaseTranslatorContext
    {
      protected SvgTextTranslatorSpecsContext()
      {
        this.SvgTextTranslator = new SvgTextBaseTranslator<SvgText>(this.EplTransformer,
                                                                    this.EplCommands);
      }

      [NotNull]
      private SvgTextBaseTranslator<SvgText> SvgTextTranslator { get; }

      protected SvgText SvgText { get; set; }

      protected override void BecauseOf()
      {
        this.SvgTextTranslator.Translate(this.SvgText,
                                         this.Matrix,
                                         this.Container);

        base.BecauseOf();
      }
    }

    [TestClass]
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

      [TestMethod]
      public void return_valid_epl_code()
      {
        Assert.AreEqual(@"A50,58,2,1,1,1,N,""hello""",
                        this.Actual);
      }
    }

    [TestClass]
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

      [TestMethod]
      public void return_valid_epl_code()
      {
        Assert.AreEqual(@"A50,58,2,1,1,1,R,""hello""",
                        this.Actual);
      }
    }
  }
}