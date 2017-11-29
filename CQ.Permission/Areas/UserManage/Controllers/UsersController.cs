using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CQ.Permission.Areas.UserManage.Controllers
{
    public class UsersController : Controller
    {
        // GET: UserManage/Users
        public ActionResult Index()
        {
            return View();
        }
    }
}