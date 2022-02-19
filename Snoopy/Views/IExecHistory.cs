using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Snoopy.Views
{
    public interface IHistory
    {
        event Func<object, object[], bool> OnExecHistory;
        //void AddExecHistory(object dfResult);        
        void BindExecHistory(BindingList<IFoundItem> execHistorySource);
    }
}
