using Chidono.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Chidono.Services
{
    public class SaveImage : ISaveImage
    {
        public async Task<string> SaveImageAsync(HttpPostedFileBase file, string Id)
        {
           
                ApplicationDbContext context = new ApplicationDbContext();
                MemoryStream stream = new MemoryStream();
                await file.InputStream.CopyToAsync(stream);
                var postedFile = await context.Users.Where(m => m.Id == Id).Include(m => m.PostedImageTable).FirstOrDefaultAsync();
                postedFile.PostedImageTable.Add(new PostedImagesTable
                {
                    ContentLenght = file.ContentLength,
                    ContentType = file.ContentType,
                    FileName = file.FileName,
                    InputeData = stream.ToArray()
                });
                await context.SaveChangesAsync();
                var Img = context.PostedImagesTable.Where(m => m.FileName == file.FileName && m.ContentLenght == file.ContentLength).FirstOrDefault();
                return Img.Id.ToString();

        }

        public async Task<string> UpdateImageAsync(HttpPostedFileBase file, string Id)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            MemoryStream stream = new MemoryStream();
            await file.InputStream.CopyToAsync(stream);
            var postedFile = await context.Users.Where(m => m.Id == Id).Include(m => m.PostedImageTable).FirstOrDefaultAsync();
            foreach (var doc in postedFile.PostedImageTable)
            {
                doc.ContentType = file.ContentType;
                doc.ContentLenght = file.ContentLength;
                doc.FileName = file.FileName;
                doc.InputeData = stream.ToArray();
            }
            await context.SaveChangesAsync();
            var Img = context.PostedImagesTable.Where(m => m.FileName == file.FileName && m.ContentLenght == file.ContentLength).FirstOrDefault();
            return Img.Id.ToString();
        }

    }
}