using System.Drawing;

namespace WorksManager
{
    partial class Overview
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

        #region Windows Form Designer generated code

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
            // 
            // Workers
            // 
            this.Workers.ColumnHeadersHeightSizeMode =
                System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Workers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]
                {this.WorkersGridGroups, this.WorkersGridLabels, this.WorkersGridKind});
            this.Workers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Workers.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.Workers.Font = new System.Drawing.Font("Courier New", 8.25F);
            this.Workers.Location = new System.Drawing.Point(0, 0);
            this.Workers.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Workers.Name = "Workers";
            this.Workers.Size = new System.Drawing.Size(878, 519);
            this.Workers.TabIndex = 0;
            this.Workers.CellDoubleClick +=
                new System.Windows.Forms.DataGridViewCellEventHandler(this.OnWorkerCellClick);
            // 
            // WorkersGridGroups
            // 
            this.WorkersGridGroups.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.WorkersGridGroups.FillWeight = 132.4873F;
            this.WorkersGridGroups.HeaderText = "Group";
            this.WorkersGridGroups.Name = "WorkersGridGroups";
            this.WorkersGridGroups.Visible = false;
            // 
            // WorkersGridLabels
            // 
            this.WorkersGridLabels.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.WorkersGridLabels.FillWeight = 132.4873F;
            this.WorkersGridLabels.HeaderText = "Label";
            this.WorkersGridLabels.Name = "WorkersGridLabels";
            // 
            // WorkersGridKind
            // 
            this.WorkersGridKind.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.WorkersGridKind.FillWeight = 132.4873F;
            this.WorkersGridKind.HeaderText = "Kind";
            this.WorkersGridKind.Name = "WorkersGridKind";
            // 
            // WorkerOverview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(878, 519);
            this.Controls.Add(this.Workers);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Overview";
            this.Text = "Worker Overview";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnOverviewSave);
            this.Load += new System.EventHandler(this.OnOverviewLoad);
            ((System.ComponentModel.ISupportInitialize) (this.Workers)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataGridView Workers;
        private System.Windows.Forms.DataGridViewTextBoxColumn WorkersGridLabels;
        private System.Windows.Forms.DataGridViewTextBoxColumn WorkersGridGroups;
        private System.Windows.Forms.DataGridViewTextBoxColumn WorkersGridKind;
    }
}