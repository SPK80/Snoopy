//#define Debug
using System;
using System.Windows.Forms;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Snoopy.Views.GridTools;
using System.ComponentModel;
using System.Reflection;

#if Debug
using SPKLib.Watcher;
#endif

namespace Snoopy.Views
{
	public partial class MainForm : Form, IMainForm, ISourcesView, IResults, IHistory
    {
        public MainForm()
		{
			InitializeComponent();
		}

        //internal class CaptionIndexDictionary : Dictionary<string, int> { };
        //internal class ColumnsDictionary : Dictionary<string, CaptionIndexDictionary> { };



        //public bool uiConfirm(string message)
        //{
        //	DialogResult dialogResult = MessageBox.Show(message, "Подтверждение", MessageBoxButtons.YesNo);
        //	return dialogResult == DialogResult.Yes;
        //}

        //public void InitProcessingFields(IEnumerable<string> fieldNames)
        //{
        //    scannerBox.InitProcessingFields(fieldNames.ToList());
        //}

        //public IList GetSources()
        //{
        //    return lbSources.Items;
        //}

        //public void AddSource(string name, object source)
        //{
        //    (lbSources.DataSource as IList).Add(source);
        //}

        #region IMainForm

        public event Func<ISettingsView, bool> OnMainLoad;
        public event Func<bool> OnMainClose;

        //public void InitSettings(IDictionary<string, object> settingsDic)
        //{
        //    settingsForm.settingsDic = settingsDic as Dictionary<string, object>;
        //    //throw new NotImplementedException();
        //}

        public bool Confirm(string message, string caption = "Подтверждение действия") =>
            MessageBox.Show(message, caption, MessageBoxButtons.YesNo) == DialogResult.Yes;

        public void ShowMessage(string message, object source)
        {
            if (source == null)
                //this.Text = message;
                MessageBox.Show(message);
            else
                ltbSources.ShowMessage(message, source);
        }

        public SettingsForm SettingsForm;

        class Command
        {
            public EventHandler action { get; }
            public string Name { get; }
            public string Caption { get; }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            EventHandler backColorChanged = (s, e_) =>
            {
                ((DataGridView)s).BackgroundColor = ((Control)s).BackColor;
            };

            ((Control)dgvExecHistory).BackColorChanged += backColorChanged;
            ((Control)dgvFoudResults).BackColorChanged += backColorChanged;

            SettingsForm = new SettingsForm(ltbSources, dgvFoudResults, dgvExecHistory, queryPanel, cbQuery);

            SettingsForm.ShowHistoryChanged += 
                (s, ch) => 
                scIndexResults.Panel2Collapsed = !ch;


            newSourceForm = new NewSourceForm();

            Text = Application.ProductName;            

            //запрет обработки F4 (выпадение списка)			
            cbQuery.KeyDown += (s_, e_) => e_.Handled = e_.KeyValue == 115;


            //cmsResultActions.Items.Add()


            //#region ColumnsSettengs

            EventHandler dirFileClick = (s_, e_) => //ColumnsMenu_Click;
            {    //управление видимостью колонок
                var colsMenuItem = s_ as ToolStripMenuItem;
                if (colsMenuItem == null) return;

                var dgv = (colsMenuItem.Owner as ContextMenuStrip)?.SourceControl as DataGridView;
                if (dgv == null) return;

                if (dgv.Columns.Count > 0)
                {
                    if (!colsMenuItem.Checked)
                        colsMenuItem.Tag = -1;
                    else
                        colsMenuItem.Tag = dgv.Columns.Count - 1;
                }
                if (dgvFoudResults.Columns.Contains(colsMenuItem.Name))
                    dgvFoudResults.Columns[colsMenuItem.Name].Visible = colsMenuItem.Checked;
                if (dgvExecHistory.Columns.Contains(colsMenuItem.Name))
                    dgvExecHistory.Columns[colsMenuItem.Name].Visible = colsMenuItem.Checked;
            };

