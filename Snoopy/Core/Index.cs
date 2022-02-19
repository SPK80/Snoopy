using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Snoopy.Core.DFN;
using CommonLib.Extentions.String;
using System.IO;
using Snoopy.Files;
using CommonLib.LogHelper;

namespace Snoopy.Core
{
    public class Index : Source
    {
        //private string name;
        public override string Name
        {
            //get
            //{
            //    if (name == "") //при новом сканировании
            //        return (Tree == null) ? "" : FileStorge.FileName(Tree.RootPath, true);
            //    else
            //        return name; 
            //}
            //protected set { name = value; }
            get => System.IO.Path.GetFileNameWithoutExtension(Path);

            protected set {
                //throw new Exception("Нельзя присвоить значение "+ nameof(Name));
            }
        }
        
        public override string Path
        {
            get ;
            //=> (Name!="") ? fileStorge.Path + Name + Extantion : "";
            protected set;
        }

        public override string Extantion { get; protected set; }

        public DirsFilesTree Tree { get; private set; }

        public DateTime LastUpdate =>
            Path!="" ? File.GetLastWriteTime(Path): DateTime.Now;
            //Path!="" ? FileStorge.LastWriteTime(Path): DateTime.Now;

        public string RootPath { get => Tree?.RootPath ?? Name; } //?????

        public int Count { get => Tree?.RootNode.Cast<DFNode>().Count() ?? 0; }

        public Index(string path, 
            //FileStorge fileStorge,
            Action<object, int, string> processChanged,
            Action<object> processDone, 
            Action<object> processCancelled,
            Action<object, IEnumerable<object>> gotResults
            ) : base("", "", ".ind")
        {
            ProcessCancelled = processCancelled;
            ProcessChanged = processChanged;
            ProcessDone = processDone;
            GotResults = gotResults;
            //this.fileStorge = fileStorge;

            Path = path;
        }

        public override void CancelProcess()
        {
            cancellationTokenSource.Cancel();
        }

        public static Index ScanNew(string name, string scanPath,
            string savePath,
            //FileStorge fileStorge,
            List<string> processingFields,
            Action<object, int, string> processChanged, 
            Action<object> processDone, 
            Action<object> processCancelled,
            Action<object, IEnumerable<object>> gotResults)
        {
            var newIndex = new Index(name, 
                //fileStorge, 
                processChanged, processDone, processCancelled, gotResults);
            newIndex.scan(scanPath, savePath, processingFields);
            
            return newIndex;            
        }

        public void Update(List<string> processingFields)
        {
            //if (fileStorge == null) return;

            scan("", "", processingFields);

        }

        public bool Loaded => Tree != null;        

        //private FileStorge fileStorge=null;
        
        
        private CancellationTokenSource cancellationTokenSource;
                
        private async void scan(string scanPath, string savePath="", List<string> processingFields = null)
        {
            //if (fileStorge == null) return;
            cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;
            Task<DirsFilesTree> treeTask = null;
            Task<bool> saveTask = null;
            try
            {
                var rootPath_ = scanPath;
                if (rootPath_ == "") //путь не задан => обновление 
                {
                    if (!Loaded) //загружаем...
                    {
                        treeTask = Task<DirsFilesTree>.Factory.StartNew(() =>
                        //fileStorge.Load<DirsFilesTree>(Path, this, token, ProcessChanged), token);
                        JsonFile.Read<DirsFilesTree>(Path));
                        await treeTask;
                        if (treeTask.Result == null) throw new Exception("Ошибка загрузки");
                        Tree = treeTask.Result;
                    }
                    rootPath_ = this.RootPath;//при ибновлении используем текущий путь
                }
                //сканируем...
                treeTask = Task<DirsFilesTree>.Factory.StartNew(() =>
                DFScanner.ScanNew(rootPath_, this, processingFields, token, ProcessChanged), token);
                await treeTask;
                if (treeTask.Result == null) throw new Exception("Ошибка сканирования");

                Tree = treeTask.Result;
                if (scanPath != "")
                {
                    //Name = System.IO.Path.GetFileName(Tree.RootPath); //задаём имя по имени корневой директории
                    //Name = FileStorge.FileName(Tree.RootPath); //задаём имя по имени корневой директории
                    if  (savePath!="")
                    {
                        Path = savePath;
                        if (System.IO.Path.GetFileName(Path) =="")
                            Path = Path + System.IO.Path.GetFileName(Tree.RootPath) + Extantion;
                    }
                }

                saveTask = Task<bool>.Factory.StartNew(() =>
                //fileStorge.Save(Path, Tree), token);
                JsonFile.Write(Path, Tree));
                await saveTask;
                if (saveTask.Result)
                    ProcessDone?.Invoke(this);
                else
                    ProcessCancelled?.Invoke(this);
            }
            catch (OperationCanceledException oce)
            {
                ProcessCancelled?.Invoke(this);
                return;
            }
            catch (Exception ex)
            {
                Log.Write(ex, this.ToString() + "scan");
                return;
            }
        }

