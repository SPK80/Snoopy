using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snoopy.Views.GridTools
{
    public class ListBoxManager<DataType> where DataType : class
    {
        private ContextMenuStrip contextMenu;

        public static ListBoxManager<DataType> GetFromTag(Control control) => 
            control.Tag as ListBoxManager<DataType>;

        public ListBoxManager(ListBox listBox)
        {
            ListBox = listBox ?? throw new ArgumentNullException(nameof(listBox));

            ListBox.DataSource = new BindingSource<DataType>();
            ListBox.Tag = this;
            ListBox.ContextMenuStrip = contextMenu = new ContextMenuStrip();
            contextMenu.Name = ListBox.Name + ".ContextMenu";
        }

        public ListBox ListBox { get; private set; }

        private ToolStripMenuItem newActionMenuItem(string name, string text, Action<ListBox> action)
        {
            var menuItem = new ToolStripMenuItem { Name = name, Text = text };
            menuItem.Click += (s, e) => action(ListBox);
            return menuItem;
        }

        public void AddAction(Action<ListBox> action, string name, string caption = "", bool inToolStripMenu = true,
            Keys key = Keys.None, Keys modifiers = Keys.None, bool onDoubleClick = false)
        {
            if (inToolStripMenu)
            {
                var ami = newActionMenuItem(name, (caption == "") ? name : caption, action);
                contextMenu.Items.Add(ami);
            }

            if (key != Keys.None)
                ListBox.KeyDown += (s, e) =>
                {
                    if ((modifiers == Keys.None && e.KeyCode == key) ||
                        (modifiers != Keys.None && e.Modifiers == modifiers && e.KeyCode == key))
                        action(ListBox);
                };
            if (onDoubleClick)
                ListBox.DoubleClick += (s, e) =>
                {
                    if (!(s is ListBox)) return;
                    action(ListBox);
                };
        }

        public static IEnumerable<DataType> Selected(ListBox sender)
        {
            return sender.SelectedItems.Cast<DataType>();
        }

        public IEnumerable<DataType> Selected()
        {
            return Selected(ListBox);
        }
    }
}
