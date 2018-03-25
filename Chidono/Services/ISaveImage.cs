using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Chidono.Services
{
    public interface ISaveImage
    {
        Task<string> SaveImageAsync(HttpPostedFileBase file, string Id);
        Task<string> UpdateImageAsync(HttpPostedFileBase file, string Id);
    }
}
