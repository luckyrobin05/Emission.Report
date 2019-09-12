
#region

using System;
using System.Collections.Generic;
using System.Text;

using Autofac;

using Emission.Report.Common.Logging;
using log4net.Config;

using Emission.Report.Library.Settings;
using Emission.Report.Library.Process;
using Emission.Report.Library.Enums;
using Emission.Report.Library.Job;
using Emission.Report.Library.Service;
using Emission.Report.Library.FileOps;
using Emission.Report.Library.Output;
using Emission.Report.Library.Calculate.ActualHeatRate;
using Emission.Report.Library.Calculate.Emissions;
using Emission.Report.Library.Calculate.GenerationValue;
using NDesk.Options;

#endregion

namespace Emission.Report.Library.DependencyInjection
{

  public class BootStrapper : IDisposable
  {

    #region Fields

    private IContainer _container;

    #endregion Fields

    #region Methods

    public void Dispose()
    {
      Dispose(true);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (disposing && this._container != null)
      {
        this._container = null;
      }
    }

    public StartupParams ParseArguments(IEnumerable<string> inputAruments)
    {
      var runAsService = false;
      var showHelp = false;

      var inputOptions = new OptionSet
      {
        { "r:", "RunAsSerivce: true, will run as service, default false, run as console", r => { bool.TryParse(r, out runAsService); } },
        { "h:", "Help!", h => { showHelp = h != null; } },
      };

      inputOptions.Parse(inputAruments);

      if (showHelp)
      {
        this.ShowHelp(inputOptions);
        Environment.Exit(0);
      }

      return new StartupParams(runAsService);
    }

    private void ShowHelp(OptionSet inputAruments)
    {
      Console.WriteLine("Options for Starting EmissionCalculator:");
      Console.WriteLine();
      inputAruments.WriteOptionDescriptions(Console.Out);
    }

    public IContainer Setup(IStartupParams startupParams)
    {
      XmlConfigurator.Configure();

      var containerBuiler = new ContainerBuilder();

      containerBuiler.RegisterType<LoggerCreator>().As<ILoggerCreator>().SingleInstance();
      containerBuiler.RegisterType<ConfigSettings>().As<IConfigSettings>().SingleInstance();

      containerBuiler.RegisterType<ReportJob>().As<IReportJob>();
      containerBuiler.RegisterType<ReportService>().As<IReportService>();
      containerBuiler.RegisterType<InputReportProcessor>().As<IInputReportProcessor>();

      containerBuiler.RegisterType<FileSerializerBuilder>().As<IFileSerializerBuilder>();
      containerBuiler.RegisterType<FolderWatcher>().As<IFolderWatcher>();
      containerBuiler.RegisterType<FileMover>().As<IFileMover>();

      containerBuiler.RegisterType<TotalGenerationValueCalculator>().As<ITotalGenerationValueCalculator>();
      containerBuiler.RegisterType<DailyGenerationValueCalculator>().As<IDailyGenerationValueCalculator>();
      containerBuiler.RegisterType<ValueFactorRetriever>().As<IValueFactorRetriever>();
      containerBuiler.RegisterType<DailyEmissionValueCalculator>().As<IDailyEmissionValueCalculator>();
      containerBuiler.RegisterType<CoalActualHeatRateCalculator>().As<IActualHeatRateCalculator>();

      containerBuiler.RegisterType<TotalsOutput>().As<ITotalsOutput>();
      containerBuiler.RegisterType<EmissionFactorRetriever>().As<IEmissionFactorRetriever>();
      containerBuiler.RegisterType<MaxEmissionGeneratorsOutput>().As<IMaxEmissionGeneratorsOutput>();
      containerBuiler.RegisterType<ActualHeatRatesOutput>().As<IActualHeatRatesOutput>();
      containerBuiler.RegisterType<OutputReportCreator>().As<IOutputReportCreator>();

      var container = containerBuiler.Build();
      return container;
    }


    #endregion Methods

  }
}
