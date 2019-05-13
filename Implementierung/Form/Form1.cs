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
namespace GUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        static string ConfigFilePath = "config.txt";



        private void Form1_Load(object sender, EventArgs e)
        {
            if (!File.Exists(ConfigFilePath))
            {
                File.Create(ConfigFilePath).Close();
            }
            StreamReader sr;

            try
            {
                sr = new StreamReader(ConfigFilePath);
            }
            catch (System.IO.IOException)
            {
                MessageBox.Show("The program is currently copying the files. Please retry in a few moments.");
                Application.Exit();
                return;
                
            }
           
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                string[] elements = line.Split('|');

                dataGridView.Rows.Add(new object[] { elements[1], elements[2] });
            }
            sr.Close();
        }

        private void Button_OK_Click(object sender, EventArgs e)
        {
            if (!File.Exists(ConfigFilePath))
                File.Create(ConfigFilePath);

            StreamWriter sw = new StreamWriter(ConfigFilePath);
            for (int i = 0; i < dataGridView.Rows.Count - 1; i++)
            {
                DataGridViewRow row = dataGridView.Rows[i];
                sw.Write("1|");
                sw.Write(row.Cells[0].Value);
                sw.Write("|");
                sw.WriteLine(row.Cells[1].Value);
            }

            sw.Close();
            this.Close();
        }

        private void Button_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = folderBrowserDialog1.SelectedPath;
        }
    }
}
