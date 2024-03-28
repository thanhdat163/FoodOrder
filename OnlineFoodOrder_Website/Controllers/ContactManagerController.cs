using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineFoodOrder_Website.Models;
using PagedList;
using PagedList.Mvc;

namespace OnlineFoodOrder_Website.Controllers
{
    
    public class ContactManagerController : Controller
    {
        OnlineFoodOrder_DBEntities1 db = new OnlineFoodOrder_DBEntities1();
        // GET: ContactManager
        public ActionResult ContactList(int? page)
        {
            int pageNum = page ?? 1;
            int pageSize = 10;
            var lstContact = db.Contacts;
            return View(lstContact.OrderBy(x => x.ContactID).ToPagedList(pageNum, pageSize));
        }
    }
}