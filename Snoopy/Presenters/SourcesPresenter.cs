using CommonLib.LogHelper;
using Snoopy.Core;
using Snoopy.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
 

namespace Snoopy.Presenters
{
    public sealed class SourcesPresenter //singleton
    {
        private static SourcesPresenter instanceHolder = null;
        public static SourcesPresenter Create(ISourcesView viewSources, SettingsPresenter settings, BindingList<IFoundItem> foundItems)
        {
            if (instanceHolder == null)
                return instanceHolder = new SourcesPresenter(viewSources, settings, foundItems);
            else
                throw new Exception("SourcesPresenter уже был инициирован!");
        }

        private readonly ISourcesView viewSources;
        private readonly SettingsPresenter settingsPresenter;
        //private readonly FileStorge programStorge;

        private readonly Action<object, int, string> processChanged;
        private readonly Action<object> processDone;
        private readonly Action<object> processCancelled;
        private readonly Action<object, IEnumerable<object>> gotResults;

        private Dictionary<string, bool> processingFields 
        {
            get => settingsPresenter?.GetSetting<Dictionary<string, bool>>(SettingsPresenter.ViewCatalogName, nameof(processingFields));                
            
            set => settingsPresenter.SetSetting(SettingsPresenter.ViewCatalogName, nameof(processingFields), value);            
        }

        private List<string> processingFieldsList =>
            processingFields.Where(pf => pf.Value).Select(pf => pf.Key).ToList();

        //private bool sourcesExists(string path)
        //{

        //}

        private SourcesPresenter(ISourcesView viewSources, SettingsPresenter settingsPresenter, BindingList<IFoundItem> resultItems)
        {
            this.viewSources = viewSources ?? throw new ArgumentNullException(nameof(viewSources));
            this.settingsPresenter = settingsPresenter ?? throw new ArgumentNullException(nameof(settingsPresenter));
            //this.programStorge = programStorge ?? throw new ArgumentNullException(nameof(programStorge));
            
            this.viewSources.OnAddSource += Sources_OnAddSource;
            this.viewSources.OnCancelProgress += Sources_OnCancelProgress;
            this.viewSources.OnFindInSource += Sources_OnFindInSource;
            this.viewSources.OnNewSource += Sources_OnNewSource;
            this.viewSources.OnRemoveSource += Sources_OnRemoveSource;
            this.viewSources.OnRenameSource += Sources_OnRenameSource;
            this.viewSources.OnUpdateSource += Sources_OnUpdateSource;
            
            processChanged = viewSources.SetSourceProcess;
            processDone = (s) => viewSources.SetSourceProcess(s, -1, "завершено");
            processCancelled = (s) => viewSources.SetSourceProcess(s, -1, "отменено");
            gotResults = (s, results) =>
            {
                foreach (var result in results)
                {
                    resultItems.Add(result as IFoundItem);
                }
            };

            processingFields = new Dictionary<string, bool> //задаём значения по умолчанию
            {
                [nameof(IFoundItem.Name)] = true,
                [nameof(IFoundItem.Length)] = true,
                [nameof(IFoundItem.Path)] = true,
                //[nameof(IFoundItem.SourceName)] = true,
                //[nameof(IFoundItem.SourcePath)] = true,
                [nameof(IFoundItem.Updated)] = true,
            };

            if (sourcesList != null)
                sourcesList.ForEach(name => Sources_OnAddSource(name));

        }

        private const string sourcesListName = "Sources";

        private List<string> sourcesList
        {
            get => settingsPresenter.GetSetting(SettingsPresenter.CoreCatalogName, sourcesListName, typeof(List<string>)) as List<string>;
            //set => settingsPresenter.SetSetting(SettingsPresenter.ViewCatalogName, sourcesListName, value);
        }

        //private void addSourcesList(string path)
        //{
        //    //var newsourcesList = sourcesList;
        //    sourcesList.Add(path);
        //    //sourcesList = newsourcesList;
        //}

        //public List<string> Paths => (viewSources.Items.Cast<Source>()).Select(s=>s.Path).ToList();

        private bool Sources_OnUpdateSource(object source)
        {            
            try
            {
                if (source is Index)
                {
                    (source as Index).Update(processingFieldsList);
                }   
                return true;
            }
            catch (Exception ex)
            {
                Log.Write(ex, $"Sources_OnAddSource { source.ToString() }");
                return false;
            }
            //throw new NotImplementedException();
        }

