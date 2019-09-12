
#region

using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;

using Emission.Report.Library.Types.Serializable.Input;
using InputDay = Emission.Report.Library.Types.Serializable.Input.Day;

#endregion

namespace Emission.Report.UnitTest.Output
{
  [TestClass]
  public class TotalsOutputTest : BaseTestFramework
  {

    #region Setup

    #endregion Setup

    #region Tests

    [TestMethod]
    public void Test_Null_GenerationReport()
    {
      var totals = TotalsOutput.Get(null);

      Assert.IsNull(totals);
    }

    [TestMethod]
    public void Test_Null_Coal_Gas()
    {
      var basicReport = new GenerationReport
      {
        Coal = null,
        Gas = null,
        Wind = null,
      };

      var totals = TotalsOutput.Get(basicReport);

      Assert.IsNull(totals);
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

      var totals = TotalsOutput.Get(basicReport);

      Assert.IsNull(totals);
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

      var totals = TotalsOutput.Get(basicReport);

      Assert.IsNull(totals);
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

      var totals = TotalsOutput.Get(basicReport);

      var expectedTotal = 
        basicReport.Coal.Generators.Select(x => x.Generation).SelectMany(x => x.Days).Sum(x => x.Energy * x.Price * ValueFactorMedium);

      Assert.IsNotNull(totals);
      Assert.AreEqual(totals.Generator.Count, 1);
      Assert.AreEqual(totals.Generator[0].Name, generatorName);
      Assert.AreEqual(totals.Generator[0].Total, expectedTotal);
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

      var totals = TotalsOutput.Get(basicReport);

      var expectedTotal =
        basicReport.Gas.Generators.Select(x => x.Generation).SelectMany(x => x.Days).Sum(x => x.Energy * x.Price * ValueFactorMedium);

      Assert.IsNotNull(totals);
      Assert.AreEqual(totals.Generator.Count, 1);
      Assert.AreEqual(totals.Generator[0].Name, generatorName);
      Assert.AreEqual(totals.Generator[0].Total, expectedTotal);
    }

    [TestMethod]
    [DataRow("TestWindOffShore", 9, 3, 6, "offshore", ValueFactorHigh)]
    [DataRow("TestWindOnShore", 9, 3, 6, "onshore", ValueFactorHigh)]
    public void Test_Input_SingleWindGenerator(string generatorName, double totalHeatInput, double actualNetGeneration, double emissionsrating, string location, double valueFactor)
    {
      var generation = new Generation { Days = GetBasicListOfInputDays() };
      var windGenerator = GetBasicWindGenerator(generatorName, generation, totalHeatInput, actualNetGeneration, emissionsrating, location);

      var basicReport = new GenerationReport
      {
        Wind = new Wind
        {
          Generators = new List<WindGenerator>
          {
            { windGenerator },
          }
        }
      };

      var totals = TotalsOutput.Get(basicReport);

      var expectedTotal =
        basicReport.Wind.Generators.Select(x => x.Generation).SelectMany(x => x.Days).Sum(x => x.Energy * x.Price * valueFactor);

      Assert.IsNotNull(totals);
      Assert.AreEqual(totals.Generator.Count, 1);
      Assert.AreEqual(totals.Generator[0].Name, generatorName);
      Assert.AreEqual(totals.Generator[0].Total, expectedTotal);
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

      var totals = TotalsOutput.Get(basicReport);

      var expectedTotalCoal1 =
        basicReport.Coal.Generators.Where(x => x.Name == coalGenerator1.Name).Select(x => x.Generation).SelectMany(x => x.Days).Sum(x => x.Energy * x.Price * ValueFactorMedium);

      var expectedTotalCoal2 =
        basicReport.Coal.Generators.Where(x => x.Name == coalGenerator2.Name).Select(x => x.Generation).SelectMany(x => x.Days).Sum(x => x.Energy * x.Price * ValueFactorMedium);

      Assert.IsNotNull(totals);
      Assert.AreEqual(totals.Generator.Count, 2);
      Assert.AreEqual(totals.Generator[0].Name, coalGenerator1.Name);
      Assert.AreEqual(totals.Generator[1].Name, coalGenerator2.Name);
      Assert.AreEqual(totals.Generator[0].Total, expectedTotalCoal1);
      Assert.AreEqual(totals.Generator[1].Total, expectedTotalCoal2);
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

      var totals = TotalsOutput.Get(basicReport);

      var expectedTotalGas1 =
        basicReport.Gas.Generators.Where(x => x.Name == gasGenerator1.Name).Select(x => x.Generation).SelectMany(x => x.Days).Sum(x => x.Energy * x.Price * ValueFactorMedium);

      var expectedTotalGas2 =
        basicReport.Gas.Generators.Where(x => x.Name == gasGenerator2.Name).Select(x => x.Generation).SelectMany(x => x.Days).Sum(x => x.Energy * x.Price * ValueFactorMedium);

      Assert.IsNotNull(totals);
      Assert.AreEqual(totals.Generator.Count, 2);
      Assert.AreEqual(totals.Generator[0].Name, gasGenerator1.Name);
      Assert.AreEqual(totals.Generator[1].Name, gasGenerator2.Name);
      Assert.AreEqual(totals.Generator[0].Total, expectedTotalGas1);
      Assert.AreEqual(totals.Generator[1].Total, expectedTotalGas2);
    }

