using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Emma.Common.ExtensionMethodProviders
{
    public class AssemblyEmProvider : ICachedEmProvider
    {
        private readonly Assembly _asm;
        private readonly string _filename;

        public AssemblyEmProvider(Assembly asm)
        {
            _asm = asm;
            _filename = _asm.Location;
        }
        public Task<DateTimeOffset> LastUpdated() =>
            Task.FromResult(new DateTimeOffset(File.GetLastWriteTimeUtc(_filename)));

        public Task<IEnumerable<ExtensionMethod>> Provide() =>
            Task.FromResult(ExtensionMethodParser.Parse(_asm)
            );

        public void SetCache(DateTimeOffset timestamp, IEnumerable<ExtensionMethod> extensionMethods) =>
            throw new NotImplementedException();

        public Task<bool> CacheAvailable() =>
            Task.FromResult(File.Exists(_filename));
    }
}