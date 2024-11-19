using System;
using System.Data;
using Microsoft.Data.SqlClient;

class Program
{
    static void Main()
    {
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ClassWork1;Integrated Security=True;Connect Timeout=30;";
        SqlConnection connection = new SqlConnection(connectionString);

        while (true)
        {
            Console.WriteLine("\n--- Меню ---");
            Console.WriteLine("1. Підключитися до бази даних");
            Console.WriteLine("2. Від'єднатися від бази даних");
            Console.WriteLine("3. Відобразити всю інформацію");
            Console.WriteLine("4. Відобразити ПІБ усіх студентів");
            Console.WriteLine("5. Відобразити всі середні оцінки");
            Console.WriteLine("6. Показати студентів із мінімальною оцінкою > N");
            Console.WriteLine("7. Показати унікальні предмети з мінімальними оцінками");
            Console.WriteLine("8. Показати мінімальну середню оцінку");
            Console.WriteLine("9. Показати максимальну середню оцінку");
            Console.WriteLine("10. Кількість студентів із мін. оцінкою з математики");
            Console.WriteLine("11. Кількість студентів із макс. оцінкою з математики");
            Console.WriteLine("12. Кількість студентів у кожній групі");
            Console.WriteLine("13. Середня оцінка групи");
            Console.WriteLine("0. Вийти");

            Console.Write("Ваш вибір: ");
            string choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        ConnectToDatabase(connection);
                        break;
                    case "2":
                        DisconnectFromDatabase(connection);
                        break;
                    case "3":
                        ExecuteQuery(connection, "SELECT * FROM StudentMarks1");
                        break;
                    case "4":
                        ExecuteQuery(connection, "SELECT StudentName FROM StudentMarks1");
                        break;
                    case "5":
                        ExecuteQuery(connection, "SELECT Mark FROM StudentMarks1");
                        break;
                    case "6":
                        Console.Write("Введіть мінімальну оцінку: ");
                        decimal minMark = decimal.Parse(Console.ReadLine());
                        ExecuteQuery(connection, $"SELECT StudentName FROM StudentMarks1 WHERE Mark > {minMark}");
                        break;
                    case "7":
                        ExecuteQuery(connection, "SELECT DISTINCT MinSubject FROM StudentMarks1");
                        break;
                    case "8":
                        ExecuteQuery(connection, "SELECT MIN(Mark) AS MinAverage FROM StudentMarks1");
                        break;
                    case "9":
                        ExecuteQuery(connection, "SELECT MAX(Mark) AS MaxAverage FROM StudentMarks1");
                        break;
                    case "10":
                        ExecuteQuery(connection, "SELECT COUNT(*) FROM StudentMarks1 WHERE MinSubject = 'Math'");
                        break;
                    case "11":
                        ExecuteQuery(connection, "SELECT COUNT(*) FROM StudentMarks1 WHERE MaxSubject = 'Math'");
                        break;
                    case "12":
                        ExecuteQuery(connection, "SELECT GroupName, COUNT(*) AS StudentCount FROM StudentMarks1 GROUP BY GroupName");
                        break;
                    case "13":
                        ExecuteQuery(connection, "SELECT GroupName, AVG(Mark) AS AvgMark FROM StudentMarks1 GROUP BY GroupName");
                        break;
                    case "0":
                        Console.WriteLine("До побачення!");
                        return;
                    default:
                        Console.WriteLine("Невірний вибір, спробуйте ще раз.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }
    }

    static void ConnectToDatabase(SqlConnection connection)
    {
        try
        {
            connection.Open();
            Console.WriteLine("Підключення до бази даних успішне!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка підключення: {ex.Message}");
        }
    }

    static void DisconnectFromDatabase(SqlConnection connection)
    {
        try
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
                Console.WriteLine("Від'єднання від бази даних успішне!");
            }
            else
            {
                Console.WriteLine("Підключення вже закрито.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка від'єднання: {ex.Message}");
        }
    }

    static void ExecuteQuery(SqlConnection connection, string query)
    {
        try
        {
            if (connection.State != ConnectionState.Open)
            {
                Console.WriteLine("Будь ласка, спочатку підключіться до бази даних.");
                return;
            }

            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();

            DataTable table = new DataTable();
            table.Load(reader);
            reader.Close();

            foreach (DataColumn column in table.Columns)
            {
                Console.Write($"{column.ColumnName}\t");
            }
            Console.WriteLine();

            foreach (DataRow row in table.Rows)
            {
                foreach (var item in row.ItemArray)
                {
                    Console.Write($"{item}\t");
                }
                Console.WriteLine();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка виконання запиту: {ex.Message}");
        }
    }
}