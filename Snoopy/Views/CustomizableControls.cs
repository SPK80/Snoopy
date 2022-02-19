using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snoopy.Views
{
    /// <summary>
    /// Автоматизированный набор Control
    /// </summary>
    public class CustomizableControlsManager
    {
        /// <summary>
        /// хранит пару Control и Control.Enabled
        /// </summary>
        private Dictionary<Control, bool> controlsAndEnableds = new Dictionary<Control, bool>();

        //public IEnumerator<Control> GetEnumerator()
        //{
        //          return (controlsAndEnableds.Select(it=>it.Key)).GetEnumerator();
        //}

        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //	return (controlsAndEnableds.Select(it => it.Key)).GetEnumerator();
        //}
        public IList<Control> Controls =>
            controlsAndEnableds.Select(it => it.Key).ToList();

        private void ForEach(Action<Control> action)
        {
            foreach (var c in controlsAndEnableds.Keys)
            {
                action(c);
            }
        }

        private void ForEach(Action<Control, bool> action)
        {
            foreach (var kv in controlsAndEnableds)
            {
                action(kv.Key, kv.Value);
            }
        }

        /// <summary>
        /// Всем controls ставит Enabled = true
        /// </summary>
        private void enableAll()
        {
            ForEach((c, e) => { e = c.Enabled; c.Enabled = true; });
        }

        /// <summary>
        /// Всем controls восстанавливает Enabled из сохранённых значений
        /// </summary>
        private void revertEnableAll()
        {
            ForEach((c, e) => { c.Enabled = e; });
        }

        /// <summary>
        /// Добавляет Control и запаминает значение поля Enabled
        /// Испульзуется конструктором по умолчанию
        /// </summary>
        /// <param name="control"></param>
        public void Add(Control control) =>
            controlsAndEnableds.Add(control, control.Enabled);

        /// <summary>
        /// Ищет Control по имени
        /// </summary>
        private List<Control> FindControl(string controlName) =>
            controlsAndEnableds.
            Select((kv) => kv.Key).
            Where((c) => c.Name == controlName).
            ToList();

        #region Undo
        private UniUndoStack undo = new UniUndoStack();

        public void Undo() => undo.Undo();
        public void UndoAll() => undo.UndoAll();
        public void ClearUndo() => undo.Clear();
        #endregion Undo

        #region Font
        /// <summary>
        /// Обработчик события.
        /// Выбирает (в FontDialog) и устанавливает шрифт (Control)sender
        /// </summary>
        private void fontSetter(object sender, EventArgs e)
        {
            var fontDialog = new FontDialog();
            fontDialog.Font = ((Control)sender).Font;
            if (fontDialog.ShowDialog() != DialogResult.OK) return;

            undo.Add(sender as Control, ((Control)sender).Font, (c, v) => c.Font = v as Font);
            ((Control)sender).Font = fontDialog.Font;
        }

        /// <summary>
        /// True устанавливет обработчик Click, который выбирает (в FontDialog) и устанавливает шрифт
        /// False удаляет обработчик Click
        /// </summary>
        public bool UseFont
        {
            set
            {
                if (value)
                {
                    enableAll();//Click работает только при Enabled=true
                    ForEach(c => c.Click += fontSetter);
                }
                else
                {
                    revertEnableAll();
                    ForEach(c => c.Click -= fontSetter);
                }
            }
        }

        /// <summary>
        /// Находит Control и устанавливает Font
        /// </summary>
        public void SetFont(string controlName, Font font)
        {
            FindControl(controlName).ForEach((c) =>
            {
                c.Font = font;
            });
        }

        /// <summary>
        /// Собирает Dictionary для сохранения
        /// </summary>
        public Dictionary<string, Font> DictionaryOfFonts
        {
            get
            {
                var result = new Dictionary<string, Font>();
                ForEach((c) => result.Add(c.Name, c.Font));
                return result;
            }
        }
        #endregion Font

        #region ForeColor
        /// <summary>
        /// Обработчик события.
        /// Выбирает (в ColorDialog) и устанавливает ForeColor (Control)sender
        /// </summary>
        private void foreColorSetter(object sender, EventArgs e)
        {
            var colorDialog = new ColorDialog();
            colorDialog.Color = ((Control)sender).ForeColor;
            if (colorDialog.ShowDialog() != DialogResult.OK) return;
            undo.Add(sender as Control, ((Control)sender).ForeColor, (c, v) => c.ForeColor = (Color)v);
            ((Control)sender).ForeColor = colorDialog.Color;
            if (sender is DataGridView)
                (sender as DataGridView).GridColor = colorDialog.Color;
        }

        /// <summary>
        /// True устанавливет обработчик Click, который выбирает (в ColorDialog) и устанавливает ForeColor
        /// False удаляет обработчик Click
        /// </summary>
        public bool UseForeColor
        {
            set
            {
                if (value)
                {
                    enableAll();
                    ForEach((c) => c.Click += foreColorSetter);
                }
                else
                {
                    revertEnableAll();
                    ForEach((c) => c.Click -= foreColorSetter);

                }
            }
        }

        /// <summary>
        /// Находит Control и устанавливает ForeColor
        /// </summary>
        public void SetForeColor(string controlName, Color color)
        {
            FindControl(controlName).ForEach((c) =>
            {
                c.ForeColor = color;
            });
        }

        /// <summary>
        /// Собирает Dictionary для сохранения
        /// </summary>
        public Dictionary<string, int> DictionaryOfForeColors
        {
            get
            {
                var result = new Dictionary<string, int>();
                ForEach((c) => result.Add(c.Name, c.ForeColor.ToArgb()));
                return result;
            }
        }
        #endregion ForeColor

        #region BackColor
        /// <summary>
        /// Обработчик события.
        /// Выбирает (в ColorDialog) и устанавливает BackgroundColor (Control)sender
        /// </summary>
        private void backColorSetter(object sender, EventArgs e)
        {
            var colorDialog = new ColorDialog();
            colorDialog.Color = (sender is DataGridView) ? (sender as DataGridView).BackgroundColor : (sender as Control).BackColor;
            if (colorDialog.ShowDialog() != DialogResult.OK) return;
            if (sender is DataGridView)
            {
                undo.Add(sender as DataGridView, (sender as DataGridView).BackgroundColor, (c, v) => (c as DataGridView).BackgroundColor = (Color)v);
                (sender as DataGridView).BackgroundColor = colorDialog.Color;
            }
            else
            {
                undo.Add(sender as Control, (sender as Control).BackColor, (c, v) => c.BackColor = (Color)v);
                (sender as Control).BackColor = colorDialog.Color;
            }
        }

        /// <summary>
        /// True устанавливет обработчик Click, который выбирает (в ColorDialog) и устанавливает BackgroundColor
        /// False удаляет обработчик Click
        /// </summary>
        public bool UseBackColor
        {
            set
            {
                if (value)
                {
                    enableAll();
                    ForEach((c) => c.Click += backColorSetter);
                }
                else
                {
                    revertEnableAll();
                    ForEach((c) => c.Click -= backColorSetter);
                }
            }
        }

        /// <summary>
        /// Находит Control и устанавливает BackgroundColor
        /// </summary>
        public void SetBackColor(string controlName, Color color)
        {
            FindControl(controlName).ForEach((c) =>
            {
                if (c is DataGridView)
                    (c as DataGridView).BackgroundColor = color;
                else
                    c.BackColor = color;
            });
        }

        /// <summary>
        /// Собирает Dictionary для сохранения
        /// </summary>
        public Dictionary<string, int> DictionaryOfBackColors
        {
            get
            {
                var result = new Dictionary<string, int>();
                ForEach((c) => result.Add(c.Name, (c is DataGridView) ? (c as DataGridView).BackgroundColor.ToArgb() : (c as Control).BackColor.ToArgb()));
                return result;
            }
        }

        #endregion BackColor

    }


}
