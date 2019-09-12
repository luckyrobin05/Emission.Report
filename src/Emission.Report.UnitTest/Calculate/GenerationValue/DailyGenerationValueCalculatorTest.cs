
#region

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace Emission.Report.UnitTest.Calculate.GenerationValue
{
  [TestClass]
  public class DailyGenerationValueCalculatorTest : BaseTestFramework
  {

    #region Setup

    #endregion Setup

    #region Tests

    [TestMethod]
    [DataRow(3, 4, 0.75, 9)]
    public void Test_Calculate(double energy, double price, double valueFactor, double expectedDailyGenerationValue)
    {
      var dailyGenerationvalue = DailyGenerationValueCalculator.Calculate(energy, price, valueFactor);

      Assert.AreEqual(dailyGenerationvalue, expectedDailyGenerationValue);
    }

    #endregion Tests

  }
}
