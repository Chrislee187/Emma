using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace Emma.VSIX
{
    internal sealed class EmmaCommand
    {
        public const int CommandId = 257;

        public static readonly Guid CommandSet = new Guid("ccc2699a-6608-42ea-a6a6-3764e617d764");

        private readonly AsyncPackage _package;

        private EmmaCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            _package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        public static EmmaCommand Instance
        {
            get;
            private set;
        }

        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider => 
            _package;

        public static async Task InitializeAsync(AsyncPackage package)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            var commandService = await package.GetServiceAsync((typeof(IMenuCommandService))) as OleMenuCommandService;

            // NOTE: hotkey is regsitered in VSCommandsTable.csvt
            // see also https://stackoverflow.com/questions/10392622/how-to-make-a-shortcut-to-run-a-vsix-method
            Instance = new EmmaCommand(package, commandService);
        }

        private void Execute(object sender, EventArgs e)
        {
            this._package.JoinableTaskFactory.RunAsync(async delegate
            {
                var window = await this._package.ShowToolWindowAsync(
                    typeof(EmmaMainToolWindowPane), 0, true, this._package.DisposalToken);
                if (window?.Frame == null)
                {
                    throw new NotSupportedException("Cannot create tool window");
                }
            });
        }
    }
}
