using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Emma.Common.ExtensionMethodProviders
{
    public interface ICachedEmProvider : IExtensionMethodProvider
    {
        void SetCache(DateTimeOffset timestamp, IEnumerable<ExtensionMethod> extensionMethods);
        Task<bool> Available();
    }
}