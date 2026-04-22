using System;

namespace EnterpriseHierarchy
{
    // 1. Абстрактний базовий клас "Організація"
    // Реалізує стандартний .NET інтерфейс IComparable
    public abstract class Organization : IComparable<Organization>
    {
        public string Name { get; set; }
        public int EmployeeCount { get; set; }

        // Масив для демонстрації індексатора (відділи організації)
        private string[] departments = new string[3];

        public Organization(string name, int employeeCount)
        {
            Name = name;
            EmployeeCount = employeeCount;
        }

        // Індексатор
        public string this[int index]
        {
            get
            {
                if (index >= 0 && index < departments.Length)
                    return departments[index];
                return "Невідомий відділ";
            }
            set
            {
                if (index >= 0 && index < departments.Length)
                    departments[index] = value;
            }
        }

        // Не віртуальний метод
        public void HireEmployee()
        {
            EmployeeCount++;
            Console.WriteLine($"[{Name}] Найнято нового співробітника. Загальна кількість: {EmployeeCount}");
        }

        // Абстрактний метод (реалізація обов'язкова в похідних класах)
        public abstract void DisplayActivity();

        // Віртуальний метод (можна перевизначити)
        public virtual void PrintInfo()
        {
            Console.WriteLine($"Організація: {Name}, Співробітників: {EmployeeCount}");
        }

        // Перевантаження оператора + (наприклад, злиття компаній - сумуємо співробітників)
        public static Organization operator +(Organization a, Organization b)
        {
            // Для простоти повернемо новий безіменний завод з сумарною кількістю співробітників
            return new Factory($"{a.Name} + {b.Name} (Об'єднання)", a.EmployeeCount + b.EmployeeCount, "Змішана продукція");
        }

        // Реалізація інтерфейсу .NET (IComparable)
        public int CompareTo(Organization other)
        {
            if (other == null) return 1;
            return this.EmployeeCount.CompareTo(other.EmployeeCount); // Порівняння за кількістю співробітників
        }

        // Деструктор (Finalizer)
        ~Organization()
        {
            // Спрацьовує під час збирання сміття
            Console.WriteLine($"[Деструктор] Організацію {Name} закрито/видалено з пам'яті.");
        }
    }

    // 2. Похідний клас: Страхова компанія
    public class InsuranceCompany : Organization
    {
        public int ClientsCount { get; set; }

        public InsuranceCompany(string name, int employeeCount, int clientsCount) 
            : base(name, employeeCount)
        {
            ClientsCount = clientsCount;
        }

        public override void DisplayActivity()
        {
            Console.WriteLine($"[{Name}] Діяльність: Надання страхових послуг.");
        }

        public override void PrintInfo()
        {
            base.PrintInfo();
            Console.WriteLine($"   Кількість застрахованих клієнтів: {ClientsCount}");
        }

        // Особистий метод
        public void IssuePolicy()
        {
            Console.WriteLine($"[{Name}] Випущено новий страховий поліс!");
        }
    }

    // 3. Похідний клас: Нафтогазова компанія
    public class OilGasCompany : Organization
    {
        public double OilReserves { get; set; } // У барелях

        public OilGasCompany(string name, int employeeCount, double oilReserves) 
            : base(name, employeeCount)
        {
            OilReserves = oilReserves;
        }

        public override void DisplayActivity()
        {
            Console.WriteLine($"[{Name}] Діяльність: Видобуток та переробка нафти і газу.");
        }

        // Особистий метод
        public void ExtractOil()
        {
            Console.WriteLine($"[{Name}] Видобуто 1000 барелів нафти. Залишок резервів: {OilReserves - 1000}");
            OilReserves -= 1000;
        }
    }

    // 4. Похідний клас: Завод
    public class Factory : Organization
    {
        public string ProductName { get; set; }

        public Factory(string name, int employeeCount, string productName) 
            : base(name, employeeCount)
        {
            ProductName = productName;
        }

        public override void DisplayActivity()
        {
            Console.WriteLine($"[{Name}] Діяльність: Виробництво товарів ({ProductName}).");
        }

        // Особистий метод
        public void ProduceGoods()
        {
            Console.WriteLine($"[{Name}] Виготовлено нову партію товару: {ProductName}");
        }
    }

    // Головний клас для тестування
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Створення об'єктів
            var insurance = new InsuranceCompany("Оранта", 500, 15000);
            var oilGas = new OilGasCompany("Укрнафта", 12000, 5000000);
            var factory = new Factory("Завод ім. Малишева", 3000, "Важке машинобудування");

            // Тестування індексатора
            factory[0] = "Складальний цех";
            factory[1] = "Відділ якості";
            Console.WriteLine($"Відділ 0 на заводі: {factory[0]}");
            Console.WriteLine(new string('-', 40));

            // Тестування перевантаження оператора
            var combined = insurance + factory;
            Console.WriteLine($"Результат злиття: {combined.Name}, Співробітників: {combined.EmployeeCount}");
            Console.WriteLine(new string('-', 40));

            // --- СТВОРЕННЯ МАСИВУ ІНТЕРФЕЙСНИХ ПОСИЛАНЬ ---
            IComparable<Organization>[] comparableOrganizations = new IComparable<Organization>[]
            {
                insurance,
                oilGas,
                factory
            };

            Console.WriteLine("--- Виклик інтерфейсних методів (CompareTo) ---");
            // Порівнюємо першу компанію з другою через інтерфейсний метод
            int comparisonResult = comparableOrganizations[0].CompareTo(oilGas);
            if (comparisonResult < 0)
                Console.WriteLine($"{insurance.Name} менша за {oilGas.Name} за кількістю працівників.");
            
            Console.WriteLine(new string('-', 40));

            // --- ДЕМОНСТРАЦІЯ typeof, is, as ---
            Console.WriteLine("--- Демонстрація ідентифікації типів (is, as, typeof) ---");
            foreach (var item in comparableOrganizations)
            {
                // Використання as
                Organization org = item as Organization;
                if (org != null)
                {
                    org.PrintInfo(); // Виклик віртуального методу
                }

                // Використання typeof
                if (item.GetType() == typeof(OilGasCompany))
                {
                    Console.WriteLine(">>> Це точно нафтогазова компанія (перевірено через typeof)!");
                }
            }
            Console.WriteLine(new string('-', 40));

            // --- ВИКЛИК ОСОБЛИВИХ МЕТОДІВ ЧЕРЕЗ TYPE PATTERN ---
            Console.WriteLine("--- Виклик специфічних методів (Type Pattern) ---");
            CallSpecificMethods(comparableOrganizations);
            
            Console.ReadLine();
        }

        // Метод, який використовує паттерн типу (Type Pattern)
        static void CallSpecificMethods(IComparable<Organization>[] array)
        {
            foreach (var item in array)
            {
                // Використання switch з паттернами типу (C# 7.0+)
                switch (item)
                {
                    case InsuranceCompany ic:
                        Console.WriteLine($"[Тип: Страхова компанія]");
                        ic.IssuePolicy();
                        break;

                    case OilGasCompany ogc:
                        Console.WriteLine($"[Тип: Нафтогазова компанія]");
                        ogc.ExtractOil();
                        break;

                    case Factory f:
                        Console.WriteLine($"[Тип: Завод]");
                        f.ProduceGoods();
                        break;

                    case Organization org: // Fallback для базового типу, якщо додадуть інші
                        org.DisplayActivity();
                        break;

                    default:
                        Console.WriteLine("Невідомий тип об'єкта.");
                        break;
                }
                Console.WriteLine();
            }
        }
    }
}
