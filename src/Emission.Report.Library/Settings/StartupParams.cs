
#region

using Emission.Report.Library.Enums;

#endregion

namespace Emission.Report.Library.Settings
{
  public class StartupParams : IStartupParams
  {

    #region Fields

    #endregion Fields

    #region Properties

    public bool RunAsService { get; private set; }

    #endregion Properties

    #region Constructor

    public StartupParams(bool runAsService)
    {
      this.RunAsService = runAsService;
    }

    #endregion Constructor

    #region Methods

    #endregion Methods

  }
}
