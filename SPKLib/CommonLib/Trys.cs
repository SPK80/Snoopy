using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib.LogHelper;

namespace SPKLib.Trys
{
    public static class Trys
    {
        public static bool TryAct(Action action, string exceptionMassage="")
        {
            try
            {
                action();
                return true;
            }
            catch (Exception ex)
            {
                Log.Write(ex, exceptionMassage);
                return false;
            }
        }
        public static bool TryAct(Func<bool> action, string exceptionMassage = "")
        {
            try
            {
                return action();
            }
            catch (Exception ex)
            {
                Log.Write(ex, exceptionMassage);
                return false;
            }
        }
    }
}
