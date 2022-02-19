using CollectionExtentions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Bindings
{
    public partial class PropertiesEditor : UserControl
    {
        public PropertiesEditor()
        {
            InitializeComponent();
        }

        private SortableBindingList<BindedProperty> properties = new SortableBindingList<BindedProperty>();
        public IEnumerable<BindedProperty> Properties => properties;

        #region Properties

        private Font strToFont(string str)
        {
            var fp = str.Split(';', ',');
            if (fp.Length > 0)
            {
                var ffam = fp[0].Trim();
                float fs = 8;
                if (fp.Length > 1)
                {
                    if (fp[1].EndsWith("pt"))
                        fp[1] = fp[1].Remove(fp[1].LastIndexOf("pt"));
                    float.TryParse(fp[1].Trim(), out fs);
                }
                return new Font(ffam, fs);
            }
            else
                return null;

        }
        private Color strToColor(string str)
        {
            int getInt(string s)
            {
                int result = 0;
                int.TryParse(s, out result);
                return result;
            }


            var rgba = str.Split(';', ',').Select(s => getInt(s)).ToArray();
            if (rgba.Count() == 3) //RGB
            {
                return Color.FromArgb(rgba[0], rgba[1], rgba[2]);
            }
            else if (rgba.Count() == 4) //RGBA
            {
                return Color.FromArgb(rgba[0], rgba[1], rgba[2], rgba[3]);
            }
            else
                return Color.FromName(str);
        }       

        private PropertyWrap selectedProperty => (cbAllProperties.SelectedItem as PropertyWrap);

        public void SetPropDict<T>(string propName, Dictionary<string, T> group)
        {
            foreach (var p in properties.Where(p => p.Name == propName))
            {
                p.Value = group.First(c => c.Key == p.ObjectName).Value;
            }
        }

        public Dictionary<string, T> GetPropDict<T>(string propName) 
        {
            var result = new Dictionary<string, T>();
            //if (caster!=null)            
            //    foreach (var p in properties.Where(p => p.Name == propName))
            //    {
            //        result.Add(p.ObjectName, caster(p.Value));
            //    }            
            //else

            foreach (var p in properties.Where(p => p.Name == propName))
            {
                //object v = p.Value.ToString();
                //if (typeof(T) == typeof(Font))
                //{
                //    v = strToFont(p.Value.ToString());
                //}
                //else if (typeof(T) == typeof(Color))

                //{
                //    v = strToColor(p.Value.ToString());
                //}
                //if (v!=null)
                //result.Add(p.ObjectName, (T)v);     
                var v = p.Value;
                if (typeof(T) != v.GetType())
                    throw new Exception($"Неверный тип! Должен быть {v.GetType()}");
                else
                    result.Add(p.ObjectName, (T)p.Value);
            }
            return result;
        }

        private string getObjectName(object obj)
        {
            object name = null;
            try //пробуем получить name из возможных полей
            {
                new[] { "Name", "Caption" }.
                    First(s => (obj.GetPropertyValue(s, out name)));
            }
            catch { } //name=null
            if (!(name is string result))
                result = obj.ToString();
            return result;
        }

        /// <summary>
        /// Добавляет одно свойство одного объёкта
        /// </summary>
        public void AddProperty(string propertyName, object obj, string objName)
        {
            var pI = obj.GetType().GetProperty(propertyName);
            if (pI != null)
            {
                registerObject(obj, objName);
                properties.Add(new BindedProperty(obj, pI, objName));
            }
        }

        /// <summary>
        /// Добавление одноимённого свойства всех заданных объектов
        /// </summary>
        /// <param name="propertyName">имя свойства</param>
        /// <param name="objectsWithNames">массив пар объёкт+имя(может быть "") </param>
        public void AddProperty(string propertyName, Dictionary<object, string> objectsWithNames)
        {
            if (propertyName == null || propertyName == "") return;
            foreach (var objAndName in objectsWithNames)
            {
                var objName = objAndName.Value != "" ? objAndName.Value : getObjectName(objAndName.Key);
                AddProperty(propertyName, objAndName.Key, objName);               
            }
        }

        private void cbAllProperties_SelectedIndexChanged(object sender, EventArgs e)
        {
            clbObjects.ClearSelected(); //чистим выборку чтоб отображались свойства по всем объектам
            for (int i = 0; i < clbObjects.Items.Count; i++)
                clbObjects.SetItemChecked(i, true);

            grid.DataSource = properties.Where(p => 
                p.Name == selectedProperty.Name //фильтруем по имени свойства
                && p.GetPropertyType() == selectedProperty.Type) //фильтруем по типу свойства
                .ToList();
        }
        #endregion Properties

        #region Objects

        private IEnumerable<ClassWrap> selectedObjectWraps => clbObjects.CheckedItems.Cast<ClassWrap>();

        /// <summary>
        /// Заносит объект в список clbObjects если его там еще нет
        /// </summary>
        private bool registerObject(object obj, string objName)
        {
            bool result = false;
            try
            {
                result = clbObjects.Items.Cast<ClassWrap>().First(cw => cw.Name == objName && cw.Object == obj) != null;
            }catch
            {}
            
            if (!result)
                clbObjects.Items.Add(new ClassWrap(objName, obj));
            return result;
        }
        /// <summary>
        /// ищет в cbAllProperties обёрнутое PropertyWrap свойство
        /// </summary>
        private PropertyWrap findProperty(string propertyName, Type propertyType)
        {
            try
            {//ищем свойство
                return cbAllProperties.Items.Cast<PropertyWrap>().First(pr => pr.Name == propertyName && pr.Type == propertyType);
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// добавляет объёкты propertyNames, не регистрирует объект
        /// </summary>
        private IList<BindedProperty> addProperties(object obj, string objName, string[] propertyNames = null)
        {
            var result = new List<BindedProperty>();
            var pIs = obj.GetType().GetProperties();
            if (!propertyNames.IsVoid()) //если указаны propertyNames то фильтруем
                pIs = pIs.Where(pI => propertyNames.Contains(pI.Name)).ToArray();

            foreach (var pI in pIs)
            {
                BindedProperty newBP;
                properties.Add(newBP = new BindedProperty(obj, pI, objName));
                result.Add(newBP);
                var fr = findProperty(pI.Name, pI.PropertyType);
                if (fr == null) //если не найдено то добавляем
                {
                    cbAllProperties.Items.Add(new PropertyWrap(pI.Name, pI.PropertyType));
                }
            }
            return result;
        }

        /// <summary>
        /// Добавление заданного списка свойств для объекта
        /// </summary>
        /// <param name="obj">объект</param>
        /// <param name="objectName">имя объекта (может быть "")</param>
        /// <param name="propertyNames">массив имён свойств, если не задан то добавляются все свойства</param>
        public IList<BindedProperty> AddObject(object obj, string objectName="", params string[] propertyNames)
        {
            if (obj == null) return null;
            if (objectName=="") objectName = getObjectName(obj);
            registerObject(obj, objectName);
           
            return addProperties(obj, objectName, propertyNames);                        
        }

        public void RemoveObject(object obj)
        {
            clbObjects.Items.Remove(obj);
            //удаление всех свойств объекта
            BindedProperty property = properties.First(p => p.GetObject()==obj);
            while (property != null) 
            {
                properties.Remove(property);
                property = properties.First(p => p.GetObject()==obj);
            }
        }

        private void clbObjects_SelectItem(object sender, EventArgs e)
        {
            var selectedObjects = selectedObjectWraps.Select(s => s.Object);

            var props = properties.
                Where(p => selectedObjects.Contains(p.GetObject()));

            for (int i = 0; i < clbObjects.Items.Count; i++)
            {
                var ci = clbObjects.Items[i] as ClassWrap;
                clbObjects.SetItemChecked(i, (selectedObjects.Contains(ci.Object)));
            }

            if (selectedProperty != null)
                props = props.Where(p => selectedProperty.IsIdentic(p));
            grid.DataSource = null;
            grid.DataSource = props.ToList();
        }

        #endregion Objects

        public IList<string> ObjectNames => clbObjects.Items.Cast<ClassWrap>().Select(o => o.Name).ToList();
    }

    internal class ClassWrap
    {
        public string Name { get; private set; }
        public object Object { get; private set; }
        private bool showType;

        public ClassWrap(string name, object obj, bool showType = false)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Object = obj ?? throw new ArgumentNullException(nameof(obj));
            this.showType = showType;
        }

        public override string ToString()
        {
            if (showType)
                return Name + $"[{Object.GetType().Name}]";
            else
                return Name;
        }
    }

    internal class PropertyWrap
    {
        public string Name { get; private set; }
        public Type Type { get; private set; }        
        
        public PropertyWrap(string name, Type type=null)
        {            
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Type = type;
        }

        public override string ToString()
        {
            if (Type!=null)
                return Name+$"[{Type.Name}]";
            else
                return Name;
        }


        public bool IsIdentic(BindedProperty bindedProperty)
        {
            return Name == bindedProperty.Name && 
                Type == bindedProperty.GetPropertyType();
        }
    }

}