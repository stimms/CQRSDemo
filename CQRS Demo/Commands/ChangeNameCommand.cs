using CQRS_Demo.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CQRS_Demo.Commands
{
    public class ChangeNameCommand : IRequest
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}