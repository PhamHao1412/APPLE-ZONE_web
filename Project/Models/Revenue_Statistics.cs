using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Models
{
    public class Revenue_Statistics
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public long Total_Revenue { get; set; }
       
    }
}