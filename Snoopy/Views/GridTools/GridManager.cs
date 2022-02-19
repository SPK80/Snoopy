using Bindings;
using CommonLib.LogHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snoopy.Views.GridTools
{	
	public class BindingSource<T> : SortableBindingList<T> where T : class
    {		
		public BindingSource(): base() {}

        public void AddRange(IEnumerable<T> items, bool ReverseAdding = false)
        {
            foreach (var r in items)
                if (!Contains(r))
                    Add(r);
        }

        public void Add(T item, bool ReverseAdding = false)
        {
            if (!Contains(item))
                if (!ReverseAdding)
                    base.Add(item);
                else base.Insert(0, item);
        }

        //public bool Contains(object item)
        //{   
        //    if (item is IDFResult)
        //    {
        //        foreach (IDFResult idfr in this)
        //        { //ищем совпадение по имени и пути
        //            if (idfr.Name == (item as IDFResult).Name && idfr.Path == (item as IDFResult).Path)
        //                return true;
        //        }
        //        return false;
        //    }
        //    else
        //        return base.Contains(item as T);            
        //}

        public bool Remove(IEnumerable<T> items)
        {
            bool result = false;
            foreach (var item in items)
            {
                result = result || base.Remove(item);
            }
            return result;
        }
    }

    public class GridManager<DataType> where DataType : class
    {
        private List<ToolStripMenuItem> columnMenuItems;
        private List<ToolStripMenuItem> actionMenuItems;
        private ContextMenuStrip contextMenu;

        public static GridManager<DataType> GetFromTag(Control control) => control.Tag as GridManager<DataType>;

        public void AddAction(Action<DataGridView> action, string name, string caption="", bool inToolStripMenu=true, 
            Keys key= Keys.None, Keys modifiers = Keys.None, bool onDoubleClick=false)
        {
            if (inToolStripMenu)
            {
                var ami = newActionMenuItem(name, (caption == "") ? name : caption, action);
                actionMenuItems.Add(ami);
                contextMenu.Items.Add(ami);
            }
                
            if (key != Keys.None)
                Grid.KeyDown += (s, e) => 
                {
                    if ((modifiers == Keys.None && e.KeyCode == key) ||
                        (modifiers != Keys.None && e.Modifiers == modifiers && e.KeyCode == key))
                    action(Grid);                    
                };
            if (onDoubleClick)
                Grid.CellDoubleClick += (s, e) =>
                {
                    if (!(s is DataGridView) || (e.RowIndex < 0)) return;
                    action(Grid);
                };
        }

        public GridManager(DataGridView dataGridView,
            List<ToolStripMenuItem> columnMenuItems) : base()
        {
            Grid = dataGridView ?? throw new ArgumentNullException(nameof(dataGridView));
            //Grid.DataSource = new BindingSource<DataType>();
            Grid.Tag = this;
            Grid.ColumnAdded += columnAdded;
            Grid.ContextMenuStrip = contextMenu = new ContextMenuStrip();

            this.columnMenuItems = columnMenuItems;
            actionMenuItems = new List<ToolStripMenuItem>();

            contextMenu.Name = Grid.Name + ".ContextMenu";
            contextMenu.Opening += contextMenuOpening;
        }

        //private ToolStripMenuItem NewColumnsMenuItem(ColumnSettengs columnSettengs)
        //{
        //    var result = new ToolStripMenuItem();
        //    result.CheckOnClick = true;
        //    result.Name = columnSettengs.Name;
        //    result.Text = columnSettengs.Caption;
        //    result.Tag = columnSettengs.Index;
        //    result.Checked = columnSettengs.Visible;
        //    if (columnSettengs.OnClick !=null)
        //    result.Click += columnSettengs.OnClick;
        //    return result;
        //}        

        public DataGridView Grid { get; private set; }

        public void SynchronizGridColumns(DataGridView SynchronizGrid)
        {
            Grid.ColumnDisplayIndexChanged += (s, e) =>
            {
                try
                {
                    (columnMenuItems.Find(c => c.Name == e.Column.Name)).Tag = e.Column.DisplayIndex;
                }
                catch (Exception ex)
                {
                    Log.Write(ex, "ColumnDisplayIndexChanged");
                }
                try
                {
                    SynchronizGrid.Columns[e.Column.Name].DisplayIndex = e.Column.DisplayIndex;
                }
                catch (Exception ex)
                {
                    Log.Write(ex, "ColumnDisplayIndexChanged");
                }
            };
            Grid.ColumnWidthChanged += (s, e) =>
            {
                try
                {
                    SynchronizGrid.Columns[e.Column.Name].Width = e.Column.Width;
                }
                catch //(Exception ex)
                {
                    //LogHelper.Log.Write(ex, "ColumnWidthChanged");
                }
            };
        }

        /// <summary>
        /// В зависимости от положения popup показывает соответствующую группу columnMenuItems или actionMenuItems
        /// </summary>
        private void contextMenuOpening(object sender, CancelEventArgs e)
        {
            var cp = Grid.PointToClient(new System.Drawing.Point(contextMenu.Left, contextMenu.Top));
            bool popupInHeader = cp.Y < Grid.ColumnHeadersHeight;
            //грид пустой? => запрет открытия меню
            if (!popupInHeader && Grid.RowCount < 1) { e.Cancel = true; return; };
            //перекидываем columnMenuItems в менюшку
            contextMenu.Items.AddRange(columnMenuItems.ToArray());
            //выводим/показываем соответствующие inHeader пункты меню
            columnMenuItems.ForEach(cmi => cmi.Visible = popupInHeader);
            actionMenuItems.ForEach(cmi => cmi.Visible = !popupInHeader);
            //если менюшка пустая то блокируем показ меню
            if (popupInHeader)
                e.Cancel = columnMenuItems.Count() < 1;
            else
                e.Cancel = actionMenuItems.Count() < 1;            
        }

        /// <summary>
        /// Настраивает отображение колонки по columnMenuItems
        /// </summary>
        private void columnAdded(object sender, DataGridViewColumnEventArgs e)
		{
			if (columnMenuItems == null || sender == null) return;
			var dgv = sender as DataGridView;
			//находим настройки колонки по инени колонки
			var menuItem = columnMenuItems.Find(c => c.Name == e.Column.Name);
			if (menuItem == null || dgv.Columns.Count < 1) return;
            //настраиваем колонку в по данным в menuItem
            try
            {
				//e.Column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
				e.Column.HeaderText = menuItem.Text;         //Caption
				e.Column.Visible = menuItem.Checked;         //видимость					
				if (menuItem.Checked)
					e.Column.DisplayIndex = (int)menuItem.Tag;
				else
					e.Column.DisplayIndex = dgv.Columns.Count - 1;
			}
			catch
			{
				e.Column.DisplayIndex = dgv.Columns.Count - 1;
				menuItem.Tag = e.Column.DisplayIndex;
			}
		}

        /// <summary>
        /// Инициирует все колонки по информации из columnMenuItems
        /// </summary>
        public void RefreshColumns()
		{
			foreach (DataGridViewColumn col in Grid.Columns)
			{
				columnAdded(Grid, new DataGridViewColumnEventArgs(col));
			}
		}
		
        private ToolStripMenuItem newActionMenuItem(string name, string text, Action<DataGridView> action)
        {
            var menuItem = new ToolStripMenuItem { Name = name, Text = text };
            menuItem.Click += (s, e) => action(Grid);
            return menuItem;
        }

		//private void setKeyDown(Action<IDFResult> onExec, Action<IDFResult> onExplore)
		//{
  //          Grid.KeyDown += (s, e) =>
		//	{
		//		if (!(s is DataGridView)) return;
		//		var dgv = s as DataGridView;
		//		switch (e.KeyCode)
		//		{
		//			case Keys.Enter:
		//				{
		//					if (e.Shift)
		//						Selected(s as DataGridView).ToList().ForEach(r => onExplore(r));
		//					else
		//						Selected(s as DataGridView).ToList().ForEach(r => onExec(r));

		//					e.Handled = true;
		//				}
		//				break;

		//			case Keys.Delete:
		//				{
		//					if (!uiConfirm("Удалить?")) break;
		//					if (dgv.AreAllCellsSelected(true))
  //                              (dgv.DataSource as BindingDFResultList).Clear();
		//					else
  //                              (dgv.DataSource as BindingDFResultList).Remove(Selected(s as DataGridView));
		//				}
		//				break;
		//		}
		//	};
		//}

		//private void setCellDoubleClick(Action<IDFResult> onExec)
		//{
  //          Grid.CellDoubleClick += (s, e) =>
		//	{
  //              if (!(s is DataGridView) || (e.RowIndex < 0)) return;
  //              var dgv = s as DataGridView;
  //              bool ge = dgv.Enabled;
  //              dgv.Enabled= false; // временная заморозка
		//		onExec(selected(e, s as DataGridView));
  //              dgv.Enabled = ge;              
		//	};
		//}

		private bool uiConfirm(string message)
		{
			DialogResult dialogResult = MessageBox.Show(message, "Подтверждение", MessageBoxButtons.YesNo);
			return dialogResult == DialogResult.Yes;
		}
        
        public static IEnumerable<DataType> Selected(DataGridView sender)
        {
            return sender.SelectedRows.Cast<DataGridViewRow>().Select(s => s.DataBoundItem as DataType);
        }

        public IEnumerable<DataType> Selected()
		{
            return Selected(Grid);
        }
        
	}
}
