using System;
using System.Collections.Generic;
using Emma.Common.Cache;
using GithubRepositoryModel;

namespace Emma.Common.MethodSources
{
    public class GithubRepoExtensionMethodsSource : ExtensionMethodsSource
    {
        private readonly ExtensionMethodCache _cache;
        private readonly IGithub _github;
        private readonly string _userName;
        private readonly string _repoName;

        private GithubRepoExtensionMethodsSource(IGithub github, 
            string localCacheId,
            string userName, string repoName,
            ExtensionMethodCache cache)
        : base(new List<ExtensionMethod>(), DateTimeOffset.Now, SourceKind.Github, (userName, repoName))
        {
            _github = github;
            _cache = cache;
            _userName = userName;
            _repoName = repoName;
            InitCache(localCacheId);
        }

        private void InitCache(string cacheId)
        {
            if (!_cache.Contains(cacheId))
            {
                GetExtensionMethodsFromGithub();
                _cache.Add(cacheId, this);
            }
            else
            {
                var m = _cache.Get(cacheId);
                var ghRepository = _github.User(_userName).Result
                    .GetRepository(_repoName).Result;
                
                var updateCache = ghRepository.UpdatedAt > m.LastUpdated;
                if (updateCache)
                {
                    GetExtensionMethodsFromGithub();
                    _cache.Add(cacheId, this);
                }

                Methods = m.Methods;
                LastUpdated = m.LastUpdated;
            }
        }

        private void GetExtensionMethodsFromGithub()
        {
            // Note: Had probllems when using the Async calls, because of being called from CTOR
            var user = _github.User(_userName).Result;
            var repo = user.GetRepository(_repoName).Result;
            var defaultBranch = repo.GetBranch(repo.DefaultBranch).Result;
            Methods = ExtensionMethodParser.Parse(defaultBranch.Root).Result;
            LastUpdated = repo.UpdatedAt;
        }

    }
}