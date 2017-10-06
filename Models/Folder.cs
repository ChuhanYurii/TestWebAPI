using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace TestAppWebAPI.Models
{
    public class Folder
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<File> Files { get; set; }
       
        public Folder()
        {
            Files = new List<File>();
        }
    }
}