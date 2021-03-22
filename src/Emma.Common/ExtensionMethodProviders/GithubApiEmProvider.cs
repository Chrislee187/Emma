using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Emma.Common.Utils;
using GithubRepositoryModel;
using Octokit;

namespace Emma.Common.ExtensionMethodProviders
{
    public class GithubApiEmProvider : IExtensionMethodProvider
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


        public GithubApiEmProvider(string githubUsername, string reponame)
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

    public class GithubCloneEmProvider : IExtensionMethodProvider
    {
        private readonly string _repoUrl;
        private readonly string _clonePath;

        public GithubCloneEmProvider(string repoUrl)
        {
            _repoUrl = repoUrl;
            _clonePath = ClonePath(
                Path.GetFileNameWithoutExtension(repoUrl));
        }

        private string ClonePath(string reponame)
        {
            var parent = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Emma");
            CreateFolder(parent);

            var clones = Path.Combine(parent, "Clones");
            CreateFolder(clones);

            return Path.Combine(clones, reponame);
        }

        private static void CreateFolder(string parent)
        {
            if (!Directory.Exists(parent))
            {
                Directory.CreateDirectory(parent);
            }
        }

        public async Task<DateTimeOffset> LastUpdated()
        {
            await UpdateMethodsSource();
            var di = new DirectoryInfo(_clonePath);
            return new DateTimeOffset(di.LastWriteTimeUtc);
        }

        private async Task UpdateMethodsSource()
        {
            if (!Directory.Exists(_clonePath))
            {
                await Clone();
            }
            else
            {
                await Pull();
            }
        }

        public async Task<IEnumerable<ExtensionMethod>> Provide()
        {
            await UpdateMethodsSource();

            return await ExtensionMethodParser.Parse(_clonePath);
        }

        private async Task<string> Pull()
        {
            var arguments = $"pull";
            var output = await ExecShell(GitStartInfo(arguments, _clonePath));
            return output;
        }

        private ProcessStartInfo GitStartInfo(string arguments, string workingDirectory = null)
        {
            var processStartInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                FileName = @"git",
                Arguments = arguments,
                RedirectStandardOutput = true,
            };

            if (!string.IsNullOrEmpty(workingDirectory))
            {
                processStartInfo.WorkingDirectory = workingDirectory;
            }
            return processStartInfo;
        }

        private async Task<string> Clone()
        {
            var arguments = $"clone {_repoUrl} {_clonePath}";
            var output = await ExecShell(GitStartInfo(arguments));
            return output;
        }

        private async Task<string> ExecShell(ProcessStartInfo startInfo)
        {
            var output = "";
            using (var process = new Process
            {
                StartInfo = startInfo,
            })
            {
                process.Start();
                process.WaitForExit();
                output += await process.StandardOutput.ReadToEndAsync();
                Debug.Assert(process.HasExited);
            }

            return output;
        }
    }

}