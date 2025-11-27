using Microsoft.Data.SqlClient;
using Randam_ConsoleApp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

// !!! ВАЖНО !!! Если вы не переименовывали namespace в Data.cs, удалите эту строку:
// using Randam_ConsoleApp; 

public class Program
{
    // --- Configuration ---
    // Установлено 10 для быстрой проверки после устранения ошибки таймаута
    private const int NumRecords = 1000000;

    // ВАЖНО: Убедитесь, что эта строка подключения корректна для вашего SQL Server.
    // ТЕКУЩАЯ СТРОКА:
    // private const string ConnectionString = "Server=localhost;Database=ADB;Trusted_Connection=True;TrustServerCertificate=True;Timeout=60;";

    // ЕСЛИ ОШИБКА ТАЙМАУТА ПРОДОЛЖАЕТСЯ, ПОПРОБУЙТЕ ОДНУ ИЗ ЭТИХ АЛЬТЕРНАТИВ:
    // Альтернатива 1 (для SQL Express):
    // private const string ConnectionString = "Server=.\\SQLEXPRESS;Database=ADB;Trusted_Connection=True;TrustServerCertificate=True;Timeout=60;";
    // Альтернатива 2 (для LocalDB, часто используется в Visual Studio):
    // private const string ConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=ADB;Trusted_Connection=True;TrustServerCertificate=True;Timeout=60;";

    // Я оставляю вашу текущую строку, но добавил в неё "Timeout=60"
    private const string ConnectionString = "Server=localhost;Database=ADB;Trusted_Connection=True;TrustServerCertificate=True;Timeout=60;";
    private const string DestinationTableName = "Users";

    public static void Main(string[] args)
    {
        Console.WriteLine($"Generating {NumRecords} random user records...");

        var users = DataGenerator.GenerateRandomUsers(NumRecords, 1001);

        Console.WriteLine("Generation complete. Starting Bulk Insert...");

        // Step 1: Convert List<User> to DataTable
        DataTable userTable = ListToDataTable(users);

        // Step 2: Execute the Bulk Copy operation
        BulkInsertData(userTable, ConnectionString, DestinationTableName);
        Console.WriteLine("\nPress Enter to exit...");
        Console.ReadLine();
    }

    /// <summary>
    /// Converts a List<User> to a DataTable. Required for SqlBulkCopy.
    /// </summary>
    public static DataTable ListToDataTable(List<User> users)
    {
        var dataTable = new DataTable();
        // Используем BindingFlags, чтобы получить все 30 публичных свойств из класса User
        var properties = typeof(User).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        // 1. Add Columns to DataTable
        foreach (var prop in properties)
        {
            Type type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
            dataTable.Columns.Add(prop.Name, type);
        }

        // 2. Add Rows to DataTable
        foreach (var user in users)
        {
            var row = dataTable.NewRow();
            foreach (var prop in properties)
            {
                if (prop.CanRead)
                {
                    row[prop.Name] = prop.GetValue(user) ?? DBNull.Value;
                }
            }
            dataTable.Rows.Add(row);
        }
        return dataTable;
    }

    /// <summary>
    /// Performs the high-speed bulk insert operation into SQL Server, 
    /// explicitly setting a timeout and batch size to prevent common errors.
    /// </summary>
    public static void BulkInsertData(DataTable sourceTable, string connectionString, string destinationTable)
    {
        try
        {
            using (var connection = new SqlConnection(connectionString))
            {
                // Если таймаут происходит здесь (connection.Open()), проблема в имени сервера или сети
                connection.Open();
                Console.WriteLine("✅ Database connection successful. Preparing Bulk Copy...");

                using (var bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = destinationTable;

                    // *** УСТАНОВКА ТАЙМАУТА КОПИРОВАНИЯ В 60 СЕКУНД ***
                    bulkCopy.BulkCopyTimeout = 60;

                    // Установка внутреннего размера пакета
                    bulkCopy.BatchSize = 10000;

                    // Map ALL columns from the DataTable to the SQL Table.
                    foreach (DataColumn column in sourceTable.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                    }

                    bulkCopy.WriteToServer(sourceTable);
                }
            }
            // ИСПРАВЛЕНИЕ: Изменено 'destinationTableName' на 'destinationTable' (совпадает с параметром)
            Console.WriteLine($"🎉 Success! Inserted {sourceTable.Rows.Count} records into the '{destinationTable}' table.");
        }
        catch (SqlException ex)
        {
            Console.WriteLine("❌ SQL Error: Failed to write data. Check connection string, table schema, and server name.");
            Console.WriteLine($"Error Message: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ General Error: {ex.Message}");
        }
    }
}