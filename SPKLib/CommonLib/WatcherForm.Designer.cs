namespace CommonLib.Watcher
{
	partial class WatcherForm
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
			this.lbResults = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// lbResults
			// 
			this.lbResults.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbResults.FormattingEnabled = true;
			this.lbResults.Location = new System.Drawing.Point(0, 0);
			this.lbResults.Name = "lbResults";
			this.lbResults.Size = new System.Drawing.Size(664, 263);
			this.lbResults.TabIndex = 0;
			// 
			// WatcherForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(664, 263);
			this.Controls.Add(this.lbResults);
			this.Name = "WatcherForm";
			this.Text = "WatcherForm";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WatcherForm_FormClosing);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox lbResults;
	}
}