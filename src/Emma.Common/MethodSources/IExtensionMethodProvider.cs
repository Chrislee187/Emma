using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using GithubRepositoryModel;
using Newtonsoft.Json;
using Octokit;
using static Emma.Common.Utils.AsyncHelper;

namespace Emma.Common.MethodSources
{
    public interface IExtensionMethodProvider
    {
        Task<DateTimeOffset> LastUpdated();
        Task<IEnumerable<ExtensionMethod>> Provide();
    }

    public interface ICachedEmProvider : IExtensionMethodProvider
    {
        void SetCache(DateTimeOffset timestamp, IEnumerable<ExtensionMethod> extensionMethods);
        Task<bool> Available();
    }

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
                _user = RunSynchronously(() => Client.User(_githubUsername));
                _repo = RunSynchronously(() => _user.GetRepository(_reponame));
                _branch = RunSynchronously(() => _repo.GetBranch(_repo.DefaultBranch));
            }
        }
    }

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
            if (!_initialised && Available().Result)
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

        public Task<bool> Available()
        {
            return Task.FromResult(File.Exists(_filename));
        }
    }
}