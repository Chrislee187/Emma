using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Emma.Common.Extensions;
using GithubRepositoryModel;
using Octokit;

namespace Emma.Common.MethodSources
{
    public class ExtensionMethodsSource
    {
        private static Github _github;

        private static Github Client
        {
            get
            {
                if(_github == null)
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
        }
        public static ExtensionMethodsSource Create(Type type)
        {
            var methods = ExtensionMethodParser.Parse(type.ExtensionMethods());
            return new ExtensionMethodsSource(
                methods, 
                File.GetCreationTime(type.Assembly.Location), 
                SourceKind.Binary, 
                type);
        }
        public static ExtensionMethodsSource Create(Assembly asm)
        {
            var methods = ExtensionMethodParser.Parse(asm);
            return new ExtensionMethodsSource(
                methods, 
                File.GetCreationTime(asm.Location), 
                SourceKind.Binary, asm);
        }
        public static ExtensionMethodsSource Create(string githubUsername, string reponame)
        {
            var user = Client.User(githubUsername).Result;
            var repo = user.GetRepository(reponame).Result;
            var main = repo.GetBranch(repo.DefaultBranch).Result;

            var methods = ExtensionMethodParser.Parse(new GhFolder(_github, repo, main)).Result;
            return new ExtensionMethodsSource(
                methods, 
                repo.UpdatedAt, 
                SourceKind.Github, 
                (githubUsername, reponame));
        }

        public DateTimeOffset LastUpdated { get; protected set; }
        public IEnumerable<ExtensionMethod> Methods { get; protected set; }

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
            Methods = methods;
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


        public enum SourceKind
        {
            Binary, SourceCode,
            Github
        }
    }
}