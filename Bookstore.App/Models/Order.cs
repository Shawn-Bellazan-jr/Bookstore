﻿namespace Bookstore.App.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public decimal Total { get; set; }
    }
}
