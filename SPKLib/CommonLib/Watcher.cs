using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Watcher
{
	//struct NamedAction
	//{
	//	public string Name;
	//	public Action action;

	//	public NamedAction(string name, Action action)
	//	{
	//		Name = name ?? throw new ArgumentNullException(nameof(name));
	//		this.action = action ?? throw new ArgumentNullException(nameof(action));
	//	}
	//}


	public class Watcher 
	{
		public static Watcher watcher= new Watcher();

		/// <summary>
		/// Время выполнения Action code
		/// </summary>
		public static long Elapsed(Action code)
		{
			var watch = System.Diagnostics.Stopwatch.StartNew();
			code();
			watch.Stop();
			return watch.ElapsedMilliseconds;
		}

		private List<WatcherForm> forms = new List<WatcherForm>();

		//private List<NamedAction> actions = new List<NamedAction>();

		private WatcherForm findWatcherForm(string title)
			 => forms.Find((_wf) => _wf.Text == title);
		

		private void Show(string title, string result="", bool onTop=false)
		{
			if (title == "") return;
			var wf = findWatcherForm(title);
			if (wf==null)
			{
				forms.Add(wf = new WatcherForm(title));
				wf.Show();
			}

			if (result != "")
				wf.AddWatchResult(result +  $" [{ DateTime.Now}]");
			if (onTop)
				wf.BringToFront();
		}

		private Action<Action, string> calcElapsed = (code, title) => { };

		public Watcher()
		{
			if (watcher!=null)
				throw new Exception("Watcher already created. Use static field watcher");
		}

		//private NamedAction findAction(string title)
		//	 => actions.Find((na) => na.Name == title);

		public void Close(string title)
		{
			var wf = findWatcherForm(title);
			if (wf == null)
				wf.Close();
		}

		public void Watch(string title, Action code)
		{
			Show(title, $"Elapsed: {Elapsed(code)} ms");
		}

	}
}
