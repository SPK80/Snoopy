using System;
using System.Diagnostics;
using System.IO;
using CommonLib.LogHelper;

namespace Snoopy.Core
{

    public static class FileExecuter
    {
        public static bool ShowInExplorer(string path)
        {
            var PrFolder = new Process();
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Normal;
            psi.FileName = "explorer";
            psi.Arguments = $"/n, /select,\"{path}\"";
            PrFolder.StartInfo = psi;
            bool result = Directory.Exists(path) || File.Exists(path);
            if (!result) return false;
            try
            {
                PrFolder.Start();
            }
            //catch (System.ComponentModel.Win32Exception ex)
            //{
            //	Log.Write(ex);
            //}
            //catch (ObjectDisposedException ex)
            //{
            //	Log.Write(ex);
            //}
            catch (Exception ex)
            {
                Log.Write(ex, "ShowInExplorer");
                result = false;
            }
            finally
            {
                PrFolder.Close();
            }
            return result;
        }

        public static bool ExecFile(string path)
        {
            var PrFile = new Process();
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.CreateNoWindow = false;
            psi.WindowStyle = ProcessWindowStyle.Normal;
            psi.FileName = path;
            PrFile.StartInfo = psi;
            bool result = Directory.Exists(path) || File.Exists(path);
            if (!result) return false;
            try
            {
                PrFile.Start();
            }
            //catch (System.ComponentModel.Win32Exception ex)
            //{
            //	Log.Write(ex);
            //}
            //catch (ObjectDisposedException ex)
            //{
            //	Log.Write(ex);
            //}
            catch (Exception ex)
            {
                Log.Write(ex, "ExecFile");
                result = false;
            }
            finally
            {
                PrFile.Close();
            }
            return result;
        }
    }
}