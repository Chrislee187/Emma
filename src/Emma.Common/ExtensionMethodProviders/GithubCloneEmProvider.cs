using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Emma.Common.ExtensionMethodProviders
{
    public class GithubCloneEmProvider : IExtensionMethodProvider
    {
        private readonly string _repoUrl;
        private readonly string _clonePath;
        private bool _hasBeenPulled;

        public GithubCloneEmProvider(string repoUrl)
        {
            _repoUrl = repoUrl;
            _clonePath = ClonePath(
                Path.GetFileNameWithoutExtension(repoUrl));
        }

        public async Task<DateTimeOffset> LastUpdated(bool refresh = false)
        {
            await UpdateMethodsSource(refresh);
            var di = new DirectoryInfo(_clonePath);
            return new DateTimeOffset(di.LastWriteTimeUtc);
        }

        public async Task<IEnumerable<ExtensionMethod>> Provide(bool refresh = false)
        {
            await UpdateMethodsSource(refresh);

            return await ExtensionMethodParser.Parse(_clonePath);
        }

        private async Task UpdateMethodsSource(bool refresh = false)
        {
            if (!Directory.Exists(_clonePath))
            {
                await Clone();
            }
            else
            {
                if (!_hasBeenPulled || refresh)
                {
                    await Pull();
                }
            }
        }

        // ReSharper disable once UnusedMethodReturnValue.Local
        private async Task<string> Pull()
        {
            var arguments = $"pull";
            var output = await ExecShell(GitStartInfo(arguments, _clonePath));
            _hasBeenPulled = true;
            return output;
        }

        // ReSharper disable once UnusedMethodReturnValue.Local
        private async Task<string> Clone()
        {
            var arguments = $"clone {_repoUrl} {_clonePath}";
            var output = await ExecShell(GitStartInfo(arguments));
            return output;
        }

        private ProcessStartInfo GitStartInfo(string arguments, string workingDirectory = null)
        {
            var processStartInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
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
    }

}