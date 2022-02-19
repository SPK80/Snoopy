using Snoopy.Core;
using Snoopy.Files;
using Snoopy.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Snoopy.Presenters
{
    public sealed class HistoryPresenter //singleton
    {
        private static HistoryPresenter instanceHolder = null;
        public static HistoryPresenter Create(IHistory history, string name)
        {
            if (instanceHolder == null)
                return instanceHolder = new HistoryPresenter(history, name);
            else
                throw new Exception("ExecHistoryPresenter уже был инициирован!");
        }

        private readonly IHistory history;
        public BindingList<IFoundItem> Items { get; private set; }

        public HistoryPresenter(IHistory history, string name)
        {
            this.history = history ?? throw new ArgumentNullException(nameof(history));
            this.history.OnExecHistory += History_OnExecHistory;
            history.BindExecHistory(Items = new BindingList<IFoundItem>());
            
            var list = JsonFile.Read<List<FoundItem>>(name);
            if (list != null)
            {
                list.ForEach(fi => Items.Add(fi));                
            }
            
        }


        public bool Save(string name)
        {
            return JsonFile.Write(name, Items);
        }

        private bool History_OnExecHistory(object result, object[] options)
        {
            if (result is FoundItem)
            {
                var fitem = result as FoundItem;

                bool explore = false;
                if (options.Length > 0)
                {
                    if (options[0] is bool)
                        explore = (bool)options[0];                    
                }
                if (explore)
                    fitem.Explore(null);
                else
                    fitem.Exec(null);
                return true;
            }
            else return false;
        }
    }
}
