using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TinyTasksDashboard.Properties;
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
            this.overview = new System.Windows.Forms.DataGridView();
            
            this.overviewGroup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.overviewLabel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.overviewObject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            
            ((ISupportInitialize) (this.overview)).BeginInit();
            
            this.SuspendLayout();
            
            this.overview.Location = new System.Drawing.Point(0, 0);
            this.overview.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.overview.Name = "overview";
            this.overview.Size = new System.Drawing.Size(878, 519);
            this.overview.TabIndex = 0;
            
            this.overview.Dock = System.Windows.Forms.DockStyle.Fill;
            
            this.overview.ShowEditingIcon = false;
            this.overview.RowHeadersVisible = false;
            this.overview.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.overview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.overview.AllowUserToResizeColumns = false;
            this.overview.AllowUserToResizeRows = false;
            this.overview.AllowUserToDeleteRows = true;
            this.overview.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.overview.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]
            {
                this.overviewGroup,
                this.overviewLabel,
                this.overviewObject
            });
            this.overview.CellDoubleClick += OnCellClick;
            this.overview.UserDeletedRow += OnRowDelete;
            
            this.overviewGroup.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.overviewGroup.FillWeight = 132.4873F;
            this.overviewGroup.HeaderText = "Group";
            this.overviewGroup.Name = "overviewGroup";
            this.overviewGroup.ReadOnly = true;
            
            this.overviewLabel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.overviewLabel.FillWeight = 132.4873F;
            this.overviewLabel.HeaderText = "Label";
            this.overviewLabel.Name = "overviewLabel";
            this.overviewLabel.ReadOnly = true;
            
            this.overviewObject.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.overviewObject.FillWeight = 132.4873F;
            this.overviewObject.HeaderText = "Object <Internal>";
            this.overviewObject.Name = "overviewObject";
            this.overviewObject.ReadOnly = true;
            this.overviewObject.Visible = false;
            
            this.Controls.Add(this.overview);
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(878, 519);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Dashboard";
            this.Text = "Worker Dashboard";
            this.Font = new System.Drawing.Font("Courier New", 8.25F);
            this.Icon = Icon.ExtractAssociatedIcon("Resources/helmet.ico");
            
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnFormLoading);
            
            ((System.ComponentModel.ISupportInitialize) (this.overview)).EndInit();
            this.ResumeLayout(false);
        }

        #region Windows Form Designer generated code

        #endregion

        private System.Windows.Forms.DataGridView overview;
        private System.Windows.Forms.DataGridViewTextBoxColumn overviewLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn overviewGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn overviewObject;
    }
}