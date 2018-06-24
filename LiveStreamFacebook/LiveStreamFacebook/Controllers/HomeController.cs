using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LiveStreamFacebook.Models;

namespace LiveStreamFacebook.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public async Task<ActionResult> Index()
        {

            return View();
        }

        public JsonResult VerifyFacebook(string token, string linkYoutube)
        {
            Task.Run(() => ServiceYoutube.Instance(linkYoutube, Server).GetLinkYoutube(token, linkYoutube));
            return Json("");
        }
    }
}