using System.Collections.Generic;

namespace FileStorage.Models
{
    public class Folder
    {
        public int FolderId { get; set; }
        public string FolderName { get; set; }
        public ICollection<File> Files { get; set; }

        public Folder()
        {
            Files = new List<File>();
        }
    }
}