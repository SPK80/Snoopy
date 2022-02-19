using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snoopy.UI_
{
	public enum ResultListMode
	{
		Results,
		History
	}

	/// <summary>
	///  
	/// </summary>
	public class ResultsList : List<IDFResult>
	{
		private DataGridView dataGridView;		
		private List<ToolStripMenuItem> columnMenuItems;
		private List<ToolStripMenuItem> actionMenuItems;
		private ContextMenuStrip contextMenu;

		/// <summary>
		/// Создаёт ResultList и линкует его к dataGridView.Tag
		/// </summary>		
		public static void InvokeToDataGridView(DataGridView dataGridView,
			Action<IDFResult> onExec, Action<IDFResult> onExplore, List<ToolStripMenuItem> columnMenuItems, ResultListMode mode)
		{
			var result = new ResultsList(dataGridView, onExec, onExplore, columnMenuItems, mode);
			dataGridView.Tag = result;
		}

		public ResultsList(DataGridView dataGridView, 
			Action<IDFResult> onExec, Action<IDFResult> onExplore, List<ToolStripMenuItem> columnMenuItems, ResultListMode mode)
		{
			this.dataGridView = dataGridView ?? throw new ArgumentNullException(nameof(dataGridView));

			this.columnMenuItems = columnMenuItems;		

			initActionMenuItems(onExec, onExplore, mode);
			setCellDoubleClick(onExec);
			setKeyDown(onExec, onExplore);
			dataGridView.ColumnAdded += (s, e) =>
			{
				if (columnMenuItems == null || s == null) return;
				var dgv = s as DataGridView;
				//находим настройки колонки по инени
				var menuItem = columnMenuItems.Find(c => c.Name == e.Column.Name);
				if (menuItem == null || dgv.Columns.Count<1) return;
				//настраиваем колонку
				try
				{
					e.Column.HeaderText = menuItem.Text;         //Caption
					e.Column.Visible = menuItem.Checked;         //видимость
					if (menuItem.Checked)
						e.Column.DisplayIndex = (int)menuItem.Tag;
					else
						e.Column.DisplayIndex = dgv.Columns.Count - 1;
				}
				catch (Exception ex)
				{
					e.Column.DisplayIndex = dgv.Columns.Count - 1;
					menuItem.Tag = e.Column.DisplayIndex;
				}

				//if (menuItem.Checked)
				//	e.Column.DisplayIndex = Math.Min((int)menuItem.Tag, dgv.Columns.Count-1);
				//else
				//	e.Column.DisplayIndex = dgv.Columns.Count-1;				
			};

			dataGridView.ContextMenuStrip = contextMenu = new ContextMenuStrip();
			contextMenu.Name = dataGridView.Name + ".ContextMenu";
			contextMenu.Items.AddRange(actionMenuItems.ToArray());
			
			contextMenu.Opening += (s, e) =>
			{
				//грид пустой => запрет открытия меню
				if (dataGridView.RowCount < 1) { e.Cancel = true; return; };				
				var cp = dataGridView.PointToClient(new System.Drawing.Point(contextMenu.Left, contextMenu.Top));
				bool inHeader = cp.Y < dataGridView.ColumnHeadersHeight;
				//перекидываем columnMenuItems в текущую менюшку
				contextMenu.Items.AddRange(columnMenuItems.ToArray());
				//выводим/воказываем сообветствующие inHeader пункты меню
				columnMenuItems.ForEach(cmi => cmi.Visible = inHeader);
				actionMenuItems.ForEach(cmi => cmi.Visible = !inHeader);
				e.Cancel = false;
			};
		}


		private void initActionMenuItems(Action<IDFResult> onExec, Action<IDFResult> onExplore, ResultListMode mode)
		{
			actionMenuItems = new List<ToolStripMenuItem>();

			actionMenuItems.Add(newActionMenuItem("Exec", "Открыть",
				(s, e) => selected(s as DataGridView).ToList().ForEach(r => onExec(r))));

			actionMenuItems.Add(newActionMenuItem("Explore", "Показать",
				(s, e) => selected(s as DataGridView).ToList().ForEach(r => onExplore(r))));

			actionMenuItems.Add(newActionMenuItem("SelectAll", "Выделить все",
				(s, e) =>
				{
					dataGridView.SelectAll();
				}));

			actionMenuItems.Add(newActionMenuItem("Copy", "Копировать",
				(s, e) =>
				{
					if (dataGridView.SelectedRows.Count < 1) return;
					var cbcontent = dataGridView.GetClipboardContent();
					Clipboard.Clear();
					Clipboard.SetDataObject(cbcontent);
				}));

			if (mode == ResultListMode.History)
			{
				actionMenuItems.Add(newActionMenuItem("Clear", "Очистить",
					(s, e) =>
					{
						if (!uiConfirm("Очистить?")) return;
						Clear();
					}));

				actionMenuItems.Add(newActionMenuItem("Delete", "Удалить",
					(s, e) =>
					{
						if (!uiConfirm("Удалить?")) return;
						if (dataGridView.AreAllCellsSelected(true))
							Clear();
						else
							Remove(selected(s as DataGridView));
					}));
			}
		}
				
		private ToolStripMenuItem newActionMenuItem(string name, string text, EventHandler click)
		{
			var menuItem = new ToolStripMenuItem { Name = name, Text = text };
			menuItem.Click += click;			
			return menuItem;
		}

		private void setKeyDown(Action<IDFResult> onExec, Action<IDFResult> onExplore)
		{
			dataGridView.KeyDown += (s, e) =>
			{
				if (!(s is DataGridView)) return;
				var dgv = s as DataGridView;
				switch (e.KeyCode)
				{
					case Keys.Enter:
						{
							if (e.Shift)
								selected(s as DataGridView).ToList().ForEach(r => onExplore(r));
							else
								selected(s as DataGridView).ToList().ForEach(r => onExec(r));

							e.Handled = true;
						}
						break;

					case Keys.Delete:
						{
							if (!uiConfirm("Удалить?")) break;							
							if (dgv.AreAllCellsSelected(true))
								Clear();
							else
								Remove(selected(s as DataGridView));							
						}
						break;
				}
			};
		}

		private void setCellDoubleClick(Action<IDFResult> onExec)
		{
			dataGridView.CellDoubleClick += (s, e) =>
			{
				if (!(s is DataGridView) || (e.RowIndex < 0)) return;
				onExec(selected(e, s as DataGridView));
			};
		}

		private bool uiConfirm(string message)
		{
			DialogResult dialogResult = MessageBox.Show(message, "Подтверждение", MessageBoxButtons.YesNo);
			return dialogResult == DialogResult.Yes;
		}

		private IDFResult selected(DataGridViewCellEventArgs e, DataGridView sender = null)
		{
			if (sender == null)			
				return dataGridView.Rows[e.RowIndex].DataBoundItem as IDFResult;
			else
				return sender.Rows[e.RowIndex].DataBoundItem as IDFResult;
		}

		private IEnumerable<IDFResult> selected(DataGridView sender =null)
		{
			if (sender == null)
				return dataGridView.SelectedRows.Cast<DataGridViewRow>().Select(s => s.DataBoundItem as IDFResult);
			else
				return sender.SelectedRows.Cast<DataGridViewRow>().Select(s => s.DataBoundItem as IDFResult);
		}

		private void refreshdataGridView()
		{
			if (dataGridView != null)
			{
				dataGridView.DataSource = null;
				if (Count > 0)
					dataGridView.DataSource = this;
			}
		}

		public new void AddRange(IEnumerable<IDFResult> collection)
		{
			foreach (var r in collection)
				if (!Contains(r))
					base.Add(r);
			refreshdataGridView();
		}

		public new void Add(IDFResult item)
		{
			if (Contains(item)) return;
			base.Add(item);
			refreshdataGridView();
		}

		public new bool Contains(IDFResult item)
		{
			if (base.Contains(item)) return true;

			foreach (var idfr in this)
			{ //ищем совпадение по имени и пути
				if (idfr.Name == item.Name && idfr.Path == item.Path)
					return true;
			}
			return false;
		}

		public bool Remove(IEnumerable<IDFResult> items)
		{
			bool result = false;
			foreach (var item in items)
			{
				result = result || base.Remove(item);
			}
			if (result)
				refreshdataGridView();
			return result;
		}
		public new bool Remove(IDFResult item)
		{
			bool result = base.Remove(item);
			refreshdataGridView();
			return result;
		}

		public new void Clear()
		{
			base.Clear();
			refreshdataGridView();
		}
	}
}
