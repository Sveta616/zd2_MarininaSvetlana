using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;

namespace Zadanie2_Telephone
{
    public partial class Form1 : Form
    {
        //Создание экземпляра
        private PhoneBook phoneBook = new PhoneBook();

        public Form1()
        {
            InitializeComponent();
          
          
        }
        //Метод для загрузки контактов из файла
        private void LoadContacts()
        {
            PhoneBookLoader.Load(phoneBook);
            RefreshContact();
        }
        //Метод для обновления содержимого listbox 
        private void RefreshContact()
        {
            listBoxContacts.Items.Clear();
            foreach (var contact in phoneBook.AllContacts())
            {
                listBoxContacts.Items.Add(contact);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadContacts();
        }
        //Кнопка для сохранения в файл/listbox нового контакта
        private void AddNewContact_Click(object sender, EventArgs e)
        {

            string name = Nametext.Text;
            string surname = Surnametext.Text;
            string phone = Numbertext.Text;
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname) || string.IsNullOrEmpty(phone))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }
            if (name.Length == 0 || !char.IsUpper(name[0]))
            {
                MessageBox.Show("Имя должно начинаться с заглавной буквы");
                return;
            }
            for (int i = 1; i < name.Length; i++)
            {
                if (!char.IsLower(name[i]))
                {
                    MessageBox.Show("Имя должно содержать только буквы, начиная с заглавной");
                    return;
                }
            }

      
            if (surname.Length == 0 || !char.IsUpper(surname[0]))
            {
                MessageBox.Show("Фамилия должна начинаться с заглавной буквы");
                return;
            }
            for (int i = 1; i < surname.Length; i++)
            {
                if (!char.IsLower(surname[i]))
                {
                    MessageBox.Show("Фамилия должна содержать только буквы, начиная с заглавной");
                    return;
                }
            }

        
            if (phone.Length != 11 || !phone.All(char.IsDigit) || !phone.StartsWith("89"))
            {
                MessageBox.Show("Номер телефона должен содержать ровно 11 цифр и начинаться с '89'");
                return;
            }
            if (phoneBook.PhoneExists(phone))
            {
                MessageBox.Show("Этот номер телефона уже существует");
                return;
            }

