using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konditer.models
{
    public class models
    {
        
    }
    public class Tort
    {
        public int ID_cake { get; set; }
        public string cake_name { get; set; }
        public byte[] photo { get; set; }
        public List<int> cake_category { get; set; }
    }

    public class Decor
    {
        public int ID_decor { get; set; }
        public string decor_name { get; set; }
        public double price { get; set; }
        public byte[] photo { get; set; }
    }

    public class Stuffing
    {
        public int ID_stuffing { get; set; }
        public string stuffing_name { get; set; }
        public double price { get; set; }
        public byte[] photo { get; set; }
    }

    public class CakeCategory
    {
        public int ID_cake_category { get; set; }
    }

    public class Order
    {
        public int ID_order { get; set; }
        public double price { get; set; }
        public DateTime date_start { get; set; }
        public DateTime date_end { get; set; }
        public string comment { get; set; }
        public string customer_name { get; set; }
        public string customer_phone { get; set; }
        public string customer_email { get; set; }
        public int ID_cake { get; set; }
        public int ID_stuffing { get; set; }
        public bool status { get; set; }
        public List<int> iddecor { get; set; }
        public double weight { get; set; }
    }
}
