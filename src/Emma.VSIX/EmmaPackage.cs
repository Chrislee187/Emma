using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace Emma.VSIX
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("Emma", "Extension Method Manager", "1.0")]
    [ProvideToolWindow(typeof(EmmaMainToolWindowPane), Style = VsDockStyle.Tabbed, DockedWidth = 300, Window = "DocumentWell", Orientation = ToolWindowOrientation.Left)]
    [Guid(EmmaPackage.PackageGuidString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    public sealed class EmmaPackage : AsyncPackage
    {
        public const string PackageGuidString = "2b898cca-9f4c-4b71-82d5-fbb748c90b81";

        #region Package Members

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync();
            await EmmaCommand.InitializeAsync(this);
        }

        // public override IVsAsyncToolWindowFactory GetAsyncToolWindowFactory(Guid toolWindowType)
        // {
        //     return this;
        //     // return toolWindowType.Equals(Guid.Parse(SampleToolWindow.WindowGuidString)) ? this : null;
        // }

        protected override string GetToolWindowTitle(Type toolWindowType, int id)
        {
            return "EMMA - Extension Method Manager";
        }

        protected override async Task<object> InitializeToolWindowAsync(Type toolWindowType, int id, CancellationToken cancellationToken)
        {
            // Perform as much work as possible in this method which is being run on a background thread.
            // The object returned from this method is passed into the constructor of the SampleToolWindow 
            var dte = await GetServiceAsync(typeof(EnvDTE.DTE)) as EnvDTE80.DTE2;
            //
            // return new State
            // {
            //     DTE = dte
            // };
            return null;
        }

        #endregion
    }
}
