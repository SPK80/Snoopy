using CommonLib.LogHelper;
using Snoopy.Core;
using Snoopy.Views;
using System;

namespace Snoopy.Presenters
{
    public class MainPresenter //singleton
    {
        private static MainPresenter instanceHolder = null;
        public static MainPresenter Create(IMainForm mainView)
        {
            if (instanceHolder == null)
                return instanceHolder = new MainPresenter(mainView);
            else                
                throw new Exception($"{nameof(instanceHolder.GetType)} уже был инициирован!");
        }

        private readonly IMainForm mainView;

        private SourcesPresenter sourcesPresenter;
        private ResultsPresenter resultsPresenter;
        private HistoryPresenter historyPresenter;
        private SettingsPresenter settingsPresenter;

        //private FileStorge programStorge;
        
        public MainPresenter(IMainForm mainView)
        {
            this.mainView = mainView ?? throw new ArgumentNullException(nameof(mainView)); 
            this.mainView.OnMainLoad += view_OnMainLoad;
            this.mainView.OnMainClose += view_OnMainClosing;
        }

        private const string historyFile = "history.json";
        private const string settingsFile = "settings.json";

        //private FileStorge programStorge = FileStorge.Create(new JSONConverter(true));

        private bool view_OnMainLoad(ISettingsView settingsView)
        {
            //var programStorge = FileStorge.Create(new JSONConverter(true));

            settingsPresenter = SettingsPresenter.Create(settingsView, settingsFile);
            historyPresenter = HistoryPresenter.Create(mainView as IHistory, historyFile);
            resultsPresenter = ResultsPresenter.Create(mainView as IResults, settingsPresenter, historyPresenter.Items);
            sourcesPresenter = SourcesPresenter.Create(mainView as ISourcesView, settingsPresenter, resultsPresenter.Items);

            return true;
        }

        private bool view_OnMainClosing()
        {            
            if (!settingsPresenter.Save(settingsFile))
            {
                Log.Write($"Ошибка сохранения {settingsFile}");
                return false;
            }
            if (!historyPresenter.Save(historyFile))
            {
                Log.Write($"Ошибка сохранения {historyFile}");
                return false;
            }

            //var execHistory = view.GetExecHistory();
            //programStorge.Save(historyFile, execHistory);
            return true;
        }

        ///// <summary>
        ///// Вычисляет пора ли обновлять индекс
        ///// </summary>
        ///// <param name="ind">IIndex</param>
        ///// <param name="span">период обновления</param>
        ///// <returns></returns>
        //private bool view_IndexObsolete(object ind, TimeSpan span)
        //{
        //    if (ind is Index)
        //        return (span != TimeSpan.Zero && (ind as Index).LastUpdate + span <= DateTime.Now);
        //    else
        //        return false;
        //}

    }
}