using System;
using System.Collections.Generic;

namespace IndFin.Core
{
	enum MsgType:int
	{
		IOError,
		CoreError,
		IURerror,
		Message
	}

	class Logger : ILogger
	{
		private Dictionary<MsgType, string> log;
		private IObjectStorge storge;
		public string Name { get; set; }

		public Logger(string name, IObjectStorge storge)
		{
			this.Name = name;
			this.storge = storge;
			if (storge != null) 
				Load();

			if (log == null)
				log = new Dictionary<MsgType, string>();
		}

		
		public void Add(MsgType msgType, string record)
		{
			log.Add(msgType, record);
		}

		public void Clear()
		{
			log.Clear();
			Save();
		}

		public void Load()
		{
			log=storge.Load<Dictionary<MsgType, string>>(Name);
		}

		public void Save()
		{
			storge.Save(Name, log);
		}

	}
}
