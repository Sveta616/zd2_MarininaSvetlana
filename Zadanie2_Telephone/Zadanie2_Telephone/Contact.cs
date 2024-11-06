using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadanie2_Telephone
{
    // Класс для одного контакта
    class Contact
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        //Конструктор
        public Contact(string name, string surname, string phone)
        {
            Name = name;
            Surname = surname;
            Phone = phone;
        }
        //Переопределение метода в виде строки
        public override string ToString()
        {
            return $"{Name} {Surname} - {Phone}";
        }
    }
}
