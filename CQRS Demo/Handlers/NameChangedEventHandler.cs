using CQRS_Demo.Events;
using Dapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CQRS_Demo.Handlers
{
    public class NameChangedEventHandler : INotificationHandler<NameChangedEvent>
    {
        public void Handle(NameChangedEvent notification)
        {
            using (var connection = GetConnection())
            {
                connection.Execute("update IndexUserList set FirstName = @firstName, LastName = @lastName where userid = @userId", notification);
            }
        }

        private DbConnection GetConnection()
        {
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        }
    }
}