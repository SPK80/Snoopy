using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.Text;

namespace IndFin.Config
{
	//public class BindedObject
	//{
	//	private Func<Object> getter = () => default(Object);
	//	private Action<Object> setter = (x) => { };

	//	public BindedObject(Func<object> getter, Action<object> setter)
	//	{
	//		this.getter = getter;
	//		this.setter = setter;
	//	}

	//	public void Bind(Func<Object> getter, Action<Object> setter)
	//	{
	//		this.getter = getter;
	//		this.setter = setter;
	//	}
	//}


	public class BindedValue<T>
	{
		public T Value { get => getter(); set => setter(value); }
		private Func<T> getter = () => default(T);
		private Action<T> setter = (x) => { };
		public void Bind(Func<T> getter, Action<T> setter)
		{
			this.getter = getter;
			this.setter = setter;
		}
		public BindedValue(Func<T> getter, Action<T> setter)
		{
			this.getter = getter;
			this.setter = setter;
		}
	}

	public class ListSection : List<Binder<object>>
	{
		public string Name { get; protected set; }

		public ListSection(string name) => Name = name;



		//public void Bind(string key, Func<object> getter, Action<object> setter)
		//{
		//	foreach (var kv in this)
		//		if (kv.Key == key)
		//		{ kv.Value.Bind(getter, setter); return; }
		//	this.Add(key, new BindedObject(getter, setter));

		//}

	}

	//KeyValuePair<string, object>
	//[JsonObject]
	public class Section_<TVal> : IEnumerable<KeyValuePair<string, object>>
	{
		//	public new IEnumerator<dynamic> GetEnumerator()
		//	{
		//		foreach (var item in Items)
		//		{
		//			yield return new { item.Key, item.Value };
		//		}
		//	}
		//}

		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			foreach (var item in data)
			{
				yield return new KeyValuePair<string, object>(item.Key, item.Value);
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		private SettingCollection<TVal> data;

		public string Name { get; set; }

		public Section_()
		{
			data = new SettingCollection<TVal>();
		}

		public TVal this[string key]
		{
			get
			{
				var kv = data.Find(key);
				if (kv == null)
					return default(TVal);
				else
					return kv.Value;
			}
			set
			{
				var kv = data.Find(key);
				if (kv != null)
					kv.Value = value;
			}
		}

		public void Bind(string key, Func<TVal> getter, Action<TVal> setter)
		{
			var set = data.Find(key);
			if (set != null)
				set.Bind(getter, setter);
			else
				data.Add(new Binder<TVal>(key, getter, setter));
		}

		internal class SettingCollection<T> : Collection<Binder<T>>
		{
			// Вставка с проверкой уникальности item
			protected override void InsertItem(int index, Binder<T> item)
			{
				if (!this.Contains(item))
				{
					base.InsertItem(index, item);
				}
			}

			public Binder<T> Find(string key)
			{
				Binder<T> result = null;
				foreach (var it in this)
					if (it.Key == key)
						return it;
				return result;
			}
		}

	}
}
