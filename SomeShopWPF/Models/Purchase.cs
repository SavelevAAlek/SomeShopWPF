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
        public string Email { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }

        public Purchase(int id, string email, int code, string name)
        {
            Id = id;
            Email = email;
            Code = code;
            Name = name;
        }

    }
}
