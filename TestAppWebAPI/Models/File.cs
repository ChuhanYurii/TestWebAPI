using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace TestAppWebAPI.Models
{
    public class File
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string FileExtension { get; set; }
        
        public int Size { get; set; }
        
        public string Path { get; set; }

        public int FolderId { get; set; }

        public virtual Folder Folder { get; set; }
    }
}