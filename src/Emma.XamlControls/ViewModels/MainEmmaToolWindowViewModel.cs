using System.Collections.Generic;
using System.Linq;
using Emma.Core;

namespace Emma.XamlControls.ViewModels
{
    public class MainEmmaToolWindowViewModel : ViewModelBase
    {
        public override string DisplayName { get; protected set; } = "Emma Tool Window";
 
        private readonly ExtensionMethodLibrary _emLibrary;

        private const string AnyItemIndicator = "*";
        private static readonly List<string> AnyItem = new List<string> { AnyItemIndicator };

        #region Method List Filtering
        public IEnumerable<ExtensionMethodViewModel> Methods => _emLibrary.Find(Query)
            .Select(ExtensionMethodViewModel.Create);

        string MatchAny(string s) => s == AnyItemIndicator ? "" : s;
        private ExtensionMethodQuery _query;
        private ExtensionMethodQuery Query
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
                OnPropertyChanged(nameof(MemberSearch));
                OnPropertyChanged(nameof(Methods));
            }
        }
        private IEnumerable<string> _memberNames;
        public IEnumerable<string> MemberNames
        {
            get
            {
                return _memberNames ?? (_memberNames =
                    AnyItem.Concat(
                    _emLibrary.Methods
                    .Select(m => m.Name)
                    .Distinct()));
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
                OnPropertyChanged(nameof(ExtendingTypeSearch));
                OnPropertyChanged(nameof(Methods));
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
                            .Distinct()));
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
                OnPropertyChanged(nameof(ReturnTypeSearch));
                OnPropertyChanged(nameof(Methods));
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
                                .Distinct())
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
        }
    }
}