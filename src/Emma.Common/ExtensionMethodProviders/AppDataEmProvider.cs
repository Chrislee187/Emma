using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Emma.Common.ExtensionMethodProviders
{
    public class AppDataEmProvider : ICachedEmProvider
    {

        private readonly string _filename;
        private DateTimeOffset _timestamp;
        private IEnumerable<ExtensionMethod> _extensionMethods;

        private bool _initialised;
        public AppDataEmProvider(string appFolder, string dataFilename)
        {
            var folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                appFolder);

            if(!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            _filename = Path.ChangeExtension(
                Path.Combine(folder, dataFilename),
                ".emma");

            if (File.Exists(_filename))
            {
                Init();
            }
        }

        public Task<DateTimeOffset> LastUpdated()
        {
            Init();
            return Task.FromResult(_timestamp);
        }

        public Task<IEnumerable<ExtensionMethod>> Provide()
        {
            Init();
            return Task.FromResult(_extensionMethods);
        }

        private void Init()
        {
            var cacheAvailable = File.Exists(_filename);
            if (!_initialised && cacheAvailable)
            {
                _timestamp = File.GetLastWriteTimeUtc(_filename);
                var json = File.ReadAllText(_filename);
                _extensionMethods = JsonConvert.DeserializeObject<IEnumerable<ExtensionMethod>>(json);
                _initialised = true;
            }
        }
        public void SetCache(DateTimeOffset timestamp, IEnumerable<ExtensionMethod> extensionMethods)
        {
            _extensionMethods = extensionMethods;
            _timestamp = timestamp;

            File.WriteAllText(_filename, JsonConvert.SerializeObject(_extensionMethods));
            _initialised = true;
        }
    }
}