using System;
using Emma.Core;
using Emma.Core.MethodSources;
using Emma.XamlControls.Tests.Support;
using Emma.XamlControls.ViewModels;
using NUnit.Framework;
using Shouldly;

namespace Emma.XamlControls.Tests.ViewModels
{
    
    public class ViewModelTestsBase
    {
        protected readonly PropertiesChanged PropertiesNotified = new PropertiesChanged();

    }

    [TestFixture]
    public class MainEmmaToolWindowViewModelTests : ViewModelTestsBase
    {
        private MainEmmaToolWindowViewModel _viewModel;


        [SetUp]
        public void SetUp()
        {
            var mis = ExtensionMethodParser.Parse(typeof(MainEmmaToolWindowControl).Assembly);
            var src = new ExtensionMethodsSource() { Methods = mis, LastUpdated = DateTimeOffset.Now };
            var library = new ExtensionMethodLibrary(src);
            _viewModel = new MainEmmaToolWindowViewModel(library);

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
            var emvm = ExtensionMethodViewModel.Create(new ExtensionMethod() { Name = "Test" });
            _viewModel.MethodSelected(emvm);
            PropertiesNotified.ShouldContain(nameof(MainEmmaToolWindowViewModel.CodePreviewText));
        }

        [Test]
        public void CodePreviewText_shows_message_for_assemblies()
        {
            const string sourceLocation = "Test Location";
            var emvm = ExtensionMethodViewModel.Create(new ExtensionMethod()
            {
                Name = "Test",
                SourceType = ExtensionMethodSourceType.Assembly,
                SourceLocation = sourceLocation
            });
            _viewModel.MethodSelected(emvm);

            _viewModel.CodePreviewText.ShouldContain(sourceLocation);
        }

        [Test]
        public void CodePreviewText_shows_code_for_source()
        {
            const string testSourceCode = "Test Source Code";
            var emvm = ExtensionMethodViewModel.Create(new ExtensionMethod()
            {
                Name = "Test",
                SourceType = ExtensionMethodSourceType.SourceCode,
                Source = testSourceCode
            });
            _viewModel.MethodSelected(emvm);

            _viewModel.CodePreviewText.ShouldContain(testSourceCode);
        }
    }
}