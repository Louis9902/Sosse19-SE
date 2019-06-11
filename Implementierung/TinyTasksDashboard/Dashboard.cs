using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Windows.Forms;
using TinyTasksDashboard.Properties;
using TinyTasksKit;
using TinyTasksKit.Worker;
using TinyTasksKit.Worker.Group;

namespace TinyTasksDashboard
{
    public partial class Dashboard : Form
    {
        private readonly Workers manager;
        private readonly IDictionary<Guid, IWorker> workers;

        public Dashboard()
        {
            InitializeLogger();
            InitializeComponent();
            manager = new Workers(GetDefaultConfiguration());
            workers = new Dictionary<Guid, IWorker>();
        }

        private static string GetDefaultConfiguration()
        {
            var user = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            return Path.Combine(user, "TintyTasks.cache");
        }

        private static void InitializeLogger()
        {
            Logger.Erroring += HandleLoggerMessage;
#if DEBUG
            Logger.Debugging += HandleLoggerMessage;
#endif
        }

        private static void HandleLoggerMessage(string message)
        {
            MessageBox.Show(
                message,
                Resources.MessageLoggerEventHeader,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        private void LoadWorkers()
        {
            manager.Load(workers);
        }

        private void SaveWorkers()
        {
            manager.Save(workers);
        }

        private void ViewRefresh()
        {
            overview.ClearSelection();
            overview.Rows.Clear();
            foreach (var worker in workers.Values) ShowWorker(worker);
        }

        private void ShowWorker(IWorker worker)
        {
            overview.Rows.Add(worker.GetType().Name, worker.Label, worker);
        }

        private void OnFormLoading(object sender, EventArgs args)
        {
            LoadWorkers();
            ViewRefresh();
        }

        private void OnFormClosing(object sender, FormClosingEventArgs args)
        {
            
            switch (args.CloseReason)
            {
                case CloseReason.UserClosing:
                    {
                        var result = MessageBox.Show(
                            Resources.MessageCloseSave,
                            Resources.MessageCloseSaveHeader,
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning);

                        if (result != DialogResult.Yes) break;

                        SaveWorkers();
                        break;
                    }
            }

            if (args.CloseReason != CloseReason.WindowsShutDown && args.CloseReason != CloseReason.TaskManagerClosing)
            {
                StartBackgroundProcess();
            }

        }


        /// <summary>
        /// Starts the background process of the application in case it isnt running already
        /// </summary>
        private void StartBackgroundProcess()
        {
            //GetPRcessesByName only throws one exception which can occure when the programm runs under Windows XP or older
            System.Diagnostics.Process[] ps = System.Diagnostics.Process.GetProcessesByName("TinyTasksService");
            if (ps.Length == 0)
            {
                try
                {
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo("TinyTasksService.exe", "--file \"" + GetDefaultConfiguration() + "\"");
                    System.Diagnostics.Process process = System.Diagnostics.Process.Start(startInfo);
                }
                catch (Exception e)
                {
                    MessageBox.Show(Resources.BackgroundServiceStartupError);
                }
               
            }
        }

        private void OnCellClick(object sender, DataGridViewCellEventArgs args)
        {
            if (args.RowIndex < 0) return;
            var current = overview.Rows[args.RowIndex];

            var create = false;
            if (!(current.Cells[2].Value is IWorker worker))
            {
                if (!DialogWorkerType(out var group, out var label)) return;
                if (!WorkerGroups.ObjectBindings.GetOrNothing(group, out var clazz))
                {
                    Logger.Warn("Unable to get class for group id {0}", group);
                    return;
                }

                create = true;
                worker = DefaultWorker.Instantiate(clazz, group, label);
            }

            using (var parameters = new Parameters(worker))
            {
            reopen:
                var result = parameters.ShowDialog();

                switch (result)
                {
                    case DialogResult.OK when create:
                        {
                            ShowWorker(worker);
                            workers[worker.Label] = worker;
                            break;
                        }

                    case DialogResult.No when !create:
                        {
                            SystemSounds.Exclamation.Play();
                            goto reopen;
                        }
                }
            }
        }

        private void OnRowDelete(object sender, DataGridViewRowEventArgs args)
        {
            if (args.Row.Cells[2].Value is IWorker worker)
            {
                workers.Remove(worker.Label);
            }
        }

        private static bool DialogWorkerType(out Guid group, out Guid label)
        {
            label = default;
            var clazz = typeof(SyncWorker); //ToDo: make this able to choose

            if (!WorkerGroups.ObjectBindings.GetOrNothing(clazz, out group)) return false;
            label = Guid.NewGuid();
            return true;
        }
    }
}