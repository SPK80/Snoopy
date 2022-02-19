using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DialogsEx
{
    public class FolderBrowserDialogEx: CommonDialog
	{
		/// <summary>
		/// Конструктор
		/// </summary>		
		public FolderBrowserDialogEx(IContainer container)
		{
			Reset();
		}

		FolderBrowserDialog folderBrowserDialog;
		string SelectedPath
		{
			get => folderBrowserDialog?.SelectedPath??"";
			set
			{
				if (folderBrowserDialog == null)
					folderBrowserDialog.SelectedPath = value;
			}
		}
		
		public override void Reset()
		{
			//throw new NotImplementedException();
			folderBrowserDialog = new FolderBrowserDialog();			
		}

		protected override bool RunDialog(IntPtr hwndOwner)
		{
			var result= folderBrowserDialog.ShowDialog();
			return result == DialogResult.OK || result == DialogResult.Yes;
		}
	}
}
