using CQ.Application.BusinessData;
using CQ.Core;
using CQ.Domain.Entity.BusinessData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CQ.Permission.Areas.ContentManage.Controllers
{
    public class GamesController : BaseController
    {

        private ProductApp productApp = new ProductApp();
        private ImageApp imageApp = new ImageApp();
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword)
        {
            var data = new
            {
                rows = productApp.GetList(pagination, keyword),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = productApp.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult SubmitForm(ProductEntity productEntity, string keyValue)
        {
            string imgListStr = productEntity.F_Remark;
            productApp.SubmitForm(productEntity, imgListStr.TrimEnd(',').Split(','), keyValue);
            return Success("操作成功。");
        }

        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            productApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }

        public JsonResult UploadImg()
        {
            var oFile = Request.Files["txt_file"];

            var oStream = oFile.InputStream;
            //得到了文件的流对象，我们不管是用NPOI、GDI还是直接保存文件都不是问题了吧。。。。

            //后台TODO
            var imgType = oFile.FileName.Split('.')[1];
            try
            {
                byte[] bytes = new byte[oStream.Length];
                oStream.Read(bytes, 0, bytes.Length);
                oStream.Seek(0, System.IO.SeekOrigin.Begin);
                Session["img1"] = bytes;
            }
            catch (Exception ex)
            {

                throw;
            }
            Random rd = new Random();
            System.Drawing.Image img = System.Drawing.Bitmap.FromStream(oStream);
            Bitmap bmp = new Bitmap(img);
            MemoryStream bmpStream = new MemoryStream();
            bmp.Save(bmpStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + rd.Next(100000, 1000000);
            string folderName = DateTime.Now.ToString("yyyy-MM-dd");
            string rootPath = System.Web.HttpContext.Current.Server.MapPath("~/");
            string fullPath = rootPath + "Upload/" + folderName;
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
            FileStream fs = new FileStream(fullPath + "/" + fileName + "." + imgType, FileMode.Create);
            bmpStream.WriteTo(fs);
            bmpStream.Close();
            fs.Close();
            bmpStream.Dispose();
            fs.Dispose();
            string imgPath = "/Upload/" + folderName + "/" + fileName + "." + imgType;
            return Json(new { Success = true, ImgPaht = imgPath }, JsonRequestBehavior.AllowGet);
        }
    }
}