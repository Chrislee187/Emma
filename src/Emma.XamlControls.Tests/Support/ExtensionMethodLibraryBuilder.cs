using System;
using Emma.Common;
using Emma.Common.MethodSources;

namespace Emma.XamlControls.Tests.Support
{
    public class ExtensionMethodLibraryBuilder
    {
        public ExtensionMethodLibrary Build()
        {
            var mis = ExtensionMethodParser.Parse(typeof(MainEmmaToolWindowControl).Assembly);
            var src = ExtensionMethodsSource.Create(typeof(MainEmmaToolWindowControl).Assembly);


            return new ExtensionMethodLibrary(src);

        }
    }
}