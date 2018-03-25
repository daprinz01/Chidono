using Chidono.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Chidono.Controllers
{
    public class ServiceController : Controller
    {
        [HttpGet]
        public async Task<FileStreamResult> DisplayContent(string fileId)
        {
            Guid guid = Guid.Parse(fileId);
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var file = await context.PostedImagesTable.Where(m => m.Id == guid).FirstOrDefaultAsync();
                MemoryStream stream = new MemoryStream(file.InputeData, 0, file.ContentLenght, true, true);
                return new FileStreamResult(stream, file.ContentType);
            }
        }
    }
}