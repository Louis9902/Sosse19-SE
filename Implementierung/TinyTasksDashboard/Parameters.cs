using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using TinyTasksDashboard.Properties;
using TinyTasksKit.Worker;
using TinyTasksKit.Worker.Preferences;

namespace TinyTasksDashboard
{
    public partial class Parameters : Form
    {
        private readonly IWorker worker;
        private bool bypassCloseEvent;

        public Parameters(IWorker worker)
        {
            this.worker = worker;
            InitializeComponent();
            Initialize();
        }

        public bool HasAllValues { get; private set; }
        public bool Abort { get; private set; }

        private void Initialize()
        {
            Group.Text = $@"Group: {worker.Group}";
            Label.Text = $@"Label: {worker.Label}";
            LoadRows();
        }

        private void LoadRows()
        {
            var preferences = worker.Preferences;

            foreach (var preference in preferences.GetVisible())
            {
                Options.Rows.Add(preference, preference.Name, preference.ToView());
            }
        }

        private void SaveRows()
        {
            foreach (DataGridViewRow row in Options.Rows)
            {
                var preference = (IPreference) row.Cells[0].Value;
                var value = row.Cells[2].Value as string;
                preference.FromView(value);
            }
        }

        private void OnWindowClose(object sender, FormClosingEventArgs args)
        {
            if (bypassCloseEvent) return;
            switch (args.CloseReason)
            {
                case CloseReason.TaskManagerClosing:
                case CloseReason.WindowsShutDown:
                {
                    SaveRows();
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
                        {
                            SaveRows();
                            Abort = false;
                            HasAllValues = worker.Preferences.GetAll()
                                .Where(preference => preference.Visible)
                                .All(preference => preference.HasValueSet);
                            break;
                        }

                        case DialogResult.No:
                        {
                            Abort = true;
                            HasAllValues = worker.Preferences.GetAll()
                                .Where(preference => preference.Visible)
                                .All(preference => preference.HasValueSet);
                            break;
                        }

                        case DialogResult.Cancel:
                        {
                            args.Cancel = true;
                            break;
                        }
                    }

                    break;
                }
            }
        }

        private void OnButtonOkay(object sender, EventArgs args)
        {
            bypassCloseEvent = true;
            SaveRows();
            Abort = false;
            HasAllValues = worker.Preferences.GetAll()
                .Where(preference => preference.Visible)
                .All(preference => preference.HasValueSet);
            Close();
        }

        private void OnButtonCancel(object sender, EventArgs args)
        {
            bypassCloseEvent = true;
            Abort = true;
            HasAllValues = worker.Preferences.GetAll()
                .Where(preference => preference.Visible)
                .All(preference => preference.HasValueSet);
            Close();
        }

        private void OnCellClick(object sender, DataGridViewCellEventArgs args)
        {
            if (args.RowIndex < 0) return;
            if (args.ColumnIndex != 2) return;
            var preference = (IPreference) Options.Rows[args.RowIndex].Cells[0].Value;

            switch (preference.DataType)
            {
                case PreferenceDataType.Path when preference is ScalarPreference<string> content:
                {
                    using (var browser = new FolderBrowserDialog())
                    {
                        browser.SelectedPath = content.Value ?? string.Empty;
                        var result = browser.ShowDialog();
                        if (result != DialogResult.OK) break;
                        content.Value = browser.SelectedPath;
                        Options.Rows[args.RowIndex].Cells[2].Value = browser.SelectedPath;
                    }

                    break;
                }

                case PreferenceDataType.Primitive:
                    break;
                case PreferenceDataType.Collection:
                    break;
            }
        }

        public void Reset()
        {
            bypassCloseEvent = false;
            HasAllValues = false;
            Abort = false;
        }
    }
}