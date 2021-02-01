using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Emma.Common;
using Emma.Common.ExtensionMethodProviders;
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
            var src = new ExtensionMethodsSource(
                new GithubRepoEmProvider("chrislee187", "methodbrary"),
                new AppDataEmProvider("emma", $"github-methodbrary")
            );
            
            var lib = new ExtensionMethodLibrary(src);
            return lib;
        }
    }
}
