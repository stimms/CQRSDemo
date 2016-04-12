using CQRS_Demo.Events;
using CQRS_Demo.Search;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CQRS_Demo.Handlers
{
    public class NameChangedIndexUpdateHandler : INotificationHandler<NameChangedEvent>
    {
        public void Handle(NameChangedEvent notification)
        {
            var search = new UserSearchEngine();
            search.Update(new UserSearchItem { UserId = notification.UserId, FirstName = notification.FirstName, LastName = notification.LastName });
        }
    }
}
