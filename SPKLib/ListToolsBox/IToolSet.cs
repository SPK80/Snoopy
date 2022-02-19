using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListToolsBox
{
    public interface IToolSet
    {
        string Name { get; }
        bool Selected { get; set; }
        int ProgressValue { get; set; }
        string ProgressText { get; set; }

        event EventHandler OnCancelProgress;
        event EventHandler OnSelect;
        event EventHandler OnDeSelect;
        event EventHandler OnRename;
        event EventHandler OnStartProgress;
        event EventHandler OnStopProgress;


    }
}
