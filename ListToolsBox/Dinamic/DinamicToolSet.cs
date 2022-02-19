using System;
using System.Drawing;
using System.Windows.Forms;

namespace IndexTools.Dinamic
{
    public class DinamicToolSet: ToolStrip
    {
        private ImageList imageList;

        public DinamicToolSet(DinamicToolsBox parent, ImageList imageList)
        {
            ImageList = imageList;
            base.Parent = parent;
        }



        public new ImageList ImageList
        {
            get => imageList;
            set
            {
                imageList = value;                
                foreach (ToolStripItem item in Items)
                {
                    try
                    {
                        item.Image = imageList.Images[item.Name];
                    }
                    catch { };

                }
            }
        }

        public new DinamicToolsBox Parent => base.Parent as DinamicToolsBox;

        public ToolStripButton AddButton(string name, string text, EventHandler click)
        {
            int bh = Parent.ItemHeight - 2;
            var result = new ToolStripButton
            {
                Name = name,
                Text = text,
                Size = new Size(bh, bh),
                Visible = true,
                Height = bh,
                CheckOnClick = false,
                Image = imageList?.Images[name],                
            };            
            if (click != null)
                result.Click += click;
            result.DisplayStyle = (result.Image == null) ? ToolStripItemDisplayStyle.Text : ToolStripItemDisplayStyle.Image;
            return result;
        }

        public ToolStripLabel AddLabel(string name, string text, EventHandler click=null)
        {
            int bh = Parent.ItemHeight - 2;
            var result = new ToolStripLabel
            {
                Name = name,
                Text = text,
                Size = new Size(bh, bh),
                Visible = true,
                Height = bh,
                Image = imageList?.Images[name],
            };
            if (click != null)
                result.Click += click;
            return result;
        }

        public ToolStripSeparator AddSeparator(string name, EventHandler click =null)
        {
            int bh = Parent.ItemHeight - 2;
            var result = new ToolStripSeparator
            {
                Name = name,
                Image = imageList?.Images[name],
                Size = new Size(bh, bh),
                Visible = true,
                Height = bh,
            };
            if (click!=null)
                result.Click += click;
            return result;
        }

        public ToolStripComboBox AddComboBox(string name, string text, object[] items, EventHandler selectedIndexChanged = null)
        {
            int bh = Parent.ItemHeight - 2;
            var result = new ToolStripComboBox
            {
                Name = name,
                Text = text,
                Size = new Size(bh, bh),
                Visible = true,
                Height = bh,
                Image = imageList?.Images[name],
            };
            if (selectedIndexChanged != null)
                result.SelectedIndexChanged += selectedIndexChanged;
            result.Items.AddRange(items);
            return result;
        }

        public ToolStripTextBox AddTextBox(string name, string text, EventHandler textChanged = null)
        {
            int bh = Parent.ItemHeight - 2;
            var result = new ToolStripTextBox
            {
                Name = name,
                Image = imageList?.Images[name],
                Text = text,
                Size = new Size(bh, bh),
                Visible = true,
                Height = bh,
            };
            if (textChanged != null)
                result.TextChanged += textChanged;
            return result;
        }

        public ToolStripProgressBar AddProgressBar(string name, string text,  int value, EventHandler click = null)
        {            
            int bh = Parent.ItemHeight - 2;
            var result = new ToolStripProgressBar
            {
                Name = name,
                Image = imageList?.Images[name],
                Text = text,
                Size = new Size(bh, bh),
                Visible = true,
                Height = bh,
                Value = value
            };
            if (click != null)
                result.Click += click;
            return result;
        }

    }
}