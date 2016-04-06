using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using DapperExtensions;
using CQRS_Demo.Models;
using GenFu;

namespace CQRS_Demo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var sql = @"Select AccountExpiry, FirstName, LastName, Street, City, c.Name from users u left outer join 
			                          persons p on p.Id = u.PersonId left outer join 
			                          Addresses a on a.Id = p.AddressId left outer join 
			                          Countries c on c.id = a.CountryId";
            IEnumerable<dynamic> model = new List<dynamic>();
            using (var connection = GetConnection())
            {
                model = connection.Query(sql);
            }
            return View(model);
        }

        public ActionResult Populate()
        {
            using (var connection = GetConnection())
            {
                A.Configure<Address>().Fill(x => x.CountryId).WithinRange(1, 14);
                connection.Insert<Address>(A.ListOf<Address>(50));
                A.Configure<Person>().Fill(x => x.AddressId).WithinRange(1, 50);
                connection.Insert<Person>(A.ListOf<Person>(50));
                A.Configure<User>().Fill(x => x.PersonId).WithinRange(1, 50)
                                   .Fill(x=>x.AccountExpiry).AsFutureDate();
                
                connection.Insert<User>(A.ListOf<User>(50));
            }

            return View();
        }

        private static SqlConnection GetConnection()
        {
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}