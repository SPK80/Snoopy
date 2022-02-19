namespace Snoopy.Views
{
	partial class NewSourceForm
	{
		/// <summary>
		/// Обязательная переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Освободить все используемые ресурсы.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Код, автоматически созданный конструктором форм Windows

		/// <summary>
		/// Требуемый метод для поддержки конструктора — не изменяйте 
		/// содержимое этого метода с помощью редактора кода.
		/// </summary>
		private void InitializeComponent()
		{
            this.bConfirm = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.tbPath = new System.Windows.Forms.TextBox();
            this.bShowPathDialog = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bConfirm
            // 
            this.bConfirm.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bConfirm.Location = new System.Drawing.Point(9, 118);
            this.bConfirm.Name = "bConfirm";
            this.bConfirm.Size = new System.Drawing.Size(75, 23);
            this.bConfirm.TabIndex = 2;
            this.bConfirm.Text = "Начать\r\nсканирование";
            this.bConfirm.UseVisualStyleBackColor = true;
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(90, 118);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 3;
            this.bCancel.Text = "Отменить";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // tbPath
            // 
            this.tbPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPath.Location = new System.Drawing.Point(9, 92);
            this.tbPath.Name = "tbPath";
            this.tbPath.Size = new System.Drawing.Size(403, 20);
            this.tbPath.TabIndex = 4;
            this.tbPath.TextChanged += new System.EventHandler(this.tbPath_TextChanged);
            this.tbPath.DoubleClick += new System.EventHandler(this.tbPath_DoubleClick);
            // 
            // bShowPathDialog
            // 
            this.bShowPathDialog.Location = new System.Drawing.Point(409, 92);
            this.bShowPathDialog.Name = "bShowPathDialog";
            this.bShowPathDialog.Size = new System.Drawing.Size(22, 20);
            this.bShowPathDialog.TabIndex = 5;
            this.bShowPathDialog.Text = "...";
            this.bShowPathDialog.UseVisualStyleBackColor = true;
            this.bShowPathDialog.Click += new System.EventHandler(this.bShowPathDialog_Click);
            // 
            // NewSourceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 151);
            this.Controls.Add(this.bShowPathDialog);
            this.Controls.Add(this.tbPath);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bConfirm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewSourceForm";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Сканер";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ScannerBox_FormClosing);
            this.Load += new System.EventHandler(this.ScannerBox_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Button bConfirm;
		private System.Windows.Forms.Button bCancel;
		private System.Windows.Forms.TextBox tbPath;
		private System.Windows.Forms.Button bShowPathDialog;
	}
}