        public override async void Find(string what, params object[] options)
        {
            IEnumerable<FoundItem> toFoundItems(IEnumerable<DFNode> dfNodes)
            {
                foreach (var dfnode in dfNodes)
                {
                    var p = this.RootPath + dfnode?.DirectoryPath() ?? "";
                    if (dfnode is DirNode)
                        yield return new FoundItem(                            
                            ((DirNode)dfnode).Name,
                            p,
                            0,
                            ((DirNode)dfnode).LastWriteTime,
                            this.Name,
                            this.Path
                        );
                    else if (dfnode is FileNode)
                        yield return new FoundItem(
                            ((FileNode)dfnode).Name,
                            p,
                            ((FileNode)dfnode).Length,
                            ((FileNode)dfnode).LastWriteTime,
                            this.Name,
                            this.Path
                        );
                    //{
                    //    Source = this,
                    //    Name = ((FileNode)item).Name,
                    //    Path = p,
                    //    Length = ((FileNode)item).Length,
                    //    Updated = ((FileNode)item).LastWriteTime,
                    //};
                }
            }

            //if (fileStorge == null) return;
            cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;
            Task<DirsFilesTree> treeTask = null;            
            try
            {
                if (!Loaded) //загружаем...
                {
                    treeTask = Task<DirsFilesTree>.Factory.StartNew(() =>
                    //fileStorge.Load<DirsFilesTree>(Path, this, token, ProcessChanged), token);
                    JsonFile.Read<DirsFilesTree>(Path));
                    await treeTask;
                    if (treeTask.Result!=null)
                        ProcessDone?.Invoke(this);
                    else                        
                        throw new Exception("Ошибка загрузки");
                    Tree = treeTask.Result;
                }

                //разбор options
                bool incDirs = true;
                bool parallel = false;
                char splitter = char.MinValue;
                if (options.Length > 0)
                {
                    if (options[0] is bool) incDirs = (bool)options[0];
                    if (options.Length > 1)
                    {
                        if (options[1] is bool) parallel = (bool)options[1];
                        if (options.Length > 2)
                        {
                            if (options[2] is char)
                                splitter = (char)options[2];
                        }
                    }
                }

                //поиск
                var dfTree = Tree.RootNode.Cast<DFNode>();
                if (parallel) dfTree = dfTree.AsParallel(); //если нужно то параллелим запрос
                IEnumerable<DFNode> found = null;
                what = what.Trim();
                what = what.Trim('\\', '-', ',', '.');
                if (splitter == char.MinValue)
                {
                    found = dfTree.
                    Where((ii) => ii.Name.IndexOf(what, StringComparison.OrdinalIgnoreCase) >= 0 && (ii is FileNode || incDirs));
                }
                else
                {
                    found = dfTree.
                    Where((ii) => ii.Name.CompareByPattern(what, StringComparison.OrdinalIgnoreCase, splitter) && (ii is FileNode || incDirs));
                }
                GotResults?.Invoke(this, toFoundItems(found));
            }
            catch (OperationCanceledException oce)
            {
                ProcessCancelled?.Invoke(this);
                return;
            }
            catch (Exception ex)
            {
                Log.Write(ex, this.ToString() + "find");
                return;
            }
        }
        

