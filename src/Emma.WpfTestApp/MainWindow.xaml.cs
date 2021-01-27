using System;
using System.Windows;
using Emma.Core;
using Emma.Core.Extensions;
using Emma.Core.MethodSources;
using Emma.XamlControls;
using Emma.XamlControls.ViewModels;

namespace Emma.WpfTestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            // Allow all controls in the window to 
            // bind to the ViewModel by setting the 
            // DataContext, which propagates down 
            // the element tree.
            // window.DataContext = "viewModel";

            // When the ViewModel asks to be closed, 
            // close the window.
            // EventHandler handler = null;
            // handler = delegate
            // {
            //     viewModel.RequestClose -= handler;
            //     window.Close();
            // };
            // viewModel.RequestClose += handler;

        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            var methods = typeof(StringExtensions).ExtensionMethods();
            var testSrc = new ExtensionMethodsSource
            {
                Methods = ExtensionMethodParser.Parse(methods, DateTime.Now)
            };
            var lib = new ExtensionMethodLibrary(testSrc);

            var mainEmmaToolWindowControl = new MainEmmaToolWindowControl
            {
                DataContext = new MainEmmaToolWindowViewModel(lib)
            };
            Content = mainEmmaToolWindowControl;
            mainEmmaToolWindowControl.Focus();
        }
    }
}