            //var dirFileColumnsSettengs = new ColumnsSettengs(new[] {
            //        new ColumnSettengs{ Name = FieldNames.Name, Caption = "Имя", Visible=true, Index=0, OnClick=dirFileClick },
            //        new ColumnSettengs{ Name = FieldNames.Length, Caption = "Размер", Visible=false, Index=1, OnClick=dirFileClick },
            //        new ColumnSettengs{ Name = FieldNames.CreationTime, Caption = "Создан", Visible=false, Index=2, OnClick=dirFileClick },
            //        new ColumnSettengs{ Name = FieldNames.LastWriteTime, Caption = "Изменён", Visible=false, Index=3, OnClick=dirFileClick },
            //        new ColumnSettengs{ Name = FieldNames.LastAccessTime, Caption = "Открыт", Visible=false, Index=4, OnClick=dirFileClick },
            //        new ColumnSettengs{ Name = FieldNames.Path, Caption = "Путь", Visible=false, Index=5, OnClick=dirFileClick },
            //    });

            //var processingFieldsArray = SettingsForm.ProcessingFields.ToArray();
            var members = typeof(IFoundItem).GetMembers();
            var processingFieldsDic = new Dictionary<string, string>();
                
            foreach(var m in members.Where(m => m.MemberType == MemberTypes.Property))
            {
                var attr = m.GetCustomAttribute(typeof(CaptionAttribute)) as CaptionAttribute;
                string caption = attr?.Caption?? m.Name;
                processingFieldsDic.Add(m.Name, caption);
            }

            var csList = new List<ColumnSettengs>();
            var processingFields = processingFieldsDic.ToArray();
            var columnsSettengs = new ColumnsSettengs();

            ToolStripMenuItem tsm;

            for (int i = 0; i < processingFields.Count(); i++)
            //foreach (var pf in SettingsForm.ProcessingFields)
            {
                columnsSettengs.Add(tsm = new ToolStripMenuItem(processingFields[i].Value, null, dirFileClick));
                tsm.Name = processingFields[i].Key; 
                tsm.Visible = tsm.Checked = tsm.CheckOnClick = true;
                tsm.Tag = i;
            }
            //var columnsSettengs = new ColumnsSettengs(csList.ToArray());


            //EventHandler contextClick = (s_, e_) => //ColumnsMenu_Click;
            //{    //управление видимостью колонок
            //    var colsMenuItem = s_ as ToolStripMenuItem;
            //    if (colsMenuItem == null) return;

            //    var dgv = (colsMenuItem.Owner as ContextMenuStrip)?.SourceControl as DataGridView;
            //    if (dgv == null) return;
            //    if (dgv.Columns.Count > 0)
            //    {
            //        if (!colsMenuItem.Checked)
            //            colsMenuItem.Tag = -1;
            //        else
            //            colsMenuItem.Tag = dgv.Columns.Count - 1;
            //    }
            //    if (dgv.Columns.Contains(colsMenuItem.Name))
            //        dgv.Columns[colsMenuItem.Name].Visible = colsMenuItem.Checked;

            //};

            //var contentColumnsSettengs = new ColumnsSettengs(new[] {
            //    new ColumnSettengs{ Name = "Text", Caption = "Текст", Visible=true, Index=0, OnClick=contextClick },
            //    new ColumnSettengs{ Name = "Address", Caption = "Положение", Visible=true, Index=1, OnClick=contextClick },
            //    new ColumnSettengs{ Name = "Page", Caption = "Страница", Visible=true, Index=2, OnClick=contextClick },
            //    new ColumnSettengs{ Name = "SourceFile", Caption = "Файл", Visible=true, Index=3, OnClick=contextClick },
            //});


            //#endregion ColumnsSettengs

            //tsbSourceAdd.Image = buttonsImg.Images["Open"];
            //tsbFSourceDel.Image = buttonsImg.Images["Close"];
            //tsbRefreshList.Image = buttonsImg.Images["Rescan"];

