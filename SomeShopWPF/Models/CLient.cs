using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeShopWPF.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymics { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public List<Purchase> Purchases { get; set; }

        public Client(int id, string surname, string name, string patronymics, string phone, string email)
        {
            Id = id;
            Surname = surname;
            Name = name;
            Patronymics = patronymics;
            Phone = phone;
            Email = email;
        }
    }
}
