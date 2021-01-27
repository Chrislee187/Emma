using System.Collections.Generic;
using System.Linq;
using Emma.Core;

namespace Emma.XamlControls
{
    public class MainEmmaToolWindowViewModel : ViewModelBase
    {
        private readonly ExtensionMethodLibrary _emLibrary;
        public override string DisplayName { get; protected set; } = "Emma Tool Window";

        public string MemberSearch { get; set; } = "TOSTRINxG";


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
                return _extendingTypes ?? (_extendingTypes = _emLibrary.Methods
                    .Select(m => m.ExtendingType)
                    .Distinct());
            }
        }
        public string ExtendingTypeSearch { get; set; }

        private IEnumerable<string> _returnTypes;
        public IEnumerable<string> ReturnTypes
        {
            get
            {
                return _returnTypes ?? (_returnTypes = _emLibrary.Methods
                    .Select(m => m.ReturnType)
                    .Distinct());
            }
        }
        public string ReturnTypeSearch { get; set; }

        public IEnumerable<ExtensionMethod> Methods => _emLibrary.Methods;
        public ExtensionMethod SelectedMethod { get; private set; }

        public MainEmmaToolWindowViewModel(ExtensionMethodLibrary emLibrary)
        {
            _emLibrary = emLibrary;
        }

        public void SignatureSelected(ExtensionMethod selected)
        {
            SelectedMethod = selected;
            OnPropertyChanged(nameof(SelectedMethod));
        }
    }
}