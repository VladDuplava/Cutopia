using System;

namespace ExceptionHandlingApp
{
    // 1. ВЛАСНИЙ КЛАС ВИНЯТКУ
    public class InvalidPersonDataException : Exception
    {
        public InvalidPersonDataException() : base("Невірні дані персони.") { }
        
        public InvalidPersonDataException(string message) : base(message) { }

        public InvalidPersonDataException(string message, Exception innerException) 
            : base(message, innerException) { }
    }

    // Інтерфейс з попереднього завдання
    public interface IPersona : IComparable<IPersona>
    {
        string Surname { get; }
        DateTime BirthDate { get; }
        void PrintInfo();
        int GetAge();
    }

    // Абстрактний базовий клас
    public abstract class Person : IPersona
    {
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public string Faculty { get; set; }

        protected Person(string surname, DateTime birthDate, string faculty)
        {
            // ГЕНЕРАЦІЯ ВЛАСНОГО ВИНЯТКУ
            if (birthDate > DateTime.Now)
            {
                throw new InvalidPersonDataException($"Помилка створення персони {surname}: дата народження ({birthDate.ToShortDateString()}) не може бути в майбутньому!");
            }

            Surname = surname;
            BirthDate = birthDate;
            Faculty = faculty;
        }

        public virtual int GetAge()
        {
            DateTime today = DateTime.Today;
            int age = today.Year - BirthDate.Year;
            if (BirthDate.Date > today.AddYears(-age)) age--;
            return age;
        }

        public abstract void PrintInfo();

        public int CompareTo(IPersona other)
        {
            if (other == null) return 1;
            return this.GetAge().CompareTo(other.GetAge());
        }
    }

    // Похідні класи
    public class Abiturient : Person
    {
        public Abiturient(string surname, DateTime birthDate, string faculty) 
            : base(surname, birthDate, faculty) { }

        public override void PrintInfo() => 
            Console.WriteLine($"[Абітурієнт] {Surname}, Вік: {GetAge()}");
    }

    public class Student : Person
    {
        public int Course { get; set; }

        public Student(string surname, DateTime birthDate, string faculty, int course) 
            : base(surname, birthDate, faculty)
        {
            Course = course;
        }

        public override void PrintInfo() => 
            Console.WriteLine($"[Студент] {Surname}, Курс: {Course}, Вік: {GetAge()}");
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("--- ДЕМОНСТРАЦІЯ ОБРОБКИ ВИНЯТКІВ ---");

            // ТЕСТ 1: Перевірка власного винятку (InvalidPersonDataException)
            try
            {
                Console.WriteLine("\nСпроба створити студента з датою народження у 2050 році...");
                // Це викличе виняток, оскільки дата в майбутньому
                Student futureStudent = new Student("Мартинюк", new DateTime(2050, 5, 1), "ФІОТ", 1);
                futureStudent.PrintInfo();
            }
            catch (InvalidPersonDataException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[ПЕРЕХОПЛЕНО ВЛАСНИЙ ВИНЯТОК] {ex.Message}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Невідома помилка: {ex.Message}");
            }

            // ТЕСТ 2: Перевірка стандартного винятку (InvalidCastException) згідно з варіантом 3.4
            try
            {
                Console.WriteLine("\nСпроба виконати невірне приведення типів (InvalidCastException)...");
                
                // Створюємо об'єкт Абітурієнта і ховаємо його за посиланням на базовий клас Person
                Person somePerson = new Abiturient("Іваненко", new DateTime(2005, 1, 15), "Кібернетика");
                
                // Намагаємося явно привести Абітурієнта до Студента (це неможливо і викличе помилку)
                Student invalidCastStudent = (Student)somePerson;
                
                invalidCastStudent.PrintInfo(); // Цей код не виконається
            }
            catch (InvalidCastException ex)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"[ПЕРЕХОПЛЕНО СТАНДАРТНИЙ ВИНЯТОК] Помилка приведення типів!");
                Console.WriteLine($"Системне повідомлення: {ex.Message}");
                Console.ResetColor();
            }
            finally
            {
                Console.WriteLine("\n[Блок finally] Обробку винятків завершено. Звільнення ресурсів...");
            }

            Console.ReadLine();
        }
    }
}
