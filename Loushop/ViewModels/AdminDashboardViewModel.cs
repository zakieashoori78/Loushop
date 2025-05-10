using System;
using System.Collections.Generic;

namespace Loushop.ViewModels
{
    public class AdminDashboardViewModel
    {
        public List<ProductReportViewModel> ProductReports { get; set; }
        public List<UserReportViewModel> UserReports { get; set; }

        // گزارش‌های خلاصه
        public int TotalProducts { get; set; }
        public string ProductWithHighestStock { get; set; }
        public string ProductWithLowestStock { get; set; }
        public string BestSellingProduct { get; set; }
        public int TotalUsers { get; set; }
        public int TotalAdmins { get; set; }
    }

    public class ProductReportViewModel
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int OrderCount { get; set; }
    }

    public class UserReportViewModel
    {
        public string Email { get; set; }
        public DateTime RegisterDate { get; set; }
        public bool IsAdmin { get; set; }
        public int OrderCount { get; set; }
    }
}

