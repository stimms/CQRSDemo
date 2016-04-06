using System;
using System.Collections.Generic;
using System.Linq;

namespace CQRS_Demo.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public DateTime AccountExpiry { get; set; }
        public int PersonId { get; set; }
    }
}
