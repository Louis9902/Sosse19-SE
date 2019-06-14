using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;

namespace TinyTasksDashboard
{
    partial class Parameters
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
        
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelGroup = new System.Windows.Forms.Label();
            this.labelLabel = new System.Windows.Forms.Label();
            
            this.options = new System.Windows.Forms.DataGridView();
            this.optionsObject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.optionsLabel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.optionsValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            
            this.controlOkay = new System.Windows.Forms.Button();
            this.controlCancel = new System.Windows.Forms.Button();
            
            ((System.ComponentModel.ISupportInitialize) (this.options)).BeginInit();
            this.SuspendLayout();
            
            {
                this.labelGroup.Location = new System.Drawing.Point(15, 15);
                this.labelGroup.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
                this.labelGroup.Name = "labelGroup";
                this.labelGroup.Size = new System.Drawing.Size(620, 25);
                this.labelGroup.TabIndex = 0;
                this.labelGroup.Text = "Group = ?";
            }
            {
                this.labelLabel.Location = new System.Drawing.Point(15, 45);
                this.labelLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
                this.labelLabel.Name = "labelLabel";
                this.labelLabel.Size = new System.Drawing.Size(620, 25);
                this.labelLabel.TabIndex = 1;
                this.labelLabel.Text = "Label = ?";
            }

            {
                {
                    this.options.Location = new System.Drawing.Point(15, 75);
                    this.options.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
                    this.options.Name = "options";
                    this.options.Size = new System.Drawing.Size(620, 320);
                    this.options.TabIndex = 2;
                }
                {
                    this.options.ShowEditingIcon = false;
                    this.options.RowHeadersVisible = false;
                    this.options.EditMode = DataGridViewEditMode.EditOnKeystroke;
                    this.options.AllowUserToResizeColumns = false;
                    this.options.AllowUserToResizeRows = false;
                    this.options.AllowUserToAddRows = false;
                    this.options.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                    this.options.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]
                    {
                        this.optionsLabel,
                        this.optionsValue,
                        this.optionsObject
                    });
                }
                {
                    this.options.CellClick += this.OnCellClick;
                    this.options.CellEnter += this.OnCellClick;
                }
            }

            {
                this.optionsLabel.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.optionsLabel.FillWeight = 132.4873F;
                this.optionsLabel.HeaderText = "Label";
                this.optionsLabel.Name = "optionsLabel";
                this.optionsLabel.ReadOnly = true;
            }
            {
                this.optionsValue.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.optionsValue.FillWeight = 132.4873F;
                this.optionsValue.HeaderText = "Value";
                this.optionsValue.Name = "optionsValue";
            }
            {
                this.optionsObject.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.optionsObject.FillWeight = 132.4873F;
                this.optionsObject.HeaderText = "Object <Internal>";
                this.optionsObject.Name = "optionsObject";
                this.optionsObject.Visible = false;
            }
            {
                this.controlOkay.Location = new System.Drawing.Point(515, 400);
                this.controlOkay.Name = "controlOkay";
                this.controlOkay.Size = new System.Drawing.Size(120, 35);
                this.controlOkay.TabIndex = 3;
                this.controlOkay.Text = "Okay";
                this.controlOkay.UseVisualStyleBackColor = true;
                this.controlOkay.Click += new EventHandler(ClickAccept);
            }
            {
                this.controlCancel.Location = new System.Drawing.Point(390, 400);
                this.controlCancel.Name = "controlCancel";
                this.controlCancel.Size = new System.Drawing.Size(120, 35);
                this.controlCancel.TabIndex = 4;
                this.controlCancel.Text = "Cancel";
                this.controlCancel.UseVisualStyleBackColor = true;
                this.controlCancel.Click += new EventHandler(ClickCancel);
            }


            {
                this.Controls.Add(this.controlCancel);
                this.Controls.Add(this.controlOkay);
                this.Controls.Add(this.labelLabel);
                this.Controls.Add(this.labelGroup);
                this.Controls.Add(this.options);
            }
            {
                this.ClientSize = new System.Drawing.Size(650, 445);
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
                this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
                this.Name = "Parameters";
                this.Text = "Parameters";
                this.Font = new System.Drawing.Font("Courier New", 8.25F);
                this.Icon = Icon.ExtractAssociatedIcon("Resources/helmet.ico");
            }

            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);

            ((System.ComponentModel.ISupportInitialize) (this.options)).EndInit();
            this.ResumeLayout(false);
        }
        
        private System.Windows.Forms.Label labelGroup;
        private System.Windows.Forms.Label labelLabel;
        private System.Windows.Forms.DataGridView options;
        private System.Windows.Forms.DataGridViewTextBoxColumn optionsObject;
        private System.Windows.Forms.DataGridViewTextBoxColumn optionsLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn optionsValue;
        private System.Windows.Forms.Button controlOkay;
        private System.Windows.Forms.Button controlCancel;
    }
}