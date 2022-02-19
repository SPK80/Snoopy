using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ListToolsBox
{	
	internal abstract class AbstractToolSet : ToolStrip
	{
        //protected IEnumerable<ToolStripItem> getItemsByTag(ControllTags tag)
        //{			
        //	foreach (ToolStripItem item in Items)
        //	{
        //		if (item?.Tag != null && (ControllTags)item.Tag == tag)
        //			yield return item;
        //	}
        //}

        //protected ToolStripItem getItemByTag(ControllTags tag)
        //{			
        //	foreach (ToolStripItem item in Items)
        //	{
        //		if (item?.Tag != null && (ControllTags)item.Tag == tag)
        //			return item;
        //	}
        //	return null;
        //}

        #region public

        public new ListToolsBox Parent => base.Parent as ListToolsBox;


        #endregion public

        /// <summary>
        /// Конструктор для потомков
        /// </summary>
        protected AbstractToolSet(string name, ListToolsBox parent)
		{
			AutoSize = true;
			base.Parent = parent;
			Height = Parent.ItemHeight;
			//MinimumSize = Size;
			Name = name;
			int bh = Parent.ItemHeight - 2;
			if (parent.ImagesAutoScaling) ImageScalingSize = new Size(bh, bh);
			if (parent.FontAutoScaling) Font = new Font(Font.Name, bh * 0.5f);

			parent.OnFontAutoScalingChanged += FontAutoScalingChanged;
			parent.OnImagesAutoScalingChanged += ImagesAutoScalingChanged;
			parent.OnItemHeightChanged += ItemHeightChange;
			parent.OnImageListChanged += ImageListChange;
		}

        #region Update

        private void FontAutoScalingChanged(object sender, EventArgs e)
		{
			var newFontAutoScaling = ((ListToolsBox)sender).FontAutoScaling;
			if (!newFontAutoScaling) return;
			int bh = Parent.ItemHeight - 2;
			Font = new Font(Font.Name, bh * 0.5f);
		}

		private void ImagesAutoScalingChanged(object sender, EventArgs e)
		{
			var newImagesAutoScaling = ((ListToolsBox)sender).ImagesAutoScaling;
			if (!newImagesAutoScaling) return;
			int bh = Parent.ItemHeight - 2;
			ImageScalingSize = new Size(bh, bh);
            foreach (ToolStripItem i in Items)
                i.Height = bh;
		}

		private void ImageListChange(object sender, EventArgs e)
		{   
            var newImageList = ((ListToolsBox)sender).ImageList;
            foreach (ToolStripItem i in Items)
            {
                try
                {
                    i.Image = newImageList.Images[i.Name];                    
                }
                catch { i.Image = null; };
                i.DisplayStyle = (i.Image == null) ? ToolStripItemDisplayStyle.Text : ToolStripItemDisplayStyle.Image;
            }
                
        }

		private void ItemHeightChange(object sender, EventArgs e)
		{			
			Height = ((ListToolsBox)sender).ItemHeight;

			FontAutoScalingChanged(sender, e);
			ImagesAutoScalingChanged(sender, e);
		}

		#endregion Update

		#region New
		//protected ToolStripProgressBar NewToolStripProgress()
		//{
		//	return new ToolStripProgressBar
		//	{
		//		Name = this.Name + ".Progress",
		//		Text = this.Name,
		//		Visible = false,
		//		Tag = ControllTags.Progress
		//	};
		//}
				
		//protected ToolStripButton NewToolStripButton(ButtonSettings settings, ControllTags tag, bool checkOnClick=false)
		//{
		//	if (settings == null) return null;
		//	int bh = Parent.ItemHeight - 2;
		//	var result = new ToolStripButton
		//	{
		//		Size = new Size(bh, bh),
		//		Visible = true,
		//		Tag = tag,				
		//		Height = bh,
		//		CheckOnClick = checkOnClick,				
		//		DisplayStyle = settings.Image == null ? ToolStripItemDisplayStyle.Text : ToolStripItemDisplayStyle.Image,
		//	};
		//	ApplySettings(result, settings);
		//	return result;
		//}
		
		#endregion New
	}
	
}
