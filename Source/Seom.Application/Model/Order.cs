using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Net.Mail;

namespace Seom.Application.Model
{
    public class Order
    {
        public Order(Customer customer, DateTime dateTime)
        {
            Customer = customer;
            DateTime = dateTime;
            Placed = false;
        }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected Order() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public Customer Customer { get; set; }
        public DateTime DateTime { get; set; }
        public bool Placed { get; set; }
        public List<OrderItem> OrderItems { get; } = new();


    }
}
