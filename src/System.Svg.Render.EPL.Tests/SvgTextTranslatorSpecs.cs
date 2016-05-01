using System.Svg.Transforms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest;

namespace System.Svg.Render.EPL.Tests
{
  public static class SvgTextTranslatorSpecs
  {
    public class SvgTextTranslatorSpecsContext : ContextSpecification
    {
      public SvgTextTranslatorSpecsContext()
      {
        var svgUnitCalculator = new SvgUnitCalculator();

        this.SvgTextTranslator = new SvgTextTranslator(svgUnitCalculator);
      }

      protected SvgTextTranslator SvgTextTranslator { get; }
      protected SvgText SvgText { get; set; }
      protected object Actual { get; set; }

      protected override void BecauseOf()
      {
        base.BecauseOf();

        this.Actual = this.SvgTextTranslator.Translate(this.SvgText);
      }
    }

    [TestClass]
    public class when_valid_specs_are_given : SvgTextTranslatorSpecsContext
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
                             }
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
    public class when_90_degree_rotation_is_given : SvgTextTranslatorSpecsContext
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
                                      }
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
    public class when_180_degree_rotation_is_given : SvgTextTranslatorSpecsContext
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
                                      }
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
    public class when_270_degree_rotation_is_given : SvgTextTranslatorSpecsContext
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
                                      }
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
    public class when_invalid_rotation_is_given : SvgTextTranslatorSpecsContext
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
                                      }
        };
      }

      [TestMethod]
      public void return_valid_epl_code()
      {
        Assert.AreEqual(string.Empty,
                        this.Actual);
      }
    }

  }
}