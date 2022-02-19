namespace Bindings
{
    partial class PropertiesEditor
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

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.grid = new System.Windows.Forms.DataGridView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.clbObjects = new System.Windows.Forms.CheckedListBox();
            this.cbAllProperties = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grid
            // 
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.Location = new System.Drawing.Point(0, 0);
            this.grid.Name = "grid";
            this.grid.Size = new System.Drawing.Size(323, 348);
            this.grid.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.clbObjects);
            this.splitContainer1.Panel1.Controls.Add(this.cbAllProperties);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.grid);
            this.splitContainer1.Size = new System.Drawing.Size(490, 348);
            this.splitContainer1.SplitterDistance = 163;
            this.splitContainer1.TabIndex = 1;
            // 
            // clbObjects
            // 
            this.clbObjects.CheckOnClick = true;
            this.clbObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clbObjects.FormattingEnabled = true;
            this.clbObjects.Location = new System.Drawing.Point(0, 21);
            this.clbObjects.Name = "clbObjects";
            this.clbObjects.Size = new System.Drawing.Size(163, 327);
            this.clbObjects.TabIndex = 0;
            this.clbObjects.SelectedIndexChanged += new System.EventHandler(this.clbObjects_SelectItem);
            // 
            // cbAllProperties
            // 
            this.cbAllProperties.Dock = System.Windows.Forms.DockStyle.Top;
            this.cbAllProperties.FormattingEnabled = true;
            this.cbAllProperties.Location = new System.Drawing.Point(0, 0);
            this.cbAllProperties.Name = "cbAllProperties";
            this.cbAllProperties.Size = new System.Drawing.Size(163, 21);
            this.cbAllProperties.TabIndex = 1;
            this.cbAllProperties.SelectedIndexChanged += new System.EventHandler(this.cbAllProperties_SelectedIndexChanged);
            // 
            // PropertiesEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "PropertiesEditor";
            this.Size = new System.Drawing.Size(490, 348);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView grid;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.CheckedListBox clbObjects;
        private System.Windows.Forms.ComboBox cbAllProperties;
    }
}
