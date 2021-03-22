using System;
using System.Windows.Controls;
using Emma.Common;
using Emma.Common.ExtensionMethodProviders;
using Emma.Common.MethodSources;
using Emma.XamlControls.ViewModels;

namespace Emma.WpfTestApp
{
    /// <summary>
    /// Interaction logic for EmmaHostPage.xaml
    /// </summary>
    public partial class EmmaHostPage : Page
    {
        public EmmaHostPage()
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
                new GithubCloneEmProvider("https://github.com/chrislee187/methodbrary"),
                new AppDataEmProvider("emma", $"default-methodbrary")
            );

            var lib = new ExtensionMethodLibrary(src);
            return lib;
        }
    }
}
