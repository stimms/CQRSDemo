using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CQRS_Demo.Events
{
    public class NameChangedEvent:INotification
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}