using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeShopWPF.Models
{
    public class Purchase
    {
        public int Id { get; set; }
        public string ClientEmail { get; set; }
        public string Code { get; set; }
        public string ProductName { get; set; }
    }
}
