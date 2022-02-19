using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snoopy.Views
{
	/// <summary>
	/// Хранит информацию для выполнения Undo
	/// </summary>
	internal class UndoItem
	{
		private Action<Control, object> setter;
		private Control control;
		private object value;

		public void Undo() => setter(control, value);
		public UndoItem(Control control, object value, Action<Control, object> setter)
		{
			this.setter = setter ?? throw new ArgumentNullException(nameof(setter));
			this.control = control ?? throw new ArgumentNullException(nameof(control));
			this.value = value ?? throw new ArgumentNullException(nameof(value));
		}
	}

	/// <summary>
	/// Стэк UndoItem
	/// </summary>
	public class UniUndoStack
	{
		Stack<UndoItem> stack = new Stack<UndoItem>(100);
		
		/// <param name="control"> содержит сохраняемое поле передаваемое со значением value</param>
		/// <param name="value">указатель на сохраняемое значение</param>
		/// <param name="setter">делегат для восстановления значения</param>
		public void Add(Control control, object value, Action<Control, object> setter)
		{
			stack.Push(new UndoItem(control, value, setter));
		}

		public void Undo()
		{
			if (stack.Count < 1) return;
			stack.Pop().Undo();
		}

		public void UndoAll()
		{
			while (stack.Count>0)
			{
				Undo();
			}
		}

		public void Clear()
		{
			stack.Clear();
		}
	}
}
