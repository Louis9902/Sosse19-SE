using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using WorksKit;
using WorksKit.Worker;
using WorksKit.Worker.Preferences;

namespace GUI
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        static string ConfigFilePath = "Workers.dat";
        Dictionary<Guid, IWorker> dictionary = new Dictionary<Guid, IWorker>();
        Workers w = new Workers(ConfigFilePath);

        bool changes = false;

        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            if (!File.Exists(ConfigFilePath))
                w.CreateEmptyFile();



            if (w.Load(dictionary))
            {
                foreach (KeyValuePair<Guid, IWorker> item in dictionary)
                {
                    Guid guid = item.Key;
                    IWorker worker = item.Value;
                    PreferenceSet preferences = worker.Preferences;
                    Preference<string> source = preferences.Preference<string>("source");
                    Preference<string> target = preferences.Preference<string>("target");

                    dataGridView.Rows.Add(new object[] { source.Value, target.Value, worker.Label.ToString() });
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
            if (!w.Save(dictionary))
            {
                MessageBox.Show("Could not save configuration");
            }
            Application.Exit();
        }

        private void Button_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            changes = true;
            if (dataGridView.Rows[e.RowIndex].Cells[0].Value == null && dataGridView.Rows[e.RowIndex].Cells[1].Value == null)
            {
                //If a new line is being edited, we have to add a new empty line
                dataGridView.Rows.AddCopy(dataGridView.Rows.Count - 1);
            }    

            DialogResult result = folderBrowserDialog1.ShowDialog();
            dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = folderBrowserDialog1.SelectedPath;
            
        }

        private void dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            
            if (e.RowIndex < 0)
                return;
            changes = true;
            if (dictionary.Count <= e.RowIndex)
            {
                SyncWorker newWorker =  Workers.CreateNewWorker<SyncWorker>();
                newWorker.Source = "";
                newWorker.Target = "";
                dictionary.Add(newWorker.Label, newWorker);
                dataGridView.Rows[e.RowIndex].Cells[2].Value = newWorker.Label.ToString();
            }

            IWorker worker;
            dictionary.TryGetValue(Guid.Parse((string)dataGridView.Rows[e.RowIndex].Cells[2].Value), out worker);
            if (e.ColumnIndex == 0)
            {
                worker.Preferences.Preference<string>("source").Value = (string)dataGridView.Rows[e.RowIndex].Cells[0].Value;

            }
            else if (e.ColumnIndex == 1)
            {
                worker.Preferences.Preference<string>("target").Value = (string)dataGridView.Rows[e.RowIndex].Cells[1].Value;
            }
        }

        private void dataGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (dictionary.Count == 0)
            {
                //Some systems send some RowsRemoved-events at startup. 
                //Those are stopped here, because otherwise the app crashes
                return;
            }
            changes = true;
            for (int row = e.RowIndex; row < e.RowIndex + e.RowCount; row++)
            {
                dictionary.Remove(dictionary.ElementAt(row).Value.Label);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.WindowsShutDown)
            {
                Button_OK_Click(sender, new EventArgs());
            }
            else if(e.CloseReason == CloseReason.UserClosing && changes)
            {
                DialogResult result = MessageBox.Show("Do you want to save changes?", "Warning", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                    Button_OK_Click(sender, new EventArgs());
                }
                else if(result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
            
        }
    }
}
