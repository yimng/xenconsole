﻿namespace XenAdmin.Wizards.NewSRWizard_Pages.Frontends
{
    partial class LVMoMirrorSummaryPage
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridViewSummary = new XenAdmin.Controls.DataGridViewEx.DataGridViewEx();
            this.ColumnArrow = new System.Windows.Forms.DataGridViewImageColumn();
            this.ColumnDetails = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSummary)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewSummary
            // 
            this.dataGridViewSummary.AllowUserToResizeColumns = false;
            this.dataGridViewSummary.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridViewSummary.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridViewSummary.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridViewSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSummary.ColumnHeadersVisible = false;
            this.dataGridViewSummary.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnArrow,
            this.ColumnDetails});
            this.dataGridViewSummary.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewSummary.Name = "dataGridViewSummary";
            this.dataGridViewSummary.ReadOnly = true;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            this.dataGridViewSummary.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewSummary.Size = new System.Drawing.Size(533, 380);
            this.dataGridViewSummary.TabIndex = 0;
            this.dataGridViewSummary.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewSummary_CellClick);
            // 
            // ColumnArrow
            // 
            this.ColumnArrow.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle1.NullValue = "System.Drawing.Bitmap";
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnArrow.DefaultCellStyle = dataGridViewCellStyle1;
            this.ColumnArrow.FillWeight = 31.26904F;
            this.ColumnArrow.Frozen = true;
            this.ColumnArrow.Name = "ColumnArrow";
            this.ColumnArrow.ReadOnly = true;
            this.ColumnArrow.Width = 5;
            // 
            // ColumnDetails
            // 
            this.ColumnDetails.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnDetails.DefaultCellStyle = dataGridViewCellStyle2;
            this.ColumnDetails.FillWeight = 172.4619F;
            this.ColumnDetails.Name = "ColumnDetails";
            this.ColumnDetails.ReadOnly = true;
            // 
            // LVMoMirrorSummaryPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.dataGridViewSummary);
            this.DoubleBuffered = true;
            this.Name = "LVMoMirrorSummaryPage";
            this.Size = new System.Drawing.Size(533, 380);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSummary)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.DataGridViewEx.DataGridViewEx dataGridViewSummary;
        private System.Windows.Forms.DataGridViewImageColumn ColumnArrow;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDetails;
    }
}
