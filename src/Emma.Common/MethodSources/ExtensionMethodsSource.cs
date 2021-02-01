using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emma.Common.ExtensionMethodProviders;
using static Emma.Common.Utils.AsyncHelper;

namespace Emma.Common.MethodSources
{
    public class ExtensionMethodsSource
    {
        private readonly IExtensionMethodProvider _originalEmProvider;
        private readonly ICachedEmProvider _localEmProvider;
        private DateTimeOffset _localTimestamp;
        private DateTimeOffset _sourceTimestamp;
        private DateTimeOffset _timeStamp;

        private async Task<IEnumerable<ExtensionMethod>> ProvideMethods()
        {
            _localTimestamp = await _localEmProvider.LastUpdated();
            _sourceTimestamp = await _originalEmProvider.LastUpdated();

            if (_sourceTimestamp > _localTimestamp)
            {
                var extensionMethods = (await _originalEmProvider.Provide()).ToList();
                _localEmProvider.SetCache(_sourceTimestamp, extensionMethods);
                return extensionMethods;
            }

            _timeStamp = _sourceTimestamp;
            return await _localEmProvider.Provide();
        }

        public DateTimeOffset LastUpdated => _timeStamp;
        public IEnumerable<ExtensionMethod> Methods => RunSynchronously(ProvideMethods);

        public ExtensionMethodsSource(
            IExtensionMethodProvider originalEmProvider, 
            ICachedEmProvider localEmProvider)
        {
            _originalEmProvider = originalEmProvider;
            _localEmProvider = localEmProvider;

            RunSynchronously( ProvideMethods);
        }


        public enum SourceKind
        {
            Binary, SourceCode,
            Github
        }
    }

}