
#region

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace Emission.Report.UnitTest.Calculate.ActualHeatRateTest
{
  [TestClass]
  public class CoalActualHeatRateCalculatorTest : BaseTestFramework
  {

    #region Setup

    #endregion Setup

    #region Tests

    [TestMethod]
    [DataRow(3, 4, 0.75)]
    public void Test_Calculate(double totalHeatInput, double actualNetGeneration, double expectedHeatRate)
    {
      var actualHeatRate = CoalActualHeatRateCalculator.Calculate(totalHeatInput, actualNetGeneration);

      Assert.AreEqual(actualHeatRate, expectedHeatRate);
    }

    [TestMethod]
    [DataRow(3, 0)]
    public void Test_Calculate_DivideByZero(double totalHeatInput, double actualNetGeneration)
    {
      Assert.ThrowsException<DivideByZeroException>(() => { var actualHeatRate = CoalActualHeatRateCalculator.Calculate(totalHeatInput, actualNetGeneration); });
    }

    #endregion Tests

  }
}
