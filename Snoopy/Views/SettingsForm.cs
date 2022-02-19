using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace Snoopy.Views
{
    public partial class SettingsForm : Form, ISettingsView
    {
        public event Action<string, object> OnSettingChanged;
        public event Func<string, Type, object> GetSetting;

        public IEnumerable<string> ProcessingFields => cblProcessingFields.CheckedItems.Cast<string>();
        public bool UpdateConfirm => cbUpdateConfirm.Checked;
        public bool ShowHistory => cbShowHistory.Checked;


        public SettingsForm(params Control[] customizeControls)
        {
            InitializeComponent();
            bindCustomizeControls(customizeControls);
        }
               
        private void bindCustomizeControls(params Control[] controls)
        {
            foreach (var control in controls)
                cbCustomizeControls.Items.Add(new CustomizeControlWrap(control));
        }
        
        private void Settings_Load(object sender, EventArgs e)
        {
            //catchSettingsDic = SettingsDic;
            Init();
        }        

        public void InitCustomizeControls()
        {
            var backColors = GetSetting?.Invoke("BackColors", typeof(Dictionary<string, Color>)) as Dictionary<string, Color>;
            if (backColors != null)
            {
                foreach(var keyValuePair in backColors)
                {
                    cbCustomizeControls.Items.Cast<CustomizeControlWrap>().
                        First(cc => cc.Control.Name == keyValuePair.Key).Control.BackColor = keyValuePair.Value;
                }               
            }

            var foreColors = GetSetting?.Invoke("ForeColors", typeof(Dictionary<string, Color>)) as Dictionary<string, Color>;
            if (foreColors != null)
            {
                foreach (var keyValuePair in foreColors)
                {
                    cbCustomizeControls.Items.Cast<CustomizeControlWrap>().
                        First(cc => cc.Control.Name == keyValuePair.Key).Control.ForeColor = keyValuePair.Value;
                }
            }

            var fonts = GetSetting?.Invoke("Fonts", typeof(Dictionary<string, Font>)) as Dictionary<string, Font>;
            if (fonts != null)
            {
                foreach (var keyValuePair in fonts)
                {
                    cbCustomizeControls.Items.Cast<CustomizeControlWrap>().
                        First(cc => cc.Control.Name == keyValuePair.Key).Control.Font = keyValuePair.Value;
                }
            }

        }
        
        public void Init()
        {
            cbUpdateConfirm.Checked = (bool)(GetSetting?.Invoke(cbUpdateConfirm.Text, typeof(bool))??false);
            cbShowHistory.Checked = (bool)(GetSetting?.Invoke(cbShowHistory.Text, typeof(bool))??false);
            cbShowHistory_CheckedChanged(null, EventArgs.Empty);

            var p = GetSetting?.Invoke(tbTryRescanSpanIdle.Name, typeof(long));
            long v = (long)(p??(long)0);
            tbTryRescanSpanIdle.Text = v.ToString();

            var pfDic = GetSetting?.Invoke("processingFields", typeof(Dictionary<string, bool>)) as Dictionary<string, bool>;
            if (pfDic != null)
            {
                cblProcessingFields.Items.Clear();
                foreach (var pf in pfDic)
                {
                    cblProcessingFields.Items.Add(pf.Key, pf.Value);
                }
            }

            InitCustomizeControls();

            //var colors = GetSetting?.Invoke("BackColors", typeof(Dictionary<string, Color>)) as Dictionary<string, Color>;
            //if (colors != null)
            //    propertiesEditor.SetPropDict("BackColor", colors);

            //colors = GetSetting?.Invoke("ForeColors", typeof(Dictionary<string, Color>)) as Dictionary<string, Color>;
            //if (colors != null)
            //    propertiesEditor.SetPropDict("ForeColor", colors);

            //var fonts = GetSetting?.Invoke("Fonts", typeof(Dictionary<string, Font>)) as Dictionary<string, Font>;
            //if (fonts != null)
            //    propertiesEditor.SetPropDict("Font", fonts);


        }
                
        private void bConfirm_Click(object sender, EventArgs e)
        {            
           var pfDic = new Dictionary<string, bool>();
            foreach (string pf in cblProcessingFields.Items)
            {
                pfDic.Add(pf, false);
            }
            foreach (string chi in cblProcessingFields.CheckedItems)
            {
                pfDic[chi] = true;
            }
            OnSettingChanged?.Invoke("processingFields", pfDic);

            OnSettingChanged?.Invoke(cbUpdateConfirm.Text, cbUpdateConfirm.Checked);
            OnSettingChanged?.Invoke(cbShowHistory.Text, cbShowHistory.Checked);
            long spanIdle = 0;
            long.TryParse(tbTryRescanSpanIdle.Text, out spanIdle);
            OnSettingChanged?.Invoke(tbTryRescanSpanIdle.Name, spanIdle);

            //var colors = propertiesEditor.GetPropDict<Color>("BackColor");
            var backColors = new Dictionary<string, Color>();
            var foreColors = new Dictionary<string, Color>();
            var fonts = new Dictionary<string, Font>();

            var CustomizeControls = cbCustomizeControls.Items.Cast<CustomizeControlWrap>().Select(cc => cc.Control);
            foreach (var control in CustomizeControls)
            {
                backColors.Add(control.Name, control.BackColor);
                foreColors.Add(control.Name, control.ForeColor);
                fonts.Add(control.Name, control.Font);
            }

            OnSettingChanged?.Invoke("BackColors", backColors);
            OnSettingChanged?.Invoke("ForeColors", foreColors);
            OnSettingChanged?.Invoke("Fonts", fonts);

            //colors = propertiesEditor.GetPropDict<Color>("ForeColor");
            //OnSettingChanged?.Invoke("ForeColors", colors);

            //var fonts = propertiesEditor.GetPropDict<Font>("Font");
            //OnSettingChanged?.Invoke("Fonts", fonts);
        }

        public event EventHandler<bool> ShowHistoryChanged;
        private void cbShowHistory_CheckedChanged(object sender, EventArgs e)
        {
            ShowHistoryChanged?.Invoke(sender, cbShowHistory.Checked);
        }

        private void cblProcessingFields_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void fontDialog1_Apply(object sender, EventArgs e)
        {

        }

        private void bBackColor_Click(object sender, EventArgs e)
        {
            var result = colorDialog.ShowDialog();
            if (result== DialogResult.OK)
            {
                foreach (CustomizeControlWrap CheckedItem in cbCustomizeControls.CheckedItems)
                {
                    CheckedItem.Control.BackColor = colorDialog.Color;
                }
            }                
        }

        private void bForeColor_Click(object sender, EventArgs e)
        {
            var result = colorDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                foreach (CustomizeControlWrap CheckedItem in cbCustomizeControls.CheckedItems)
                {
                    CheckedItem.Control.ForeColor = colorDialog.Color;
                }
            }
        }

        private void bFont_Click(object sender, EventArgs e)
        {
            var result = fontDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                foreach (CustomizeControlWrap CheckedItem in cbCustomizeControls.CheckedItems)
                {
                    CheckedItem.Control.Font = fontDialog.Font;
                }
            }
        }


        //private void cbShowHistory_CheckedChanged(object sender, EventArgs e)
        //{
        //    //scContentResults.Panel2Collapsed = !(dgvContentHistory.Visible = cbShowContentHistory.Checked);
        //}

        /// <summary>
        /// Кол-во секунд до проверки необходимости пересканировать
        /// </summary>
        //private int tryRescanSpanIdle = 0;
        /// <summary>
        /// счётчик секунд 
        /// </summary>
        //      private int waitCounter = 0;

        //      private void rescanTimer_Tick(object sender, EventArgs e)
        //{
        //	if (tryRescanSpanIdle == 0) return;
        //	if (waitCounter*rescanTimer.Interval > tryRescanSpanIdle*rescanTimer.Interval)
        //	{
        //              rescanTimer.Enabled = false;
        //              //OnRescanSpan?.Invoke();
        //              RescanSpan();
        //              rescanTimer.Enabled = tryRescanSpanIdle > 0;
        //              waitCounter = 0;
        //	}
        //	else
        //		waitCounter++;
        //	//this.Text = waitCounter.ToString();
        //}

    }

    class CustomizeControlWrap
    {
        public CustomizeControlWrap(Control control)
        {
            Control = control ?? throw new ArgumentNullException(nameof(control));
        }

        public Control Control { get; }
        public override string ToString()
        {
            //return Control.Text==""? Control.Name: Control.Text;
            return Control.Name;
        }
        
    }

}
