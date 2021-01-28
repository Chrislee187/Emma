using Emma.Common;

namespace Emma.XamlControls.ViewModels
{
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