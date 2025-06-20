using System;
using System.Linq;

//Класс "Продукт"
class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public string Category { get; set; }
    public Product(int id, string name, int price, string category)
    {
        Id = id;
        Name = name;
        Price = price;
        Category = category;
    }
}

//Класс "Заказ"
class Order
{
    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public DateTime OrderDate { get; set; }
    public Order (int orderId, int customerId, int productId, int quantity,DateTime orderDate)
    {
        OrderId = orderId;
        CustomerId = customerId;
        ProductId = productId;
        Quantity = quantity;
        OrderDate = orderDate;
    }
}

//Класс "Покупатель"
class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    public Customer(int id, string name, string city)
    {
        Id = id;
        Name = name;
        City = city;
    }
}

class Programm
{
    static void Main(string[] args)
    {
        //Список продуктов
        List<Product> products = new List<Product>
        {
            new Product(1, "Шоколад", 74, "Сладости"),
            new Product(2, "Молоко", 89, "Молочная продукция"),
            new Product(3, "Сыр", 255, "Молочная продукция"),
            new Product(4, "Рис", 99, "Крупы"),
            new Product(5, "Крупа гречневая", 40, "Крупы"),
            new Product(6, "Наггетсы", 209, "Замороженные продукты"),
            new Product(7, "Мороженое", 80, "Замороженные продукты"),
            new Product(8, "Энергетик", 119, "Напитки"),
            new Product(9, "Сырок", 55, "Молочная продукция"),
            new Product(10, "Сливки", 79, "Молочная продукция"),
            new Product(11, "Мармеладки", 39, "Сладости"),
            new Product(12, "Сок вишневый", 120, "Напитки"),
            new Product(13, "Булочка с корицей", 49, "Выпечка"),
        };
        //Список покупателей
        List<Customer> customers = new List<Customer>
        {
            new Customer(1, "Анна Ковалева", "Москва"),
            new Customer(2, "Виктор Серов", "Нижний Новгород"),
            new Customer(3, "Алина Панчук", "Краснодар"),
            new Customer(4, "Михаил Антонов", "Тверь"),
            new Customer(5, "Максим Светлов", "Череповец"),
            new Customer(6, "Артур Богтюжский", "Вологда"),
        };
        //Список заказов
        List<Order> orders = new List<Order>
        {

            new Order(1, 6, 1, 1, new DateTime(2022, 12, 13)),
            new Order(2, 1, 4, 2, new DateTime(2022, 12, 15)),
            new Order(3, 1, 3, 1, new DateTime(2022, 12, 15)),
            new Order(4, 1, 12, 3, new DateTime(2022, 12, 19)),
            new Order(5, 4, 5, 1, new DateTime(2022, 12, 20)),
            new Order(6, 1, 6, 1, new DateTime(2022, 12, 21)),
            new Order(7, 2, 13, 1, new DateTime(2022, 12, 23)),
            new Order(8, 5, 1, 2, new DateTime(2022, 12, 28)),
            new Order(9, 2, 8, 5, new DateTime(2022, 12, 29)),
            new Order(10, 3, 2, 1, new DateTime(2022, 12, 29)),
            new Order(11, 6, 2, 1, new DateTime(2023, 1, 3)),
            new Order(12, 1, 13, 2, new DateTime(2023, 1, 3)),
            new Order(13, 1, 10, 1, new DateTime(2023, 1, 5)),
            new Order(14, 1, 9, 3, new DateTime(2023, 1, 6)),
            new Order(15, 4, 1, 1, new DateTime(2023, 1,7)),
            new Order(16, 1, 6, 1, new DateTime(2023, 1, 13)),
            new Order(17, 2, 7, 1, new DateTime(2023, 1, 15)),
            new Order(18, 5, 9, 2, new DateTime(2023, 1, 16)),
            new Order(19, 2, 13, 5, new DateTime(2023, 1, 18)),
            new Order(20, 3, 10, 1, new DateTime(2023, 1, 25))
        };

        //Фильтрация
        var recentOrders = orders.Where(o => o.OrderDate > new DateTime(2023, 1, 1)).ToList();

        Console.WriteLine("Заказы, сделанные после 1 января 2023 года:");
        foreach (var order in recentOrders)
        {
            Console.WriteLine($"ID заказа: {order.OrderId}, Дата заказа: {order.OrderDate}");
        }

        //Проекция
        var productListFromOrders = orders
            .Join(products, o => o.ProductId, p => p.Id, (order, product) => new { product.Name, product.Price })
            .ToList();

        Console.WriteLine("\nСписок названий продуктов и их цен из всех заказов:");
        foreach (var item in productListFromOrders)
        {
            Console.WriteLine($"Название продукта: {item.Name}, Цена: {item.Price}");
        }

        //Сортировка
        var sortedOrders = orders.OrderByDescending(o => o.OrderDate).ToList();
        Console.WriteLine("Отсортированные заказы по дате:");
        foreach (var order in sortedOrders)
        {
            Console.WriteLine($"Id заказа: {order.OrderId}, Дата заказа: {order.OrderDate}");
        }

        //Группировка
        var groupedOrders = orders.GroupBy(o => o.ProductId)
            .Select(g => new {
                ProductId = g.Key,
                TotalQuantity = g.Sum(o => o.Quantity)
            })
            .ToList();

        Console.WriteLine("\nОбщие проданные единицы по продуктам:");
        foreach (var group in groupedOrders)
        {
            var product = products.FirstOrDefault(p => p.Id == group.ProductId);
            Console.WriteLine($"Продукт: {product?.Name}, Общее количество: {group.TotalQuantity}");
        }

        //Объединение
        var query = from customer in customers
                    join order in orders on customer.Id equals order.CustomerId
                    join product in products on order.ProductId equals product.Id
                    select $"Клиент: {customer.Name}, Заказ: {product.Name}, Количество: {order.Quantity}";

        Console.WriteLine("Информация о клиентах и заказах:");
        foreach (var result in query)
        {
            Console.WriteLine(result);
        }


    }
}
