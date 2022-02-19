using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snoopy.Core.DFN
{
    /// <summary>
    /// Дерево из DFNode
    /// </summary>
    public class DirsFilesTree
    {
        public string RootPath { get; set; }//начальный путь по которому было создано(сканировано)
        public DirNode RootNode { get; set; }//корневой узел дерева

        /// <summary>
        ///Конструктор по умолчанию (нужен для десериализатора)
        /// </summary>
        public DirsFilesTree() { }

    }
}
