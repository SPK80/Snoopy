using SPKLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ListToolsBox
{
    public partial class ListToolsBox : Panel
    {   
        public ListToolsBox()
        {
            InitializeComponent();
            ItemHeight = 20;
            InitCommonSet();
        }

        public ListToolsBox(IContainer container)
        {
            ItemHeight = 20;
            container.Add(this);
            InitializeComponent();
            InitCommonSet();
        }

        #region Public

        public void Clear()
        {
            commonToolsSet = null;
            Controls.Clear();
        }

        /// <summary>
        /// Отмеченные IndexToolSets
        /// </summary>
        public IEnumerable<object> Selected =>
            ItemToolSets.Where((ts) => ts.Selected).Select(ts => ts.Tag);
        public IEnumerable<object> Progressing =>
            ItemToolSets.Where((ts) => ts.Progressing).Select(ts => ts.Tag);

        public void RemoveItem(object item)
        {
            var DelItems = ItemToolSets.Where(i => i.Tag == item);
            foreach (var it in DelItems)
            {
                Controls.Remove(it);
            }
            //commonToolsSet.BtnClose.Enabled = Selected.Count() > 0;
        }

        public void AddItem(object item)
        {
            var newItem = new ItemToolSet(item, this);

            newItem.OnSelect += (s, e) =>
            {
                var itemToolSet = doItemEvent(OnSelectItem, s);
                if (itemToolSet == null) return;

                //логика выбора.../////////////////////
                if (!MultiSelect)
                {
                    itemToolSet.Selected = true;
                    if (lastSelected != null && lastSelected != itemToolSet)
                        lastSelected.Selected = false;
                }
                lastSelected = itemToolSet;
                ///////////////////////////////////////

                //commonToolsSet.BtnClose.Enabled = Selected.Count() > 0;
                //commonToolsSet.BtnRescan.Enabled = commonToolsSet.BtnClose.Enabled;
            };

            newItem.OnCancelProgress += (s, e) => doItemEvent(OnCancelItem, s);
            newItem.OnRename += (s, e) => doItemEvent(OnRenameItem, s);

        }

        public void RenameItem(object item, string newName)
        {
            ItemToolSet(item).Name = newName;
        }

        public void SetProgressValue(object item, int value)
        {
            ItemToolSet(item).ProgressValue = value;
        }

        //public bool Progressing(object item)
        //{
        //    return ItemToolSet(item).Progressing;
        //}

        //public void SetItemMode(object item, ToolSetMode newMode)
        //{
        //    ItemToolSet(item).Mode = newMode;
        //    //if (AreProgressing().Count() == 0)
        //    //    commonToolsSet.BtnCancelScan.Enabled = false;
        //}

        //public void SetItemProgress(object item, int value, string tipText ="")
        //{
        //    var ts = ItemToolSet(item);

        //    if (value >= ts.BtnSelector.Maximum)
        //        ts.BtnSelector.Maximum = value * 2;

        //    ts.BtnSelector.Value = value;
        //    if (tipText!="")
        //        ts.BtnSelector.ToolTipText = tipText;
        //}


        /// <summary>
        /// Выполняет клик по указанному селектору
        /// </summary>
        /// <param name="pos">порядковый номер</param>
        //private void ClickSelector(int pos)
        //{
        //    var indexes = ItemToolSets; //todo: проверить необходимость ToArray()
        //    int c = indexes.Count();
        //    if (pos >= c) return;
        //    indexes.ToArray()[c - pos - 1].BtnSelector.PerformClick();
        //}


        public event EventHandler<ItemEventArgs> OnRenameItem;
        public event EventHandler<ItemEventArgs> OnSelectItem;
        public event EventHandler<ItemEventArgs> OnOpenItem;
        public event EventHandler<ItemEventArgs> OnCloseItem;
        public event EventHandler<ItemEventArgs> OnNewItem;
        public event EventHandler<ItemEventArgs> OnUpdateItem;
        public event EventHandler<ItemEventArgs> OnCancelItem;


        //public event EventHandler<TimeSpanEventArgs> RescanSpanChanged;
        //private void onRescanSpanChanged(object sender, EventArgs e)
        //{
        //    var tse = (sender as ToolStripComboBox)?.SelectedItem as TimeSpanExt;
        //    var par = (sender as Control)?.Parent;
        //    if (tse != null && par != null)
        //    {
        //        RescanSpanChanged?.Invoke(this, TimeSpanEventArgs.Create(tse.timeSpan, par.Tag));
        //    }
        //}

        #endregion Public


        #region Настройки

        internal event EventHandler OnFontAutoScalingChanged;
        private bool fontAutoScaling = true;
        public bool FontAutoScaling
        {
            get => fontAutoScaling;
            set
            {
                fontAutoScaling = value;
                OnFontAutoScalingChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        internal event EventHandler OnImagesAutoScalingChanged;
        private bool imagesAutoScaling = true;
        public bool ImagesAutoScaling
        {
            get => imagesAutoScaling;
            set
            {
                imagesAutoScaling = value;
                OnImagesAutoScalingChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        internal event EventHandler OnItemHeightChanged;
        private int itemHeight;
        public int ItemHeight
        {
            get => itemHeight;
            set
            {
                if (value < 0)
                    itemHeight = -value;
                else
                    itemHeight = value;
                OnItemHeightChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void dropSelectors()
        {
            foreach (var ts in ItemToolSets)
            {
                if (ts != lastSelected)
                    ts.Selected = false;
            }
        }

        private ItemToolSet lastSelected;
        public bool MultiSelect { get; private set; } = false;

        //public bool ToolsButton
        //{
        //    get => commonToolsSet.BtnShowTools.Visible;
        //    set => commonToolsSet.BtnShowTools.Visible = value;
        //}

        //private void onChangeShowTools(object sender, EventArgs e)
        //{
        //    var tsi = sender as ToolStripButton;
        //    foreach (var its in ItemToolSets)
        //    {
        //        its.ShowTools = tsi.Checked;
        //    }
        //    commonToolsSet.BtnMoveUp.Visible = commonToolsSet.BtnMoveDown.Visible = tsi.Checked;
        //    ShowToolsChanged?.Invoke(this, EventArgs.Empty);
        //}

        //public event EventHandler ShowToolsChanged;

        //public bool ShowTools
        //{
        //    get => commonToolsSet.BtnShowTools.Checked;
        //    set
        //    {
        //        if (ShowTools != value)
        //        {
        //            commonToolsSet.BtnShowTools.Checked = value;
        //            onChangeShowTools(commonToolsSet.BtnShowTools, EventArgs.Empty);
        //        }
        //    }
        //}

        //protected override void OnFontChanged(EventArgs e)
        //{
        //    base.OnFontChanged(e);
        //    foreach (Control c in Controls)
        //    {
        //        c.Font = Font;
        //    }
        //}
        //protected override void OnBackColorChanged(EventArgs e)
        //{
        //    base.OnBackColorChanged(e);
        //    foreach (Control c in Controls)
        //    {
        //        c.BackColor = BackColor;
        //    }
        //}

        #endregion Настройки

        //private void InitButtons()
        //{
        //    settings = new Dictionary<ControllTags, ButtonSettings>
        //    {
        //        { ControllTags.Selector, new ButtonSettings(this, "", "")},
        //        { ControllTags.btnOpen, new ButtonSettings(this, "Open", "Открыть") },
        //        { ControllTags.btnClose, new ButtonSettings(this, "Close", "Закрыть") },
        //        { ControllTags.btnRescan, new ButtonSettings(this, "Rescan", "Обновить") },
        //        { ControllTags.btnScanNew, new ButtonSettings(this, "ScanNew",  "Сканировать Новый")},
        //        { ControllTags.btnCancelScan, new ButtonSettings(this, "CancelScan",  "Отмена Сканирования") },
        //        { ControllTags.btnShowTools, new ButtonSettings(this, "ShowTools",  "Показать Инструменты") },
        //        { ControllTags.btnMultySelect, new ButtonSettings(this, "MultySelect",  "Несколько") },
        //        { ControllTags.btnMoveUp, new ButtonSettings(this, "MoveUp",  "Сдвинуть вверх") },
        //        { ControllTags.btnMoveDown, new ButtonSettings(this, "MoveDown",  "Сдвинуть вниз") },
        //    };
        //}

        private ItemToolSet doItemEvent(EventHandler<ItemEventArgs> eventHandler, object sender)
        {
            var itemToolSet = (sender as ItemToolSet);
            if (itemToolSet != null)
                eventHandler?.Invoke(this, new ItemEventArgs(itemToolSet.Tag));
            return itemToolSet;
        }

        private void doCommonEvent(EventHandler<ItemEventArgs> eventHandler)
        {
            if (!MultiSelect)
                eventHandler?.Invoke(this, new ItemEventArgs(lastSelected.Tag));
            else
                eventHandler?.Invoke(this, ItemEventArgs.Empty);
        }

        private const string commonName = "/Common/";
        private CommonToolSet commonToolsSet;

        private void InitCommonSet()
        {
            if (commonToolsSet == null)
            {
                commonToolsSet = new CommonToolSet(commonName, this);
                commonToolsSet.GripStyle = ToolStripGripStyle.Hidden;
                commonToolsSet.OnOpen += (s, e) => doCommonEvent(OnOpenItem);
                commonToolsSet.OnClose += (s, e) => doCommonEvent(OnCloseItem);
                commonToolsSet.OnNew += (s, e) =>
                {
                    OnNewItem?.Invoke(this, ItemEventArgs.Empty);
                };
                commonToolsSet.OnUpdate += (s, e) => doCommonEvent(OnUpdateItem);
                commonToolsSet.OnCancel += (s, e) => doCommonEvent(OnCancelItem);
                commonToolsSet.OnMoveUp += (s, e) => MoveToolSet(1);
                commonToolsSet.OnMoveDown += (s, e) => MoveToolSet(-1);
                commonToolsSet.OnMultySelectCheck += (s, e) => MultiSelect = e.Checked;
                Controls.Add(commonToolsSet);
            }
        }


        /// <summary>
        /// Перемещение ToolSet 
        /// Если tag = null, то перемещает Selected
        /// </summary>
        /// <param name="shift"></param>
        /// <param name="tag"></param>
        private void MoveToolSet(int shift, object tag = null)
        {
            //Определяем перемещаемый ItemToolSet
            ItemToolSet ts;
            try
            {
                if (tag == null)
                    ts = this.lastSelected;
                else
                    ts = ItemToolSet(tag);
            }
            catch (InvalidOperationException)
            {
                return;
            }

            SuspendLayout();

            var toolsetList = new List<ItemToolSet>(ItemToolSets);

            int beginPos = toolsetList.IndexOf(ts); //начальная позиция
            if (beginPos < 0) return;

            //сдвигаем если конечная позиция допустима
            int endPos = beginPos + shift;
            if (endPos >= 0 && endPos < toolsetList.Count)
                toolsetList.Move(beginPos, beginPos + shift);
            //Чистим контейнер Controls
            Controls.Clear();
            //while (Controls.Count > 0)
            //{
            //    Controls.RemoveAt(0);
            //}
            //Заполняем контейнер Controls в новом порядке
            foreach (var c in toolsetList)
                Controls.Add(c);
            Controls.Add(commonToolsSet);

            ResumeLayout();
        }

        //ХЗ...
        //public object[] GetIndexOrder()
        //{
        //    return AllTags.ToArray();
        //}

        internal event EventHandler OnImageListChanged;
        private ImageList imageList;
        public ImageList ImageList
        {
            get => imageList;
            set
            {
                imageList = value;
                OnImageListChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        //public void ShowMessage(string msg, object tag=null)
        //{			
        //	if (tag!=null)
        //		foreach (var its in ToolSets().Where((ts) => ts.Tag == tag))
        //		{
        //			its.ShowMessage(msg);
        //		}
        //}

        /// <summary>
        /// Все ItemToolSet
        /// </summary>
        private IEnumerable<ItemToolSet> ItemToolSets => Controls.OfType<ItemToolSet>();

        /// <summary>
        /// Один конкретный ItemToolSet, где Tag=tag
        /// </summary>
        private ItemToolSet ItemToolSet(object tag) => ItemToolSets.First(i => i.Tag == tag);

        /// <summary>
        /// Перечисление всех Tag
        /// </summary>
        private IEnumerable<object> AllTags => ItemToolSets.Select(its => its.Tag).Reverse();


    }

    //public class TimeSpanEventArgs : EventArgs
    //{
    //    public TimeSpan TimeSpan;
    //    public object Tag;
    //    public static TimeSpanEventArgs Create(TimeSpan timeSpan, object tag)
    //        => new TimeSpanEventArgs { TimeSpan = timeSpan };

    //}

    public class ItemEventArgs : EventArgs
    {
        public ItemEventArgs(object item)
        {
            Item = item;
        }
        public object Item { get; private set; }
        public new static ItemEventArgs Empty => new ItemEventArgs(null);
    }

    //   public class ButtonSettings
    //{
    //	private ToolsBox owner;

    //	public ButtonSettings(ToolsBox owner, string name, string caption)
    //	{
    //		this.owner = owner ?? throw new ArgumentNullException(nameof(owner));
    //		Name = name ?? throw new ArgumentNullException(nameof(name));
    //		Caption = caption ?? throw new ArgumentNullException(nameof(caption));
    //	}

    //	public string Name;
    //	public string Caption;
    //	public Image Image => owner?.ImageList?.Images[Name]; //получение Image из owner по имени Name
    //	public int SelectorsWidth;
    //}

    //public class SettingsEventArgs : EventArgs
    //{
    //	public Dictionary<ControllTags, ButtonSettings> settings;
    //}
}