    [TestMethod]
    [DataRow(9, 3, 6, "offshore", ValueFactorLow)]
    [DataRow(9, 3, 6, "onshore", ValueFactorHigh)]
    public void Test_Input_MultipleWindGenerator(double totalHeatInput, double actualNetGeneration, double emissionsrating, string location, double valueFactor)
    {
      var generation1 = new Generation { Days = GetBasicListOfInputDays() };
      var generation2 = new Generation { Days = GetBasicListOfInputDays_ForMultiple() };
      var windGenerator1 = GetBasicWindGenerator("Wind1", generation1, totalHeatInput, actualNetGeneration, emissionsrating, location);
      var windGenerator2 = GetBasicWindGenerator("Wind2", generation2, totalHeatInput, actualNetGeneration, emissionsrating, location);

      var basicReport = new GenerationReport
      {
        Wind = new Wind
        {
          Generators = new List<WindGenerator>
          {
            { windGenerator1 },
            { windGenerator2 },
          }
        }
      };

      var totals = TotalsOutput.Get(basicReport);

      var expectedTotalCoal1 =
        basicReport.Wind.Generators.Where(x => x.Name == windGenerator1.Name).Select(x => x.Generation).SelectMany(x => x.Days).Sum(x => x.Energy * x.Price * valueFactor);

      var expectedTotalCoal2 =
        basicReport.Wind.Generators.Where(x => x.Name == windGenerator2.Name).Select(x => x.Generation).SelectMany(x => x.Days).Sum(x => x.Energy * x.Price * valueFactor);

      Assert.IsNotNull(totals);
      Assert.AreEqual(totals.Generator.Count, 2);
      Assert.AreEqual(totals.Generator[0].Name, windGenerator1.Name);
      Assert.AreEqual(totals.Generator[1].Name, windGenerator2.Name);
      Assert.AreEqual(totals.Generator[0].Total, expectedTotalCoal1);
      Assert.AreEqual(totals.Generator[1].Total, expectedTotalCoal2);
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
          }
        },
        Gas = new Gas
        {
          Generators = new List<GasGenerator>
          {
            { gasGenerator1 },
            { gasGenerator2 },
          }
        }
      };

      var totals = TotalsOutput.Get(basicReport);

      var expectedTotalCoal1 =
        basicReport.Coal.Generators.Where(x => x.Name == coalGenerator1.Name).Select(x => x.Generation).SelectMany(x => x.Days).Sum(x => x.Energy * x.Price * ValueFactorMedium);

      var expectedTotalCoal2 =
        basicReport.Coal.Generators.Where(x => x.Name == coalGenerator2.Name).Select(x => x.Generation).SelectMany(x => x.Days).Sum(x => x.Energy * x.Price * ValueFactorMedium);

      var expectedTotalGas1 =
        basicReport.Gas.Generators.Where(x => x.Name == gasGenerator1.Name).Select(x => x.Generation).SelectMany(x => x.Days).Sum(x => x.Energy * x.Price * ValueFactorMedium);

      var expectedTotalGas2 =
        basicReport.Gas.Generators.Where(x => x.Name == gasGenerator2.Name).Select(x => x.Generation).SelectMany(x => x.Days).Sum(x => x.Energy * x.Price * ValueFactorMedium);

      Assert.IsNotNull(totals);
      Assert.AreEqual(totals.Generator.Count, 4);
      Assert.AreEqual(totals.Generator[0].Name, coalGenerator1.Name);
      Assert.AreEqual(totals.Generator[1].Name, coalGenerator2.Name);
      Assert.AreEqual(totals.Generator[2].Name, gasGenerator1.Name);
      Assert.AreEqual(totals.Generator[3].Name, gasGenerator2.Name);
      Assert.AreEqual(totals.Generator[0].Total, expectedTotalCoal1);
      Assert.AreEqual(totals.Generator[1].Total, expectedTotalCoal2);
      Assert.AreEqual(totals.Generator[2].Total, expectedTotalGas1);
      Assert.AreEqual(totals.Generator[3].Total, expectedTotalGas2);
    }

    [TestMethod]
    [DataRow(9, 3, 6)]
    public void Test_Input_Multiple_CoalGasWindGenerator(double totalHeatInput, double actualNetGeneration, double emissionsrating)
    {
      var generation1 = new Generation { Days = GetBasicListOfInputDays() };
      var generation2 = new Generation { Days = GetBasicListOfInputDays_ForMultiple() };
      var coalGenerator1 = GetBasicCoalGenerator("Coal1", generation1, totalHeatInput, actualNetGeneration, emissionsrating);
      var coalGenerator2 = GetBasicCoalGenerator("Coal2", generation2, totalHeatInput, actualNetGeneration, emissionsrating);

      var generation3 = new Generation { Days = GetBasicListOfInputDays_Gas() };
      var generation4 = new Generation { Days = GetBasicListOfInputDays_ForMultiple_Gas() };
      var gasGenerator1 = GetBasicGasGenerator("Gas1", generation3, totalHeatInput, actualNetGeneration, emissionsrating);
      var gasGenerator2 = GetBasicGasGenerator("Gas2", generation4, totalHeatInput, actualNetGeneration, emissionsrating);

      var generation5 = new Generation { Days = GetBasicListOfInputDays() };
      var generation6 = new Generation { Days = GetBasicListOfInputDays_ForMultiple() };
      var windGenerator1 = GetBasicWindGenerator("Wind1", generation5, totalHeatInput, actualNetGeneration, emissionsrating, "offshore");
      var windGenerator2 = GetBasicWindGenerator("Wind2", generation6, totalHeatInput, actualNetGeneration, emissionsrating, "onshore");


      var basicReport = new GenerationReport
      {
        Coal = new Coal
        {
          Generators = new List<CoalGenerator>
          {
            { coalGenerator1 },
            { coalGenerator2 },
          }
        },
        Gas = new Gas
        {
          Generators = new List<GasGenerator>
          {
            { gasGenerator1 },
            { gasGenerator2 },
          }
        },
        Wind = new Wind
        {
          Generators = new List<WindGenerator>
          {
            { windGenerator1 },
            { windGenerator2 },
          }
        }
      };

      var totals = TotalsOutput.Get(basicReport);

      var expectedTotalCoal1 =
        basicReport.Coal.Generators.Where(x => x.Name == coalGenerator1.Name).Select(x => x.Generation).SelectMany(x => x.Days).Sum(x => x.Energy * x.Price * ValueFactorMedium);

      var expectedTotalCoal2 =
        basicReport.Coal.Generators.Where(x => x.Name == coalGenerator2.Name).Select(x => x.Generation).SelectMany(x => x.Days).Sum(x => x.Energy * x.Price * ValueFactorMedium);

      var expectedTotalGas1 =
        basicReport.Gas.Generators.Where(x => x.Name == gasGenerator1.Name).Select(x => x.Generation).SelectMany(x => x.Days).Sum(x => x.Energy * x.Price * ValueFactorMedium);

      var expectedTotalGas2 =
        basicReport.Gas.Generators.Where(x => x.Name == gasGenerator2.Name).Select(x => x.Generation).SelectMany(x => x.Days).Sum(x => x.Energy * x.Price * ValueFactorMedium);

      var expectedTotalWind1 =
        basicReport.Wind.Generators.Where(x => x.Name == windGenerator1.Name).Select(x => x.Generation).SelectMany(x => x.Days).Sum(x => x.Energy * x.Price * ValueFactorLow);

      var expectedTotalWind2 =
        basicReport.Wind.Generators.Where(x => x.Name == windGenerator2.Name).Select(x => x.Generation).SelectMany(x => x.Days).Sum(x => x.Energy * x.Price * ValueFactorHigh);

      Assert.IsNotNull(totals);
      Assert.AreEqual(totals.Generator.Count, 6);
      Assert.AreEqual(totals.Generator[0].Name, coalGenerator1.Name);
      Assert.AreEqual(totals.Generator[1].Name, coalGenerator2.Name);
      Assert.AreEqual(totals.Generator[2].Name, gasGenerator1.Name);
      Assert.AreEqual(totals.Generator[3].Name, gasGenerator2.Name);
      Assert.AreEqual(totals.Generator[4].Name, windGenerator1.Name);
      Assert.AreEqual(totals.Generator[5].Name, windGenerator2.Name);
      Assert.AreEqual(totals.Generator[0].Total, expectedTotalCoal1);
      Assert.AreEqual(totals.Generator[1].Total, expectedTotalCoal2);
      Assert.AreEqual(totals.Generator[2].Total, expectedTotalGas1);
      Assert.AreEqual(totals.Generator[3].Total, expectedTotalGas2);
      Assert.AreEqual(totals.Generator[4].Total, expectedTotalWind1);
      Assert.AreEqual(totals.Generator[5].Total, expectedTotalWind2);
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

    private WindGenerator GetBasicWindGenerator(
      string name, Generation generation, double totalHeatInput, double actualNetGeneration, double emissionsRating, string location)
    {
      return new WindGenerator
      {
        Name = name,
        Generation = generation,
        Location = location,
      };
    }

    #endregion Test

  }
}
