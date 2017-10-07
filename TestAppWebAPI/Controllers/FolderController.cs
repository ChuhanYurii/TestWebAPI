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

namespace TestAppWebAPI.Controllers
{
    //[Authorize]
    public class FolderController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET api/Folder
        //[Authorize]
        public IQueryable<FolderDTO> GetFolders()
        {
            var folders = from f in db.Folders
                          select new FolderDTO()
                          {
                            Id = f.ID,
                            Name = f.Name,
                            UserName = f.User.Email,
                            UserId = f.User.Id
                          };

            return folders;
        }

        // GET api/Folder/5
        [ResponseType(typeof(Folder))]
        public IHttpActionResult GetFolder(int id)
        {
            var folder = db.Folders.Include(f => f.User).Select(f =>
                new FolderDTO()
                {
                    Id = f.ID,
                    Name = f.Name,
                    UserName = f.User.Email,
                    UserId = f.User.Id
                }).SingleOrDefault(f => f.Id == id);
            if (folder == null)
            {
                return NotFound();
            }

            return Ok(folder);
        }

        // PUT api/Folder/5
        public IHttpActionResult PutFolder(int id, Folder folder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != folder.ID)
            {
                return BadRequest();
            }

            db.Entry(folder).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FolderExists(id))
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

        // POST api/Folder
        //[Authorize]
        [ResponseType(typeof(Folder))]
        public IHttpActionResult PostFolder(Folder folder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (folder.Name != null)
            {
                db.Folders.Add(folder);
                db.SaveChanges();

                // Load user name
                db.Entry(folder).Reference(X => X.User).Load();

                var DTO = new FolderDTO()
                {
                    Id = folder.ID,
                    Name = folder.Name,
                    UserName = folder.User.Email,
                    UserId = folder.User.Id
                };

                return CreatedAtRoute("DefaultApi", new { ID = folder.ID }, DTO);
            }
            else return Ok(folder);
        }

        // DELETE api/Folder/5
        [ResponseType(typeof(Folder))]
        public IHttpActionResult DeleteFolder(int id)
        {
            Folder folder = db.Folders.Find(id);
            if (folder == null)
            {
                return NotFound();
            }

            db.Folders.Remove(folder);
            db.SaveChanges();

            return Ok(folder);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FolderExists(int id)
        {
            return db.Folders.Count(e => e.ID == id) > 0;
        }
    }
}