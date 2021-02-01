using System.Collections.Generic;
using Emma.Common;
using Emma.XamlControls.Tests.Builders;

namespace Emma.Core.Tests.Builders
{
    public class ExtensionMethodsBuilder
    {

        public IEnumerable<ExtensionMethod> Build()
        {
            return new List<ExtensionMethod>
            {
                new ExtensionMethodBuilder().Build()
            };
        }
    }
}