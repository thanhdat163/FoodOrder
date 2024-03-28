using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineFoodOrder_Website.Models;

namespace OnlineFoodOrder_Website.Controllers
{
    public class BookTableController : Controller
    {
        // GET: BookTable
        OnlineFoodOrder_DBEntities1 db = new OnlineFoodOrder_DBEntities1();
        public ActionResult BookTable()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddContact(Contact addcontact, FormCollection form)
        {
            Contact contact = new Contact();
            contact.ContactName = addcontact.ContactName;
            contact.Email = addcontact.Email;
            contact.Subjects = addcontact.Subjects;
            contact.Messagess = addcontact.Messagess;
            contact.CreateDate = Convert.ToDateTime(form["txtdate"]);
            db.Contacts.Add(contact);
            db.SaveChanges();
            return RedirectToAction("BookTable");
        }
    }
}