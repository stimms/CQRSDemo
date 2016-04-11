using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using CQRS_Demo.Models;


namespace CQRS_Demo.Controllers
{
    public class UserController : BaseController
    {
        public ActionResult Edit(int id)
        {
            ViewBag.UserId = id;
            using (var connection = GetConnection())
            {
                var personName = connection.Query<PersonName>("select firstName, lastName from persons p where p.id = (select max(personid) from users where id=@id)", new { id }).Single();
                return View(personName);
            }
                
        }
        [HttpPost]
        public ActionResult ChangeName(int userId, string firstName, string lastName)
        {
            return RedirectToAction("Index3", "Home");
        }
    }
}