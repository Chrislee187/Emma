using System.Collections.Generic;
using Emma.XamlControls.ViewModels;
using Shouldly;

namespace Emma.XamlControls.Tests.Support
{
    public class PropertiesChanged
    {
        private List<string> _propertiesChanged= new List<string>();

        public void Register(ViewModelBase vm)
        {
            vm.PropertyChanged += (sender, args) =>
            {
                _propertiesChanged.Add(args.PropertyName);
            };
        }
        

        public void Clear()
        {
            _propertiesChanged.Clear();
        }
        public void ShouldContain(string propertyname)
            => _propertiesChanged.Contains(propertyname).ShouldBeTrue();

        public void Add(string propertyName)
        {
            _propertiesChanged.Add(propertyName);
        }
    }
}