using Emma.Common;
using Emma.Common.MethodSources;
using Emma.XamlControls.Tests.Builders;
using Emma.XamlControls.ViewModels;
using NUnit.Framework;
using Shouldly;

namespace Emma.XamlControls.Tests.ViewModels
{
    [TestFixture]
    public class MainEmmaToolWindowViewModelTests : ViewModelTestsBase
    {
        private MainEmmaToolWindowViewModel _viewModel;


        [SetUp]
        public void SetUp()
        {
            _viewModel = new MainEmmaToolWindowViewModel(
                new ExtensionMethodLibrary(
                    EmsFactory.Create(typeof(MainEmmaToolWindowControl).Assembly)));

            PropertiesNotified.Clear();
            PropertiesNotified.Register(_viewModel);
        }

        [Test]
        public void Changing_MemberSearch_updates_query()
        {
            _viewModel.Query.Name.ShouldBeEmpty();

            _viewModel.MemberSearch = "ABC";

            _viewModel.Query.Name.ShouldBe("ABC");
        }

        [Test]
        public void Changing_MemberSearch_triggers_Methods_property_changed()
        {
            _viewModel.MemberSearch = "ABC";
            PropertiesNotified.ShouldContain(nameof(MainEmmaToolWindowViewModel.Methods));
        }

        [Test]
        public void Changing_ExtendingTypeSearch_updates_query()
        {
            _viewModel.Query.ExtendingType.ShouldBeEmpty();

            _viewModel.ExtendingTypeSearch = "ABC";

            _viewModel.Query.ExtendingType.ShouldBe("ABC");
        }

        [Test]
        public void Changing_ExtendingTypeSearch_triggers_Methods_property_changed()
        {
            _viewModel.ExtendingTypeSearch = "ABC";
            PropertiesNotified.ShouldContain(nameof(MainEmmaToolWindowViewModel.Methods));
        }

        [Test]
        public void Changing_ReturnTypeSearch_updates_query()
        {
            _viewModel.Query.ReturnType.ShouldBeEmpty();

            _viewModel.ReturnTypeSearch = "ABC";

            _viewModel.Query.ReturnType.ShouldBe("ABC");
        }

        [Test]
        public void Changing_ReturnTypeSearch_triggers_Methods_property_changed()
        {
            _viewModel.ExtendingTypeSearch = "ABC";
            PropertiesNotified.ShouldContain(nameof(MainEmmaToolWindowViewModel.Methods));
        }

        [Test]
        public void Selecting_Method_triggers_CorePreview_property_changed()
        {
            var emvm = ExtensionMethodViewModel.Create(new ExtensionMethodBuilder().Build());
            _viewModel.MethodSelected(emvm);
            PropertiesNotified.ShouldContain(nameof(MainEmmaToolWindowViewModel.CodePreviewText));
        }

        [Test]
        public void CodePreviewText_shows_message_for_assemblies()
        {
            const string sourceLocation = "Test Location";
            var emvm = ExtensionMethodViewModel.Create(
                new ExtensionMethodBuilder()
                    .WithSourceLocation(sourceLocation)
                    .Build()
                );


            _viewModel.MethodSelected(emvm);

            _viewModel.CodePreviewText.ShouldContain(sourceLocation);
        }

        [Test]
        public void CodePreviewText_shows_code_for_source()
        {
            const string testSourceCode = "Test Source Code";
            var emvm = ExtensionMethodViewModel.Create(
                new ExtensionMethodBuilder()
                    .WithSource(testSourceCode)
                    .Build()
            );

            _viewModel.MethodSelected(emvm);

            _viewModel.CodePreviewText.ShouldContain(testSourceCode);
        }
    }
}