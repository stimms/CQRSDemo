using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CQRS_Demo.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime ValidFrom { get; set; }
        public int CountryId { get; set; }
    }
}