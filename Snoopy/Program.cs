//#define Release

using System;
using System.Reflection;
using System.Windows.Forms;
using Snoopy.Presenters;
using Snoopy.Views;

namespace Snoopy
{
	internal static class Program
	{
		
		/// <summary>
		/// Главная точка входа для приложения.
		/// </summary>
		[STAThread]
		private static void Main()
		{

#if Release
			AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
#endif
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			var mainForm = new MainForm();
			var mainPresenter = new MainPresenter(mainForm);
            
			Application.Run(mainForm);
		}

#if Release
		static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			if (args.Name.Contains("Newtonsoft.Json"))
				return Assembly.Load(Properties.Resources.Newtonsoft_Json);

			if (args.Name.Contains("SPKLib"))			
				return Assembly.Load(Properties.Resources.SPKLib);			
			else return null;
		}
#endif
    }
}