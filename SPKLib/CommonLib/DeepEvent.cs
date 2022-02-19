using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonLib
{
	public static class DeepEvent
	{
        /// <summary>
        /// Устанавливает событие Click включая все дочерние Controls, рекурсивно
        /// </summary>
        public static void DeepClick(this Control control, EventHandler click, bool Add_Remove)
		{
			if (Add_Remove)
				control.Click += click;
			else
				control.Click -= click;
			foreach (var c in control.Controls)
			{
				if (c is Control)
					(c as Control).DeepClick(click, Add_Remove);
			}
		}
		
	}
}
