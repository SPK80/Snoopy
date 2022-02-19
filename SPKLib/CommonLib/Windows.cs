using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.NativeMethods
{
	public static class NativeMethods
	{

		const int WM_GETTEXT = 0x000D;
		const int WM_GETTEXTLENGTH = 0x000E;
		const int WM_SETTEXT = 0x000C;

		/// <summary>
		/// Win32 API Imports
		/// </summary>
		/// 
		const string user32dll = "user32.dll";

		[DllImport(user32dll, SetLastError = true)]
		public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, IntPtr windowTitle);

		[DllImport(user32dll, CharSet = CharSet.Auto)]
		public static extern bool SendMessage(IntPtr hWnd, uint Msg, int wParam, StringBuilder lParam);

		[DllImport(user32dll, SetLastError = true)]
		public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wparam, int lparam);

		[DllImport(user32dll)]
		public static extern
			bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

		[DllImport(user32dll)]
		public static extern
			bool SetForegroundWindow(IntPtr hWnd);

		[DllImport(user32dll)]
		public static extern
			bool IsIconic(IntPtr hWnd);

		[DllImport(user32dll)]
		public static extern
			bool IsZoomed(IntPtr hWnd);

		[DllImport(user32dll)]
		public static extern
			IntPtr GetForegroundWindow();

		[DllImport(user32dll)]
		public static extern
			IntPtr GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);

		[DllImport(user32dll)]
		public static extern
			IntPtr AttachThreadInput(IntPtr idAttach, IntPtr idAttachTo, int fAttach);




		//[DllImport(user32dll)]
		//private static extern int GetWindowText(IntPtr IntPtr, StringBuilder lpString, int nMaxCount);

		//[DllImport(user32dll)]
		//[return: MarshalAs(UnmanagedType.Bool)]
		//static extern bool EnumChildWindows(IntPtr hwndParent, EnumWindowProc lpEnumFunc, string winName);
		//public delegate bool EnumWindowProc(IntPtr hWnd, string winName);

		//public static bool FindWindowByTitle(IntPtr hwnd, string winName)
		//{
		//	StringBuilder str = new StringBuilder(200);
		//	GetWindowText(hwnd, str, 200);
		//	if (!str.ToString().Contains(winName))
		//		return true;
		//	return false;
		//}

	}
}
