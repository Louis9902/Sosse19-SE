using System.Windows.Forms;
using static System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode;

namespace TinyTasksDashboard
{
    partial class Dashboard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Workers = new System.Windows.Forms.DataGridView();
            
            this.WorkersGridGroups = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WorkersGridLabels = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WorkersGridKind = new System.Windows.Forms.DataGridViewTextBoxColumn();
            
            ((System.ComponentModel.ISupportInitialize) (this.Workers)).BeginInit();
            
            this.SuspendLayout();
            
            this.Workers.Location = new System.Drawing.Point(0, 0);
            this.Workers.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Workers.Name = "Workers";
            this.Workers.Size = new System.Drawing.Size(878, 519);
            this.Workers.TabIndex = 0;
            
            this.Workers.Dock = System.Windows.Forms.DockStyle.Fill;
            
            this.Workers.ShowEditingIcon = false;
            this.Workers.RowHeadersVisible = false;
            this.Workers.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.Workers.AllowUserToResizeColumns = false;
            this.Workers.AllowUserToResizeRows = false;
            this.Workers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Workers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]
            {
                this.WorkersGridGroups,
                this.WorkersGridLabels,
                this.WorkersGridKind
            });
            this.Workers.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellClick);
            
            this.WorkersGridGroups.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.WorkersGridGroups.FillWeight = 132.4873F;
            this.WorkersGridGroups.HeaderText = "Group";
            this.WorkersGridGroups.Name = "WorkersGridGroups";
            this.WorkersGridGroups.Visible = false;
            
            this.WorkersGridLabels.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.WorkersGridLabels.FillWeight = 132.4873F;
            this.WorkersGridLabels.HeaderText = "Label";
            this.WorkersGridLabels.Name = "WorkersGridLabels";
            this.WorkersGridLabels.ReadOnly = true;
            
            this.WorkersGridKind.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.WorkersGridKind.FillWeight = 132.4873F;
            this.WorkersGridKind.HeaderText = "Kind";
            this.WorkersGridKind.Name = "WorkersGridKind";
            this.WorkersGridKind.ReadOnly = true;
            
            this.Controls.Add(this.Workers);
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(878, 519);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Dashboard";
            this.Text = "Worker Dashboard";
            this.Font = new System.Drawing.Font("Courier New", 8.25F);
            
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnWindowClose);
            this.Load += new System.EventHandler(this.OnOverviewLoad);
            
            ((System.ComponentModel.ISupportInitialize) (this.Workers)).EndInit();
            this.ResumeLayout(false);
        }

        #region Windows Form Designer generated code

        #endregion

        private System.Windows.Forms.DataGridView Workers;
        private System.Windows.Forms.DataGridViewTextBoxColumn WorkersGridLabels;
        private System.Windows.Forms.DataGridViewTextBoxColumn WorkersGridGroups;
        private System.Windows.Forms.DataGridViewTextBoxColumn WorkersGridKind;
    }
}