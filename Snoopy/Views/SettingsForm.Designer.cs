namespace Snoopy.Views
{
    partial class SettingsForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.bCancel = new System.Windows.Forms.Button();
            this.bConfirm = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.bForeColor = new System.Windows.Forms.Button();
            this.bFont = new System.Windows.Forms.Button();
            this.bBackColor = new System.Windows.Forms.Button();
            this.cbCustomizeControls = new System.Windows.Forms.CheckedListBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.tbTryRescanSpanIdle = new System.Windows.Forms.TextBox();
            this.cbShowHistory = new System.Windows.Forms.CheckBox();
            this.cbUpdateConfirm = new System.Windows.Forms.CheckBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.cblProcessingFields = new System.Windows.Forms.CheckedListBox();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.fontDialog = new System.Windows.Forms.FontDialog();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.bCancel);
            this.panel1.Controls.Add(this.bConfirm);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 428);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(366, 22);
            this.panel1.TabIndex = 11;
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.bCancel.Location = new System.Drawing.Point(166, 0);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(100, 22);
            this.bCancel.TabIndex = 3;
            this.bCancel.Text = "Отмена";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // bConfirm
            // 
            this.bConfirm.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bConfirm.Dock = System.Windows.Forms.DockStyle.Right;
            this.bConfirm.Location = new System.Drawing.Point(266, 0);
            this.bConfirm.Name = "bConfirm";
            this.bConfirm.Size = new System.Drawing.Size(100, 22);
            this.bConfirm.TabIndex = 4;
            this.bConfirm.Text = "OK";
            this.bConfirm.UseVisualStyleBackColor = true;
            this.bConfirm.Click += new System.EventHandler(this.bConfirm_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(366, 428);
            this.tabControl1.TabIndex = 12;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.bForeColor);
            this.tabPage1.Controls.Add(this.bFont);
            this.tabPage1.Controls.Add(this.bBackColor);
            this.tabPage1.Controls.Add(this.cbCustomizeControls);
            this.tabPage1.Location = new System.Drawing.Point(4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(358, 402);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Оформление";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // bForeColor
            // 
            this.bForeColor.Location = new System.Drawing.Point(227, 37);
            this.bForeColor.Name = "bForeColor";
            this.bForeColor.Size = new System.Drawing.Size(84, 23);
            this.bForeColor.TabIndex = 7;
            this.bForeColor.Text = "цвет шрифта";
            this.bForeColor.UseVisualStyleBackColor = true;
            this.bForeColor.Click += new System.EventHandler(this.bForeColor_Click);
            // 
            // bFont
            // 
            this.bFont.Location = new System.Drawing.Point(227, 66);
            this.bFont.Name = "bFont";
            this.bFont.Size = new System.Drawing.Size(84, 23);
            this.bFont.TabIndex = 6;
            this.bFont.Text = "шрифт";
            this.bFont.UseVisualStyleBackColor = true;
            this.bFont.Click += new System.EventHandler(this.bFont_Click);
            // 
            // bBackColor
            // 
            this.bBackColor.Location = new System.Drawing.Point(227, 8);
            this.bBackColor.Name = "bBackColor";
            this.bBackColor.Size = new System.Drawing.Size(84, 23);
            this.bBackColor.TabIndex = 5;
            this.bBackColor.Text = "цвет фона";
            this.bBackColor.UseVisualStyleBackColor = true;
            this.bBackColor.Click += new System.EventHandler(this.bBackColor_Click);
            // 
            // cbCustomizeControls
            // 
            this.cbCustomizeControls.Dock = System.Windows.Forms.DockStyle.Left;
            this.cbCustomizeControls.FormattingEnabled = true;
            this.cbCustomizeControls.Location = new System.Drawing.Point(3, 3);
            this.cbCustomizeControls.Name = "cbCustomizeControls";
            this.cbCustomizeControls.Size = new System.Drawing.Size(218, 396);
            this.cbCustomizeControls.TabIndex = 4;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.tbTryRescanSpanIdle);
            this.tabPage2.Controls.Add(this.cbShowHistory);
            this.tabPage2.Controls.Add(this.cbUpdateConfirm);
            this.tabPage2.Location = new System.Drawing.Point(4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(358, 402);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Поведение";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(199, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Интервал ожидания обновления, сек.";
            // 
            // tbTryRescanSpanIdle
            // 
            this.tbTryRescanSpanIdle.Location = new System.Drawing.Point(213, 49);
            this.tbTryRescanSpanIdle.Name = "tbTryRescanSpanIdle";
            this.tbTryRescanSpanIdle.Size = new System.Drawing.Size(110, 20);
            this.tbTryRescanSpanIdle.TabIndex = 8;
            this.tbTryRescanSpanIdle.Text = "0";
            // 
            // cbShowHistory
            // 
            this.cbShowHistory.AutoSize = true;
            this.cbShowHistory.Checked = true;
            this.cbShowHistory.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbShowHistory.Location = new System.Drawing.Point(11, 8);
            this.cbShowHistory.Name = "cbShowHistory";
            this.cbShowHistory.Size = new System.Drawing.Size(134, 17);
            this.cbShowHistory.TabIndex = 4;
            this.cbShowHistory.Text = "Отображать историю";
            this.cbShowHistory.UseVisualStyleBackColor = true;
            this.cbShowHistory.CheckedChanged += new System.EventHandler(this.cbShowHistory_CheckedChanged);
            // 
            // cbUpdateConfirm
            // 
            this.cbUpdateConfirm.AutoSize = true;
            this.cbUpdateConfirm.Location = new System.Drawing.Point(11, 29);
            this.cbUpdateConfirm.Name = "cbUpdateConfirm";
            this.cbUpdateConfirm.Size = new System.Drawing.Size(163, 17);
            this.cbUpdateConfirm.TabIndex = 3;
            this.cbUpdateConfirm.Text = "Подтверждать обновление";
            this.cbUpdateConfirm.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.cblProcessingFields);
            this.tabPage3.Location = new System.Drawing.Point(4, 4);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(358, 402);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Сканер";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // cblProcessingFields
            // 
            this.cblProcessingFields.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cblProcessingFields.CheckOnClick = true;
            this.cblProcessingFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cblProcessingFields.FormattingEnabled = true;
            this.cblProcessingFields.Location = new System.Drawing.Point(3, 3);
            this.cblProcessingFields.Name = "cblProcessingFields";
            this.cblProcessingFields.Size = new System.Drawing.Size(352, 396);
            this.cblProcessingFields.TabIndex = 2;
            this.cblProcessingFields.SelectedIndexChanged += new System.EventHandler(this.cblProcessingFields_SelectedIndexChanged);
            // 
            // fontDialog
            // 
            this.fontDialog.Apply += new System.EventHandler(this.fontDialog1_Apply);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(366, 450);
            this.ControlBox = false;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Настройки";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button bConfirm;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckBox cbShowHistory;
        private System.Windows.Forms.CheckBox cbUpdateConfirm;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbTryRescanSpanIdle;
        private System.Windows.Forms.CheckedListBox cblProcessingFields;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.FontDialog fontDialog;
        private System.Windows.Forms.Button bForeColor;
        private System.Windows.Forms.Button bFont;
        private System.Windows.Forms.Button bBackColor;
        private System.Windows.Forms.CheckedListBox cbCustomizeControls;
    }
}