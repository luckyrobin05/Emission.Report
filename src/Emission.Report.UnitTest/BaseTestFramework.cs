
#region Fields

using Moq;
using Emission.Report.Common.Logging;
using Emission.Report.Library.Settings;
using Emission.Report.Library.FileOps;
using Emission.Report.Library.Types.Serializable.Input;
using Emission.Report.Library.Types.Serializable.Output;
using Emission.Report.Library.Types.Serializable.Reference;
using Emission.Report.Library.Calculate.ActualHeatRate;
using Emission.Report.Library.Calculate.Emissions;
using Emission.Report.Library.Calculate.GenerationValue;

using Emission.Report.Library.Output;

#endregion Fields

namespace Emission.Report.UnitTest
{
  public class BaseTestFramework
  {

    #region Fields

    protected const double ValueFactorLow = 1;
    protected const double ValueFactorMedium = 1;
    protected const double ValueFactorHigh = 1;
    protected const double EmissionFactorLow = 1;
    protected const double EmissionFactorMedium = 1;
    protected const double EmissionFactorHigh = 1;

    protected const string InputFileFolder = @"C:\Emissions\Input";
    protected const string InputSuccessFileFolder = @"C:\Emissions\Input\Success";
    protected const string InputFailureFileFolder = @"C:\Emissions\Input\Failure";
    protected const string OutputFileFolder = @"C:\Emissions\Output";
    protected const string OutputFilePrefix = @"Output";

    private Mock<ILoggerCreator> _loggerCreatorMock;
    private Mock<ILogger> _loggerMock;

    private Mock<IConfigSettings> _configSettingsMock;
    private Mock<IFileSerializer> _fileSerializerMock;

    private ReferenceData _referenceData;

    private IActualHeatRateCalculator _actualHeatRateCalculator;
    private IDailyEmissionValueCalculator _dailyEmissionValueCalculator;
    private IDailyGenerationValueCalculator _dailyGenerationValueCalculator;
    private ITotalGenerationValueCalculator _totalGenerationValueCalculator;

    private IFolderWatcher _folderWatch;
    private IFileMover _fileMover;
    private IFileSerializerBuilder _fileSerializerBuilder;

    private IEmissionFactorRetriever _emissionFactorRetriever;

    private IValueFactorRetriever _valueFactorRetriever;

    private IActualHeatRatesOutput _actualHeatRatesOutput;
    private IMaxEmissionGeneratorsOutput _maxEmissionGeneratorsOutput;
    private ITotalsOutput _totalsOutput;

    #endregion Fields

    #region Properties

    protected Mock<ILogger> LoggerMock
    {
      get
      {
        if (_loggerMock == null)
        {
          _loggerMock = new Mock<ILogger>(); ;
        }
        return _loggerMock;
      }
    }

    protected Mock<ILoggerCreator> LoggerCreatorMock
    {
      get
      {
        if (_loggerCreatorMock == null)
        {
          _loggerCreatorMock = new Mock<ILoggerCreator>();
          _loggerCreatorMock.Setup(x => x.GetTypeLogger<CoalActualHeatRateCalculator>()).Returns(LoggerMock.Object);
          _loggerCreatorMock.Setup(x => x.GetTypeLogger<TotalGenerationValueCalculator>()).Returns(LoggerMock.Object);
          _loggerCreatorMock.Setup(x => x.GetTypeLogger<FolderWatcher>()).Returns(LoggerMock.Object);
          _loggerCreatorMock.Setup(x => x.GetTypeLogger<FileMover>()).Returns(LoggerMock.Object);
          _loggerCreatorMock.Setup(x => x.GetTypeLogger<FileSerializerBuilder>()).Returns(LoggerMock.Object);
          _loggerCreatorMock.Setup(x => x.GetTypeLogger<FolderWatcher>()).Returns(LoggerMock.Object);
          _loggerCreatorMock.Setup(x => x.GetTypeLogger<EmissionFactorRetriever>()).Returns(LoggerMock.Object);
          _loggerCreatorMock.Setup(x => x.GetTypeLogger<ValueFactorRetriever>()).Returns(LoggerMock.Object);
          _loggerCreatorMock.Setup(x => x.GetTypeLogger<ActualHeatRatesOutput>()).Returns(LoggerMock.Object);
          _loggerCreatorMock.Setup(x => x.GetTypeLogger<MaxEmissionGeneratorsOutput>()).Returns(LoggerMock.Object);
          _loggerCreatorMock.Setup(x => x.GetTypeLogger<TotalsOutput>()).Returns(LoggerMock.Object);
        }
        return _loggerCreatorMock;
      }
    }

    protected Mock<IFileSerializer> FileSerializerMock
    {
      get
      {
        if (_fileSerializerMock == null)
        {
          _fileSerializerMock = new Mock<IFileSerializer>();
          _fileSerializerMock.Setup(x => x.Read<ReferenceData>(It.IsAny<string>())).Returns(ReferenceData);
        }
        return _fileSerializerMock;
      }
    }
    
