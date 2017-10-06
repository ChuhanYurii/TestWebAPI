using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestAppWebAPI.Models
{
    public class FolderDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
    }
}