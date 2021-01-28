using System;
using System.IO;
using Emma.Common.Extensions;
using Emma.Common.MethodSources;
using Newtonsoft.Json;

namespace Emma.Common.Cache
{
    public class AppDataExtensionMethodJsonCache : ExtensionMethodCache
    {
        public override bool Contains(string cacheId) =>
            File.Exists(CacheFilename(cacheId));

        public override ExtensionMethodsSource Get(string cacheId)
        {
            if (!Contains(cacheId))
            {
                throw new ArgumentException($"CacheId not found!", nameof(cacheId));
            }

            var data = File.ReadAllText(CacheFilename(cacheId));
            
            return JsonConvert.DeserializeObject<ExtensionMethodsSource>(data);
        }

        public override void Remove(string cacheId)
        {
            if (Contains(cacheId))
            {
                File.Delete(CacheFilename(cacheId));
            }
        }

        public override void Add(string cacheId, ExtensionMethodsSource extensionMethodsSource)
        {
            File.WriteAllText(CacheFilename(cacheId), JsonConvert.SerializeObject(extensionMethodsSource));
        }
        
        private string CacheFilename(string cacheId)
        {
            var file = cacheId.ToValidFilename();
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Emma");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var full = Path.Combine(path,file) + ".emmaCache";

            return full;
        }
    }
}