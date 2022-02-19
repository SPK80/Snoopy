using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snoopy.Views
{
	partial class NewSourceForm : Form
	{
		public NewSourceForm()
		{
			InitializeComponent();			
		}
		
		//private void InitProcessingFields(bool initVal = true)
		//{
		//	cblProcessingFields.Items.Clear();

			//cblProcessingFields.Items.Add(nameof(IFoundItem.Name), initVal);
			//cblProcessingFields.Items.Add(nameof(IFoundItem.Length), initVal);
			//cblProcessingFields.Items.Add(nameof(IFoundItem.Path), initVal);
			//cblProcessingFields.Items.Add(nameof(IFoundItem.Source), initVal);
			//cblProcessingFields.Items.Add(FieldNames.CreationTime, initVal);
			//cblProcessingFields.Items.Add(nameof(IFoundItem.Updated), initVal);
			//cblProcessingFields.Items.Add(FieldNames.LastAccessTime, initVal);

		//}

        //public void BindProcessingFields(IProcessingFields processingFields)
        //{
        //    cblProcessingFields.DataSource = processingFields;
        //}

  //      public void InitProcessingFields(IEnumerable<string> fieldNames)
		//{
		//	if (fieldNames == null || fieldNames.Count() < 1)
		//	{
		//		InitProcessingFields(true);				
		//	}
  //          else
  //          {                
  //              InitProcessingFields(false);
  //              foreach (var fieldName in fieldNames)
  //              {
  //                  var i = cblProcessingFields.FindStringExact(fieldName);
  //                  if (i >= 0)
  //                      cblProcessingFields.SetItemChecked(i, true);
  //              }
  //          }			
		//}

		private void cblProcessingFields_ItemCheck(object sender, ItemCheckEventArgs e)
		{
			if (e.Index == 0 && e.NewValue == CheckState.Unchecked) //do NOT let Uncheck Name!
				e.NewValue = CheckState.Checked;			
		}

		//public event EventHandler<ProcessingFieldsEventArgs> ProcessingFieldsChanged;

		private void ScannerBox_FormClosing(object sender, FormClosingEventArgs e)
		{
			//передаём результаты наружу
			//ProcessingFieldsChanged?.Invoke(this, new ProcessingFieldsEventArgs(cblProcessingFields.CheckedItems.Cast<string>().ToList()));			
		}

		//public List<string> CheckedFields
		//{
		//	get => cblProcessingFields.CheckedItems.Cast<string>().ToList();
		//}
		
		public string SelectedPath
		{
			get => tbPath.Text;
			set => tbPath.Text=value;
		}

		private void tbPath_DoubleClick(object sender, EventArgs e)
		{
			var folderDlg = new FolderBrowserDialog();
			folderDlg.SelectedPath = SelectedPath;
			var dialogResult = folderDlg.ShowDialog();
			if (dialogResult == DialogResult.OK)
			{
				tbPath.Text = folderDlg.SelectedPath;
			}
			bConfirm.Enabled = SelectedPath != "";
		}

		private void ScannerBox_Load(object sender, EventArgs e)
		{
			bConfirm.Enabled = SelectedPath != "";
		}

		private void bShowPathDialog_Click(object sender, EventArgs e)
		{
			tbPath_DoubleClick(sender, e);
		}

		private void tbPath_TextChanged(object sender, EventArgs e)
		{
			bConfirm.Enabled = SelectedPath != "";
		}
        
	}

	//public class ProcessingFieldsEventArgs : EventArgs
	//{
	//	public List<string> ProcessingFields;

	//	public ProcessingFieldsEventArgs(List<string> processingFields)
	//	{
	//		ProcessingFields = processingFields ?? throw new ArgumentNullException(nameof(processingFields));
	//	}
	//}
}
