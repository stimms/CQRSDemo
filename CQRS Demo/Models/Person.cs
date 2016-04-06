using System;
using System.Collections.Generic;
using System.Linq;

namespace CQRS_Demo.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EMailAddress { get; set; }
        public int AddressId { get; set; }
    }
}
