using SPKCollections.Extentions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace ListToolsBox
{
    public partial class ListToolsBox: Panel
    {
        public ListToolsBox()
        {
            InitializeComponent();

            ItemHeight = 20;
            InitCommonSet();
        }

        public ListToolsBox(IContainer container)
        {
            container.Add(this);
            InitializeComponent();

            ItemHeight = 20;
            InitCommonSet();
        }

        #region Public

        //private BindingList<IToolSet> dataSource;
        //public BindingList<IToolSet> DataSource
        //{
        //    get => dataSource;

        //    set
        //    {

        //        dataSource = value;
        //    }
        //}

        public void ShowMessage(string message, object item)
        {
            if (item != null)
                ItemToolSet(item).ShowMessage(message);
        }

        public void Clear()
        {
            commonToolsSet = null;
            Controls.Clear();
        }

        public IEnumerable<object> Items =>
            ItemToolSets.Select(ts => ts.Tag);

        /// <summary>
        /// Отмеченные IndexToolSets
        /// </summary>
        public IEnumerable<object> SelectedItems =>
            ItemToolSets.Where((ts) => ts.Selected).Select(ts => ts.Tag);
        public IEnumerable<object> ProgressingItems =>
            ItemToolSets.Where((ts) => ts.Progressing).Select(ts => ts.Tag);
        public IEnumerable<object> SelectedProgressingItems => 
            ProgressingItems.Intersect(SelectedItems);        
        
        public string SelectedProgressingNames => 
            SelectedProgressingItems.Aggregate("", (av, s) => av + s.ToString() + " ");
        public string SelectedNames =>
            SelectedItems.Aggregate("", (av, s) => av + s.ToString() + " ");

        public void SetSelected(object item, bool value)
        {
            //if (SelectedItems.Contains(item)) return;
            ItemToolSets.Where((ts) => ts.Tag == item).ToList().ForEach(ts=>ts.Selected= value);
        }
        public void SetSelected(int index, bool value)
        {
            if (index<ItemToolSets.Count())
                ItemToolSets.Reverse().ToArray()[index].Selected = value;            
        }

        //public Dictionary<object, int> ProgressValues;

        //public void SetName(object item, string name)
        //{
        //        ItemToolSet(item).Name = name;
        //}

        public void SetProgressValue(object item, int value, string text = "")
        {
            if (SelectedProgressingItems.Count() > 0)
            {
                if (ItemToolSet(item).ProgressValue<0 && value>=0)//progressing
                {
                    commonToolsSet.SetEnabled(true, "Cancel");                    
                }                    
                else if (ItemToolSet(item).ProgressValue >=0 && value < 0)//unprogressing
                {
                    commonToolsSet.SetEnabled(false, "Cancel");                    
                }
            }
            ItemToolSet(item).ProgressValue = value;
            if (value>=0 && text != "")
                ItemToolSet(item).ProgressText = text;
            if (value<0)
                ItemToolSet(item).Name = item.ToString();
        }

        public void IncProgressValue(object item, int addValue, string text = "")
        {
            SetProgressValue(item, ItemToolSet(item).ProgressValue + addValue, text);
        }

        public void RemoveItem(object item)
        {
            var DelToolSets = ItemToolSets.Where(i => i.Tag == item);
            if (DelToolSets.Contains(lastSelected)) lastSelected = null;
            foreach (var it in DelToolSets)
            {
                Controls.Remove(it);
            }
            if (SelectedItems.Count()==0)
                commonToolsSet.SetEnabled(false, "Close", "Update", "MoveUp", "MoveDown", "Cancel");

            //commonToolsSet.BtnClose.Enabled = Selected.Count() > 0;
        }

        public void AddItem(object item)
        {
            var newItem = new ItemToolSet(item, this);
            //commonToolsSet.SetEnabled(false, "Close", "MoveUp", "MoveDown", "Cancel", "Update");

            newItem.OnSelect += (s, e) =>
            {                
                if (!doItemEvent(OnSelectItem, s)) return;
                var itemToolSet = (s as ItemToolSet);
                
                //логика выбора.../////////////////////
                if (!MultiSelect && lastSelected != null && lastSelected != itemToolSet)
                    lastSelected.Selected = false;
                lastSelected = itemToolSet;
                /////////////////////////////////////////
                commonToolsSet.SetEnabled(!ProgressingItems.Contains(item), "Update");                    
                commonToolsSet.SetEnabled(true, "Close", "MoveUp", "MoveDown");
                commonToolsSet.SetEnabled(SelectedProgressingItems.Count() > 0, "Cancel");
            };

            newItem.OnDeSelect += (s, e) =>
            {
                if (ItemToolSets.Where((ts) => ts.Selected).Count() < 1)
                    commonToolsSet.SetEnabled(false, "Close", "Update", "MoveUp", "MoveDown");
                commonToolsSet.SetEnabled(SelectedProgressingItems.Count() > 0, "Cancel");
            };

            newItem.OnCancelProgress += (s, e) =>
            {
                doItemEvent(OnCancelItem, s);
            };

            newItem.OnRename += (s, e) => 
            {
                var itemToolSet = (s as ItemToolSet);
                if (itemToolSet != null)
                    OnRenameItem?.Invoke(this, new ItemEventArgs(itemToolSet.Tag, itemToolSet.Text));
            };

            //newItem.OnStartProgress += (s, e) => commonToolsSet.SetEnabled(true, "Cancel");
            //newItem.OnStopProgress += (s, e) => commonToolsSet.SetEnabled(false, "Cancel");
            Controls.SetChildIndex(newItem, 0);
        }

        public void RenameItem(object item, string newName)
        {
            ItemToolSet(item).Name = newName;
        }

        public event EventHandler<ItemEventArgs> OnRenameItem;
        public event EventHandler<ItemEventArgs> OnSelectItem;
        public event EventHandler<ItemEventArgs> OnAddItem;
        public event EventHandler<ItemEventArgs> OnRemoveItem;
        public event EventHandler<ItemEventArgs> OnNewItem;
        public event EventHandler<ItemEventArgs> OnUpdateItem;
        public event EventHandler<ItemEventArgs> OnCancelItem;
        public event EventHandler<ItemEventArgs> OnMoveUpItem;
        public event EventHandler<ItemEventArgs> OnMoveDownItem;

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

        //private void dropSelectors()
        //{
        //    foreach (var ts in ItemToolSets)
        //    {
        //        if (ts != lastSelected)
        //            ts.Selected = false;
        //    }
        //}

        private ItemToolSet lastSelected; //последний выбранный ItemToolSet

        private bool multiSelect = false;
        public bool MultiSelect
        {
            get => multiSelect;
            private set
            {
                multiSelect = value;
                if (!multiSelect)
                {
                    foreach (var its in ItemToolSets)
                    {
                        if (its.Selected && its!= lastSelected)
                            its.Selected = false;
                    }                    
                }
            }
        }

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
        
        private bool doItemEvent(EventHandler<ItemEventArgs> eventHandler, object sender)
        {
            var itemToolSet = (sender as ItemToolSet);
            if (itemToolSet != null)
            {
                var e = new ItemEventArgs(itemToolSet.Tag);
                eventHandler?.Invoke(this, e);
                return !e.Cancel;
            }                
            return false;
        }

        private bool doCommonEvent(EventHandler<ItemEventArgs> eventHandler)
        {            
            var item = MultiSelect ? null : lastSelected?.Tag;
            
            var e = new ItemEventArgs(item);
                eventHandler?.Invoke(this, e);

            return !e.Cancel;
        }

        private const string commonName = "/Common/";
        private CommonToolSet commonToolsSet;

        private void InitCommonSet()
        {
            if (commonToolsSet == null)
            {
                commonToolsSet = new CommonToolSet(commonName, this);
                commonToolsSet.GripStyle = ToolStripGripStyle.Hidden;

                commonToolsSet.OnOpen += (s, e) => doCommonEvent(OnAddItem);
                commonToolsSet.OnClose += (s, e) => doCommonEvent(OnRemoveItem);
                commonToolsSet.OnNew += (s, e) => doCommonEvent(OnNewItem);
                commonToolsSet.OnUpdate += (s, e) => doCommonEvent(OnUpdateItem);
                commonToolsSet.OnCancel += (s, e) => doCommonEvent(OnCancelItem);
                commonToolsSet.OnMoveUp += (s, e) =>
                {
                    if (doCommonEvent(OnMoveUpItem))
                        moveToolSet(1);
                };
                commonToolsSet.OnMoveDown += (s, e) =>
                {   
                    if (doCommonEvent(OnMoveDownItem))
                        moveToolSet(-1);                    
                };
                commonToolsSet.OnMultySelectCheck += (s, e) => MultiSelect = e.Checked;
                Controls.SetChildIndex(commonToolsSet, 0);
                commonToolsSet.SetEnabled(false, "Close", "Update", "MoveUp", "MoveDown", "Cancel");
            }
        }

        /// <summary>
        /// Перемещение ToolSet 
        /// Если tag = null, то перемещает Selected
        /// </summary>
        /// <param name="shift"></param>
        /// <param name="tag"></param>
        private void moveToolSet(int shift, object tag = null)
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
        public ItemEventArgs() { }

        public ItemEventArgs(object item, object value=null)
        {
            Item = item;
            Value = value;
        }

        /// <summary>
        /// Объект над которым производится действие
        /// </summary>
        public object Item { get; private set; } = null;
        /// <summary>
        /// Передаётся значение если есть, или null если нет
        /// </summary>
        public object Value { get; private set; } = null;
        /// <summary>
        /// Для запрета/отмены действия
        /// </summary>
        public bool Cancel { get; set; } = false;

        public new static ItemEventArgs Empty => new ItemEventArgs(null, null);
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