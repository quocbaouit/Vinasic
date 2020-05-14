using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace VINASIC.Controllers
{
    public class StickerController : Controller
    {
        //
        // GET: /Sticker/

        public ActionResult Index()
        {
            return View();
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult GetAllSticker()
        {
            try
            {
                Thread.Sleep(200);
                //var customers =null;
                return null;
                //return Json(new { Result = "OK", Records = new   });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }

    }
}
