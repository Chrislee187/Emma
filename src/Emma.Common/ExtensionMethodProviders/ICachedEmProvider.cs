using System;
using System.Collections.Generic;

namespace Emma.Common.ExtensionMethodProviders
{
    public interface ICachedEmProvider : IExtensionMethodProvider
    {
        void SetCache(DateTimeOffset timestamp, IEnumerable<ExtensionMethod> extensionMethods);
    }
}