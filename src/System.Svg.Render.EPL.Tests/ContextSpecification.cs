// see http://mrclyfar.blogspot.co.at/2010/02/amazing-mapping-demo-at-ted-2010.html

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
  public abstract class ContextSpecification
  {
    public TestContext TestContext { get; set; }

    [TestInitialize]
    public void TestInitialize()
    {
      this.Context();
      this.BecauseOf();
    }

    [TestCleanup]
    public void TestCleanup()
    {
      this.Cleanup();
    }

    protected virtual void Context() {}

    protected virtual void BecauseOf() {}

    protected virtual void Cleanup() {}
  }
}