            #region GridManager
            ////Инициируем все действия над IDFResult
            var dFActions = new Dictionary<string, Action<DataGridView>>
            {
                {"Exec", (dgv) =>
                GridManager<IFoundItem>.GetFromTag(dgv).Selected().ToList().ForEach(r => OnExecResult?.Invoke(r, new object[]{false, true})) },
                {"ExecHistiry", (dgv) =>
                GridManager<IFoundItem>.GetFromTag(dgv).Selected().ToList().ForEach(r => OnExecResult?.Invoke(r, new object[]{false, false})) },
                {"Explore", (dgv) =>
                GridManager<IFoundItem>.GetFromTag(dgv).Selected().ToList().ForEach(r => OnExecResult?.Invoke(r, new object[]{true, true})) },
                {"SelectAll", (dgv) => dgv.SelectAll() },
                {"Copy", (dgv) =>
                    {
                        if (dgv.SelectedRows.Count < 1) return;
                        var cbcontent = dgv.GetClipboardContent();
                        Clipboard.Clear();
                        Clipboard.SetDataObject(cbcontent);
                    }
                },
                {"Clear", (dgv) =>
                    {                        
                        if (!Confirm("Очистить?")) return;
                        var bList=dgv.DataSource as BindingList<IFoundItem>;
                        bList.Clear();
                    }
                },
                {"Delete", (dgv) =>
                    {
                         if (!Confirm("Удалить?")) return;
                        var bList=dgv.DataSource as BindingList<IFoundItem>;
                        if (dgv.AreAllCellsSelected(true))
                            bList.Clear();
                        else
                            foreach (var s in GridManager<IFoundItem>.GetFromTag(dgv).Selected())
                            {
                                bList.Remove(s);
                            }
                    }
                },
            };

            var execHistoryGM = new GridManager<IFoundItem>(dgvExecHistory, columnsSettengs);
            execHistoryGM.AddAction(dFActions["ExecHistiry"], "Exec", "Открыть", true, Keys.Enter, Keys.None, true);
            execHistoryGM.AddAction(dFActions["Explore"], "Explore", "Показать");
            execHistoryGM.AddAction(dFActions["SelectAll"], "SelectAll", "Выделить все");
            execHistoryGM.AddAction(dFActions["Copy"], "Copy", "Копировать");
            execHistoryGM.AddAction(dFActions["Clear"], "Clear", "Очистить");
            execHistoryGM.AddAction(dFActions["Delete"], "Delete", "Удалить");

            var foudResultsGM = new GridManager<IFoundItem>(dgvFoudResults, columnsSettengs);
            foudResultsGM.AddAction(dFActions["Exec"], "Exec", "Открыть", true, Keys.Enter, Keys.None, true);
            foudResultsGM.AddAction(dFActions["Explore"], "Explore", "Показать");
            foudResultsGM.AddAction(dFActions["Copy"], "Copy", "Копировать");
            foudResultsGM.AddAction(dFActions["SelectAll"], "SelectAll", "Выделить все");

            ////var contentActions = new Dictionary<string, Action<DataGridView>>
            ////{
            ////    {"Explore", (dgv) =>
            ////        GridManager<ExcelRangeResult>.GetFromTag(dgv).Selected().ToList().ForEach(r => r.Show()) },
            ////};

            ////var contentResultsGM = new GridManager<ExcelRangeResult>(dgvContentResults, contentColumnsSettengs);
            ////contentResultsGM.AddAction(contentActions["Explore"], "Explore", "Показать", true, Keys.Enter, Keys.None, true);

            ////var contentMenuGM = new ListBoxManager<object>(lbSources);
            ////var contentMenuActions = new Dictionary<string, Action<ListBox>>
            ////{
            ////    {"Close", (lb) =>
            ////        {
            ////            ListBoxManager<object>.GetFromTag(lb).Selected().ToList().ForEach(r => OnCloseSource?.Invoke(r));
            ////            OnTakeSources?.Invoke(lb.DataSource as IList);
            ////        }
            ////    },
            ////    {"Favorite", (lb) =>
            ////        {
            ////            ListBoxManager<object>.GetFromTag(lb).Selected().ToList().ForEach(r => OnFavoriteSource?.Invoke(r));
            ////            OnTakeSources?.Invoke(lb.DataSource as IList);
            ////        }
            ////    },
            ////};
            ////contentMenuGM.AddAction(contentMenuActions["Close"], "Close");
            ////contentMenuGM.AddAction(contentMenuActions["Favorite"], "Запомнить");
            #endregion GridManager

            OnMainLoad?.Invoke(SettingsForm);
            SettingsForm.Init();


            //execHistoryGM.RefreshColumns();
            //foudResultsGM.RefreshColumns();

