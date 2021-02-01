using System;
using System.Collections.Generic;
using System.Linq;
using Emma.Common;

namespace Emma.XamlControls.ViewModels
{
    public class MainEmmaToolWindowViewModel : ViewModelBase
    {
        public override string DisplayName { get; protected set; } = "Emma Tool Window";
 
        private readonly ExtensionMethodLibrary _emLibrary;

        private const string AnyItemIndicator = "*";
        private static readonly List<string> AnyItem = new List<string> { AnyItemIndicator };

        #region Method List Filtering Properties
        public IEnumerable<ExtensionMethodViewModel> Methods => _emLibrary.Find(Query)
            .Select(ExtensionMethodViewModel.Create);

        string MatchAny(string s) => s == AnyItemIndicator ? "" : s;
        private ExtensionMethodQuery _query;
        public ExtensionMethodQuery Query
        {
            get
            {
                if (_query == null)
                {
                    _query = new ExtensionMethodQuery()
                    {
                        Name = MatchAny(MemberSearch),
                        NameMatchMode = StringMatchMode.Contains,
                        ExtendingType = MatchAny(ExtendingTypeSearch),
                        ExtendingTypeMatchMode = StringMatchMode.Equals,
                        ReturnType = MatchAny(ReturnTypeSearch),
                        ReturnTypeMatchMode = StringMatchMode.Equals
                    };
                    _query.Insensitive = true;
                }

                return _query;
            }
        }

        private string _memberSearch = AnyItemIndicator;
        public string MemberSearch
        {
            get => _memberSearch;
            set
            {
                _memberSearch = value;
                SetQueryChanged();
                Search();
            }
        }
        private IEnumerable<string> _memberNames;
        public IEnumerable<string> MemberNames
        {
            get
            {
                var orderedEnumerable = _emLibrary.Methods
                    .Select(m => m.Name)
                    .Distinct()
                    .OrderBy(a => a);
                return _memberNames ?? (_memberNames =
                    AnyItem.Concat(
                    orderedEnumerable));
            }
        }

        private string _extendingTypeSearch = AnyItemIndicator;
        public string ExtendingTypeSearch
        {
            get => _extendingTypeSearch;
            set
            {
                _extendingTypeSearch = value;
                SetQueryChanged();
                Search();
            }
        }
        private IEnumerable<string> _extendingTypes;
        public IEnumerable<string> ExtendingTypes
        {
            get
            {
                return _extendingTypes ?? (_extendingTypes =
                    AnyItem.Concat(
                        _emLibrary.Methods
                            .Select(m => m.ExtendingType)
                            .Distinct()
                            .OrderBy(a => a)));
            }
        }

        private string _returnTypeSearch = AnyItemIndicator;
        public string ReturnTypeSearch
        {
            get => _returnTypeSearch;
            set
            {
                _returnTypeSearch = value;
                SetQueryChanged();
                Search();
            }
        }
        private IEnumerable<string> _returnTypes;
        public IEnumerable<string> ReturnTypes
        {
            get
            {
                return _returnTypes ?? (_returnTypes =
                        AnyItem.Concat(
                            _emLibrary.Methods
                                .Select(m => m.ReturnType)
                                .Distinct()
                                .OrderBy(a => a))
                    );
            }
        }

        private void SetQueryChanged()
        {
            _query = null;
        }

        #endregion

        public ExtensionMethod SelectedMethod { get; set; }

        public MainEmmaToolWindowViewModel(ExtensionMethodLibrary emLibrary)
        {
            _emLibrary = emLibrary;
        }

        public void Search() => 
            OnPropertyChanged(nameof(Methods));

        public void MethodSelected(ExtensionMethodViewModel selected)
        {
            SelectedMethod = selected.ExtensionMethod;
            OnPropertyChanged(nameof(SelectedMethod));
            OnPropertyChanged(nameof(CodePreviewText));
        }
        
        public string CodePreviewText
        {
            get
            {
                if (SelectedMethod is null) return "";
                switch (SelectedMethod.SourceType)
                {
                    case ExtensionMethodSourceType.Assembly:
                        return $"method is in assembly: {SelectedMethod.SourceLocation}";
                    case ExtensionMethodSourceType.SourceCode:
                        return SelectedMethod.Source.ToString();
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}