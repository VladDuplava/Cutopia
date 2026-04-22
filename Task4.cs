using System;
using System.Collections; // Обов'язково для IEnumerable та IEnumerator

namespace EnumerableTask
{
    // 1. Клас реалізує інтерфейс IEnumerable, щоб дозволити використання foreach
    public abstract class Organization : IEnumerable
    {
        public string Name { get; set; }
        
        // Масив елементів, який ми будемо перебирати (відділи організації)
        private string[] departments;

        public Organization(string name, params string[] depts)
        {
            Name = name;
            // Якщо масив не передано, створюємо порожній, щоб уникнути NullReferenceException
            departments = depts ?? new string[0]; 
        }

        // Індексатор залишається для прямого доступу (як у 1-му завданні)
        public string this[int index]
        {
            get { return departments[index]; }
            set { departments[index] = value; }
        }

        // РЕАЛІЗАЦІЯ ІНТЕРФЕЙСУ IEnumerable
        // Метод повертає об'єкт, який знає, як перебирати внутрішню колекцію
        public IEnumerator GetEnumerator()
        {
            return new OrganizationEnumerator(departments);
        }
    }

    // 2. Власний клас-перелічувач, що реалізує IEnumerator
    public class OrganizationEnumerator : IEnumerator
    {
        private string[] _departments;
        // Початкова позиція ітератора (до першого елемента)
        private int position = -1;

        public OrganizationEnumerator(string[] departments)
        {
            _departments = departments;
        }

        // Перехід до наступного елемента
        public bool MoveNext()
        {
            position++;
            return (position < _departments.Length);
        }

        // Скидання ітератора на початок
        public void Reset()
        {
            position = -1;
        }

        // Отримання поточного елемента
        public object Current
        {
            get
            {
                try
                {
                    return _departments[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException("Перелічувач вийшов за межі колекції.");
                }
            }
        }
    }

    // Похідний клас для демонстрації
    public class Factory : Organization
    {
        public string ProductName { get; set; }

        public Factory(string name, string productName, params string[] depts) 
            : base(name, depts)
        {
            ProductName = productName;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Створюємо завод і передаємо список його відділів
            Factory myFactory = new Factory(
                "Завод 'Електрон'", 
                "Мікросхеми", 
                "Цех збірки", 
                "Відділ тестування", 
                "Склад готової продукції", 
                "Бухгалтерія"
            );

            Console.WriteLine($"Організація: {myFactory.Name}");
            Console.WriteLine("Перелік відділів (використання циклу foreach):");
            Console.WriteLine(new string('-', 40));

            // ДЕМОНСТРАЦІЯ: Тепер ми можемо використовувати foreach для об'єкта Organization
            // Під капотом foreach викликає myFactory.GetEnumerator() і потім MoveNext() / Current
            foreach (string department in myFactory)
            {
                Console.WriteLine($"- {department}");
            }

            Console.WriteLine(new string('-', 40));
            Console.ReadLine();
        }
    }
}
