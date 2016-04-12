using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using CQRS_Demo.Models;
using MediatR;
using CQRS_Demo.Commands;
using CQRS_Demo.Search;

namespace CQRS_Demo.Controllers
{
    public class UserController : BaseController
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }
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
            _mediator.Send(new ChangeNameCommand { FirstName = firstName, LastName = lastName, UserId = userId });
            return RedirectToAction("Index3", "Home");
        }

        public ActionResult List()
        {
            return View();
        }

        public ActionResult Search(string searchTerm)
        {
            var searcher = new UserSearchEngine();
            if(searchTerm == "")
            {

                return Json(searcher.Query(new List<QueryTerm> { new QueryTerm { Field = UserSearchFields.FirstName, Term = searchTerm } }), JsonRequestBehavior.AllowGet);
            }
            else
            return Json(searcher.Query(new List<QueryTerm> { new QueryTerm { Field = UserSearchFields.FirstName, Term = searchTerm },
                new QueryTerm { Field = UserSearchFields.LastName, Term = searchTerm } }), JsonRequestBehavior.AllowGet);
        }
    }
}