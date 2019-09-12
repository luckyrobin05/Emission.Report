
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
  public class MaxEmissionGeneratorsOutputTest : BaseTestFramework
  {

    #region Setup

    #endregion Setup

    #region Tests

    [TestMethod]
    public void Test_Null_GenerationReport()
    {
      var maxEmissionGenerators = MaxEmissionGeneratorsOutput.Get(null);

      Assert.IsNull(maxEmissionGenerators);
    }

    [TestMethod]
    public void Test_Null_Coal_Gas()
    {
      var basicReport = new GenerationReport
      {
        Coal = null,
        Gas = null,
      };

      var maxEmissionGenerators = MaxEmissionGeneratorsOutput.Get(basicReport);

      Assert.IsNull(maxEmissionGenerators);
    }

    [TestMethod]
    public void Test_Null_Coal_Gas_Generators()
    {
      var basicReport = new GenerationReport
      {
        Coal = new Coal
        {
          Generators = null
        },
        Gas = new Gas
        {
          Generators = null
        },
      };

      var maxEmissionGenerators = ActualHeatRatesOutput.Get(basicReport);

      Assert.IsNull(maxEmissionGenerators);
    }

    [TestMethod]
    public void Test_Empty_Coal_Gas_Generators()
    {
      var basicReport = new GenerationReport
      {
        Coal = new Coal
        {
          Generators = new List<CoalGenerator>()
        },
        Gas = new Gas
        {
          Generators = new List<GasGenerator>()
        },
      };

      var maxEmissionGenerators = ActualHeatRatesOutput.Get(basicReport);

      Assert.IsNull(maxEmissionGenerators);
    }

    [TestMethod]
    [DataRow("TestCoal", 9, 3, 6)]
    public void Test_Input_SingleCoalGenerator(string generatorName, double totalHeatInput, double actualNetGeneration, double emissionsrating)
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

      var maxEmissionGenerators = MaxEmissionGeneratorsOutput.Get(basicReport);

      Assert.IsNotNull(maxEmissionGenerators);
      Assert.AreEqual(maxEmissionGenerators.Day.Count, 3);
      Assert.AreEqual(maxEmissionGenerators.Day[0].Date, DateTime.Today.ToString());
      Assert.AreEqual(maxEmissionGenerators.Day[1].Date, DateTime.Today.AddDays(-1).ToString());
      Assert.AreEqual(maxEmissionGenerators.Day[2].Date, DateTime.Today.AddDays(+1).ToString());
      Assert.AreEqual(maxEmissionGenerators.Day[0].Name, generatorName);
      Assert.AreEqual(maxEmissionGenerators.Day[1].Name, generatorName);
      Assert.AreEqual(maxEmissionGenerators.Day[2].Name, generatorName);
      Assert.AreEqual(maxEmissionGenerators.Day[0].Emission, 1.3d * emissionsrating * EmissionFactorHigh);
      Assert.AreEqual(maxEmissionGenerators.Day[1].Emission, 1.4d * emissionsrating * EmissionFactorHigh);
      Assert.AreEqual(maxEmissionGenerators.Day[2].Emission, 1.5d * emissionsrating * EmissionFactorHigh);
    }

    [TestMethod]
    [DataRow("TestGas", 9, 3, 6)]
    public void Test_Input_SingleGasGenerator(string generatorName, double totalHeatInput, double actualNetGeneration, double emissionsrating)
    {
      var generation = new Generation { Days = GetBasicListOfInputDays() };
      var gasGenerator = GetBasicGasGenerator(generatorName, generation, totalHeatInput, actualNetGeneration, emissionsrating);

      var basicReport = new GenerationReport
      {
        Gas = new Gas
        {
          Generators = new List<GasGenerator>
          {
            { gasGenerator },
          }
        }
      };

      var maxEmissionGenerators = MaxEmissionGeneratorsOutput.Get(basicReport);

      Assert.IsNotNull(maxEmissionGenerators);
      Assert.AreEqual(maxEmissionGenerators.Day.Count, 3);
      Assert.AreEqual(maxEmissionGenerators.Day[0].Date, DateTime.Today.ToString());
      Assert.AreEqual(maxEmissionGenerators.Day[1].Date, DateTime.Today.AddDays(-1).ToString());
      Assert.AreEqual(maxEmissionGenerators.Day[2].Date, DateTime.Today.AddDays(+1).ToString());
      Assert.AreEqual(maxEmissionGenerators.Day[0].Name, generatorName);
      Assert.AreEqual(maxEmissionGenerators.Day[1].Name, generatorName);
      Assert.AreEqual(maxEmissionGenerators.Day[2].Name, generatorName);
      Assert.AreEqual(maxEmissionGenerators.Day[0].Emission, 1.3d * emissionsrating * EmissionFactorMedium);
      Assert.AreEqual(maxEmissionGenerators.Day[1].Emission, 1.4d * emissionsrating * EmissionFactorMedium);
      Assert.AreEqual(maxEmissionGenerators.Day[2].Emission, 1.5d * emissionsrating * EmissionFactorMedium);
    }

    [TestMethod]
    [DataRow(9, 3, 6)]
    public void Test_Input_MultipleCoalGenerator(double totalHeatInput, double actualNetGeneration, double emissionsrating)
    {
      var generation1 = new Generation { Days = GetBasicListOfInputDays() };
      var generation2 = new Generation { Days = GetBasicListOfInputDays_ForMultiple() };
      var coalGenerator1 = GetBasicCoalGenerator("Coal1", generation1, totalHeatInput, actualNetGeneration, emissionsrating);
      var coalGenerator2 = GetBasicCoalGenerator("Coal2", generation2, totalHeatInput, actualNetGeneration, emissionsrating);

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

      var maxEmissionGenerators = MaxEmissionGeneratorsOutput.Get(basicReport);

      Assert.IsNotNull(maxEmissionGenerators);
      Assert.AreEqual(maxEmissionGenerators.Day.Count, 3);
      Assert.AreEqual(maxEmissionGenerators.Day[0].Date, DateTime.Today.ToString());
      Assert.AreEqual(maxEmissionGenerators.Day[1].Date, DateTime.Today.AddDays(-1).ToString());
      Assert.AreEqual(maxEmissionGenerators.Day[2].Date, DateTime.Today.AddDays(+1).ToString());
      Assert.AreEqual(maxEmissionGenerators.Day[0].Name, "Coal2");
      Assert.AreEqual(maxEmissionGenerators.Day[1].Name, "Coal1");
      Assert.AreEqual(maxEmissionGenerators.Day[2].Name, "Coal2");
      Assert.AreEqual(maxEmissionGenerators.Day[0].Emission, 1.4d * emissionsrating * EmissionFactorMedium);
      Assert.AreEqual(maxEmissionGenerators.Day[1].Emission, 1.4d * emissionsrating * EmissionFactorMedium);
      Assert.AreEqual(maxEmissionGenerators.Day[2].Emission, 1.5d * emissionsrating * EmissionFactorMedium);
    }

    [TestMethod]
    [DataRow(9, 3, 6)]
    public void Test_Input_MultipleGasGenerator(double totalHeatInput, double actualNetGeneration, double emissionsrating)
    {
      var generation1 = new Generation { Days = GetBasicListOfInputDays() };
      var generation2 = new Generation { Days = GetBasicListOfInputDays_ForMultiple() };
      var gasGenerator1 = GetBasicGasGenerator("Gas1", generation1, totalHeatInput, actualNetGeneration, emissionsrating);
      var gasGenerator2 = GetBasicGasGenerator("Gas2", generation2, totalHeatInput, actualNetGeneration, emissionsrating);

      var basicReport = new GenerationReport
      {
        Gas = new Gas
        {
          Generators = new List<GasGenerator>
          {
            { gasGenerator1 },
            { gasGenerator2 },
          }
        }
      };

      var maxEmissionGenerators = MaxEmissionGeneratorsOutput.Get(basicReport);

      Assert.IsNotNull(maxEmissionGenerators);
      Assert.AreEqual(maxEmissionGenerators.Day.Count, 3);
      Assert.AreEqual(maxEmissionGenerators.Day[0].Date, DateTime.Today.ToString());
      Assert.AreEqual(maxEmissionGenerators.Day[1].Date, DateTime.Today.AddDays(-1).ToString());
      Assert.AreEqual(maxEmissionGenerators.Day[2].Date, DateTime.Today.AddDays(+1).ToString());
      Assert.AreEqual(maxEmissionGenerators.Day[0].Name, "Gas2");
      Assert.AreEqual(maxEmissionGenerators.Day[1].Name, "Gas1");
      Assert.AreEqual(maxEmissionGenerators.Day[2].Name, "Gas2");
      Assert.AreEqual(maxEmissionGenerators.Day[0].Emission, 1.4d * emissionsrating * EmissionFactorMedium);
      Assert.AreEqual(maxEmissionGenerators.Day[1].Emission, 1.4d * emissionsrating * EmissionFactorMedium);
      Assert.AreEqual(maxEmissionGenerators.Day[2].Emission, 1.5d * emissionsrating * EmissionFactorMedium);
    }

    [TestMethod]
    [DataRow(9, 3, 6)]
    public void Test_Input_Multiple_CoalGasGenerator(double totalHeatInput, double actualNetGeneration, double emissionsrating)
    {
      var generation1 = new Generation { Days = GetBasicListOfInputDays() };
      var generation2 = new Generation { Days = GetBasicListOfInputDays_ForMultiple() };
      var coalGenerator1 = GetBasicCoalGenerator("Coal1", generation1, totalHeatInput, actualNetGeneration, emissionsrating);
      var coalGenerator2 = GetBasicCoalGenerator("Coal2", generation2, totalHeatInput, actualNetGeneration, emissionsrating);

      var generation3 = new Generation { Days = GetBasicListOfInputDays_Gas() };
      var generation4 = new Generation { Days = GetBasicListOfInputDays_ForMultiple_Gas() };
      var gasGenerator1 = GetBasicGasGenerator("Gas1", generation3, totalHeatInput, actualNetGeneration, emissionsrating);
      var gasGenerator2 = GetBasicGasGenerator("Gas2", generation4, totalHeatInput, actualNetGeneration, emissionsrating);

      var basicReport = new GenerationReport
      {
        Coal = new Coal
        {
          Generators = new List<CoalGenerator>
          {
            { coalGenerator1 },
            { coalGenerator2 },
          },
        },
        Gas = new Gas
        {
          Generators = new List<GasGenerator>
          {
            { gasGenerator1 },
            { gasGenerator2 },
          }
        },
      };

      var maxEmissionGenerators = MaxEmissionGeneratorsOutput.Get(basicReport);

      Assert.IsNotNull(maxEmissionGenerators);
      Assert.AreEqual(maxEmissionGenerators.Day.Count, 3);
      Assert.AreEqual(maxEmissionGenerators.Day[0].Date, DateTime.Today.ToString());
      Assert.AreEqual(maxEmissionGenerators.Day[1].Date, DateTime.Today.AddDays(-1).ToString());
      Assert.AreEqual(maxEmissionGenerators.Day[2].Date, DateTime.Today.AddDays(+1).ToString());
      Assert.AreEqual(maxEmissionGenerators.Day[0].Name, "Gas1");
      Assert.AreEqual(maxEmissionGenerators.Day[1].Name, "Coal1");
      Assert.AreEqual(maxEmissionGenerators.Day[2].Name, "Gas2");
      Assert.AreEqual(maxEmissionGenerators.Day[0].Emission, 1.9d * emissionsrating * EmissionFactorMedium);
      Assert.AreEqual(maxEmissionGenerators.Day[1].Emission, 1.4d * emissionsrating * EmissionFactorMedium);
      Assert.AreEqual(maxEmissionGenerators.Day[2].Emission, 1.89d * emissionsrating * EmissionFactorMedium);
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

    private List<Day> GetBasicListOfInputDays_ForMultiple()
    {
      return new List<InputDay>
      {
        { GetBasicInputDay(DateTime.Today.ToString(), 1.4d, 0.9d) },
        { GetBasicInputDay(DateTime.Today.AddDays(-1).ToString(), 1.2d, 0.7d) },
        { GetBasicInputDay(DateTime.Today.AddDays(+1).ToString(), 1.5d, 0.8d) },
      };
    }

    private List<Day> GetBasicListOfInputDays_Gas()
    {
      return new List<InputDay>
      {
        { GetBasicInputDay(DateTime.Today.ToString(), 1.9d, 0.9d) },
        { GetBasicInputDay(DateTime.Today.AddDays(-1).ToString(), 1.1d, 0.7d) },
        { GetBasicInputDay(DateTime.Today.AddDays(+1).ToString(), 1.3d, 0.8d) },
      };
    }

    private List<Day> GetBasicListOfInputDays_ForMultiple_Gas()
    {
      return new List<InputDay>
      {
        { GetBasicInputDay(DateTime.Today.ToString(), 1.4d, 0.9d) },
        { GetBasicInputDay(DateTime.Today.AddDays(-1).ToString(), 1.34d, 0.7d) },
        { GetBasicInputDay(DateTime.Today.AddDays(+1).ToString(), 1.89d, 0.8d) },
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

    private GasGenerator GetBasicGasGenerator(
      string name, Generation generation, double totalHeatInput, double actualNetGeneration, double emissionsRating)
    {
      return new GasGenerator
      {
        Name = name,
        Generation = generation,
        EmissionsRating = emissionsRating,
      };
    }

    #endregion Test

  }
}
