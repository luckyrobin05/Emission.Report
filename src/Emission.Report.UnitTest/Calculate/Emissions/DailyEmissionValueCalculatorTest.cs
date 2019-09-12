
#region

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace Emission.Report.UnitTest.Calculate.Emissions
{
  [TestClass]
  public class DailyEmissionValueCalculatorTest : BaseTestFramework
  {

    #region Setup

    #endregion Setup

    #region Tests

    [TestMethod]
    [DataRow(6.9, 6, 0.7, 28.98)]
    public void Test_Calculate(double energy, double emissionsRating, double emissionFactor, double expectedDailyEmissionValue)
    {
      var dailyEmissionvalue = DailyEmissionValueCalculator.Calculate(energy, emissionsRating, emissionFactor);

      Assert.AreEqual(dailyEmissionvalue, expectedDailyEmissionValue);
    }

    #endregion Tests

  }
}
