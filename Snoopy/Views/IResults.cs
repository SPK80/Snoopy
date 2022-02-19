using Snoopy.Core;
using System;
using System.Collections;
using System.ComponentModel;

namespace Snoopy.Views
{
    public interface IResults
    {        
        event Func<object, object[], bool> OnExecResult;
        void BindResults(BindingList<IFoundItem> resultsSource);

    }
}
