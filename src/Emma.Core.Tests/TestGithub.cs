using System.Threading.Tasks;
using GithubRepositoryModel;
using Octokit;

namespace Emma.Core.Tests
{
    public class TestGithub : IGithub
    {
        public const string AnyGithubUser = "AnyGithubUser";
        public const string AnyRepositoru = "AnyGithubRepository";

        public IGitHubClient ApiClient { get; }
        public Task<IGhUser> User(string user)
        {
            throw new System.NotImplementedException();
        }

        public Task<IGhRepository> Repository(string userName, string repoName)
        {
            throw new System.NotImplementedException();
        }

        public Task<IGhRepository> Repository(string url)
        {
            throw new System.NotImplementedException();
        }
    }
}