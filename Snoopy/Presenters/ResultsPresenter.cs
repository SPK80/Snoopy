using System;
using Snoopy.Views;
using Snoopy.Core;
using System.ComponentModel;
using CommonLib.Extentions.String;

namespace Snoopy.Presenters
{
    public sealed class ResultsPresenter //singleton
    {
        private static ResultsPresenter instanceHolder = null;
        public static ResultsPresenter Create(IResults results, SettingsPresenter settings, BindingList<IFoundItem> historyItems)
        {
            if (instanceHolder == null)
                return instanceHolder = new ResultsPresenter(results, settings, historyItems);
            else
                throw new Exception("ResultsPresenter уже был инициирован!");
        }

        private readonly IResults results;
        private readonly BindingList<IFoundItem> execHistorySource;

        public BindingList<IFoundItem> Items { get; private set; }

        private SettingsPresenter settings;

        public ResultsPresenter(IResults results, SettingsPresenter settings, BindingList<IFoundItem> execHistorySource)
        {
            this.results = results ?? throw new ArgumentNullException(nameof(results));
            this.execHistorySource = execHistorySource ?? throw new ArgumentNullException(nameof(execHistorySource));
            this.results.OnExecResult += Results_OnExecResult;
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));            
            results.BindResults(Items = new BindingList<IFoundItem>());
        }
        
        private bool Results_OnExecResult(object result, object[] options)
        {
            if (result is FoundItem)
            {
                var fitem = result as FoundItem;

                bool explore = false;
                bool addHistory = false;
                if (options.Length > 0)
                {
                    if (options[0] is bool)
                        explore = (bool)options[0];
                    if (options.Length > 1 && options[1] is bool)
                        addHistory = (bool)options[1];
                }

                if (explore)
                {
                    fitem.Explore(null);
                    //    (complited) =>
                    //{
                    //string name;
                    //string path;
                    //fitem.Path.SplitByLast(out path, out name, '\\');

                    //var exfitem = new FoundItem(name, path, fitem.Length, fitem.Updated, fitem.Source);
                    //if (complited && addHistory)
                    //    execHistorySource.Add(exfitem);
                    //}
                }
                else
                {
                    fitem.Exec((complited) => 
                    {
                        if (complited && addHistory)
                            execHistorySource.Add(fitem);
                    });
                }
                return true;
            }
            else return false;
        }
    }
}
