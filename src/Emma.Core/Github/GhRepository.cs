using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Octokit;

namespace Emma.Core.Github
{
    public class GhRepository : Repository, IGhRepository
    {
        private readonly IGithub _github;

        public GhRepository(IGithub github, Repository c) : this(c)
        {
            _github = github;
        }

        public async Task<IGhBranch> Branch(string branchName) => 
            await GhBranch.Get(_github, this, branchName ?? DefaultBranch);

        public async Task<IEnumerable<IGhBranch>> Branch() => 
            await GhBranch.All(_github, this);
        
        private GhRepository(Repository c) : base(c.Url, c.HtmlUrl, c.CloneUrl, c.GitUrl, c.SshUrl, c.SvnUrl,
            c.MirrorUrl, c.Id, c.NodeId, c.Owner, c.Name, c.FullName, c.IsTemplate, c.Description, c.Homepage, c.Language,
            c.Private, c.Fork, c.ForksCount, c.StargazersCount, c.DefaultBranch, c.OpenIssuesCount, c.PushedAt, c.CreatedAt, c.UpdatedAt,
            c.Permissions, c.Parent, c.Source, c.License, c.HasIssues, c.HasWiki, c.HasDownloads, c.HasPages, c.WatchersCount, c.Size,
            c.AllowRebaseMerge, c.AllowSquashMerge, c.AllowMergeCommit, c.Archived, c.WatchersCount)
        {
        }

        #region Api Helpers
        public static async Task<GhRepository> Get(IGithub github, string url) => throw new NotImplementedException();
        public static async Task<GhRepository> Get(IGithub github, string userName, string repoName) => 
            new GhRepository(github, await github.ApiClient.Repository.Get(userName, repoName));

        public static async Task<IEnumerable<IGhRepository>> Branch(IGithub github, string userName) => 
            (await github.ApiClient.Repository.GetAllForUser(userName))
                .Select(r => new GhRepository(github, r));

        #endregion
    }
}