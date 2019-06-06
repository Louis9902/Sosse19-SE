using System;
using System.Data;
using System.Linq;
using System.Media;
using System.Windows.Forms;
using TinyTasksDashboard.Properties;
using TinyTasksKit.Worker;
using TinyTasksKit.Worker.Preferences;

namespace TinyTasksDashboard
{
    public partial class Parameters : Form
    {
        private readonly IWorker worker;
        private readonly PreferenceSet preferences;

        private bool avoid;
        private bool abort;

        public Parameters(IWorker worker)
        {
            this.worker = worker;
            preferences = worker.Preferences;
            InitializeComponent();
            ViewRefresh();
            DialogResult = DialogResult.None;
        }

        private void ViewRefresh()
        {
            labelGroup.Text = $@"Group: {worker.Group} of type {worker.GetType().Name}";
            labelLabel.Text = $@"Label: {worker.Label}";
            foreach (var preference in preferences.GetVisible()) ShowPreference(preference);
        }

        private void ShowPreference(IPreference preference)
        {
            options.Rows.Add(preference.Name, preference.ToView(), preference);
        }

        private void SaveRows()
        {
            foreach (DataGridViewRow row in options.Rows)
            {
                var preference = (IPreference) row.Cells[2].Value;
                var value = row.Cells[1].Value as string;
                preference.FromView(value);
            }
        }

        private void RefreshDialogResult()
        {
            var consent = worker.Preferences.GetAll()
                .Where(preference => preference.Visible)
                .All(preference => preference.HasValueSet);
            DialogResult = consent ? DialogResult.OK : DialogResult.No;
        }

        private void OnFormClosing(object sender, FormClosingEventArgs args)
        {
            if (abort) args.Cancel = true;
            if (avoid) return;
            RefreshDialogResult();
        }

        private void ClickAccept(object sender, EventArgs args)
        {
            RefreshDialogResult();
            if (DialogResult != DialogResult.OK)
            {
                abort = true;
                SystemSounds.Exclamation.Play();
                return;
            }

            SaveRows();
            abort = false;
            avoid = true;
            Close();
        }

        private void ClickCancel(object sender, EventArgs args)
        {
            RefreshDialogResult();
            abort = false;
            avoid = true;
            Close();
        }

        private void OnCellClick(object sender, DataGridViewCellEventArgs args)
        {
            if (args.RowIndex < 0) return;
            if (args.ColumnIndex != 1) return;
            var preference = (IPreference) options.Rows[args.RowIndex].Cells[2].Value;

            switch (preference.DataType)
            {
                case PreferenceDataType.PathFolder when preference is ScalarPreference<string> content:
                {
                    using (var browser = new FolderBrowserDialog())
                    {
                        browser.SelectedPath = content.Value ?? string.Empty;
                        var result = browser.ShowDialog();
                        if (result != DialogResult.OK) break;
                        content.Value = browser.SelectedPath;
                        options.Rows[args.RowIndex].Cells[1].Value = browser.SelectedPath;
                    }

                    break;
                }

                case PreferenceDataType.Primitive:
                    break;
                case PreferenceDataType.Collection:
                    // ToDo: Support Collection Types
                    break;
            }
        }
    }
}