using Microsoft.VisualStudio.Shell;
using System.Runtime.InteropServices;
using Emma.Core;
using Emma.Core.Extensions;
using Emma.Core.MethodSources;
using Emma.XamlControls;
using Emma.XamlControls.ViewModels;

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
        public EmmaMainToolWindowPane() : base(null)
        {
            this.Caption = "EMMA - Extension Method Manager";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.

            var testSrc = new ExtensionMethodsSource
            {
                Methods = ExtensionMethodParser.Parse(
                    typeof(StringExtensions).Assembly)
            };
            var lib = new ExtensionMethodLibrary(testSrc);
            
            var mainEmmaToolWindowControl = new MainEmmaToolWindowControl();
            mainEmmaToolWindowControl.DataContext = new MainEmmaToolWindowViewModel(lib);
            this.Content = mainEmmaToolWindowControl;
        }
    }
}
