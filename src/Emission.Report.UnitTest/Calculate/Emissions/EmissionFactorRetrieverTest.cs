
#region

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Emission.Report.Library.Enums;
using Emission.Report.Library.Types.Serializable.Input;
using InputDay = Emission.Report.Library.Types.Serializable.Input.Day;

#endregion

namespace Emission.Report.UnitTest.Calculate.Emissions
{
  [TestClass]
  public class EmissionFactorRetrieverTest : BaseTestFramework
  {

    #region Setup

    #endregion Setup

    #region Tests

    [TestMethod]
    [DataRow(GeneratorType.Coal, EmissionFactorHigh)]
    [DataRow(GeneratorType.Gas, EmissionFactorMedium)]
    public void Test_EmissionFactor_Get(GeneratorType generatorType, double expectedValue)
    {
      var emissionFactor = EmissionFactorRetriever.Get(generatorType);

      Assert.AreNotSame(emissionFactor, expectedValue);
    }

    [TestMethod]
    [DataRow(GeneratorType.Wind)]
    public void Test_EmissionFactor_Exception(GeneratorType generatorType)
    {
      Assert.ThrowsException<NotImplementedException>( () => { var emissionFactor = EmissionFactorRetriever.Get(generatorType); });
    }

    #endregion Tests

  }
}
