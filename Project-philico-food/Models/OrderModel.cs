using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_philico_food.Models
{
    internal class OrderModel
    {

        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public string LicensePlate { get; set; }
        public string ProductName { get; set; }
        public string CustomerName { get; set; }
        public string Note { get; set; }
        public int NetWeight { get; set; }

        /// <summary>
        /// Success,Process,Cancel
        /// </summary>
        public string Status { get; set; }
    }
}
