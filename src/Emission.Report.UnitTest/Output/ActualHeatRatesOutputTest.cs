
#region

using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Emission.Report.Library.FileOps;

using System.Collections.Generic;

using Emission.Report.Library.Types.Serializable.Input;
using InputDay = Emission.Report.Library.Types.Serializable.Input.Day;

#endregion

namespace Emission.Report.UnitTest.Output
{
  [TestClass]
  public class ActualHeatRatesOutputTest : BaseTestFramework
  {

    #region Setup

    #endregion Setup

    #region Tests

    [TestMethod]
    public void Test_Null_GenerationReport()
    {
      var heatRates = ActualHeatRatesOutput.Get(null);

      Assert.IsNull(heatRates);
    }

    [TestMethod]
    public void Test_Null_Coal()
    {
      var basicReport = new GenerationReport
      {
        Coal = null
      };

      var heatRates = ActualHeatRatesOutput.Get(basicReport);

      Assert.IsNull(heatRates);
    }

    [TestMethod]
    public void Test_Null_Generators()
    {
      var basicReport = new GenerationReport
      {
        Coal = new Coal
        {
          Generators = null
        }
      };

      var heatRates = ActualHeatRatesOutput.Get(basicReport);

      Assert.IsNull(heatRates);
    }

    [TestMethod]
    public void Test_Empty_Generators()
    {
      var basicReport = new GenerationReport
      {
        Coal = new Coal
        {
          Generators = new List<CoalGenerator>()
        }
      };

      var heatRates = ActualHeatRatesOutput.Get(basicReport);

      Assert.IsNull(heatRates);
    }

    [TestMethod]
    [DataRow("TestCoal", 9, 3, 6, "3")]
    public void Test_Input_SingleCoalGenerator(string generatorName, double totalHeatInput, double actualNetGeneration, double emissionsrating, string expected)
    {
      var generation = new Generation { Days = GetBasicListOfInputDays() };
      var coalGenerator = GetBasicCoalGenerator(generatorName, generation, totalHeatInput, actualNetGeneration, emissionsrating);

      var basicReport = new GenerationReport
      {
        Coal = new Coal
        {
          Generators = new List<CoalGenerator>
          {
            { coalGenerator },
          }
        }
      };

      var heatRates = ActualHeatRatesOutput.Get(basicReport);

      Assert.IsNotNull(heatRates);
      Assert.AreEqual(heatRates.Count, 1);
      Assert.AreEqual(heatRates[0].HeatRate, expected);
      Assert.AreEqual(heatRates[0].Name, generatorName);
    }

    [TestMethod]
    public void Test_Input_MultipleCoalGenerator()
    {
      var generation = new Generation { Days = GetBasicListOfInputDays() };
      var coalGenerator1 = GetBasicCoalGenerator("Coal1", generation, 9, 3, 6);
      var coalGenerator2 = GetBasicCoalGenerator("Coal2", generation, 3, 0, 6);

      var basicReport = new GenerationReport
      {
        Coal = new Coal
        {
          Generators = new List<CoalGenerator>
          {
            { coalGenerator1 },
            { coalGenerator2 },
          }
        }
      };

      var heatRates = ActualHeatRatesOutput.Get(basicReport);

      Assert.IsNotNull(heatRates);
      Assert.AreEqual(heatRates.Count, 2);
      Assert.AreEqual(heatRates[0].Name, "Coal1");
      Assert.AreEqual(heatRates[0].HeatRate, "3");
      Assert.AreEqual(heatRates[1].Name, "Coal2");
      Assert.AreEqual(heatRates[1].HeatRate, "Cannot calculate. Actual Net Generation is 0");
    }

    private List<Day> GetBasicListOfInputDays()
    {
      return new List<InputDay>
      {
        { GetBasicInputDay(DateTime.Today.ToString(), 1.3d, 0.9d) },
        { GetBasicInputDay(DateTime.Today.AddDays(-1).ToString(), 1.4d, 0.7d) },
        { GetBasicInputDay(DateTime.Today.AddDays(+1).ToString(), 1.5d, 0.8d) },
      };
    }

    private InputDay GetBasicInputDay(string date, double energy, double price)
    {
      return new InputDay
      {
        Date = date,
        Energy = energy,
        Price = price,
      };
    }

    private CoalGenerator GetBasicCoalGenerator(
      string name, Generation generation, double totalHeatInput, double actualNetGeneration, double emissionsRating)
    {
      return new CoalGenerator
      {
        Name = name,
        Generation = generation,
        TotalHeatInput = totalHeatInput,
        ActualNetGeneration = actualNetGeneration,
        EmissionsRating = emissionsRating,
      };
    }

    #endregion Test

  }
}
