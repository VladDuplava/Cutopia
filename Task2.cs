using System;

namespace PersonaHierarchy
{
    // 1. Інтерфейс IPersona, що успадковує стандартний .NET інтерфейс IComparable
    public interface IPersona : IComparable<IPersona>
    {
        string Surname { get; }
        DateTime BirthDate { get; }
        
        void PrintInfo();
        int GetAge();
    }

    // 2. Абстрактний базовий клас для спільної логіки (щоб не дублювати код у похідних класах)
    public abstract class Person : IPersona
    {
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public string Faculty { get; set; }

        protected Person(string surname, DateTime birthDate, string faculty)
        {
            Surname = surname;
            BirthDate = birthDate;
            Faculty = faculty;
        }

        // Реалізація методу визначення віку
        public virtual int GetAge()
        {
            DateTime today = DateTime.Today;
            int age = today.Year - BirthDate.Year;
            // Якщо день народження ще не настав у поточному році, віднімаємо 1 рік
            if (BirthDate.Date > today.AddYears(-age))
            {
                age--;
            }
            return age;
        }

        // Абстрактний метод виводу інформації (реалізується у похідних класах)
        public abstract void PrintInfo();

        // Реалізація методу інтерфейсу IComparable (порівняння за віком)
        public int CompareTo(IPersona other)
        {
            if (other == null) return 1;
            return this.GetAge().CompareTo(other.GetAge());
        }
    }

    // 3. Похідний клас: Абітурієнт
    public class Abiturient : Person
    {
        public Abiturient(string surname, DateTime birthDate, string faculty) 
            : base(surname, birthDate, faculty) { }

        public override void PrintInfo()
        {
            Console.WriteLine($"[Абітурієнт] Прізвище: {Surname}, Факультет: {Faculty}, Вік: {GetAge()} ({BirthDate.ToShortDateString()})");
        }
    }

    // 4. Похідний клас: Студент
    public class Student : Person
    {
        public int Course { get; set; }

        public Student(string surname, DateTime birthDate, string faculty, int course) 
            : base(surname, birthDate, faculty)
        {
            Course = course;
        }

        public override void PrintInfo()
        {
            Console.WriteLine($"[Студент] Прізвище: {Surname}, Факультет: {Faculty}, Курс: {Course}, Вік: {GetAge()} ({BirthDate.ToShortDateString()})");
        }
    }

    // 5. Похідний клас: Викладач
    public class Teacher : Person
    {
        public string Position { get; set; }
        public int Experience { get; set; } // Стаж у роках

        public Teacher(string surname, DateTime birthDate, string faculty, string position, int experience) 
            : base(surname, birthDate, faculty)
        {
            Position = position;
            Experience = experience;
        }

        public override void PrintInfo()
        {
            Console.WriteLine($"[Викладач] Прізвище: {Surname}, Посада: {Position}, Факультет: {Faculty}, Стаж: {Experience} р., Вік: {GetAge()} ({BirthDate.ToShortDateString()})");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Створення бази (масиву) з n персон
            IPersona[] database = new IPersona[]
            {
                new Abiturient("Коваленко", new DateTime(2006, 5, 12), "Кібернетика"),
                new Abiturient("Іванов", new DateTime(2005, 11, 30), "Прикладна математика"),
                new Student("Шевченко", new DateTime(2004, 3, 15), "Інженерія ПЗ", 2),
                new Student("Ткачук", new DateTime(2002, 8, 22), "Кібернетика", 4),
                new Teacher("Бойко", new DateTime(1980, 1, 10), "Кібернетика", "Доцент", 15),
                new Teacher("Мельник", new DateTime(1975, 7, 25), "Прикладна математика", "Професор", 22)
            };

            // 1. Виведення повної інформації з бази на екран
            Console.WriteLine("=== БАЗА ДАНИХ ПЕРСОН ===");
            foreach (var person in database)
            {
                person.PrintInfo();
            }
            Console.WriteLine();

            // 2. Демонстрація сортування масиву завдяки інтерфейсу IComparable
            Console.WriteLine("=== БАЗА ДАНИХ ПІСЛЯ СОРТУВАННЯ ЗА ВІКОМ ===");
            Array.Sort(database);
            foreach (var person in database)
            {
                person.PrintInfo();
            }
            Console.WriteLine();

            // 3. Організація пошуку персон у заданому діапазоні віку
            int minAge = 18;
            int maxAge = 25;
            
            Console.WriteLine($"=== ПОШУК ПЕРСОН ЗА ВІКОМ (від {minAge} до {maxAge} років) ===");
            bool found = false;

            foreach (var person in database)
            {
                int age = person.GetAge();
                if (age >= minAge && age <= maxAge)
                {
                    person.PrintInfo();
                    found = true;
                }
            }

            if (!found)
            {
                Console.WriteLine("Персон у заданому віковому діапазоні не знайдено.");
            }

            Console.ReadLine();
        }
    }
}
