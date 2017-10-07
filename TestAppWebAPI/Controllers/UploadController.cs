using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Configuration;
using System.IO;

namespace TestAppWebAPI.Controllers
{
    public class UploadController : ApiController
    {

        [HttpPost]
        public async Task<IHttpActionResult> PostFile()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return BadRequest();
            }          

            var provider = new MultipartMemoryStreamProvider();
            // путь к папке на сервере
            string path = ConfigurationManager.AppSettings["pathUploadFiles"];
            string root = System.Web.HttpContext.Current.Server.MapPath(path);
            root += "Temp/";

            DirectoryInfo di = Directory.CreateDirectory(root);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }

            await Request.Content.ReadAsMultipartAsync(provider);

            foreach (var _file in provider.Contents)
            {
                var filename = _file.Headers.ContentDisposition.FileName.Trim('\"');                             

                byte[] fileArray = await _file.ReadAsByteArrayAsync();

                using (System.IO.FileStream fs = new System.IO.FileStream(root + filename, System.IO.FileMode.Create))
                {
                    await fs.WriteAsync(fileArray, 0, fileArray.Length);
                }
            }
            return Ok("Файлы загружены в " + root);
        }

    }
}
