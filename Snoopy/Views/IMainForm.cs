using System;
using System.Collections.Generic;

namespace Snoopy.Views
{
    public interface IView
    {

        void ShowMessage(string message, object indTag);
        bool Confirm(string message, string caption);

    }

    public interface IMainForm:IView
    {
        event Func<ISettingsView, bool> OnMainLoad;
        event Func<bool> OnMainClose;

    }
}
