using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WorksKit;
using WorksKit.Worker;

namespace WorksForm
{
    public partial class Form1 : Form
    {
        private const string ConfigFilePath = "Workers.dat";

        private readonly Dictionary<Guid, IWorker> workers;
        private readonly Workers handler = new Workers(ConfigFilePath);

        private bool changes;

        public Form1()
        {
            workers = new Dictionary<Guid, IWorker>();
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            if (handler.Load(workers))
            {
                foreach (var worker in workers.Values)
                {
                    var preferences = worker.Preferences;
                    var source = preferences.Preference<string>("source");
                    var target = preferences.Preference<string>("target");

                    dataGridView.Rows.Add(source.Value, target.Value, worker.Label.ToString());
                }
            }
            else
            {
                MessageBox.Show("Could not load configuration");
                Application.Exit();
            }
        }

        private void Button_OK_Click(object sender, EventArgs e)
        {
            if (!handler.Save(workers))
            {
                MessageBox.Show("Could not save configuration");
            }

            Application.Exit();
        }

        private void Button_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            changes = true;
            if (dataGridView.Rows[e.RowIndex].Cells[0].Value == null &&
                dataGridView.Rows[e.RowIndex].Cells[1].Value == null)
            {
                //If a new line is being edited, we have to add a new empty line
                dataGridView.Rows.AddCopy(dataGridView.Rows.Count - 1);
            }

            var result = folderBrowserDialog1.ShowDialog();
            dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = folderBrowserDialog1.SelectedPath;
        }

        private void dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            changes = true;
            if (workers.Count <= e.RowIndex)
            {
                var newWorker = Workers.CreateWorker<SyncWorker>();
                newWorker.Source = "";
                newWorker.Target = "";
                workers.Add(newWorker.Label, newWorker);
                dataGridView.Rows[e.RowIndex].Cells[2].Value = newWorker.Label.ToString();
            }

            if (!workers.TryGetValue(Guid.Parse((string) dataGridView.Rows[e.RowIndex].Cells[2].Value), out var worker))
            {
                return;
            }

            switch (e.ColumnIndex)
            {
                case 0:
                    worker.Preferences.Preference<string>("source").Value =
                        (string) dataGridView.Rows[e.RowIndex].Cells[0].Value;
                    break;
                case 1:
                    worker.Preferences.Preference<string>("target").Value =
                        (string) dataGridView.Rows[e.RowIndex].Cells[1].Value;
                    break;
            }
        }

        private void dataGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (workers.Count == 0)
            {
                //Some systems send some RowsRemoved-events at startup. 
                //Those are stopped here, because otherwise the app crashes
                return;
            }

            changes = true;
            for (var row = e.RowIndex; row < e.RowIndex + e.RowCount; row++)
            {
                workers.Remove(workers.ElementAt(row).Value.Label);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            switch (e.CloseReason)
            {
                case CloseReason.WindowsShutDown:
                    Button_OK_Click(sender, new EventArgs());
                    break;
                case CloseReason.UserClosing when changes:
                {
                    var result = MessageBox.Show("Do you want to save changes?", "Warning",
                        MessageBoxButtons.YesNoCancel);
                    switch (result)
                    {
                        case DialogResult.Yes:
                            Button_OK_Click(sender, new EventArgs());
                            break;
                        case DialogResult.Cancel:
                            e.Cancel = true;
                            break;
                    }

                    break;
                }
            }
        }
    }
}