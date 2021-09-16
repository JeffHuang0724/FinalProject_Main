using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProject.Models
{
    public class OrderModel
    {
        public DateTime OrderDate { get; set; }
        public string SalesNo { get; set; }
        public OrderDetail[] OrderDetail { get; set; }
        public string Remark { get; set; }
    }

    public class OrderDetail
    {
        public string ItemNo { get; set; }
        public int ItemCount { get; set; }
        public string Remark { get; set; }
    }


}