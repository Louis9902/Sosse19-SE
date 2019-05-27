using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using WorksKit;
using WorksKit.Worker;
using WorksKit.Worker.Group;
using WorksKit.Worker.Preferences;
using WorksManager.Properties;

namespace WorksManager
{
    public partial class WorkerOverview : Form
    {
        private const string Configuration = "Workers.dat";

        private readonly IDictionary<Guid, IWorker> cache;
        private readonly Workers workers = new Workers(Configuration);
        private bool hasMadeChanges;

        public WorkerOverview()
        {
            InitializeComponent();
            cache = new Dictionary<Guid, IWorker>();
        }

        private void OnOverviewLoad(object sender, EventArgs args)
        {
            SubscribeToLogger();
            workers.Load(cache);
            FillInWorkers();
        }

        private void OnOverviewSave(object sender, FormClosingEventArgs args)
        {
            //if (!hasMadeChanges) return;
            switch (args.CloseReason)
            {
                case CloseReason.TaskManagerClosing:
                case CloseReason.WindowsShutDown:
                {
                    break;
                }

                case CloseReason.UserClosing:
                {
                    var result = MessageBox.Show(Resources.SaveMessage, Resources.Warning,
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Warning);
                    
                    switch (result)
                    {
                        case DialogResult.Yes:
                            workers.Save(cache);
                            break;
                        case DialogResult.No:
                            break;
                        case DialogResult.Cancel:
                            args.Cancel = true;
                            break;
                    }

                    break;
                }
            }
        }

        private static void SubscribeToLogger()
        {
            Logger.Erroring += delegate(string message) { MessageBox.Show(message, Resources.LoggerErrorTitle); };
            Logger.Debugging += delegate(string message) { MessageBox.Show(message, Resources.LoggerErrorTitle); };
        }

        private void FillInWorkers()
        {
            foreach (var worker in cache.Values)
            {
                Workers.Rows.Add(worker.Group, worker.Label, worker.GetType().Name);
            }
        }

        private void OnWorkerCellClick(object sender, DataGridViewCellEventArgs e)
        {
            var current = Workers.Rows[e.RowIndex];

            var group = current.Cells[0].Value as Guid? ?? default;
            var label = current.Cells[1].Value as Guid? ?? default;

            if (group == Guid.Empty || label == Guid.Empty)
            {
                if (!OpenWorkerChoose(out group, out label)) return;
            }

            OpenWorkerOptions(group, label);
        }

        private bool OpenWorkerChoose(out Guid group, out Guid label)
        {
            label = Guid.NewGuid();

            var clazz = typeof(SyncWorker); //ToDo: make this able to choose
            if (!WorkerGroups.ObjectBindings.GetOrNothing(clazz, out group)) return false;

            Workers.Rows.Add(group, label, clazz.Name);
            return true;
        }

        private void OpenWorkerOptions(Guid group, Guid label)
        {
            if (!cache.TryGetValue(label, out var worker))
            {
                if (!WorkerGroups.ObjectBindings.GetOrNothing(group, out var clazz))
                {
                    Logger.Warn("Unable to get type for group id {0}, skipping creation process", group);
                    return;
                }

                worker = DefaultWorker.Instantiate(clazz, group, label);
            }

            var str = new StringBuilder("Preferences:").AppendLine();

            foreach (var preference in worker.Preferences)
            {
                if (!(preference is Preference<string> option)) continue;
                str.Append(option.Name).Append(" :").Append(option.Value).AppendLine();
            }

            MessageBox.Show($"Group:\n{group}\nLabel:\n{label}\n\n{str}");
        }
    }
}