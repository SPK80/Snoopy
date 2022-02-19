using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IndFin.UI
{
	sealed class IndListBox: ListBox
	{
		//вспомогательные переменные для отрисовки
		int x, y, itemWidth, itemHeight;		

		public IndListBox()
		{
			DrawMode = DrawMode.OwnerDrawVariable;			
		}

		//protected override void OnSizeChanged(EventArgs e)
		//{
		//	//вызываем обновление компонента
		//	Refresh();
		//	base.OnSizeChanged(e);
		//}

		protected override void OnMeasureItem(MeasureItemEventArgs e)
		{
			//если это элемент
			if (e.Index > -1)
			{
				//задаем высоту
				e.ItemHeight = 30;
				//ширину
				e.ItemWidth = Width;
			}
		}

		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			//e - элемент, с которым мы дальше и работаем
			//если текущего элемента нет или в списке нет вообще элементов, выходим из метода
			if (e.Index <= -1 || this.Items.Count == 0)
				return;

			//получаем текст элемента
			string s = Items[e.Index].ToString();

			//формат строки для рисования текста
			StringFormat sf = new StringFormat();
			//формат выставляем по центру
			sf.Alignment = StringAlignment.Near;
			sf.FormatFlags = StringFormatFlags.NoWrap;

			//создаем обычную кисть с заданным цветом
			Brush textBrush = new SolidBrush(Color.Black);
			Brush bgBrush = new SolidBrush(SystemColors.Control);

			//определяем координаты элемента в списке
			//т.к. для каждого элемента они разные
			x = e.Bounds.X;
			y = e.Bounds.Y;

			//также определяем его ширину и высоту
			itemWidth = e.Bounds.Width;
			itemHeight = e.Bounds.Height;

			if ((e.State & DrawItemState.Focus) == DrawItemState.Focus)//если активный
			{
				//рисуем выбранный элемент
				e.Graphics.FillRectangle(new SolidBrush(SystemColors.Info), x + 2, y + 2,
					itemWidth - 4, itemHeight - 4);

				//рисуем текст элемента
				e.Graphics.DrawString(s, Font, new SolidBrush(Color.Black),
					new Rectangle(5, y + 10, itemWidth, 16), sf);

				//рисуем границы элемента
				e.Graphics.DrawLine(new Pen(Color.White), x + 1, y + 1, itemWidth, y + 1);
				e.Graphics.DrawLine(new Pen(Color.White), x + 1, y + 1, x + 1, y + itemHeight - 1);
				e.Graphics.DrawLine(new Pen(SystemColors.ControlDarkDark), itemWidth - 1, y + 1, itemWidth - 1, y + itemHeight - 1);
				e.Graphics.DrawLine(new Pen(Color.Gray), itemWidth - 2, y + 2, itemWidth - 2, y + itemHeight - 2);
				e.Graphics.DrawLine(new Pen(SystemColors.ControlDarkDark), x + 1, y + itemHeight - 1, itemWidth - 1, y + itemHeight - 1);
				e.Graphics.DrawLine(new Pen(Color.Gray), x + 2, y + itemHeight - 2, itemWidth - 2, y + itemHeight - 2);
			}
			else // если не активный
			{
				//заполняем прямоугольник выбранным цветом
				//this.BackColor.
				//e.Graphics.FillRectangle
				e.Graphics.FillRectangle(bgBrush, x, y, itemWidth, itemHeight + 1);

				//пишем текст
				e.Graphics.DrawString(s, Font, textBrush,
					new Rectangle(5, y + 10, itemWidth, 16), sf);

				//рисуем границы элемента
				//e.Graphics.DrawLine(new Pen(Color.White), x + 1, y + 1, itemWidth, y + 1);
				//e.Graphics.DrawLine(new Pen(Color.White), x + 1, y + 1, x + 1, y + itemHeight - 1);
				//e.Graphics.DrawLine(new Pen(SystemColors.ControlDarkDark), itemWidth - 1, y + 1, itemWidth - 1, y + itemHeight - 1);
				//e.Graphics.DrawLine(new Pen(Color.Gray), itemWidth - 2, y + 2, itemWidth - 2, y + itemHeight - 2);
				//e.Graphics.DrawLine(new Pen(SystemColors.ControlDarkDark), x + 1, y + itemHeight - 1, itemWidth - 1, y + itemHeight - 1);
				//e.Graphics.DrawLine(new Pen(Color.Gray), x + 2, y + itemHeight - 2, itemWidth - 2, y + itemHeight - 2);
			}			
		}

	}
}