        public bool ReName(string newPath)
        {
            //if (Tree == null) return false;
            try
            {
                File.Move(Path, newPath);
                return true;
            }
            catch (Exception ex)
            {
                Log.Write(ex, "ReName");
                return false;
            }
            //if (this.fileStorge.ReName(Path, newPath))
            //{
            //    Path = newPath;
            //    return true;
            //}
            //else return false;
        }

        private void CleanVoids() => Tree.RootNode.OfType<DirNode>().ToList().ForEach(dn => dn.CleanVoids());        
        
    }

    internal class DFScanner
    {
        public static DirsFilesTree ScanNew(string rootPath, object id, List<string> processingFields, CancellationToken cancellationToken, Action<object, int, string> onProgress)
        {
            var dfScanner = new DFScanner(id, new DFSource(rootPath, processingFields));
            dfScanner.OnProgress = onProgress;
            return dfScanner.Scan(cancellationToken);
        }

        public event Action<object, int, string> OnProgress;
        private object id;
        private DFSource dfSource;

        public DFScanner(object id, DFSource dfSource)
        {
            this.id = id ?? throw new ArgumentNullException(nameof(id));
            this.dfSource = dfSource ?? throw new ArgumentNullException(nameof(dfSource));
        }

        public DirsFilesTree Scan(CancellationToken cancellationToken)
        {
            Stack<DirNode> nodes; //стек узлов
            int AddFiles(DirNode parentDir, string[] names)
            {
                if (names == null) return 0;
                foreach (string name in names)
                    parentDir.AddFile((DFData)dfSource.GetDFData(name));
                return names.Length;
            }

            int AddDirs(DirNode parentDir, string[] names)
            {
                if (names == null) return 0;
                foreach (string name in names)
                {
                    var childDir = parentDir.AddDir((DFData)dfSource.GetDFData(name));
                    if (childDir == null)
                        throw new Exception($"IIndexItem child {name} == null !!!");
                    nodes.Push(childDir);//запоминаем директорию для обработки в будущем цикле
                }
                return names.Length;
            }

            if (dfSource == null) return null;
            var rootDFData = dfSource.GetDFData("");
            if (rootDFData == null) return null;//ошибка получения данных корневой директории (например неверный путь)

            var pState = new ScanProgressState();

            nodes = new Stack<DirNode>(20);
            var rootNode = new DirNode((DFData)rootDFData); //корневой узел

            nodes.Push(rootNode);
            while (nodes.Count > 0)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var curNode = nodes.Pop(); //получение очередного узла для обработки
                                           //заполнение узла данными по спискам файлов и директорий
                                           //добавление файлов по списку
                pState.files += AddFiles(curNode, dfSource.GetFiles(curNode.FilePath()));
                //добавление директорий по списку						
                pState.dirs += AddDirs(curNode, dfSource.GetDirectories(curNode.FilePath()));

                OnProgress?.Invoke(id, pState.dirs+pState.files, $"{pState.dirs}/{pState.files}:{dfSource.RootPath}");
            }
            return new DirsFilesTree { RootPath = dfSource.RootPath, RootNode = rootNode };
        }
    }

    internal class ScanProgressState
    {
        //public IIndex Index { get; }
        public int dirs { get; set; } = 0;
        public int files { get; set; } = 0;

        //public ScanProgressState(IIndex Index)
        //{
        //	this.Index = Index ?? throw new ArgumentNullException(nameof(Index));
        //}
    }
}
