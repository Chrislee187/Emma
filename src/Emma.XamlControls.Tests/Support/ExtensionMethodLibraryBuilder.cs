using System;
using Emma.Core;
using Emma.Core.MethodSources;

namespace Emma.XamlControls.Tests.Support
{
    public class ExtensionMethodLibraryBuilder
    {
        public ExtensionMethodLibrary Build()
        {
            var mis = ExtensionMethodParser.Parse(typeof(MainEmmaToolWindowControl).Assembly);
            var src = new ExtensionMethodsSource() { Methods = mis, LastUpdated = DateTimeOffset.Now };


            return new ExtensionMethodLibrary(src);

        }
    }
}