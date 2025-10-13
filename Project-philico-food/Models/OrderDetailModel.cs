using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_philico_food.Models
{
    internal class OrderDetailModel
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public string Datez { get; set; }
        public string Timez {  get; set; }
        public int Weight { get; set; }
        public string WeightType { get; set; }
        public string LicensePlate { get; set; }
    }
}
