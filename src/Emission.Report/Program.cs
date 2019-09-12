
#region

using System;

using Emission.Report.Library.Job;
using Emission.Report.Library.DependencyInjection;
using Emission.Report.Library.Service;

using Topshelf;
using Autofac;

#endregion

namespace Emission.Report
{
  class Program
  {

    #region Fields

    private const string EMISSION_REPORT = "Emission Report";

    #endregion Fields

    #region Constructor


    #endregion Constructor

    #region Methods

    static void Main(string[] args)
    {
      try
      {
        using (var bootstrapper = new BootStrapper())
        {
          var inputArguments = bootstrapper.ParseArguments(args);
          var container = bootstrapper.Setup(inputArguments);
          
          if (inputArguments.RunAsService)
          {
            HostFactory.Run(configurator =>
            {
              var service = container.Resolve<IReportService>();
              configurator.AddCommandLineDefinition("r", runOnce => { });
              configurator.AddCommandLineDefinition("h", help => { });
              configurator.ApplyCommandLine();

              configurator.Service<IReportService>(configService =>
              {
                configService.ConstructUsing(name => service);
                configService.WhenStarted(proc => proc.Start());
                configService.WhenStopped(proc => proc.Stop());
              });

              configurator.RunAsLocalService();
              configurator.SetDisplayName(EMISSION_REPORT);
              configurator.SetServiceName(EMISSION_REPORT);
            });
          }
          else
          {
            container.Resolve<IReportJob>().Run();
          }
          Environment.Exit(0);
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine("Error while starting the Emission Report: " + ex.Message + Environment.NewLine + "Stack Tracke : " + ex.StackTrace);
        Environment.Exit(1);
      }
    }

    #endregion Methods

  }
}
