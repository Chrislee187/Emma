using System;
using System.Collections.Generic;

namespace Emma.Common.MethodSources
{
    public class ExtensionMethodsSource
    {
        
        public DateTimeOffset LastUpdated { get; set; }
        public IEnumerable<ExtensionMethod> Methods { get; set; }
    }
}