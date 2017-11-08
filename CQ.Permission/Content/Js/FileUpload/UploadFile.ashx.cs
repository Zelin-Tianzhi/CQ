using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
namespace WebApplication1.bootstrap_fileinput_master.examples
{
    /// <summary>
    /// UploadFile 的摘要说明
    /// </summary>
    public class UploadFile : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string json = "{\"msg\":\"上传成功!\"}";
            
            try
            {
                context.Response.ContentType = "text/plain";
                Stream sr = context.Request.InputStream;
                byte[] bt = new byte[sr.Length];
                HttpPostedFile file = context.Request.Files["model_file"];
                string savepath = context.Request["savepath"];//获取文件保存的路径
                string fileName = file.FileName;
                sr.Read(bt, 0, bt.Length);
                savepath = context.Server.MapPath(savepath) + "\\" + fileName;
                FileStream fs = new FileStream(savepath, FileMode.Create);
                fs.Write(bt, 0, bt.Length);
                fs.Close();
                sr.Close();
            }
            catch (Exception ex)
            {
                //失败时返回的参数必须加 error键
                json = "{\"error\":\""+ex.Message+"\"}";
            }
            context.Response.Write(json);
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}