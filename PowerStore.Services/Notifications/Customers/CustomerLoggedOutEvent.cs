﻿using PowerStore.Domain.Customers;
using MediatR;

namespace PowerStore.Services.Notifications.Customers
{
    /// <summary>
    /// Customer logged-in event
    /// </summary>
    public class CustomerLoggedOutEvent : INotification
    {
        public CustomerLoggedOutEvent(Customer customer)
        {
            Customer = customer;
        }

        /// <summary>
        /// Customer
        /// </summary>
        public Customer Customer {
            get; private set;
        }
    }
}
