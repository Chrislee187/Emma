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
        private const string DefaultMethodbrary = "https://github.com/chrislee187/methodbrary";

        /// <summary>
        /// Initializes a new instance of the <see cref="EmmaMainToolWindowPane"/> class.
        /// </summary>
        public EmmaMainToolWindowPane()
        {
            this.Caption = "EMMA - Extension Method Manager";
        }

        protected override void Initialize()
        {
            var package = (EmmaPackage)this.Package;

            var opts = (OptionPageGrid) package.GetServiceAsync(typeof(OptionPageGrid)).Result;

            var repo = string.IsNullOrEmpty(opts.DefaultMethodbraryRepo) 
                ? DefaultMethodbrary : opts.DefaultMethodbraryRepo;

            try
            {
                var emProvider = new GithubCloneEmProvider(repo);
                var appDataEmProvider = new AppDataEmProvider("emma", $"default-methodbrary");
                
                var src = new ExtensionMethodsSource(
                    emProvider,
                    appDataEmProvider
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
                MessageBox.Show($"Github repo {repo} not found!");
            }

        }

        private OptionPageGrid _options;

        private OptionPageGrid Options()
        {            
            var package = (EmmaPackage)this.Package;

            package.JoinableTaskFactory.RunAsync(async () 
                => _options = (OptionPageGrid) await package.GetServiceAsync(typeof(OptionPageGrid)));

            return _options; 
        }
    }
}
