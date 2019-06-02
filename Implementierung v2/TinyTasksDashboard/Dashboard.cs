using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TinyTasksDashboard.Properties;
using TinyTasksKit;
using TinyTasksKit.Worker;
using TinyTasksKit.Worker.Group;

namespace TinyTasksDashboard
{
    public partial class Dashboard : Form
    {
        private const string Configuration = "Workers.dat";

        private readonly IDictionary<Guid, IWorker> cache;
        private readonly Workers workers = new Workers(Configuration);
        private bool hasMadeChanges;

        public Dashboard()
        {
            InitializeLogger();
            InitializeComponent();
            cache = new Dictionary<Guid, IWorker>();
        }

        private static void InitializeLogger()
        {
            Logger.Erroring += delegate(string message) { MessageBox.Show(message, Resources.LoggerErrorTitle); };
            Logger.Debugging += delegate(string message) { MessageBox.Show(message, Resources.LoggerErrorTitle); };
        }

        private void OnOverviewLoad(object sender, EventArgs args)
        {
            workers.Load(cache);
            RefreshWorkersView();
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
                        case DialogResult.OK:
                        case DialogResult.Yes:
                        {
                            workers.Save(cache);
                            break;
                        }

                        case DialogResult.Abort:
                        case DialogResult.Cancel:
                        {
                            args.Cancel = true;
                            break;
                        }

                        case DialogResult.None:
                        case DialogResult.Retry:
                        case DialogResult.Ignore:
                        case DialogResult.No:
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    break;
                }
            }
        }

        private void RefreshWorkersView()
        {
            foreach (var worker in cache.Values)
            {
                Workers.Rows.Add(worker.Group, worker.Label, worker.GetType().Name);
            }
        }

        private void OnWorkerCellClick(object sender, DataGridViewCellEventArgs args)
        {
            var current = Workers.Rows[args.RowIndex];

            var group = current.Cells[0].Value as Guid? ?? default;
            var label = current.Cells[1].Value as Guid? ?? default;

            if (group == Guid.Empty || label == Guid.Empty)
            {
                if (!OpenWorkerSelect(out group, out label)) return;
            }

            OpenWorkerProperties(group, label);
        }

        /// <summary>
        /// Allows the user to choose a type of worker, this will search the guid of the group, if this does not exist
        /// the method will return false and the group guid is set to the default empty value.
        /// </summary>
        /// <param name="group">returns the group guid from the chosen worker</param>
        /// <param name="label">returns the label guid from the choose worker</param>
        /// <returns>true if a worker type was chosen successfully, otherwise false</returns>
        private bool OpenWorkerSelect(out Guid group, out Guid label)
        {
            var clazz = typeof(SyncWorker); //ToDo: make this able to choose
            label = Guid.NewGuid();

            if (!WorkerGroups.ObjectBindings.GetOrNothing(clazz, out group)) return false;

            Workers.Rows.Add(group, label, clazz.Name);
            return true;
        }

        /// <summary>
        /// Opens a dialog window which allows the user to set the custom preferences as well as seeing the preferences
        /// that are already set.
        /// Note: This will create a new worker with the supplied <paramref name="group"/> and <paramref name="label"/>
        /// if the cache does not already have a value cached.
        /// </summary>
        /// <param name="group">The group guid of the worker</param>
        /// <param name="label">The label guid of the worker</param>
        private void OpenWorkerProperties(Guid group, Guid label)
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

            using (var options = new Parameters(worker))
            {
                options.ShowDialog();

                if (options.RequirementsAreSet)
                {
                    cache[label] = worker;
                }
                else
                {
                    // ToDo: Inform user of missing required preferences
                }
            }
        }
    }
}