using Microsoft.VisualStudio.Shell;
using System.Runtime.InteropServices;
using System.Windows;
using Emma.Common;
using Emma.Common.ExtensionMethodProviders;
using Emma.Common.MethodSources;
using Emma.XamlControls;
using Emma.XamlControls.ViewModels;
using Octokit;


namespace Emma.VSIX
{
    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid("0dbc2e41-6720-49a1-b05c-2e22aaae9b20")]
    public class EmmaMainToolWindowPane : ToolWindowPane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmmaMainToolWindowPane"/> class.
        /// </summary>
        public EmmaMainToolWindowPane()
        {
            this.Caption = "EMMA - Extension Method Manager";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.


        }

        private OptionPageGrid _options;
        private OptionPageGrid Options()
        {            
            var package = (EmmaPackage)this.Package;

            package.JoinableTaskFactory.RunAsync(async () 
                => _options = (OptionPageGrid) await package.GetServiceAsync(typeof(OptionPageGrid)));

            return _options ?? (OptionPageGrid) package.GetServiceAsync(typeof(OptionPageGrid)).Result;
        }

        protected override void Initialize()
        {
            var package = (EmmaPackage)this.Package;

            var opts = (OptionPageGrid) package.GetServiceAsync(typeof(OptionPageGrid)).Result;

            var owner = string.IsNullOrEmpty(opts.LibraryGithubOwner) ? "chrislee187" : opts.LibraryGithubOwner;
            var repo = string.IsNullOrEmpty(opts.LibraryRepo) ? "methodbrary" : opts.LibraryRepo;

            ExtensionMethodsSource src;
            try
            {
                src = new ExtensionMethodsSource(
                    new GithubRepoEmProvider(owner, repo),
                    new AppDataEmProvider("emma", $"github-methodbrary")
                );

                var lib = new ExtensionMethodLibrary(src);

                var mainEmmaToolWindowControl = new MainEmmaToolWindowControl
                {
                    DataContext = new MainEmmaToolWindowViewModel(lib)
                };
                this.Content = mainEmmaToolWindowControl;
            }
            catch (NotFoundException e)
            {
                MessageBox.Show($"Github repo {owner}/{repo} not found!");
            }

        }
    }
}
