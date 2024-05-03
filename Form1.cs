using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Text.Json;

namespace day_20
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private List<Author> sampleAuthors; // Объявляем переменную как поле класса
        private void AddSampleAuthors()
        {
            // Очищаем ListBox перед добавлением новых авторов
            listBox1.Items.Clear();

            // Пример 10 авторов
            sampleAuthors = new List<Author>()
            {
                new Author() { FullName = "Александр Пушкин", Country = "Россия", ID = 1 },
                new Author() { FullName = "Лев Толстой", Country = "Россия", ID = 2 },
                new Author() { FullName = "Фёдор Достоевский", Country = "Россия", ID = 3 },
                new Author() { FullName = "Эрнест Хемингуэй", Country = "США", ID = 4 },
                new Author() { FullName = "Джоан Роулинг", Country = "Великобритания", ID = 5 },
                new Author() { FullName = "Габриэль Гарсиа Маркес", Country = "Колумбия", ID = 6 },
                new Author() { FullName = "Агата Кристи", Country = "Великобритания", ID = 7 },
                new Author() { FullName = "Франц Кафка", Country = "Чехия", ID = 8 },
                new Author() { FullName = "Уильям Шекспир", Country = "Англия", ID = 9 },
                new Author() { FullName = "Джордж Оруэлл", Country = "Великобритания", ID = 10 }
            };

            // Добавляем авторов в ListBox
            foreach (var author in sampleAuthors)
            {
                listBox1.Items.Add(author.FullName);
            }
        }


     


        private void Form1_Load(object sender, EventArgs e)
        {
            AddSampleAuthors();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Создаем новый экземпляр класса BookFile
                BookFile bookFile = new BookFile();

                // Заполняем свойства нового экземпляра класса данными из полей ввода
                bookFile.Format = textBox1.Text;
                bookFile.FileSize = long.Parse(textBox3.Text);
                bookFile.Title = comboBox1.Text;
                bookFile.UDC = textBox2.Text;
                bookFile.PageCount = int.Parse(textBox3.Text);
                bookFile.Publisher = textBox4.Text;
                bookFile.Year = int.Parse(numericUpDown1.Text);
                bookFile.UploadDate = DateTime.Now;

                // Добавляем авторов в список авторов книги
                foreach (Author author in listBox1.SelectedItems)
                {
                    bookFile.Authors.Add(author);
                }

                // Добавляем книгу в список книг
                listBox2.Items.Add(bookFile.Title);

                // Преобразуем элементы listBox2 в список объектов BookFile
                List<BookFile> bookFiles = listBox2.Items.Cast<string>().Select(title => new BookFile { Title = title }).ToList();

                // Создаем объект класса XmlSerializer для сериализации списка книг
                XmlSerializer serializer = new XmlSerializer(typeof(List<BookFile>));

                // Открываем поток для записи в файл
                using (TextWriter writer = new StreamWriter("books.xml"))
                {
                    // Сериализуем список книг в XML-файл
                    serializer.Serialize(writer, bookFiles);
                }

                // Сериализуем список книг в JSON-строку
                string jsonString = JsonSerializer.Serialize(bookFiles);

                // Записываем JSON-строку в файл
                File.WriteAllText("books.json", jsonString);

                MessageBox.Show("Данные о книгах успешно сохранены!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения данных о книгах: " + ex.Message);
            }
        }
        // Метод для сохранения данных в XML файл
        private void SaveToXml(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Author>));
            using (TextWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, sampleAuthors);
            }
        }

        // Метод для чтения данных из XML файла
        private List<Author> ReadFromXml(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Author>));
            using (TextReader reader = new StreamReader(filePath))
            {
                return (List<Author>)serializer.Deserialize(reader);
            }
        }

        // Метод для сохранения данных в JSON файл
        private void SaveToJson(string filePath)
        {
            string jsonString = JsonSerializer.Serialize(sampleAuthors);
            File.WriteAllText(filePath, jsonString);
        }

        // Метод для чтения данных из JSON файла
        private List<Author> ReadFromJson(string filePath)
        {
            string jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Author>>(jsonString);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Запрашиваем у пользователя путь к файлу
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XML files (*.xml)|*.xml|JSON files (*.json)|*.json";
            saveFileDialog.Title = "Сохранить данные";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;

                // Проверяем, какая радиокнопка выбрана
                if (radioButton1.Checked)
                {
                    // Сохраняем данные в XML файл
                    SaveToXml(filePath);
                }
                else if (radioButton2.Checked)
                {
                    // Сохраняем данные в JSON файл
                    SaveToJson(filePath);
                }

                MessageBox.Show("Данные успешно сохранены!");
            }
        }
        string filePath;
        private void button3_Click(object sender, EventArgs e)
        {
            // Запрашиваем у пользователя путь к файлу
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML files (*.xml)|*.xml|JSON files (*.json)|*.json";
            openFileDialog.Title = "Загрузить данные";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                 filePath = openFileDialog.FileName;

                // Проверяем, какой формат файла выбран
                if (filePath.EndsWith(".xml"))
                {
                    // Считываем данные из XML-файла
                    sampleAuthors = ReadFromXml(filePath);
                }
                else if (filePath.EndsWith(".json"))
                {
                    // Считываем данные из JSON-файла
                    sampleAuthors = ReadFromJson(filePath);
                }

                // Очищаем textBox3 перед выводом данных
                textBox5.Text = "";

                // Выводим данные в textBox3
                foreach (var author in sampleAuthors)
                {
                    textBox5.Text += $"{author.FullName} ({author.Country})" + Environment.NewLine;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
           /* OpenFileDialog openFileDialog = new OpenFileDialog();
            // Проверяем, загружен ли файл
            if (sampleAuthors == null || sampleAuthors.Count == 0)
            {
                MessageBox.Show("Необходимо сначала загрузить данные из файла.");
                return;
            }

            // Получаем данные из textBox5
            string editedText = textBox5.Text;

            // Запрашиваем у пользователя подтверждение сохранения изменений
            DialogResult result = MessageBox.Show("Сохранить изменения в файл?", "Сохранение изменений", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                // Открываем поток для записи в файл
                using (TextWriter writer = new StreamWriter(filePath))
                {
                    // Записываем отредактированный текст в файл
                    writer.Write(editedText);
                }

                MessageBox.Show("Изменения успешно сохранены!");
            }
           */
        }
    }
}
