using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Zadanie2_Telephone
{
    //Kласс для телефонной книги
    class PhoneBook
    {
        private List<Contact> contacts = new List<Contact>();
        //Метод для добавления контактов, принимает объект Contact
        public void AddContact(string name, string surname, string phone)
        {
            Contact contact = new Contact(name, surname, phone);
            contacts.Add(contact);
        }
        //Перегрузка, метод, который принимает три отдельные строки 
        public void AddContact(Contact contact)
        {
            contacts.Add(contact);
        }
        //Метод для удаления контакта
        public bool RemoveContact(string fullinfo)
        {
            Contact contact = contacts.FirstOrDefault(c => $"{c.Name} {c.Surname} {c.Phone}" == fullinfo);
            if (contact != null)
            {
                contacts.Remove(contact);
                RemoveContactFile(contact);
                return true;
            }
            return false;
        }
        //Метод для удаления контакта из файла
        private void RemoveContactFile(Contact contact)
        {
     
            var lines = File.ReadAllLines("contacts.txt").ToList();

          
            string contactRemove = $"{contact.Name} {contact.Surname} {contact.Phone}";

          
            lines.Remove(contactRemove);

         
            File.WriteAllLines("contacts.txt", lines);
        }
        //Метод для поиска по имени и фамилии
        public List<Contact> FindContacts(string fullName)
         {
            return contacts.Where(c => $"{c.Name} {c.Surname}" == fullName).ToList();
         }
        //Метод для редактирования контакта
        public bool EditContact(string fullinfo, Contact editContact)
        {
            Contact contact = contacts.FirstOrDefault(c => $"{c.Name} {c.Surname} {c.Phone}" == fullinfo);
            if (contact != null)
            {
            
                contacts.Remove(contact);
                contacts.Add(editContact);
                EditContactFile(contact, editContact);
                return true;
            }
            return false;
        }
        //Метод для редактирования контакта в файле
        private void EditContactFile(Contact oldContact, Contact newContact)
        {
            var lines = File.ReadAllLines("contacts.txt").ToList();
            string old = $"{oldContact.Name} {oldContact.Surname} {oldContact.Phone}";
            string newinf = $"{newContact.Name} {newContact.Surname} {newContact.Phone}";
            lines[lines.IndexOf(old)] = newinf;
            File.WriteAllLines("contacts.txt", lines);
        }
        //Метод,который даёт доступ ко всему списку контактов
        public List<Contact> AllContacts()
        {
            return contacts;
        }
        //Метод, который определяет уникальность номера при помощи LINQ
        public bool PhoneExists(string phone)
        {
            return contacts.Any( p=> p.Phone == phone);
        }
     
       
    }
}

