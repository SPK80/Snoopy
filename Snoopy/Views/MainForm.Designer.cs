namespace Snoopy.Views
{
	partial class MainForm
	{
		/// <summary>
		/// Обязательная переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Освободить все используемые ресурсы.
		/// </summary>
		/// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.scIndexes = new System.Windows.Forms.SplitContainer();
            this.scIndexResults = new System.Windows.Forms.SplitContainer();
            this.dgvFoudResults = new System.Windows.Forms.DataGridView();
            this.dgvExecHistory = new System.Windows.Forms.DataGridView();
            this.buttonsImg = new System.Windows.Forms.ImageList(this.components);
            this.queryPanel = new System.Windows.Forms.Panel();
            this.cbExtantions = new System.Windows.Forms.ComboBox();
            this.cbAutoSizeColumns = new System.Windows.Forms.CheckBox();
            this.cbIncDirs = new System.Windows.Forms.CheckBox();
            this.cbQuery = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.rescanTimer = new System.Windows.Forms.Timer(this.components);
            this.cmsResultActions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ltbSources = new ListToolsBox.ListToolsBox(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.scIndexes)).BeginInit();
            this.scIndexes.Panel1.SuspendLayout();
            this.scIndexes.Panel2.SuspendLayout();
            this.scIndexes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scIndexResults)).BeginInit();
            this.scIndexResults.Panel1.SuspendLayout();
            this.scIndexResults.Panel2.SuspendLayout();
            this.scIndexResults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFoudResults)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvExecHistory)).BeginInit();
            this.queryPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // scIndexes
            // 
            this.scIndexes.BackColor = System.Drawing.Color.Transparent;
            this.scIndexes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scIndexes.Location = new System.Drawing.Point(0, 55);
            this.scIndexes.Name = "scIndexes";
            // 
            // scIndexes.Panel1
            // 
            this.scIndexes.Panel1.Controls.Add(this.scIndexResults);
            // 
            // scIndexes.Panel2
            // 
            this.scIndexes.Panel2.Controls.Add(this.ltbSources);
            this.scIndexes.Size = new System.Drawing.Size(856, 417);
            this.scIndexes.SplitterDistance = 619;
            this.scIndexes.SplitterWidth = 6;
            this.scIndexes.TabIndex = 0;
            // 
            // scIndexResults
            // 
            this.scIndexResults.BackColor = System.Drawing.Color.Transparent;
            this.scIndexResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scIndexResults.Location = new System.Drawing.Point(0, 0);
            this.scIndexResults.Name = "scIndexResults";
            this.scIndexResults.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scIndexResults.Panel1
            // 
            this.scIndexResults.Panel1.Controls.Add(this.dgvFoudResults);
            this.scIndexResults.Panel1MinSize = 100;
            // 
            // scIndexResults.Panel2
            // 
            this.scIndexResults.Panel2.Controls.Add(this.dgvExecHistory);
            this.scIndexResults.Panel2MinSize = 1;
            this.scIndexResults.Size = new System.Drawing.Size(619, 417);
            this.scIndexResults.SplitterDistance = 219;
            this.scIndexResults.SplitterWidth = 6;
            this.scIndexResults.TabIndex = 5;
            // 
            // dgvFoudResults
            // 
            this.dgvFoudResults.AllowUserToAddRows = false;
            this.dgvFoudResults.AllowUserToDeleteRows = false;
            this.dgvFoudResults.AllowUserToOrderColumns = true;
            this.dgvFoudResults.AllowUserToResizeRows = false;
            this.dgvFoudResults.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvFoudResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFoudResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFoudResults.Location = new System.Drawing.Point(0, 0);
            this.dgvFoudResults.Name = "dgvFoudResults";
            this.dgvFoudResults.ReadOnly = true;
            this.dgvFoudResults.RowHeadersVisible = false;
            this.dgvFoudResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFoudResults.Size = new System.Drawing.Size(619, 219);
            this.dgvFoudResults.TabIndex = 0;
            // 
            // dgvExecHistory
            // 
            this.dgvExecHistory.AllowUserToAddRows = false;
            this.dgvExecHistory.AllowUserToDeleteRows = false;
            this.dgvExecHistory.AllowUserToOrderColumns = true;
            this.dgvExecHistory.AllowUserToResizeRows = false;
            this.dgvExecHistory.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvExecHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvExecHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvExecHistory.Location = new System.Drawing.Point(0, 0);
            this.dgvExecHistory.Name = "dgvExecHistory";
            this.dgvExecHistory.ReadOnly = true;
            this.dgvExecHistory.RowHeadersVisible = false;
            this.dgvExecHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvExecHistory.Size = new System.Drawing.Size(619, 192);
            this.dgvExecHistory.TabIndex = 5;
            // 
            // buttonsImg
            // 
            this.buttonsImg.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("buttonsImg.ImageStream")));
            this.buttonsImg.TransparentColor = System.Drawing.Color.Transparent;
            this.buttonsImg.Images.SetKeyName(0, "New");
            this.buttonsImg.Images.SetKeyName(1, "Open");
            this.buttonsImg.Images.SetKeyName(2, "Update");
            this.buttonsImg.Images.SetKeyName(3, "Cancel");
            this.buttonsImg.Images.SetKeyName(4, "Close");
            this.buttonsImg.Images.SetKeyName(5, "MultySelect");
            this.buttonsImg.Images.SetKeyName(6, "Сolor");
            this.buttonsImg.Images.SetKeyName(7, "ShowTools");
            this.buttonsImg.Images.SetKeyName(8, "Font");
            this.buttonsImg.Images.SetKeyName(9, "Color1");
            this.buttonsImg.Images.SetKeyName(10, "Undo");
            this.buttonsImg.Images.SetKeyName(11, "Delete");
            this.buttonsImg.Images.SetKeyName(12, "Confirm");
            this.buttonsImg.Images.SetKeyName(13, "MoveDown");
            this.buttonsImg.Images.SetKeyName(14, "MoveUp");
            this.buttonsImg.Images.SetKeyName(15, "History");
            // 
            // queryPanel
            // 
            this.queryPanel.BackColor = System.Drawing.Color.Transparent;
            this.queryPanel.Controls.Add(this.cbExtantions);
            this.queryPanel.Controls.Add(this.cbAutoSizeColumns);
            this.queryPanel.Controls.Add(this.cbIncDirs);
            this.queryPanel.Controls.Add(this.cbQuery);
            this.queryPanel.Controls.Add(this.btnSearch);
            this.queryPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.queryPanel.Location = new System.Drawing.Point(0, 0);
            this.queryPanel.Name = "queryPanel";
            this.queryPanel.Size = new System.Drawing.Size(856, 55);
            this.queryPanel.TabIndex = 0;
            // 
            // cbExtantions
            // 
            this.cbExtantions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbExtantions.FormattingEnabled = true;
            this.cbExtantions.Items.AddRange(new object[] {
            "xls",
            "xlsx",
            "vsd",
            "doc",
            "exe",
            "pdf"});
            this.cbExtantions.Location = new System.Drawing.Point(773, 33);
            this.cbExtantions.Name = "cbExtantions";
            this.cbExtantions.Size = new System.Drawing.Size(80, 21);
            this.cbExtantions.TabIndex = 5;
            // 
            // cbAutoSizeColumns
            // 
            this.cbAutoSizeColumns.AutoSize = true;
            this.cbAutoSizeColumns.Location = new System.Drawing.Point(12, 37);
            this.cbAutoSizeColumns.Name = "cbAutoSizeColumns";
            this.cbAutoSizeColumns.Size = new System.Drawing.Size(133, 17);
            this.cbAutoSizeColumns.TabIndex = 4;
            this.cbAutoSizeColumns.Text = "Авторазмер колонок";
            this.cbAutoSizeColumns.UseVisualStyleBackColor = true;
            this.cbAutoSizeColumns.CheckedChanged += new System.EventHandler(this.cbAutoSizeColumns_CheckedChanged);
            // 
            // cbIncDirs
            // 
            this.cbIncDirs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbIncDirs.AutoSize = true;
            this.cbIncDirs.Location = new System.Drawing.Point(632, 37);
            this.cbIncDirs.Name = "cbIncDirs";
            this.cbIncDirs.Size = new System.Drawing.Size(135, 17);
            this.cbIncDirs.TabIndex = 3;
            this.cbIncDirs.Text = "Включая Директории";
            this.cbIncDirs.UseVisualStyleBackColor = true;
            // 
            // cbQuery
            // 
            this.cbQuery.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbQuery.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbQuery.FormattingEnabled = true;
            this.cbQuery.Location = new System.Drawing.Point(3, 5);
            this.cbQuery.Name = "cbQuery";
            this.cbQuery.Size = new System.Drawing.Size(767, 28);
            this.cbQuery.TabIndex = 2;
            this.cbQuery.SelectionChangeCommitted += new System.EventHandler(this.cbQuery_SelectionChangeCommitted);
            this.cbQuery.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cbQuery_KeyUp);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(773, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(80, 28);
            this.btnSearch.TabIndex = 0;
            this.btnSearch.Text = "Поиск";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(98, 22);
            this.toolStripButton1.Text = "toolStripButton1";
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(98, 22);
            this.toolStripButton2.Text = "toolStripButton2";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(86, 22);
            this.toolStripLabel1.Text = "toolStripLabel1";
            // 
            // rescanTimer
            // 
            this.rescanTimer.Interval = 1000;
            // 
            // cmsResultActions
            // 
            this.cmsResultActions.Name = "cmsResultActions";
            this.cmsResultActions.Size = new System.Drawing.Size(61, 4);
            // 
            // ltbSources
            // 
            this.ltbSources.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ltbSources.FontAutoScaling = true;
            this.ltbSources.ImageList = this.buttonsImg;
            this.ltbSources.ImagesAutoScaling = true;
            this.ltbSources.ItemHeight = 20;
            this.ltbSources.Location = new System.Drawing.Point(0, 0);
            this.ltbSources.Name = "ltbSources";
            this.ltbSources.Size = new System.Drawing.Size(231, 417);
            this.ltbSources.TabIndex = 0;
            this.ltbSources.OnRenameItem += new System.EventHandler<ListToolsBox.ItemEventArgs>(this.ltbSources_OnRenameItem);
            this.ltbSources.OnSelectItem += new System.EventHandler<ListToolsBox.ItemEventArgs>(this.ltbSources_OnSelectItem);
            this.ltbSources.OnAddItem += new System.EventHandler<ListToolsBox.ItemEventArgs>(this.ltbSources_OnAddItem);
            this.ltbSources.OnRemoveItem += new System.EventHandler<ListToolsBox.ItemEventArgs>(this.ltbSources_OnRemoveItem);
            this.ltbSources.OnNewItem += new System.EventHandler<ListToolsBox.ItemEventArgs>(this.ltbSources_OnNewItem);
            this.ltbSources.OnUpdateItem += new System.EventHandler<ListToolsBox.ItemEventArgs>(this.ltbSources_OnUpdateItem);
            this.ltbSources.OnCancelItem += new System.EventHandler<ListToolsBox.ItemEventArgs>(this.ltbSources_OnCancelItem);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(856, 472);
            this.Controls.Add(this.scIndexes);
            this.Controls.Add(this.queryPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.scIndexes.Panel1.ResumeLayout(false);
            this.scIndexes.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scIndexes)).EndInit();
            this.scIndexes.ResumeLayout(false);
            this.scIndexResults.Panel1.ResumeLayout(false);
            this.scIndexResults.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scIndexResults)).EndInit();
            this.scIndexResults.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFoudResults)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvExecHistory)).EndInit();
            this.queryPanel.ResumeLayout(false);
            this.queryPanel.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer scIndexes;
		private System.Windows.Forms.Panel queryPanel;
		private System.Windows.Forms.ComboBox cbQuery;
		private System.Windows.Forms.Button btnSearch;
		private System.Windows.Forms.ImageList buttonsImg;
		private System.Windows.Forms.ToolStripButton toolStripButton2;
		private System.Windows.Forms.ToolStripLabel toolStripLabel1;
		private System.Windows.Forms.ToolStripButton toolStripButton1;
		private System.Windows.Forms.CheckBox cbIncDirs;
		private System.Windows.Forms.SplitContainer scIndexResults;
		private System.Windows.Forms.DataGridView dgvExecHistory;
		private System.Windows.Forms.DataGridView dgvFoudResults;
		private System.Windows.Forms.CheckBox cbAutoSizeColumns;
		private System.Windows.Forms.ComboBox cbExtantions;
		private System.Windows.Forms.Timer rescanTimer;
        private ListToolsBox.ListToolsBox ltbSources;
        private System.Windows.Forms.ContextMenuStrip cmsResultActions;
    }
}

