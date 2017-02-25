using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Web.Mvc;

namespace VINASIC.Controllers
{
    public class CaptcharsController : Controller
    {
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetCaptcha()
        {
            var captcha = new Captcha();
            var rand = new Random((int)DateTime.Now.AddHours(14).Ticks);
            int a = rand.Next(10, 99);
            int b = rand.Next(0, 9);
            Session["Captcha"] = a + b;
            string c = Session["Captcha"].ToString();
            return Json(new { captcha = captcha.CaptchaBase64(a, b) },
            JsonRequestBehavior.AllowGet);
        }
    }
    // class tao image
    public class Captcha
    {
        public string CaptchaBase64(int a, int b, bool noisy = true)
        {
            var rand = new Random((int)DateTime.Now.AddHours(14).Ticks);
            var captcha = string.Format("{0} + {1} = ?", a, b);
            using (var mem = new MemoryStream())
            using (var bmp = new Bitmap(130, 40))
            using (var gfx = Graphics.FromImage(bmp))
            {
                gfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                gfx.SmoothingMode = SmoothingMode.AntiAlias;
                gfx.FillRectangle(Brushes.White, new Rectangle(0, 0, bmp.Width, bmp.Height));
                if (noisy)
                {
                    int i;
                    var pen = new Pen(Color.Yellow);
                    for (i = 1; i < 10; i++)
                    {
                        pen.Color = Color.FromArgb(
                        (rand.Next(0, 255)),
                        (rand.Next(0, 255)),
                        (rand.Next(0, 255)));
                        int r = rand.Next(0, (130 / 3));
                        int x = rand.Next(0, 130);
                        int y = rand.Next(0, 40);
                        gfx.DrawEllipse(pen, x - r, y - r, r, r);
                    }
                }
                gfx.DrawString(captcha, new Font("Tahoma", 17), Brushes.Gray, 2, 3);
                bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Png);
                byte[] byteImage = mem.ToArray();
                return Convert.ToBase64String(byteImage);
            }
        }
    }
}
