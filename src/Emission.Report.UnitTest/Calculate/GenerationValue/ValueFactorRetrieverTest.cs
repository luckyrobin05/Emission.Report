
#region

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Emission.Report.Library.Calculate.GenerationValue;
using Emission.Report.Library.Enums;

#endregion

namespace Emission.Report.UnitTest.Calculate.GenerationValue
{
  [TestClass]
  public class ValueFactorRetrieverTest : BaseTestFramework
  {

    #region Setup

    #endregion Setup

    #region Tests

    [TestMethod]
    [DataRow("offshore", ValueFactorLow)]
    [DataRow("OFFSHORE", ValueFactorLow)]
    [DataRow("oFfsHore", ValueFactorLow)]
    [DataRow("oFfsHore    ", ValueFactorLow)]
    [DataRow("onshore", ValueFactorHigh)]
    [DataRow("ONSHORE", ValueFactorHigh)]
    [DataRow("oNSHore", ValueFactorHigh)]
    [DataRow("oNsHore    ", ValueFactorHigh)]
    [DataRow("", ValueFactorMedium)]
    [DataRow(null, ValueFactorMedium)]
    public void Test_ValueFactor_Get(string location, double expectedValueFactor)
    {
      var valueFactor = ValueFactorRetriever.Get(location);

      Assert.AreEqual(valueFactor, expectedValueFactor);
    }

    #endregion Tests

  }
}
