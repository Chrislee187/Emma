using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace Emma.VSIX
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(EmmaPackage.PackageGuidString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(Emma.VSIX.EmmaMainToolWindowPane), Style = VsDockStyle.Tabbed, DockedWidth = 300, Window = "DocumentWell", Orientation = ToolWindowOrientation.Left)]
    [InstalledProductRegistration("Emma", "Extension Method Manager", "1.0")]
    public sealed class EmmaPackage : AsyncPackage
    {
         /// <summary>
        /// Emma.VSIXPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "2b898cca-9f4c-4b71-82d5-fbb748c90b81";

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down.</param>
        /// <param name="progress">A provider for progress updates.</param>
        /// <returns>A task representing the async work of package initialization, or an already completed task if there is none. Do not return null from this method.</returns>
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
            return "Extension Method Manager";
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
