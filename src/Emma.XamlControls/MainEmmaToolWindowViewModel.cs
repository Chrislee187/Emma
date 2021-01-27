using System.Collections.Generic;
using System.Linq;
using Emma.Core;

namespace Emma.XamlControls
{
    public class MainEmmaToolWindowViewModel : ViewModelBase
    {
        private readonly ExtensionMethodLibrary _emLibrary;

        private static readonly List<string> AnyItem = new List<string> { "*" };

        #region Search Properties

        public string MemberSearch { get; set; }

        private string _extendingTypeSearch = "*";
        public string ExtendingTypeSearch
        {
            get => _extendingTypeSearch;
            set
            {
                _extendingTypeSearch = value;
                OnPropertyChanged(nameof(ExtendingTypeSearch));
                OnPropertyChanged(nameof(Methods));
            }
        }

        private string _returnTypeSearch = "*";
        public string ReturnTypeSearch
        {
            get => _returnTypeSearch;
            set
            {
                _returnTypeSearch = value;
                OnPropertyChanged(nameof(ReturnTypeSearch));
                OnPropertyChanged(nameof(Methods));
            }
        }

        private IEnumerable<string> _memberNames;
        public IEnumerable<string> MemberNames
        {
            get
            {
                return _memberNames ?? (_memberNames = _emLibrary.Methods
                    .Select(m => m.Name)
                    .Distinct());
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
        #endregion

        public ExtensionMethod SelectedMethod { get; set; }

        public MainEmmaToolWindowViewModel(ExtensionMethodLibrary emLibrary)
        {
            _emLibrary = emLibrary;
        }

        public IEnumerable<ExtensionMethodViewModel> Methods =>
            _emLibrary.Methods
                .Where(MatchMethod)
                .Select(ExtensionMethodViewModel.Create);

        private bool MatchMethod(ExtensionMethod method)
        {
            bool MatchAny(string s) => string.IsNullOrEmpty(s) || s == "*";
            
            var name = MatchAny(MemberSearch) 
                       || method.Name.ToLower().Contains(MemberSearch.ToLower());
            var extendingType = MatchAny(ExtendingTypeSearch)
                                || method.ExtendingType.ToLower().Contains(ExtendingTypeSearch.ToLower());
            var returnType = MatchAny(ReturnTypeSearch)
                             || method.ReturnType.ToLower().Contains(ReturnTypeSearch.ToLower());

            return name && extendingType && returnType;
        }

        public void Search() => 
            OnPropertyChanged(nameof(Methods));
        
        public void MethodSelected(ExtensionMethodViewModel selected)
        {
            SelectedMethod = selected.ExtensionMethod;
            OnPropertyChanged(nameof(SelectedMethod));
        }
        public override string DisplayName { get; protected set; } = "Emma Tool Window";

    }

    public class ExtensionMethodViewModel : ViewModelBase
    {
        public static ExtensionMethodViewModel Create(ExtensionMethod em) =>
            new ExtensionMethodViewModel(
                em.Name, em.ExtendingType, em.ReturnType, em.ParamTypes, em
            );

        public string ExtendingType { get; set; }

        public string Name { get; set; }

        public string ReturnType { get; set; }

        public string[] ParamTypes { get; set; }

        internal ExtensionMethod ExtensionMethod { get; set; }

        private ExtensionMethodViewModel(
            string name, string extendingType, string returnType, string[] paramTypes,
            ExtensionMethod extensionMethod)
        {
            Name = name;
            ExtendingType = extendingType;
            ReturnType = returnType;
            ParamTypes = paramTypes;
            ExtensionMethod = extensionMethod;
        }
    }
}