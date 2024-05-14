using System;
using System.Collections.Generic;
using System.IO;

class IndexItem
{
    public string Word { get; set; }
    public List<int> PageNumbers { get; set; }

    public IndexItem(string word, List<int> pageNumbers)
    {
        Word = word;
        PageNumbers = pageNumbers;
    }
}

class SubjectIndex
{
    private List<IndexItem> indexItems;

    public SubjectIndex()
    {
        indexItems = new List<IndexItem>();
    }

    public void AddOrUpdateIndexItem(string word, List<int> pageNumbers)
    {
        // Добавление или обновление элемента индекса
        IndexItem existingItem = indexItems.Find(item => item.Word == word);
        if (existingItem != null)
        {
            existingItem.PageNumbers = pageNumbers;
        }
        else
        {
            indexItems.Add(new IndexItem(word, pageNumbers));
        }
    }

    public void RemoveIndexItem(string word)
    {
        // Удаление элемента индекса
        IndexItem itemToRemove = indexItems.Find(item => item.Word == word);
        if (itemToRemove != null)
        {
            indexItems.Remove(itemToRemove);
        }
    }

    public List<int> GetPageNumbers(string word)
    {
        // Получение списка номеров страниц для заданного слова
        IndexItem item = indexItems.Find(x => x.Word == word);
        return item != null ? item.PageNumbers : new List<int>();
    }

    public void PrintIndex()
    {
        // Печать индекса
        foreach (IndexItem item in indexItems)
        {
            Console.WriteLine($"Слово: {item.Word}, Номера страниц: {string.Join(", ", item.PageNumbers)}");
        }
    }

    public void PrintAllWordsAndIndexes()
    {
        // Печать всех слов и их индексов
        foreach (IndexItem item in indexItems)
        {
            Console.WriteLine($"Слово: {item.Word}, Номера страниц: {string.Join(", ", item.PageNumbers)}");
        }
    }

    public void SaveToFile()
    {
        // Сохранение индекса в файл
        Console.Write("Введите путь каталога для сохранения файла (например, D:\\test): ");
        string directoryPath = Console.ReadLine();

        Console.Write("Введите имя файла (без расширения): ");
        string fileName = Console.ReadLine();

        string filePath = Path.Combine(directoryPath, $"{fileName}.txt");

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (IndexItem item in indexItems)
            {
                writer.WriteLine($"{item.Word}:{string.Join(",", item.PageNumbers)}");
            }
        }

        Console.WriteLine("Индекс успешно сохранен.");
    }

    public void LoadFromFile()
    {
        // Загрузка индекса из файла
        Console.Write("Введите путь к файлу для загрузки данных индекса: ");
        string filePath = Console.ReadLine();

        if (!File.Exists(filePath))
        {
            Console.WriteLine("Файл не найден.");
            return;
        }

        indexItems.Clear();
        using (StreamReader reader = new StreamReader(filePath))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] parts = line.Split(':');
                string word = parts[0];
                List<int> pageNumbers = Array.ConvertAll(parts[1].Split(','), int.Parse).ToList();
                AddOrUpdateIndexItem(word, pageNumbers);
            }
        }

        Console.WriteLine("Индекс успешно загружен.");
    }
}

class Program
{
    static void Main(string[] args)
    {
        SubjectIndex index = new SubjectIndex();

        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1. Ввести слово и вывести список номеров страниц, где оно встречается");
            Console.WriteLine("2. Загрузить/Обновить индекс из файла");
            Console.WriteLine("3. Сохранить индекс в файл");
            Console.WriteLine("4. Вывести все слова и их индексы");
            Console.WriteLine("5. Выход");
            Console.Write("Введите ваш выбор: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    HandleWordSearch(index);
                    break;
                case "2":
                    index.LoadFromFile();
                    break;
                case "3":
                    index.SaveToFile();
                    break;
                case "4":
                    index.PrintAllWordsAndIndexes();
                    break;
                case "5":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Неверный выбор. Пожалуйста, введите число от 1 до 5.");
                    break;
            }

            Console.WriteLine();
        }
    }

    static void HandleWordSearch(SubjectIndex index)
    {
        Console.Write("Введите слово: ");
        string word = Console.ReadLine();

        List<int> pageNumbers = index.GetPageNumbers(word);
        Console.WriteLine($"Номера страниц для слова '{word}': {string.Join(", ", pageNumbers)}");

        Console.Write("Хотите отредактировать номера страниц для этого слова? (y/n): ");
        string editChoice = Console.ReadLine();
        if (editChoice.ToLower() == "y")
        {
            Console.Write("Введите новые номера страниц через запятую: ");
            string pageNumbersInput = Console.ReadLine();
            List<int> newPageNumbers = pageNumbersInput.Split(',').Select(int.Parse).ToList();
            index.AddOrUpdateIndexItem(word, newPageNumbers);
            Console.WriteLine("Номера страниц успешно обновлены.");
        }
    }
}
