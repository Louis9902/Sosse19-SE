using System.ComponentModel;
using System.Windows.Forms;

namespace TinyTasksDashboard
{
    partial class WorkerOptions
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.Group = new System.Windows.Forms.Label();
            this.Label = new System.Windows.Forms.Label();
            
            this.Options = new System.Windows.Forms.DataGridView();
            this.OptionsGridLabel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OptionsGridValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            
            this.Okay = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            
            ((System.ComponentModel.ISupportInitialize) (this.Options)).BeginInit();
            
            this.SuspendLayout();
            // 
            // Group
            // 
            this.Group.Location = new System.Drawing.Point(15, 15);
            this.Group.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Group.Name = "Group";
            this.Group.Size = new System.Drawing.Size(620, 25);
            this.Group.TabIndex = 0;
            this.Group.Text = "Group";
            // 
            // Label
            // 
            this.Label.Location = new System.Drawing.Point(15, 45);
            this.Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label.Name = "Label";
            this.Label.Size = new System.Drawing.Size(620, 25);
            this.Label.TabIndex = 1;
            this.Label.Text = "Label";
            // 
            // Options
            // 
            this.Options.Location = new System.Drawing.Point(15, 75);
            this.Options.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Options.Name = "Options";
            this.Options.Size = new System.Drawing.Size(620, 320);
            this.Options.TabIndex = 2;

            this.Options.EditMode = DataGridViewEditMode.EditOnEnter;
            this.Options.AllowUserToResizeColumns = false;
            this.Options.AllowUserToResizeRows = false;
            this.Options.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Options.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]
            {
                this.OptionsGridLabel,
                this.OptionsGridValue
            });

            this.OptionsGridLabel.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.OptionsGridLabel.FillWeight = 132.4873F;
            this.OptionsGridLabel.HeaderText = "Label";
            this.OptionsGridLabel.Name = "OptionsGridLabel";
            
            this.OptionsGridValue.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.OptionsGridValue.FillWeight = 132.4873F;
            this.OptionsGridValue.HeaderText = "Value";
            this.OptionsGridValue.Name = "OptionsGridValue";
            // 
            // Okay
            // 
            this.Okay.Location = new System.Drawing.Point(515, 400);
            this.Okay.Name = "Okay";
            this.Okay.Size = new System.Drawing.Size(120, 35);
            this.Okay.TabIndex = 3;
            this.Okay.Text = "Okay";
            this.Okay.UseVisualStyleBackColor = true;
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(390, 400);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(120, 35);
            this.Cancel.TabIndex = 4;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // WorkerOptions
            // 
            this.ClientSize = new System.Drawing.Size(650, 445);
            
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Okay);
            this.Controls.Add(this.Label);
            this.Controls.Add(this.Group);
            this.Controls.Add(this.Options);
            
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "WorkerOptions";
            this.Text = "WorkerOptions";
            this.Font = new System.Drawing.Font("Courier New", 8.25F);
            
            ((System.ComponentModel.ISupportInitialize) (this.Options)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label Group;
        private System.Windows.Forms.Label Label;
        private System.Windows.Forms.DataGridView Options;
        private System.Windows.Forms.DataGridViewTextBoxColumn OptionsGridLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn OptionsGridValue;
        private System.Windows.Forms.Button Okay;
        private System.Windows.Forms.Button Cancel;
    }
}