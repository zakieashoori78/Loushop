using System.Collections.Generic;
using System;

namespace Loushop.ViewModels
{
    public class OrderReportViewModel
    {
        public int OrderId { get; set; }
        public DateTime CreateDate { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsFinaly { get; set; }
        public List<OrderDetailViewModel> OrderDetails { get; set; }
    }

    public class OrderDetailViewModel
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public decimal Total { get { return Price * Count; } }
    }

    public class UserProfileViewModel
    {
        public string Email { get; set; }
        public DateTime RegisterDate { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalSpent { get; set; }
        public List<OrderReportViewModel> Orders { get; set; } // اضافه کردن سفارشات
    }




}