    private ReferenceData ReferenceData
    {
      get
      {
        if (_referenceData == null)
        {
          _referenceData = new ReferenceData
          {
            Factors = new Factors
            {
              ValueFactor = new ValueFactor
              {
                Low = ValueFactorLow,
                Medium = ValueFactorMedium,
                High = ValueFactorHigh,
              },
              EmissionsFactor = new EmissionsFactor
              {
                Low = EmissionFactorLow,
                Medium = EmissionFactorMedium,
                High = EmissionFactorHigh,
              },
            },
          };
        }
        return _referenceData;
      }
    }
    
    protected Mock<IConfigSettings> ConfigSettingsMock
    {
      get
      {
        if (_configSettingsMock == null)
        {
          _configSettingsMock = new Mock<IConfigSettings>();
          _configSettingsMock.Setup(x => x.InputFileFolder).Returns(InputFileFolder);
          _configSettingsMock.Setup(x => x.InputSuccessFileFolder).Returns(InputSuccessFileFolder);
          _configSettingsMock.Setup(x => x.InputFailureFileFolder).Returns(InputFailureFileFolder);
          _configSettingsMock.Setup(x => x.OutputFileFolder).Returns(OutputFileFolder);
          _configSettingsMock.Setup(x => x.OutputFilePrefix).Returns(OutputFilePrefix);
          _configSettingsMock.Setup(x => x.CurrentReferenceData).Returns(ReferenceData);
        }
        return _configSettingsMock;
      }
    }

    protected IActualHeatRateCalculator CoalActualHeatRateCalculator
    {
      get
      {
        if (_actualHeatRateCalculator == null)
        {
          _actualHeatRateCalculator = new CoalActualHeatRateCalculator();
        }
        return _actualHeatRateCalculator;
      }
    }

    protected IDailyEmissionValueCalculator DailyEmissionValueCalculator
    {
      get
      {
        if (_dailyEmissionValueCalculator == null)
        {
          _dailyEmissionValueCalculator = new DailyEmissionValueCalculator();
        }
        return _dailyEmissionValueCalculator;
      }
    }

    protected IDailyGenerationValueCalculator DailyGenerationValueCalculator
    {
      get
      {
        if (_dailyGenerationValueCalculator == null)
        {
          _dailyGenerationValueCalculator = new DailyGenerationValueCalculator();
        }
        return _dailyGenerationValueCalculator;
      }
    }

    protected IValueFactorRetriever ValueFactorRetriever
    {
      get
      {
        if (_valueFactorRetriever == null)
        {
          _valueFactorRetriever = new ValueFactorRetriever(LoggerCreatorMock.Object, ConfigSettingsMock.Object);
        }
        return _valueFactorRetriever;
      }
    }

    protected ITotalGenerationValueCalculator TotalGenerationValueCalculator
    {
      get
      {
        if (_totalGenerationValueCalculator == null)
        {
          _totalGenerationValueCalculator = 
            new TotalGenerationValueCalculator(LoggerCreatorMock.Object, DailyGenerationValueCalculator);
        }
        return _totalGenerationValueCalculator;
      }
    }

    protected IFolderWatcher FolderWatch
    {
      get
      {
        if (_folderWatch == null)
        {
          _folderWatch =
            new FolderWatcher(LoggerCreatorMock.Object);
        }
        return _folderWatch;
      }
    }

    protected IFileMover FileMover
    {
      get
      {
        if (_fileMover == null)
        {
          _fileMover =
            new FileMover(LoggerCreatorMock.Object);
        }
        return _fileMover;
      }
    }

    protected IFileSerializerBuilder FileSerializerBuilder
    {
      get
      {
        if (_fileSerializerBuilder == null)
        {
          _fileSerializerBuilder =
            new FileSerializerBuilder(LoggerCreatorMock.Object, FileMover);
        }
        return _fileSerializerBuilder;
      }
    }
    
    protected IEmissionFactorRetriever EmissionFactorRetriever
    {
      get
      {
        if (_emissionFactorRetriever == null)
        {
          _emissionFactorRetriever =
            new EmissionFactorRetriever(LoggerCreatorMock.Object, ConfigSettingsMock.Object);
        }
        return _emissionFactorRetriever;
      }
    }

    protected IActualHeatRatesOutput ActualHeatRatesOutput
    {
      get
      {
        if (_actualHeatRatesOutput == null)
        {
          _actualHeatRatesOutput =
            new ActualHeatRatesOutput(LoggerCreatorMock.Object, CoalActualHeatRateCalculator);
        }
        return _actualHeatRatesOutput;
      }
    }

    protected IMaxEmissionGeneratorsOutput MaxEmissionGeneratorsOutput
    {
      get
      {
        if (_maxEmissionGeneratorsOutput == null)
        {
          _maxEmissionGeneratorsOutput =
            new MaxEmissionGeneratorsOutput(LoggerCreatorMock.Object, DailyEmissionValueCalculator, EmissionFactorRetriever);
        }
        return _maxEmissionGeneratorsOutput;
      }
    }

    protected ITotalsOutput TotalsOutput
    {
      get
      {
        if (_totalsOutput == null)
        {
          _totalsOutput =
            new TotalsOutput(LoggerCreatorMock.Object, TotalGenerationValueCalculator, ValueFactorRetriever);
        }
        return _totalsOutput;
      }
    }

    #endregion Properties

    #region Methods

    #endregion Methods

  }
}
