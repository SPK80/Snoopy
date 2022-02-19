using System;

namespace Snoopy.Views
{
    public interface IFoundItem
    {
        [Caption("Имя")]
        string Name { get;  }
        [Caption("Длина")]
        long? Length { get; }
        [Caption("Путь")]
        string Path { get; }
        [Caption("Обновлён")]
        DateTime? Updated { get; }
        string SourceName { get; }
        string SourcePath { get; }
    }

    public class CaptionAttribute : Attribute
    {
        public string Caption{ get; private set; }
        public CaptionAttribute(string caption)
        {
            Caption = caption;
        }
    }
}