            var contact = new Contact(name, surname, phone);
            phoneBook.AddContact(contact);
            listBoxContacts.Items.Add(contact);
            Clear();
            PhoneBookLoader.Save(phoneBook);
            MessageBox.Show("Контакт сохранен!");
        }

    //Метод для очистки полей 
        private void Clear()
        {
           Nametext.Clear();
           Surnametext.Clear(); 
           Numbertext.Clear();
        }
        //Кнопка для поиска контакта по имени и фамилии
        private void Find_Click(object sender, EventArgs e)
        {

            string name = Nametxt.Text;
            string surname = Surnametxt.Text;

  
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

         
            if (name.Length == 0 || !char.IsUpper(name[0]))
            {
                MessageBox.Show("Имя должно начинаться с заглавной буквы");
                return;
            }
            for (int i = 1; i < name.Length; i++)
            {
                if (!char.IsLower(name[i]))
                {
                    MessageBox.Show("Имя должно содержать только буквы, начиная с заглавной");
                    return;
                }
            }

            if (surname.Length == 0 || !char.IsUpper(surname[0]))
            {
                MessageBox.Show("Фамилия должна начинаться с заглавной буквы");
                return;
            }
            for (int i = 1; i < surname.Length; i++)
            {
                if (!char.IsLower(surname[i]))
                {
                    MessageBox.Show("Фамилия должна содержать только буквы, начиная с заглавной");
                    return;
                }
            }


            string fullName = $"{name} {surname}";



            var contacts = phoneBook.FindContacts(fullName);

            if (contacts.Count > 0)
            {
                string message = "Контакты:\n";
                foreach (var contact in contacts)
                {
                    message += $"{contact}\n";
                }
                MessageBox.Show(message);
            }
            else
            {
                MessageBox.Show("Контакт не найден");
            }
        }

        private void Numbertext_TextChanged(object sender, EventArgs e)
        {

        }
        //Кнопка для удаления контакта
        private void Delete_Click(object sender, EventArgs e)
        {
            string name = NameDel.Text;
            string surname = SurnameDel.Text;
            string phone = NumDel.Text;

   
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname) || string.IsNullOrWhiteSpace(phone))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            if (name.Length == 0 || !char.IsUpper(name[0]) || !name.All(char.IsLetter))
            {
                MessageBox.Show("Имя должно начинаться с заглавной буквы и содержать только буквы");
                return;
            }

            if (surname.Length == 0 || !char.IsUpper(surname[0]) || !surname.All(char.IsLetter))
            {
                MessageBox.Show("Фамилия должна начинаться с заглавной буквы и содержать только буквы");
                return;
            }

            if (phone.Length != 11 || !phone.All(char.IsDigit) || !phone.StartsWith("89"))
            {
                MessageBox.Show("Номер телефона должен содержать ровно 11 цифр и начинаться с '89'");
                return;
            }

            string fullinf = $"{name} {surname} {phone}";

            if (phoneBook.RemoveContact(fullinf))
            {
                RefreshContact();
                Clear();
                MessageBox.Show("Контакт удалён");
            }
            else
            {
                MessageBox.Show("Такого контакта нет");
            }
        }
        //Кнопка для редактирования контакта
        private void Edit_Click(object sender, EventArgs e)
        {
            string name = NameOld.Text;
            string surname = SurnameOld.Text;
            string phone = NumberOld.Text;
            string name2 = NameEdit.Text;
            string surname2 = SurnameEdit.Text;
            string phone2 = NumEdit.Text;
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname) || string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(surname2) || string.IsNullOrWhiteSpace(phone2) || string.IsNullOrWhiteSpace(name2))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            if (name.Length == 0 || !char.IsUpper(name[0]) || !name.All(char.IsLetter)|| name2.Length == 0 || !char.IsUpper(name2[0]) || !name2.All(char.IsLetter))
            {
                MessageBox.Show("Имя должно начинаться с заглавной буквы и содержать только буквы");
                return;
            }

            if (surname.Length == 0 || !char.IsUpper(surname[0]) || !surname.All(char.IsLetter)|| surname2.Length == 0 || !char.IsUpper(surname2[0]) || !surname2.All(char.IsLetter))
            {
                MessageBox.Show("Фамилия должна начинаться с заглавной буквы и содержать только буквы");
                return;
            }

            if (phone.Length != 11 || !phone.All(char.IsDigit) || !phone.StartsWith("89")|| phone2.Length != 11 || !phone2.All(char.IsDigit) || !phone2.StartsWith("89"))
            {
                MessageBox.Show("Номер телефона должен содержать ровно 11 цифр и начинаться с '89'");
                return;
            }
            string old = $"{NameOld.Text} {SurnameOld.Text} {NumberOld.Text}"; 
            Contact updated = new Contact(NameEdit.Text, SurnameEdit.Text, NumEdit.Text); 

            if (phoneBook.EditContact(old, updated))
            {
                MessageBox.Show("Контакт отредактирован");
                RefreshContact(); 
            }
            else
            {
                MessageBox.Show("Контакт не найден");
            }
        }
     //Метод для выхода из приложения
        private void ExitApp()
        {
            SaveFile();
            Application.Exit();
        }
        //Метод для сохранения информации в файл
        private void SaveFile()
        {
            var lines = phoneBook.AllContacts().Select(c => $"{c.Name} {c.Surname} {c.Phone}").ToList();
            File.WriteAllLines("contacts.txt", lines);
        }
        //Кнопка для реализации выхода из приложения
        private void Exit_Click(object sender, EventArgs e)
        {
              ExitApp();
        }
    }
}



