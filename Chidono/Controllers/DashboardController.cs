using Chidono.Models;
using Chidono.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Chidono.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        public ISaveImage saveImage;
 
        public DashboardController(ISaveImage saveImageparam)
        {
            this.saveImage = saveImageparam;
        }

        
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ProfilePage()
        {
            ViewBag.imgExist = false;
            ProfileViewModels model = new ProfileViewModels();
            ApplicationDbContext context = new ApplicationDbContext();
            var getcurrentUsername = HttpContext.User.Identity.Name;
            var user = context.Users.Include(c => c.SchoolDetails).Include(d => d.NyscDetails).Include(p=>p.PostedImageTable)
                .Where(m => m.UserName == getcurrentUsername).FirstOrDefault();
            
            if (user.SchoolDetails == null && user.NyscDetails == null && user.PostedImageTable == null)
            { 
                return Redirect("/Dashboard/EditProfile");
            }
            else
            {
                model.Address = user.Address;
                model.Name = user.Name;
                model.Username = user.UserName;
                model.Age = user.Age;
                if(user.SchoolDetails != null && user.NyscDetails != null && user.PostedImageTable != null)
                {
                    foreach (var i in user.SchoolDetails)
                    {
                        model.Department = i.Department;
                        model.Faculty = i.Faculty;
                        model.University = i.University;
                    }
                    foreach (var c in user.NyscDetails)
                    {
                        model.RegDate = c.RegDate;
                        model.StateChoice1 = c.StateChoice1;
                        model.StateChoice2 = c.StateChoice2;
                        model.StateChoice3 = c.StateChoice3;
                        model.DeployDate = c.DeployDate;
                    }
                    foreach (var img in user.PostedImageTable)
                    {
                        model.ImgId = img.Id.ToString();
                    }
                    ViewBag.imgExist = true;
                }
                
                return View(model);
            }
            
            
        }
       
        [HttpGet]
        public async Task<ActionResult> EditProfile()
        {
            ViewBag.imgExist = false;
            ProfileViewModels model = new ProfileViewModels();
            ApplicationDbContext context = new ApplicationDbContext();
            var currentUsername = HttpContext.User.Identity.Name;
            var user = await context.Users.Include(c => c.SchoolDetails).Include(d => d.NyscDetails)
                .Where(m => m.UserName == currentUsername).FirstOrDefaultAsync();
            model.Address = user.Address;
            model.Name = user.Name;
            model.Username = user.UserName;
            model.Age = user.Age;
            if (user.SchoolDetails != null && user.PostedImageTable != null && user.NyscDetails != null)
            {
                foreach (SchoolDetails s in user.SchoolDetails)
                {
                    model.Department = s.Department;
                    model.Faculty = s.Faculty;
                    model.University = s.University;
                }

                foreach (var img in user.PostedImageTable)
                {
                    model.ImgId = img.Id.ToString();
                    ViewBag.imgExist = true;
                }
                foreach (NyscDetails c in user.NyscDetails)
                {
                    model.RegDate = c.RegDate;
                    model.StateChoice1 = c.StateChoice1;
                    model.StateChoice2 = c.StateChoice2;
                    model.StateChoice3 = c.StateChoice3;
                    model.DeployDate = c.DeployDate;
                }
            }
           
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> EditProfile(ProfileViewModels model, HttpPostedFileBase img)
        {
            
            ViewBag.imgExist = false;
            string imgId = string.Empty;
            ApplicationDbContext context = new ApplicationDbContext();
            var currentUsername = HttpContext.User.Identity.Name;
            var user = await context.Users
                .Where(m => m.UserName == currentUsername).FirstOrDefaultAsync();
            user.Address = model.Address;
            user.Name = model.Name;
            user.UserName = model.Username;
            user.Age = model.Age;
            //if an image already exist for the user then just update the image
            if (user.PostedImageTable == null)
            {
                if (img != null)
                {
                    imgId = await this.saveImage.SaveImageAsync(img, user.Id);
                    model.ImgId = imgId;
                    ViewBag.imgExist = true;
                }
                else
                {
                    ViewBag.imgExist = false;
                }
            }
            else
            {
                if (img != null)
                {
                    imgId = await this.saveImage.UpdateImageAsync(img, user.Id);
                    model.ImgId = imgId;
                    ViewBag.imgExist = true;
                }
                else
                {
                    ViewBag.imgExist = false;
                }
            }
            
                
            if (user.SchoolDetails == null)
            {
                user.SchoolDetails.Add(new SchoolDetails
                {
                    Department = model.Department,
                    Faculty = model.Faculty,
                    University = model.University
                });
                user.NyscDetails.Add(new NyscDetails
                {
                    RegDate = model.RegDate,
                    StateChoice1 = model.StateChoice1,
                    StateChoice2 = model.StateChoice2,
                    StateChoice3 = model.StateChoice3,
                    DeployDate = model.DeployDate
                });
                ViewBag.result = "user created";
            }
            else
            {
                foreach (var s in user.SchoolDetails)
                {
                    s.Department = model.Department;
                    s.Faculty = model.Faculty;
                    s.University = model.University;
                }
                foreach (var n in user.NyscDetails)
                {
                    n.RegDate = model.RegDate;
                    n.StateChoice1 = model.StateChoice1;
                    n.StateChoice2 = model.StateChoice2;
                    n.StateChoice3 = model.StateChoice3;
                    n.DeployDate = model.DeployDate;
                }
                ViewBag.result = "update successful";
            }
            await context.SaveChangesAsync();
            
            
        return View(model);
        }
        public string Messages()
        {
            return "ToDo:" + Environment.NewLine + "Create this view and a controller to view messages sent to " +
                "a particular user. Try integrating signalR to use asynchronous chatting between users.";
        }
        public string Settings()
        {
            return "Use this page to set the parameters for different user and create a settings table in DB to " +
                "track changes and date of changes.";
        }
    }
}