using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CollectionExtentions;

namespace Binders
{
    public partial class PropertyControls : UserControl
    {
        public PropertyControls()
        {
            InitializeComponent();
            tsbApply.Image = imageList.Images["Apply"];
        }

       // private PropertiesManager propertiesManager;
        public PropertiesManager PropertiesManager
        {
            //get => propertiesManager;
            set
            {                
                InitProperties(value);
            }
        }

        private dynamic GetValue(object tag)
        {
            //находим нужнный вложенный контрол
            var control = Controls.Cast<Control>().First(gb => gb.Tag == tag).Controls.Cast<Control>().First();
            
            //выводим значение соответствующего поля в зависимости от типа
            
            var t = control.Tag as Type;
            if (t != null)
            {
                if (t == typeof(string))
                    return (control as TextBox).Text;
                if (t == typeof(Color))
                    return (control as Button).BackColor;
                if (t == typeof(Font))
                    return (control as Button).Font;
            }
            return null;
        }

        private GroupBox FindGroup(string caption)
        {
            try
            {
                return Controls.OfType<GroupBox>().First(c => c.Text == caption);
            }
            catch { return null; }
        }

        private GroupBox NewGroup(string caption, object tag)
        {
            return new GroupBox { Text = caption, Dock = DockStyle.Top, Tag = tag };
        }

        private void InitProperties(PropertiesManager propertiesManager)
        {
            if (propertiesManager == null) return;
            foreach (var kv in propertiesManager.Properties)
            {
                var binders = kv.Value; 
                var tag = kv.Key;   //ссылка на привязанный объект
                var group = NewGroup((tag as Control).Name, tag);
                
                foreach (var b in binders)
                {
                    var propertyControl = NewProperty(b);
                    group.Controls.Add(propertyControl);
                }
                Controls.Add(group);
            }
        }

        public void ApplyProperties()
        {
            foreach (var group in Controls.OfType<GroupBox>())
            {
                var obj = group.Tag;
                foreach (var wraper in group.Controls.OfType<GroupBox>())
                {
                    var control = wraper.Controls.Cast<Control>().First();
                     var binder = control.Tag as Binder<dynamic>;
                    //if (binder!=null )
                    {
                        if (binder.Value is string)
                        {
                            binder.Value = control.Text;
                        }
                        else if (binder.Value is Color)
                        {
                            binder.Value = control.BackColor;
                        }
                        else if (binder.Value is Font)
                        {
                            binder.Value = control.Font;
                        }
                    }
                    
                }
            }
        }

        private Control NewProperty(Binder<dynamic> binder)
        {            
            Control control = null;
            dynamic initValue = binder.Value;
            if (initValue is string)
            {
                control = new TextBox { Text = initValue };
            }
            else if (initValue is Color)
            {
                control = new Button { Text = initValue.ToString(), BackColor= initValue };
                control.Click += (s, e) =>
                  {
                      var colorDialog = new ColorDialog();
                      colorDialog.Color = control.BackColor;
                      if (colorDialog.ShowDialog() != DialogResult.OK) return;
                      control.BackColor = colorDialog.Color;
                  };
            }
            else if (initValue is Font)
            {
                control = new Button { Text = initValue.ToString(), Font = initValue };
                control.Click += (s, e) =>
                {
                    var fontDialog = new FontDialog();
                    fontDialog.Font = initValue;
                    if (fontDialog.ShowDialog() != DialogResult.OK) return;
                    control.Font = fontDialog.Font;
                };
            }
            if (control!=null)
            {
                control.Tag = binder;
                control.Height = 30;
                control.Dock = DockStyle.Fill;                
            }

            var groupBox = new GroupBox { Text= binder.Name};
            groupBox.Controls.Add(control);
            return groupBox;
        }

        private void tsbApply_Click(object sender, EventArgs e)
        {
            ApplyProperties();
        }

        //public void ApplyProperties()
        //{
        //    foreach (var kv in propertiesManager.Properties)
        //    {
        //        var binders = kv.Value;
        //        var tag = kv.Key;
        //        foreach (var b in binders)
        //        {
        //            b.Value = GetValue(b);
        //        }
        //            //AddPropertyControl((tag as Control).Name + "." + b.Name, b.Value, tag);
        //    }
        //}
    }
}
