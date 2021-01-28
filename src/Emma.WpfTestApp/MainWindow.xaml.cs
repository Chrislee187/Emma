using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
                DataContext = new MainEmmaToolWindowViewModel(CreateTestLibrary().Result)
            };
            Content = mainEmmaToolWindowControl;
            mainEmmaToolWindowControl.Focus();
        }

        private static async Task<ExtensionMethodLibrary> CreateTestLibrary()
        {
            var assemblyMethods = ExtensionMethodParser.Parse(typeof(StringExtensions).ExtensionMethods());

            var srcFile = @"..\..\..\Emma.Common\Extensions\StringExtensions.cs";
            var src = File.ReadAllText(srcFile);
            var fileMethods = ExtensionMethodParser.Parse(src, srcFile, lastUpdated: DateTimeOffset.Now);
            
            var lib = new ExtensionMethodLibrary(
                ExtensionMethodsSource.Create(typeof(StringExtensions).Assembly)
                
                 , ExtensionMethodsSource.Create("chrislee187", "Methodbrary")
            );
            return lib;
        }
    }
}
