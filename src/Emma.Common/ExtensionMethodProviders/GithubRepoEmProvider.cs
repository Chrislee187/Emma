using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Emma.Common.Utils;
using GithubRepositoryModel;
using Octokit;

namespace Emma.Common.ExtensionMethodProviders
{
    public class GithubRepoEmProvider : IExtensionMethodProvider
    {
        private static IGithub _github;
        private readonly string _githubUsername;
        private readonly string _reponame;
        private IGhUser _user;
        private IGhBranch _branch;
        private IGhRepository _repo;

        public static IGithub Client
        {
            get
            {
                if (_github == null)
                {
                    var client = new GitHubClient(
                        new ProductHeaderValue("Emma"))
                    {
                        Connection = { Credentials = new Octokit.Credentials(Credentials.AppKey()) }
                    };
                    _github = new Github(client);
                }

                return _github;
            }
            set => _github = value;
        }


        public GithubRepoEmProvider(string githubUsername, string reponame)
        {
            _reponame = reponame;
            _githubUsername = githubUsername;
        }

        public Task<DateTimeOffset> LastUpdated()
        {
            if (_repo == null)
            {
                Init();
            }

            return Task.FromResult(_repo.UpdatedAt);
        }

        public async Task<IEnumerable<ExtensionMethod>> Provide()
        {
            if (_repo == null)
            {
                Init();
            }

            return await ExtensionMethodParser.Parse(new GhFolder(_github, _repo, _branch));

        }

        private void Init()
        {
            if (_branch == null)
            {
                _user =   AsyncHelper.RunSynchronously(() => Client.User(_githubUsername));
                _repo =  AsyncHelper.RunSynchronously(() => _user.GetRepository(_reponame));
                _branch = AsyncHelper.RunSynchronously(() => _repo.GetBranch(_repo.DefaultBranch));
            }
        }
    }
}