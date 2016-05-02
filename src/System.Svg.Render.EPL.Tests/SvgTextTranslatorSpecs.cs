using System.Drawing;
using System.Svg.Transforms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest;

// ReSharper disable InconsistentNaming

namespace System.Svg.Render.EPL.Tests
{
  public static class SvgTextTranslatorSpecs
  {
    public abstract class SvgTextTranslatorSpecsContext : ContextSpecification
    {
      protected SvgTextTranslatorSpecsContext()
      {
        this.SvgUnitCalculator = new SvgUnitCalculator
                                 {
                                   UserUnitTypeSubstitution = SvgUnitType.Pixel
                                 };
        this.SvgTextTranslator = new SvgTextTranslator(this.SvgUnitCalculator);
      }

      private SvgUnitCalculator SvgUnitCalculator { get; }
      private SvgTextTranslator SvgTextTranslator { get; }
      protected SvgText SvgText { get; set; }
      protected object Actual { get; set; }

      protected override void BecauseOf()
      {
        base.BecauseOf();

        this.Actual = this.SvgTextTranslator.Translate(this.SvgText,
                                                       this.SvgUnitCalculator.SourceDpi);
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
                               new SvgUnit(100f)
                             },
                         Y = new SvgUnitCollection
                             {
                               new SvgUnit(100f)
                             },
                         FontSize = new SvgUnit(12f)
                       };
      }

      [TestMethod]
      public void return_valid_epl_code()
      {
        Assert.AreEqual(@"A100,100,0,1,1,1,N,""hello""",
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
                               new SvgUnit(100f)
                             },
                         Y = new SvgUnitCollection
                             {
                               new SvgUnit(100f)
                             },
                         Fill = new SvgColourServer(Color.White),
                         FontSize = new SvgUnit(12f)
                       };
      }

      [TestMethod]
      public void return_valid_epl_code()
      {
        Assert.AreEqual(@"A100,100,0,1,1,1,R,""hello""",
                        this.Actual);
      }
    }

    [TestClass]
    public class when_svg_text_with_a_90_degree_rotation_is_translated : SvgTextTranslatorSpecsContext
    {
      protected override void Context()
      {
        base.Context();

        this.SvgText = new SvgText("hello")
                       {
                         X = new SvgUnitCollection
                             {
                               new SvgUnit(100f)
                             },
                         Y = new SvgUnitCollection
                             {
                               new SvgUnit(100f)
                             },
                         Transforms = new SvgTransformCollection
                                      {
                                        new SvgRotate(90f)
                                      },
                         FontSize = new SvgUnit(12f)
                       };
      }

      [TestMethod]
      public void return_valid_epl_code()
      {
        Assert.AreEqual(@"A100,100,1,1,1,1,N,""hello""",
                        this.Actual);
      }
    }

    [TestClass]
    public class when_svg_text_with_a_180_degree_rotation_is_translated : SvgTextTranslatorSpecsContext
    {
      protected override void Context()
      {
        base.Context();

        this.SvgText = new SvgText("hello")
                       {
                         X = new SvgUnitCollection
                             {
                               new SvgUnit(100f)
                             },
                         Y = new SvgUnitCollection
                             {
                               new SvgUnit(100f)
                             },
                         Transforms = new SvgTransformCollection
                                      {
                                        new SvgRotate(180f)
                                      },
                         FontSize = new SvgUnit(12f)
                       };
      }

      [TestMethod]
      public void return_valid_epl_code()
      {
        Assert.AreEqual(@"A100,100,2,1,1,1,N,""hello""",
                        this.Actual);
      }
    }

    [TestClass]
    public class when_svg_text_with_a_270_degree_rotation_is_translated : SvgTextTranslatorSpecsContext
    {
      protected override void Context()
      {
        base.Context();

        this.SvgText = new SvgText("hello")
                       {
                         X = new SvgUnitCollection
                             {
                               new SvgUnit(100f)
                             },
                         Y = new SvgUnitCollection
                             {
                               new SvgUnit(100f)
                             },
                         Transforms = new SvgTransformCollection
                                      {
                                        new SvgRotate(270f)
                                      },
                         FontSize = new SvgUnit(12f)
                       };
      }

      [TestMethod]
      public void return_valid_epl_code()
      {
        Assert.AreEqual(@"A100,100,3,1,1,1,N,""hello""",
                        this.Actual);
      }
    }

    [TestClass]
    public class when_svg_text_with_an_out_of_range_rotation_is_translated : SvgTextTranslatorSpecsContext
    {
      protected override void Context()
      {
        base.Context();

        this.SvgText = new SvgText("hello")
                       {
                         X = new SvgUnitCollection
                             {
                               new SvgUnit(100f)
                             },
                         Y = new SvgUnitCollection
                             {
                               new SvgUnit(100f)
                             },
                         Transforms = new SvgTransformCollection
                                      {
                                        new SvgRotate(290f)
                                      },
                         FontSize = new SvgUnit(12f)
                       };
      }

      [TestMethod]
      public void return_no_epl_code()
      {
        Assert.AreEqual(null,
                        this.Actual);
      }
    }

    // TODO add test for TryGetRotation
  }
}