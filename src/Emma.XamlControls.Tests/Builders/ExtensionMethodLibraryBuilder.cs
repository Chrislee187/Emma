using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Emma.Common;
using Emma.Common.MethodSources;
using Newtonsoft.Json;

namespace Emma.XamlControls.Tests.Builders
{
    public class ExtensionMethodLibraryBuilder
    {
        public ExtensionMethodLibrary Build() =>
            new ExtensionMethodLibrary(
                EMSFactory.Create(typeof(MainEmmaToolWindowControl).Assembly)
            );
    }

}