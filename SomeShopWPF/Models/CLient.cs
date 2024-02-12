using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
        public ICollection<Purchase> Purchases { get; set; }

        public Client() => Purchases = new List<Purchase>();
        public Client(int id, string surname, string name, string patronymics, string phone, string email) : this()
        {
            Id = id;
            Surname = surname;
            Name = name;
            Patronymics = patronymics;
            Phone = phone;
            Email = email;
        }

        public Client(Client client)
        {
            Id = client.Id;
            Surname = client.Surname;
            Name = client.Name;
            Patronymics = client.Patronymics;
            Phone = client.Phone;
            Email = client.Email;
        }
    }
}
