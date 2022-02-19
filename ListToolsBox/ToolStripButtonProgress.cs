using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ListToolsBox
{
    //public enum ViewMode { Min, AutoSize, Max }

    internal class ToolStripButtonProgress : ToolStripButton
	{
		public ToolStripButtonProgress() : base()
		{
			Value = -1;
		}

		public Brush ScrollBrush = Brushes.Red;

		private BackgroundWorker worker = new BackgroundWorker();

		private int _value = -1;
		private int prev_value = -1;
		private int green = 0;
		public bool AnimateProgress { get; set; } = true;
		public int Value
		{			
			get => _value;
			set
			{
				if (value>=0 && !worker.IsBusy && AnimateProgress)  //start
				{
					worker.WorkerSupportsCancellation = true;
					worker.DoWork += (s, e) =>
					{
						//green = 0;
						bool fw = true;
						while (_value >= 0)
						{
							if (worker.CancellationPending)
							{
								_value = -1;
								Invalidate();
								e.Cancel = true;
								return;
							}

							if (fw)
								green += 2;
							else
								green -= 2;
							
							if (green >= 254 || green <= 1)
								fw = !fw;
														
							ScrollBrush = new SolidBrush(Color.FromArgb(255, green, 0));

                            Action action =()=>Invalidate();
							if (Parent.InvokeRequired)
								Parent.Invoke(action);
							else
								action();
							
							Thread.Sleep(10);
						}
					};
					worker.RunWorkerAsync();
				}
				else if (value < 0 && worker.IsBusy)
				{
					worker.CancelAsync();
				}

				prev_value = _value;
				if (value > Maximum)
				{
					if (Cyclicity) _value = Minnimum; else _value = Maximum;
				}
				else if (value>=0 && value < Minnimum)
				{
					if (Cyclicity) _value = Maximum; else _value = Minnimum;
				}
				else
				{					
					_value = value;
				}					

				Invalidate();
			}
		}
		public int Maximum { get; set; } = 100;
		public int Minnimum { get; set; } = 0;

		public bool Cyclicity { get; set; } = false;

		protected override void OnPaint(PaintEventArgs e)
		{
				if (Value > 0 && !Selected)
				{
					int x = ContentRectangle.Left + (ContentRectangle.Right - ContentRectangle.Left) * Value / Maximum;
					e.Graphics.FillRectangle(ScrollBrush, ContentRectangle.Left, ContentRectangle.Top, x, ContentRectangle.Bottom);
				}			
				base.OnPaint(e);
		}
    }
}
