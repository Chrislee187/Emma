using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Emma.Common.Extensions;
using GithubRepositoryModel;
using Newtonsoft.Json;
using Octokit;
using static Emma.Common.Utils.AsyncHelper;

namespace Emma.Common.MethodSources
{
    public class ExtensionMethodsSource
    {
        private readonly IExtensionMethodProvider _originalEmProvider;
        private readonly ICachedEmProvider _localEmProvider;
        private DateTimeOffset _localTimestamp;
        private DateTimeOffset _sourceTimestamp;

        public static ExtensionMethodsSource Create(Assembly asm)
        {
            var methods = ExtensionMethodParser.Parse(asm);
            return new ExtensionMethodsSource(
                methods, 
                File.GetCreationTime(asm.Location), 
                SourceKind.Binary, asm);
        }

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

            return await _localEmProvider.Provide();
        }

        public DateTimeOffset LastUpdated { get; protected set; }
        public IEnumerable<ExtensionMethod> Methods => RunSynchronously(ProvideMethods);

        public SourceKind Kind { get; }
        public object Source { get; set; }

        protected ExtensionMethodsSource(
            IEnumerable<ExtensionMethod> methods, 
            DateTimeOffset lastUpdated,
            SourceKind sourceKind,
            object source
            )
        {
            Kind = sourceKind;
            LastUpdated = lastUpdated;
            switch (Kind)
            {
                case SourceKind.Binary:
                    if (source is Type)
                    {
                        Source = (source as Type).FullName;
                    }
                    else if (source is Assembly)
                    {
                        Source = (source as Assembly).Location;
                    }
                    break;
                case SourceKind.SourceCode:
                    Source = source.ToString();
                    break;
                case SourceKind.Github:
                    Source = source;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Source = source;
        }

        public ExtensionMethodsSource(
            IExtensionMethodProvider originalEmProvider, 
            ICachedEmProvider localEmProvider)
        {
            _originalEmProvider = originalEmProvider;
            _localEmProvider = localEmProvider;

            RunSynchronously(ProvideMethods);
        }


        public enum SourceKind
        {
            Binary, SourceCode,
            Github
        }
    }

}