using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Emma.Common.ExtensionMethodProviders
{
    public interface IExtensionMethodProvider
    {
        Task<DateTimeOffset> LastUpdated();
        Task<IEnumerable<ExtensionMethod>> Provide();
    }
}