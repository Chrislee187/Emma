using System;
using System.Threading.Tasks;
using GithubRepositoryModel;
using GithubRepositoryModel.Tests.GithubRepoModel;
using NUnit.Framework;
using Shouldly;

namespace Emma.Core.Tests
{
    public class GithubModelSpikes : GithubTestsBase
    {
        private const string Login = "chrislee187";
        private const string RepoName = "Emma";
        private IGithub _github;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _github = new GithubRepositoryModel.Github(ApiClient);
        }


        [Test]
        public async Task GetAGithubUser()
        {
            var user = await _github.User(Login);

            user.Login.ShouldBe("Chrislee187");
        }

        [Test]
        public async Task GetARepo()
        {
            var repo = await _github.Repository(Login, RepoName);

            repo.Name.ShouldBe(RepoName);
            repo.PushedAt.HasValue.ShouldBeTrue();
            repo.PushedAt.Value.ShouldBeGreaterThan(DateTimeOffset.MinValue);
        }
        [Test]
        public async Task GetDefaultBranch()
        {
            var repo = await _github.Repository(Login, RepoName);
            var branch = await repo.Branch(repo.DefaultBranch);

            branch.Name.ShouldBe("main");
        }
    }
}