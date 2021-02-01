using Emma.Common;
using Emma.Common.MethodSources;

namespace Emma.XamlControls.Tests.Builders
{
    public class ExtensionMethodLibraryBuilder
    {
        public ExtensionMethodLibrary Build() =>
            new ExtensionMethodLibrary(
                EmsFactory.Create(typeof(MainEmmaToolWindowControl).Assembly)
            );
    }

}