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

            {
                {
                    this.overview.Location = new Point(0, 0);
                    this.overview.Margin = new Padding(4, 3, 4, 3);
                    this.overview.Name = "overview";
                    this.overview.Size = new Size(878, 519);
                    this.overview.TabIndex = 0;
                }
                {
                    this.overview.Dock = DockStyle.Fill;
                    this.overview.ShowEditingIcon = false;
                    this.overview.RowHeadersVisible = false;
                    this.overview.EditMode = DataGridViewEditMode.EditOnKeystroke;
                    this.overview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    this.overview.AllowUserToResizeColumns = false;
                    this.overview.AllowUserToResizeRows = false;
                    this.overview.AllowUserToDeleteRows = true;
                    this.overview.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                    this.overview.Columns.AddRange(new DataGridViewColumn[]
                    {
                        this.overviewGroup,
                        this.overviewLabel,
                        this.overviewObject
                    });
                }
                {
                    this.overview.CellDoubleClick += OnCellClick;
                    this.overview.UserDeletedRow += OnRowDelete;
                }
            }

            {
                this.overviewGroup.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.overviewGroup.FillWeight = 132.4873F;
                this.overviewGroup.HeaderText = "Group";
                this.overviewGroup.Name = "overviewGroup";
                this.overviewGroup.ReadOnly = true;
            }

            {
                this.overviewLabel.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.overviewLabel.FillWeight = 132.4873F;
                this.overviewLabel.HeaderText = "Label";
                this.overviewLabel.Name = "overviewLabel";
                this.overviewLabel.ReadOnly = true;
            }

            {
                this.overviewObject.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.overviewObject.FillWeight = 132.4873F;
                this.overviewObject.HeaderText = "Object <Internal>";
                this.overviewObject.Name = "overviewObject";
                this.overviewObject.ReadOnly = true;
                this.overviewObject.Visible = false;
            }

            {
                this.Controls.Add(this.overview);
            }
            {
                this.AutoScaleDimensions = new SizeF(7F, 15F);
                this.AutoScaleMode = AutoScaleMode.Font;
                this.ClientSize = new Size(878, 519);
                this.FormBorderStyle = FormBorderStyle.FixedSingle;
                this.Margin = new Padding(4, 3, 4, 3);
                this.Name = "Dashboard";
                this.Text = "Worker Dashboard";
                this.Font = new Font("Courier New", 8.25F);
                this.Icon = Icon.ExtractAssociatedIcon("Resources/helmet.ico");
            }
            
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnFormLoading);
            
            ((System.ComponentModel.ISupportInitialize) (this.overview)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.DataGridView overview;
        private System.Windows.Forms.DataGridViewTextBoxColumn overviewLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn overviewGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn overviewObject;
    }
}