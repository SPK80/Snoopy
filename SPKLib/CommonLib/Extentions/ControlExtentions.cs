using System.Linq;
using System.Windows.Forms;

namespace CommonLib.Extentions
{
    public static class ControlExtentions
    {
        public static bool SetEnabled(this bool value, params Control[] controls)
        {
            foreach (var c in controls)
                c.Enabled = value;
            return value;
        }

        public static bool SetVisible(this bool value, params Control[] controls)
        {
            foreach (var c in controls)
                c.Visible = value;
            return value;
        }

        public static Control Find<T>(this Control control, string name)
            where T:Control
        {
            foreach(var c in control.Controls.OfType<T>())
            {
                if (c.Name == name)
                    return c;
            }
            foreach (Control c in control.Controls)
            {
                if (c.Find<T>(name) != null)
                    return c;
            }
            return null;
        }
    }
}
