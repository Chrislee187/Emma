using System.Threading;
using Emma.XamlControls.Tests.Support;
using Emma.XamlControls.ViewModels;
using NUnit.Framework;

namespace Emma.XamlControls.Tests
{
    [TestFixture, Apartment(ApartmentState.STA)]
    public class MainEmmaToolWindowControlTests
    {
        private MainEmmaToolWindowControl _control;
        private MainEmmaToolWindowViewModel _viewModel;

        private readonly PropertiesChanged _propertiesChanged = new PropertiesChanged();

        [SetUp]
        public void SetUp()
        {
            _viewModel = new MainEmmaToolWindowViewModel(
                new ExtensionMethodLibraryBuilder().Build());

            _control = new MainEmmaToolWindowControl
            {
                DataContext = _viewModel
            };

            _propertiesChanged.Clear();
            _propertiesChanged.Register(_viewModel);
        }
    }
}