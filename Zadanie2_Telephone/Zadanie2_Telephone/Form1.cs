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
            добавлениеToolStripMenuItem.Enabled = false;


        }
        //Метод для загрузки контактов из файла
        private void LoadContacts()
        {
            PhoneBookLoader.Load(phoneBook);
             NewList();
        }
        //Метод для обновления содержимого listbox 
        private void NewList()
        {
            listBoxContacts.Items.Clear();
            foreach (var contact in phoneBook.AllContacts())
            {
                listBoxContacts.Items.Add(contact);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }
        //Метод для проверки на пустые поля
        private bool ProverkaOfSpace(string p)
        {
            if (string.IsNullOrWhiteSpace(p))
            {
                MessageBox.Show("Заполните все поля");
                return false;
            }
            return true;
        }
    //Метод для проверки на буквы
       private bool ProverkaOfLetter(string name)
        {
           
            if (!char.IsUpper(name[0]))
            {
                MessageBox.Show("Имя и Фамилия должны начинаться с заглавной буквы");
                return false;
            }
            for (int i = 1; i < name.Length; i++)
            {
                if (!char.IsLower(name[i]))
                {
                    MessageBox.Show("Имя и Фамилия должны содержать только буквы, начиная с заглавной");
                    return false;
                }
            }
            return true;
        }
        //Метод для проверки на цифры
        private bool ProverkaOfDigit(string phone)
        {
            if (phone.Length != 11 || !phone.All(char.IsDigit) || !phone.StartsWith("89"))
            {
                MessageBox.Show("Номер телефона должен содержать ровно 11 цифр и начинаться с '89'");
                return false;
            }
            return true;
        }

       
     

    //Метод для очистки полей 
        private void Clear()
        {
           Nametext.Clear();
           Surnametext.Clear(); 
           Numbertext.Clear();
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
            var inf = phoneBook.AllContacts().Select(c => $"{c.Name} {c.Surname} {c.Phone}").ToList();
            File.WriteAllLines("contacts.txt", inf);
        }
      

        private void поискToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string name = Nametxt.Text;
            string surname = Surnametxt.Text;
            if (!ProverkaOfSpace(name) || !ProverkaOfSpace(surname))
            {
                return;
            }
            if (!ProverkaOfLetter(name) || !ProverkaOfLetter(surname))
            {
                return;
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
       //Меню при нажатии удаляет контакт
        private void удалениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string name = NameDel.Text;
            string surname = SurnameDel.Text;
            string phone = NumDel.Text;
            if (!ProverkaOfSpace(name) || !ProverkaOfSpace(surname) || !ProverkaOfSpace(phone))
            {
                return;
            }

            if (!ProverkaOfLetter(name) || !ProverkaOfLetter(surname))
            {
                return;
            }
            if (!ProverkaOfDigit(phone))
            {
                return;
            }

            string fullinf = $"{name} {surname} {phone}";

            if (phoneBook.RemoveContact(fullinf))
            {
                NewList();
                Clear();
                MessageBox.Show("Контакт удалён");
            }
            else
            {
                MessageBox.Show("Такого контакта нет");
            }
        }
        //Меню при нажатии редактирует контакт
        private void редактированиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string name = NameOld.Text;
            string surname = SurnameOld.Text;
            string phone = NumberOld.Text;
            string name2 = NameEdit.Text;
            string surname2 = SurnameEdit.Text;
            string phone2 = NumEdit.Text;
            if (!ProverkaOfSpace(name) || !ProverkaOfSpace(surname) || !ProverkaOfSpace(phone) || !ProverkaOfSpace(name2) || !ProverkaOfSpace(surname2) || !ProverkaOfSpace(phone2))
            {
                return;
            }
            if (!ProverkaOfLetter(name) || !ProverkaOfLetter(surname) || !ProverkaOfLetter(name2) || !ProverkaOfLetter(surname2))
            {
                return;
            }

            if (!ProverkaOfDigit(phone) || !ProverkaOfDigit(phone2))
            {
                return;
            }
            string old = $"{NameOld.Text} {SurnameOld.Text} {NumberOld.Text}";
            Contact edit = new Contact(NameEdit.Text, SurnameEdit.Text, NumEdit.Text);

            if (phoneBook.EditContact(old, edit))
            {
                MessageBox.Show("Контакт отредактирован");
                NewList();
            }
            else
            {
                MessageBox.Show("Контакт не найден");
            }
        }
        //Меню при нажатии выходит из приложения
        private void выйтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExitApp();
        }
        //Меню при нажатии добавляет в файл новый контакт
        private void добавлениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string name = Nametext.Text;
            string surname = Surnametext.Text;
            string phone = Numbertext.Text;
            if (!ProverkaOfSpace(name) || !ProverkaOfSpace(surname) || !ProverkaOfSpace(phone))
            {
                return;
            }
            if (!ProverkaOfLetter(name) || !ProverkaOfLetter(surname))
            {
                return;
            }
            if (!ProverkaOfDigit(phone))
            {
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
        //Меню при нажатии читает из файла в лист
        private void загрузкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadContacts();
            добавлениеToolStripMenuItem.Enabled = true;


        }
    }
}



