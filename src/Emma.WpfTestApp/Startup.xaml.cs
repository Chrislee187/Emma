using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Emma.Common;
using Emma.Common.ExtensionMethodProviders;
using Emma.Common.MethodSources;
using Emma.XamlControls;
using Emma.XamlControls.ViewModels;

namespace Emma.WpfTestApp
{
    /// <summary>
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        public Page2()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            EmmaControl.DataContext = new MainEmmaToolWindowViewModel(CreateTestLibrary());
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
