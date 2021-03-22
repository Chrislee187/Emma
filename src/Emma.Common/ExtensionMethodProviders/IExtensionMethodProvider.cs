using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Emma.Common.ExtensionMethodProviders
{
    public interface IExtensionMethodProvider
    {
        Task<DateTimeOffset> LastUpdated(bool refresh = false);
        Task<IEnumerable<ExtensionMethod>> Provide(bool refresh = false);
    }
}