using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestAppWebAPI.Models
{
    public class FileDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string FileExtension { get; set; }

        public int Size { get; set; }

        public string Path { get; set; }

        public int FolderId { get; set; }

        public string FolderName { get; set; }

        public string UserName { get; set; }
    }
}