using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TestAppWebAPI.Models;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;


namespace TestAppWebAPI.Controllers
{
    public class FileController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        
        // GET api/File
        //[Authorize]
        public IQueryable<FileDTO> GetFiles()
        {
            var files = from f in db.Files
                          select new FileDTO()
                          {
                              Id = f.ID,
                              Name = f.Name,
                              UserName = f.Folder.User.Email,
                              FolderId = f.FolderId,
                              FolderName = f.Folder.Name,
                              FileExtension = f.FileExtension,
                              Size = f.Size,
                              Path = f.Path
                          };

            return files;
        }

        // GET api/File/5
        [ResponseType(typeof(TestAppWebAPI.Models.File))]
        public IHttpActionResult GetFile(int id)
        {
            var file = db.Files.Include(f => f.Folder).Select(f =>
                new FileDTO()
                {
                    Id = f.ID,
                    Name = f.Name,
                    UserName = f.Folder.User.Email,
                    FolderId = f.FolderId,
                    FolderName = f.Folder.Name,
                    FileExtension = f.FileExtension,
                    Size = f.Size,
                    Path = f.Path
                }).SingleOrDefault(f => f.Id == id);
            if (file == null)
            {
                return NotFound();
            }

            return Ok(file);
        }

        // PUT api/File/5
        public IHttpActionResult PutFile(int id, TestAppWebAPI.Models.File file)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != file.ID)
            {
                return BadRequest();
            }

            db.Entry(file).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FileExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/File
        public IHttpActionResult PostFile(TestAppWebAPI.Models.File file)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (file.FolderId != 0)
            {
                db.Files.Add(file);
                db.SaveChanges();

                db.Entry(file).Reference(X => X.Folder).Load();

                var DTO = new FileDTO()
                {
                    Id = file.ID,
                    Name = file.Name,
                    UserName = file.Folder.User.Email,
                    FolderId = file.FolderId,
                    FolderName = file.Folder.Name,
                    FileExtension = file.FileExtension,
                    Size = file.Size,
                    Path = file.Path
                };

                return CreatedAtRoute("DefaultApi", new { ID = file.ID }, DTO);
            }
            else return Ok(file);        
        }

        // DELETE api/File/5
        [ResponseType(typeof(TestAppWebAPI.Models.File))]
        public IHttpActionResult DeleteFile(int id)
        {
            TestAppWebAPI.Models.File file = db.Files.Find(id);
            if (file == null)
            {
                return NotFound();
            }

            db.Files.Remove(file);
            db.SaveChanges();

            return Ok(file);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FileExists(int id)
        {
            return db.Files.Count(e => e.ID == id) > 0;
        }
    }
}