        private bool Sources_OnRenameSource(object source, string newName)
        {
            try
            {
                if (source is Index)
                    (source as Index).ReName(newName);
                return true;
            }
            catch (Exception ex)
            {
                Log.Write(ex, $"Sources_OnRenameSource {source.ToString()} , {newName}");
                return false;
            }
            //throw new NotImplementedException();
        }

        private bool Sources_OnFindInSource(string what, object source, object[] options)
        {
            try
            {                
                if (source is Source)
                    (source as Source).Find(what, options);
                return true;
            }
            catch (Exception ex)
            {
                Log.Write(ex, $"Sources_OnFindInSource {what} , {source.ToString()}");
                return false;
            }
            //throw new NotImplementedException();
        }

        private bool Sources_OnCancelProgress(object source)
        {
            try
            {
                if (source is Source)
                    (source as Source).CancelProcess();
                return true;
            }
            catch (Exception ex)
            {
                Log.Write(ex, $"Sources_OnCancelProgress {source.ToString()}");
                return false;
            }
            //throw new NotImplementedException();
        }

        private bool Sources_OnNewSource(string scanPath, object[] options)
        {
            Source newSource = null;
            bool isDir = false;
            if (options.Count() > 0 && options[0] is bool)
                isDir = (bool)options[0];
            else
                return false;
            try
            {
                if (isDir)
                //switch (Source.AutoDetectType(path))
                //{
                //    case SourceTypes.Directory:
                {
                    //var storge = FileStorge.Create(new JSONConverter());

                    Action<object> processNewIndexDone = (s) => 
                    {
                        if (s as Index == null) return;
                        if (!sourcesList.Contains(((Index)s).Path))
                            sourcesList.Add(((Index)s).Path);
                    } + processDone;

                    viewSources.AddSource(newSource = Index.ScanNew("", scanPath, AppDomain.CurrentDomain.BaseDirectory, 
                        processingFieldsList, processChanged, processNewIndexDone, processCancelled, gotResults));
                    

                    return newSource != null;
                }
                else
                {
                    throw new NotImplementedException();
                }
                //    default: return false;
                //}
            }
            catch (Exception ex)
            {
                Log.Write(ex, $"Sources_OnNewSource { scanPath }");
                return false;
            }            
        }

        private bool Sources_OnAddSource(string path)
        {
            Source newSource = null;
            //FileStorge storge = null;
            try
            {
                //if (programStorge.IsHere(path))
                    //storge = programStorge;
                //else
                //    storge = FileStorge.Create(new JSONConverter());                

                //if (!FileStorge.Exists(path))
                //{
                //    sourcesList.Remove(path);
                //    return false;
                //}

                    switch (Source.AutoDetectType(path))
                {
                    case SourceTypes.Index:
                        newSource = new Index(path, processChanged, processDone, processCancelled, gotResults);
                        viewSources.AddSource(newSource);
                        break;
                    case SourceTypes.Excel:
                        viewSources.AddSource(newSource = new ExcelBook("", path));
                        break;
                    //case SourceTypes.Doc:
                    //    break;
                    //case SourceTypes.Text:
                    //    break;
                    default: return false;
                }
                if (newSource!=null)
                {
                    //добавляем новый путь в список
                    if (!sourcesList.Contains(newSource.Path))
                        sourcesList.Add(newSource.Path);
                    return true;
                }
                else return false;

            }
            catch (Exception ex)
            {
                Log.Write(ex, $"Sources_OnAddSource { path }");
                return false;
            }
            //throw new NotImplementedException();
            //var name = FileStorge.FileName(path, true);
            //var indTag = new Index(name);
            //view.AddItemToolSet(name, indTag, span, IndexTools.ToolSetMode.Normal);
            //view.Completed(indTag, name);
        }

        private bool Sources_OnRemoveSource(object source)
        {
            try
            {
                viewSources.RemoveSource(source);
                sourcesList.Remove((source as Source).Path);
                return true;
            }
            catch (Exception ex)
            {
                Log.Write(ex, $"Sources_OnRemoveSource { source.ToString() }");
                return false;
            }
            //throw new NotImplementedException();
        }
    }
}