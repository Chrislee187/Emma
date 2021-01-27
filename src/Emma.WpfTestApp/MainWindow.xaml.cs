using System;
using System.Windows;
using Emma.Core;
using Emma.Core.Extensions;
using Emma.Core.MethodSources;
using Emma.XamlControls;
using Emma.XamlControls.ViewModels;

namespace Emma.WpfTestApp
{
    public partial class MainWindow : Window
    {
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
