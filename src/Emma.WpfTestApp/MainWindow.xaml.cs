using System;
using System.IO;
using System.Linq;
using System.Windows;
using Emma.Common;
using Emma.Common.Extensions;
using Emma.Common.MethodSources;
using Emma.XamlControls;
using Emma.XamlControls.ViewModels;

namespace Emma.WpfTestApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            var mainEmmaToolWindowControl = new MainEmmaToolWindowControl
            {
                DataContext = new MainEmmaToolWindowViewModel(CreateTestLibrary())
            };
            Content = mainEmmaToolWindowControl;
            mainEmmaToolWindowControl.Focus();
        }

        private static ExtensionMethodLibrary CreateTestLibrary()
        {
            var assemblyMethods = ExtensionMethodParser.Parse(typeof(StringExtensions).ExtensionMethods());

            var srcFile = @"..\..\..\Emma.Common\Extensions\StringExtensions.cs";
            var src = File.ReadAllText(srcFile);
            var fileMethods = ExtensionMethodParser.Parse(src, srcFile, lastUpdated: DateTimeOffset.Now);

            var lib = new ExtensionMethodLibrary(
                new ExtensionMethodsSource
                {
                    Methods = assemblyMethods
                },
                new ExtensionMethodsSource
                {
                    Methods = fileMethods
                }
            );
            return lib;
        }
    }
}
