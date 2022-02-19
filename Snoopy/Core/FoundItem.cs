using CommonLib.LogHelper;
using Snoopy.Views;
using System;
using System.Threading.Tasks;

namespace Snoopy.Core
{

    public class FoundItem : IFoundItem
    {        
        public string Name { get; private set; }
        public string Path { get; private set; }
        public long? Length { get; private set; }
        public DateTime? Updated { get; private set; }
        public string SourceName { get; private set; }
        public string SourcePath { get; private set; }
        //public Source GetSource() => source;

        public FoundItem(string name, string path, long? length, DateTime? updated, string sourceName, string sourcePath)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Path = path ?? throw new ArgumentNullException(nameof(path));
            Length = length ?? throw new ArgumentNullException(nameof(length));
            Updated = updated ?? throw new ArgumentNullException(nameof(updated));
            SourceName = sourceName ?? throw new ArgumentNullException(nameof(sourceName));
            SourcePath = sourcePath ?? throw new ArgumentNullException(nameof(sourcePath));            
        }

        public async void Exec(Action<bool> feedBack)
        {
            Task<bool> execTask = null;
            try
            {
                execTask = Task<bool>.Factory.StartNew(() => FileExecuter.ExecFile(Path+"\\"+Name));
                bool result = await execTask;
                feedBack?.Invoke(result);
            }
            catch (Exception ex)
            {
                Log.Write(ex, this.ToString() + "Exec");
                return;
            }
        }

        public async void Explore(Action<bool> feedBack)
        {
            Task<bool> execTask = null;
            try
            {
                execTask = Task<bool>.Factory.StartNew(() => FileExecuter.ShowInExplorer(Path));
                bool result = await execTask;
                feedBack?.Invoke(result);
            }
            catch (Exception ex)
            {
                Log.Write(ex, this.ToString() + "Explore");
                return;
            }
        }
    }
}
