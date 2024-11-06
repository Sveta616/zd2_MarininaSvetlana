using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;


namespace Zadanie2_Telephone
{
    //Статический класс
   static class PhoneBookLoader
    {
        //Метод для чтения/загрузки из файла 
        public static void Load(PhoneBook phoneBook)
        {
            if (File.Exists("contacts.txt"))
            {
                var filelines = File.ReadAllLines("contacts.txt");
                foreach (var lines in filelines)
                {
                    try
                    {
                        var inf = lines.Split(' ');
                        if (inf.Length == 3)
                        {
                            var contact = new Contact(inf[0], inf[1], inf[2]);
                            phoneBook.AddContact(contact);
                        }
                    }
                    catch 
                    {
                        MessageBox.Show("Ошибка загрузки");
                    }
                }
            }
            else
            {
                MessageBox.Show("Файл не найден");
            }
        }
        //Метод для  сохранения нового контакта в файл при помощи LINQ
            public static void Save(PhoneBook phoneBook)
          {
            var inf = phoneBook.AllContacts().Select(newinf => $"{newinf.Name} {newinf.Surname} {newinf.Phone}");
            File.WriteAllLines("contacts.txt", inf);
          }
    }
}
