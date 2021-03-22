using System;
using System.Windows.Controls;
using Emma.Common;
using Emma.Common.ExtensionMethodProviders;
using Emma.Common.MethodSources;
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
                new GithubApiEmProvider("chrislee187", "methodbrary"),
                new AppDataEmProvider("emma", $"github-methodbrary")
            );

            var lib = new ExtensionMethodLibrary(src);
            return lib;
        }
    }
}
