using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using VINASIC.Business.Interface;

namespace VINASIC.Controllers
{
    public class StickerController : Controller
    {
        private readonly IBllSticker _bllSticker;
        //
        // GET: /Sticker/
        public StickerController(IBllSticker bllSticker)
        {
            _bllSticker = bllSticker;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetAllSticker()
        {
            try
            {
                Thread.Sleep(200);
                var stickers =_bllSticker.GetAllSticker();
                return Json(new { Result = "OK", Records = stickers });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }

    }
}