            //execHistoryGM.SynchronizGridColumns(dgvFoudResults);
            //foudResultsGM.SynchronizGridColumns(dgvExecHistory);

            //cbShowExecHistory_CheckedChanged(sender, e);
            //cbShowContentHistory_CheckedChanged(sender, e);
            TryEnableQueryPanel();

            if (dgvFoudResults.AutoSizeColumnsMode != DataGridViewAutoSizeColumnsMode.Fill)
            {
                var asc = dgvFoudResults.AutoSizeColumnsMode;
                dgvFoudResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvFoudResults.AutoSizeColumnsMode = asc;
            }

            //выбираем первый index
            //indexToolsBox.ClickSelector(0);
            //this.DeepClick((_s, _e) => waitCounter = 0, true);
            //tcSources_SelectedIndexChanged(tcSourceTypes, EventArgs.Empty);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (var pi in ltbSources.ProgressingItems)
            {
                OnCancelProgress?.Invoke(pi);
            }

            OnMainClose?.Invoke();
            //OnSaveSettings?.Invoke(settingsForm.settingsDic);
        }
                
        /// <summary>
        /// Обработка горячих клавиш
        /// </summary>
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode >= Keys.F1 && e.KeyCode <= Keys.F24)
            {
                ltbSources.SetSelected(e.KeyCode - Keys.F1, true);
                return;
            }
            if (e.KeyCode == Keys.Escape)
            {
                if (e.Shift)
                {
                    var dialogResult = SettingsForm.ShowDialog(this);
                    if (dialogResult == DialogResult.OK)
                        return;                    
                }
                else
                {
                    if (queryPanel.Enabled)
                        cbQuery.Focus(); //фокус на поисковую строку
                }
                return;
            }
        }

        #endregion IMainForm

        #region ISources

        public List<object> Items => ltbSources.Items.ToList();

        public event Func<string, object[], bool> OnNewSource;
        public event Func<string, bool> OnAddSource;
        public event Func<object, bool> OnRemoveSource;
        public event Func<object, string, bool> OnRenameSource;
        public event Func<object, bool> OnUpdateSource;
        public event Func<object, bool> OnCancelProgress;
        public event Func<string, object, object[], bool> OnFindInSource;

        public void AddSource(object source) => ltbSources.AddItem(source);

        public void RemoveSource(object source) => ltbSources.RemoveItem(source);

        public void RenameSource(object source, string newName)
        {
            throw new NotImplementedException();
        }
               
        public void SetSourceProcess(object source, int value, string msg)
        {
            Action action = () => ltbSources.SetProgressValue(source, value, msg);
            this.Invoke(action);
        }

        private void ltbSources_OnAddItem(object sender, ListToolsBox.ItemEventArgs e)
        {
            var openDlg = new OpenFileDialog();
            openDlg.Multiselect = true;
            openDlg.InitialDirectory = Application.StartupPath;
            //openDlg.DefaultExt = indexExtantion;
            //openDlg.Filter = $"(*{indexExtantion})|*{indexExtantion}";
            var dialogResult = openDlg.ShowDialog();
            if (dialogResult != DialogResult.OK) return;

            foreach (string name in openDlg.FileNames)
                OnAddSource?.Invoke(name);
        }

        private void ltbSources_OnRemoveItem(object sender, ListToolsBox.ItemEventArgs e)
        {
            void remove(object it)
            {
                if ((bool)OnRemoveSource?.Invoke(it))
                {
                    if (ltbSources.SelectedProgressingItems.Contains(it))
                        ltbSources.SetProgressValue(it, -1);
                    ltbSources.RemoveItem(it);
                }
                else
                    ShowMessage($"Сбой при удалении!", it);
            }

            if (e.Item == null) //комманда была от общей панели
            {
                var msg = $"{ltbSources.SelectedNames}";
                if (ltbSources.SelectedProgressingItems.Count() > 0)
                    msg += $" из них обновляются {ltbSources.SelectedProgressingNames} \n Удалить ? ";
                if (!Confirm(msg))
                {
                    e.Cancel = true;
                    return;
                }
                while (ltbSources.SelectedItems.Count() > 0)
                {
                    var it = ltbSources.SelectedItems.First();
                    remove(it);
                };
            }
            else
            {
                var msg = $"{e.Item.ToString()} Удалить?";
                if (!Confirm(msg))
                {
                    e.Cancel = true;
                    return;
                }
                remove(e.Item);
            }
        }

        private void ltbSources_OnSelectItem(object sender, ListToolsBox.ItemEventArgs e)
        {
        }

        private void ltbSources_OnCancelItem(object sender, ListToolsBox.ItemEventArgs e)
        {
            string message(string names) => $"Отменить обновление {names} ?";

            if (ltbSources.SelectedProgressingItems.Count() < 1) return;

            if (e.Item == null) //комманда была от общей панели
            {
                if (!Confirm(message(ltbSources.SelectedProgressingNames)))
                {
                    e.Cancel = true;
                    return;
                }
                foreach (var s in ltbSources.SelectedProgressingItems)
                {
                    OnCancelProgress?.Invoke(s);
                }
            }
            else
            {
                if (!Confirm(message(e.Item.ToString())))
                {
                    e.Cancel = true;
                    return;
                }
                OnCancelProgress?.Invoke(e.Item);
            }
        }

        private void ltbSources_OnUpdateItem(object sender, ListToolsBox.ItemEventArgs e)
        {
            if (ltbSources.SelectedItems.Count() < 1) return;

            string message(string names) => $"Обновить {names} ?";
            if (SettingsForm.UpdateConfirm && Confirm(message(ltbSources.SelectedNames)))
            {
                foreach (var s in ltbSources.SelectedItems)
                {
                    if (!ltbSources.ProgressingItems.Contains(s))
                        OnUpdateSource?.Invoke(s);
                }
            }
            else
                e.Cancel = true;         
        }    

        private void ltbSources_OnRenameItem(object sender, ListToolsBox.ItemEventArgs e)
        {
            OnRenameSource?.Invoke(e.Item, e.Value as string);
        }

        internal NewSourceForm newSourceForm;

        private void ltbSources_OnNewItem(object sender, ListToolsBox.ItemEventArgs e)
        {
            var dialogResult = newSourceForm.ShowDialog(this);
            if (dialogResult != DialogResult.OK) return;
            var options = new List <object>();
            options.Add(true);
            options.AddRange(SettingsForm.ProcessingFields);
            OnNewSource?.Invoke(newSourceForm.SelectedPath, options.ToArray());

        }

        #endregion ISources

        #region IResults

        public event Func<object, object[], bool> OnExecResult;

        public void BindResults(BindingList<IFoundItem> resultsSource)
        {
            dgvFoudResults.DataSource = null;
            dgvFoudResults.DataSource = resultsSource;


            //if (results == null) return;

            //ReceiveResultsInProcess = true; //блокировка на время заполнения таблицы результатами запроса

            //foudResultList.AddRange(results.Cast<object>());

            //queryResultsLeft--;
            //if (queryResultsLeft < 1 && results.Count > 0)
            //{
            //    cbQuery.Enabled = true;
            //    dgvFoudResults.Focus();
            //}
            //ReceiveResultsInProcess = false;

        }
        
        //флаг блокировки на время получения результатов запроса
        //private bool ReceiveResultsInProcess = false;
        //private int queryResultsLeft = 0;

        //private DataGridView getSourceDataGridView(ToolStripMenuItem sender)
        //    => (sender?.Owner as ContextMenuStrip)?.SourceControl as DataGridView;

        //private void copyDataGridView(object sender, EventArgs e)
        //{
        //    var dataGridView = getSourceDataGridView(sender as ToolStripMenuItem);
        //    if (dataGridView == null) return;

        //    if (dataGridView.SelectedRows.Count < 1) return;
        //    Clipboard.Clear();
        //    Clipboard.SetDataObject(dataGridView.GetClipboardContent());
        //}

        //private BindingSource<object> foudResultList => dgvFoudResults.DataSource as BindingSource<object>;

        //private BindingSource<object> execHistory => dgvExecHistory.DataSource as BindingSource<object>;

        #endregion IResults

        #region IExecHistory

        public event Func<object, object[], bool> OnExecHistory;

        //public void AddExecHistory(object dfResult)
        //{
        //    execHistory.Add(dfResult, true);
        //}

        public void BindExecHistory(BindingList<IFoundItem> execHistorySource)
        {
            dgvExecHistory.DataSource = null;
            dgvExecHistory.DataSource = execHistorySource;
        }

        #endregion IExecHistory
        

        private void cbAutoSizeColumns_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked)
            {
                dgvFoudResults.AutoSizeColumnsMode = dgvExecHistory.AutoSizeColumnsMode =
                 DataGridViewAutoSizeColumnsMode.Fill;
            }
            else
            {
                dgvFoudResults.AutoSizeColumnsMode = dgvExecHistory.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.None;
            }
        }

        #region Query
        private void TryEnableQueryPanel()
		{
			queryPanel.Enabled = true;// (indexToolsBox?.Selected()?.Count() ?? 0) > 0;
			if (queryPanel.Enabled)
			{
				cbQuery.Enabled = true;
				cbQuery.Focus();
			}				
		}

        private void btnSearch_Click(object sender, EventArgs e)
        {
            (dgvFoudResults.DataSource as IList).Clear();
            foreach (var s in ltbSources.SelectedItems)
            {
                OnFindInSource?.Invoke(cbQuery.Text, s, new object[] { });
            }            
        }

        private void cbQuery_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cbQuery.Text != cbQuery.SelectedItem.ToString())
            {
                cbQuery.Text = cbQuery.SelectedItem.ToString();
                btnSearch_Click(sender, e);
            }
        }

        private void cbQuery_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    btnSearch_Click(sender, e);
                    break;
            }
        }



        //        private void SearchInIndex()
        //        {
        //            //выделенные индексы
        //            var indTags = indexToolsBox.AreSelected().Select((ts) => ts.Tag).Cast<object>().ToArray();
        //            //Делаем запрос
        //            //queryResultsLeft = indTags.Length;
        //            //if (queryResultsLeft < 1) return;
        //            indexResultList.Clear();
        //            string q = cbExtantions.Text == "" ? cbQuery.Text : cbQuery.Text + "*." + cbExtantions.Text;

        //#if Debug
        //			Watcher.watcher.Watch("Query", () => OnQuery?.Invoke(q, cbIncDirs.Checked, indTags));
        //#else
        //            ReceiveResultsInProcess = true; //блокировка на время заполнения таблицы результатами запроса
        //            OnFindAllInIndex?.Invoke(q, cbIncDirs.Checked, indTags,
        //                dgvIndexResults.DataSource as IList);
        //            ReceiveResultsInProcess = false;
        //            cbQuery.Enabled = true;
        //            dgvIndexResults.Focus();
        //#endif
        //        }

        //private void SearchInContent()
        //{
        //    if (lbSources.SelectedItems.Count < 1) return;
        //    OnFindAllInContent?.Invoke(cbQuery.Text, lbSources.SelectedItems.Cast<object>().ToArray(),
        //        dgvContentResults.DataSource as IList);
        //}
        #endregion Query

        //      private ToolStripMenuItem NewColumnsMenuItem(string name, string caption, int tag, EventHandler click)
        //{
        //	var result = new ToolStripMenuItem();
        //	result.CheckOnClick = true;
        //	result.Name = name;
        //	result.Text = caption;
        //	result.Tag = tag;
        //	result.Checked = false;
        //	result.Click += click;
        //	return result;
        //}       

        //      private List<ToolStripMenuItem> resultActions = new List<ToolStripMenuItem>();
        //private List<ToolStripMenuItem> historyActions = new List<ToolStripMenuItem>();


        //public void ChangeIndexToolSet(string name, object tag, ToolSetMode mode) => indexToolsBox.ChangeIndex(tag, mode, name);


        //private string indexExtantion;

        //private void indexToolsBox_IndexOpen(object sender, IndexEventArgs e)
        //{
        //	var openDlg = new OpenFileDialog();
        //	openDlg.Multiselect = true;
        //	openDlg.InitialDirectory = Application.StartupPath;
        //	openDlg.DefaultExt = indexExtantion;
        //	openDlg.Filter = $"(*{indexExtantion})|*{indexExtantion}";
        //	var dialogResult = openDlg.ShowDialog();
        //	if (dialogResult != DialogResult.OK) return;

        //	foreach (string name in openDlg.FileNames)			
        //		OnIndexAdd?.Invoke(name, TimeSpan.Zero);

        //}

        //private void indexToolsBox_IndexClose(object sender, IndexEventArgs e)
        //{
        //	//ничего не делаем пока не завершится заполнение таблицы результатами запроса (ShowQueryResults)
        //	if (ReceiveResultsInProcess) return;
        //	e.Result = OnIndexDel.Invoke(e.ToolStrip.Tag);
        //}

        //private void indexToolsBox_SelectClick(object sender, IndexEventArgs e)
        //{
        //	TryEnableQueryPanel();
        //}

        //private void indexToolsBox_CancelScanClick(object sender, IndexEventArgs e)
        //{
        //	OnCancel?.Invoke(e.ToolStrip.Tag);
        //}

        ///// <summary>
        ///// Выбирает сканируемый каталог
        ///// Стартует сканирование
        ///// Вовращает имя  и ссылку на рабочий сканер
        ///// </summary>
        //private void indexToolsBox_IndexScanNew(object sender, IndexEventArgs e)
        //{
        //	if (!(sender is ToolStripItem)) return;

        //	var dialogResult = scannerBox.ShowDialog(this);
        //	if (dialogResult != DialogResult.OK) return;
        //	//передаём выбранные поля для сканирования
        //	OnProcessingFieldsChanged(scannerBox.CheckedFields);

        //	OnNewScan?.Invoke(scannerBox.SelectedPath);
        //}

        //      private void indexToolsBox_RescanClick(object sender, IndexEventArgs e)
        //      {
        //	//ничего не делаем пока не завершится заполнение таблицы результатами запроса (ShowQueryResults)
        //	if (ReceiveResultsInProcess) return;
        //	//if (!(sender is ToolStripItem)) return;
        //	DoRescan(e.ToolStrip as IndexToolSet);			
        //}
        //private void indexToolsBox_IndexNameEdited(object sender, IndexEventArgs e)
        //{
        //	if (sender is IndexToolSet)
        //	{
        //		var its = sender as IndexToolSet;
        //		var newName = OnIndexReName(its.BtnSelector.Text, its.Tag);
        //		if (newName != "")
        //			its.BtnSelector.Text = newName;
        //	}
        //}

        //private void indexToolsBox_ShowToolsChanged(object sender, EventArgs e)
        //{
        //    //mainTools.Visible = indexToolsBox.ShowTools;
        //    //pAdvancedOptions.Visible = indexToolsBox.ShowTools;			
        //}


        //private void RescanSpan()
        //{
        //    //проверка актуальности и обновление индексов            
        //    foreach (var its in indexToolsBox.IndexToolSets())
        //    {
        //        // если устарел то запускаем обновление (rescan)
        //        if (IndexObsolete?.Invoke(its.Tag, its.RescanSpan) ?? false)
        //            DoRescan(its);
        //    }
        //}

        //      private void setOptionsState(bool state)
        //{
        //	if (!state)
        //	{
        //		tsbColor.Checked = tsbFont.Checked = tsbForeColor.Checked = false;

        //		//customizableControls.ClearUndo();
        //	}

        //	indexToolsBox.ShowTools = state;
        //}

        //public List<object> GetAllIndexes()
        //{
        //    return indexToolsBox.IndexToolSets().Select(its => its.Tag).ToList();
        //}

        //private void indexToolsBox_IndexChanged(object sender, EventArgs e)
        //{
        //if (!(sender is IndexToolSet)) return;
        //if (indexToolsBox.AreProgressing().Contains(sender))
        //    waitCounter = 0;
        //}

        //private void indexToolsBox_RescanSpanChanged(object sender, TimeSpanEventArgs e)
        //{
        //    RescanSpanChanged?.Invoke(e.TimeSpan, e.Tag);
        //}

        //private void tsbFileAdd_Click(object sender, EventArgs e)
        //{
        //    var ofd = new OpenFileDialog();
        //    ofd.Multiselect = true;
        //    if (ofd.ShowDialog() == DialogResult.OK)
        //    {
        //        OnAddSources?.Invoke(ofd.FileNames, lbSources.DataSource as IList);
        //    }
        //}

        //private void tsbFileDel_Click(object sender, EventArgs e)
        //{
        //    while (lbSources.SelectedItems.Count > 0)
        //    {
        //        lbSources.Items.Remove(lbSources.SelectedItems.Cast<object>().First());
        //    }
        //}

        //private void tcSources_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (tcSourceTypes.SelectedTab.Name == "tpSources")
        //    {
        //        dgvContentResults.Visible = true;
        //        dgvIndexResults.Visible = false;
        //        tsbRefreshList_Click(sender, e);
        //        cbIncDirs.Visible = false;
        //        cbExtantions.Visible = false;
        //    }
        //    else if (tcSourceTypes.SelectedTab.Name == "tpIndexes")
        //    {
        //        dgvIndexResults.Visible = true;
        //        dgvContentResults.Visible = false;
        //        cbIncDirs.Visible = true;
        //        cbExtantions.Visible = true;
        //    }
        //    //setOptionsState(tcSourceTypes.SelectedTab.Name == "tpOptions");  
        //}

        //private void tsbRefreshList_Click(object sender, EventArgs e)
        //{
        //    var selected = lbSources.SelectedItems.Cast<object>().ToList();
        //    OnTakeSources?.Invoke(lbSources.DataSource as IList);
        //    lbSources.ClearSelected();
        //    foreach (var s in selected)
        //    {
        //        var fs = lbSources.Items.Cast<object>().First(i => i.ToString() == s.ToString());

        //        lbSources.SetSelected(lbSources.Items.IndexOf(fs), true);
        //    }

        //    //for (int i=0; i<lbSources.Items.Count; i++)
        //    //{
        //    //    var item = lbSources.Items[i];
        //    //    if (selected.Find(s=>s.ToString()== item.ToString())!=null)
        //    //        lbSources.SetSelected(i, true);
        //    //}
        //}

        //private void tbTryRescanSpanIdle_TextChanged(object sender, EventArgs e)
        //{
        //    //если введено не число, то ставим 0
        //        if (!int.TryParse(tbTryRescanSpanIdle.Text, out tryRescanSpanIdle))
        //    {
        //        tbTryRescanSpanIdle.Text = "0";
        //        tryRescanSpanIdle = 0;
        //    }
        //    rescanTimer.Enabled = tryRescanSpanIdle > 0; //включаем таймер если 0>
        //}



        //public void AddExecHistory(List<IDFResult> dFResults)
        //{
        //    execHistory.AddRange(dFResults, true);
        //}

        //public List<IDFResult> GetExecHistory()
        //{
        //    return execHistory.ToList();
        //}


        /// <summary>
        /// Колличество ожидаемых пакетов результатов запроса
        /// </summary>
        //private int queryResultsLeft = 0;

        //public void ShowFoundIndexResults(IList list)
        //{
        //	if (list == null) return;
        //	ReceiveResultsInProcess = true; //блокировка на время заполнения таблицы результатами запроса
        //          var resultList = dgvIndexResults.DataSource as BindingSource<IDFResult>;
        //          if (resultList == null) return;
        //          resultList.AddRange(list.Cast<IDFResult>());
        //	queryResultsLeft --;
        //	if (queryResultsLeft  < 1 && resultList.Count > 0)
        //	{
        //		cbQuery.Enabled = true;
        //		dgvIndexResults.Focus();
        //	}
        //	ReceiveResultsInProcess = false;
        //}

        //Point wPos;
        //private void dgvResults_MouseDown(object sender, MouseEventArgs e)
        //{
        //	if (e.Button == MouseButtons.Left)
        //		wPos = MousePosition;
        //}

        //private void dgvResults_MouseMove(object sender, MouseEventArgs e)
        //{
        //	if (e.Button == MouseButtons.Left)
        //	{				
        //		int dx = MousePosition.X - wPos.X;
        //		int dy = MousePosition.Y - wPos.Y;
        //		//this.Text = $"{dx} , {dy}";
        //		if (dx!=0 || dy!=0)
        //		{
        //			Location = new Point(Location.X + dx, Location.Y + dy);
        //			wPos = MousePosition;
        //		}					
        //	}				
        //}				

    }
}