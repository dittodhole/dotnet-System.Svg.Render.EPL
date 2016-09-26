// see http://mrclyfar.blogspot.co.at/2010/02/amazing-mapping-demo-at-ted-2010.html

using NUnit.Framework;

// ReSharper disable UnusedMember.Global
// ReSharper disable VirtualMemberNeverOverriden.Global
// ReSharper disable CheckNamespace

namespace UnitTest
{
  public abstract class ContextSpecification
  {
    public TestContext TestContext { get; set; }

    [SetUp]
    public void TestInitialize()
    {
      this.Context();
      this.BecauseOf();
    }

    [TearDown]
    public void TestCleanup()
    {
      this.Cleanup();
    }

    protected virtual void Context() {}

    protected virtual void BecauseOf() {}

    protected virtual void Cleanup() {}
  }
}
