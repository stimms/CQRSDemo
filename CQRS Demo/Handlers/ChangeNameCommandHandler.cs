using MediatR;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;
using CQRS_Demo.Commands;
using CQRS_Demo.Events;

namespace CQRS_Demo.Handlers
{
    public class ChangeNameCommandHandler : RequestHandler<ChangeNameCommand>
    {
        private readonly IMediator _mediator;
        public ChangeNameCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected override void HandleCore(ChangeNameCommand message)
        {
            using (var connection = GetConnection())
            {
                connection.Execute("update p set p.FirstName = @firstName, p.LastName = @lastName from persons p inner join users u on p.id = u.personid where u.id = @userId", message);
            }
            _mediator.Publish(new NameChangedEvent
            {
                FirstName = message.FirstName,
                LastName = message.LastName,
                UserId = message.UserId
            });
        }

        private DbConnection GetConnection()
        {
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        }
    }
}