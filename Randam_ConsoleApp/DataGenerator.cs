using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Randam_ConsoleApp;


public static class DataGenerator
{
    private static readonly Random Rnd = new Random();

    // Random Value Lists
    private static readonly string[] Names = { "Ivan", "Anna", "Dmitry", "Elena", "Sergey", "Maria", "Pavel", "Olga", "Alexey", "Tatyana", "Mikhail", "Natalia", "Vladimir", "Ekaterina", "Nikita", "Yulia", "Roman", "Svetlana", "Andrey", "Sofia", "Igor", "Vera", "Anton", "Zlata", "Oleg", "Ksenia", "Rustam", "Alina" };
    private static readonly string[] LastNames = { "Smirnov", "Kozlova", "Ivanov", "Vasilieva", "Petrov", "Morozova", "Volkov", "Egorova", "Novikov", "Pavlova", "Lebedev", "Sokolova", "Kuznetsov", "Makarova", "Zakharov", "Borisova", "Orlov", "Fedorova", "Gerasimov", "Komarova", "Alekseev", "Kiselev", "Shishkin", "Fokin", "Kirillov", "Mironov", "Golubev", "Belov" };
    private static readonly string[] Countries = { "Russia", "Ukraine", "Kazakhstan", "Germany", "USA", "UK", "Canada", "France", "Spain", "Italy", "Poland", "Belarus", "China", "India", "Brazil", "Mexico", "Australia", "Japan", "South Korea", "Sweden" };
    private static readonly string[] Cities = { "Moscow", "Kyiv", "St. Petersburg", "Minsk", "Astana", "Berlin", "New York", "London", "Paris", "Toronto", "Madrid", "Rome", "Warsaw", "Shanghai", "Mumbai", "Sao Paulo", "Mexico City", "Sydney", "Tokyo", "Seoul" };
    private static readonly string[] Domains = { "test.ru", "mail.com", "corp.org", "example.net", "tech.info", "data.io", "web.site", "cloud.co", "solutions.biz", "global.agency", "my-app.dev" };
    private static readonly string[] Statuses = { "Active", "Inactive", "Suspended", "Deleted", "Trial", "Pending Verification", "On Hold", "Churned" };
    private static readonly string[] Priorities = { "High", "Medium", "Low", "Critical", "Routine", "Support", "Sales" };
    private static readonly string[] Languages = { "RU", "EN", "DE", "FR", "ES", "IT", "PL", "CN", "JP", "KR", "PT" };
    private static readonly string[] TrafficSources = { "Organic", "Direct", "Social", "Referral", "Ad", "Email", "SEO", "Partner", "Webinar", "Podcast" };
    private static readonly string[] Products = { "Product A", "Service B", "License C", "Item D", "Subscription E", "Widget F", "Consulting G", "API Access", "Pro Plan", "Free Tier", "None" };
    private static readonly string[] Comments = { "None", "Test Account", "Urgent Processing", "Archive", "Requires Follow-up", "Marketing Lead", "Billing Issue", "High Value Client", "Signed NDA", "Beta Tester" };

    /// <summary>
    /// Generates a list of random User objects.
    /// </summary>
    /// <param name="count">Number of users to generate.</param>
    /// <param name="startId">The starting ID for the first generated user (for batching).</param>
    public static List<User> GenerateRandomUsers(int count, int startId = 1)
    {
        var users = new List<User>();
        for (int i = 0; i < count; i++)
        {
            var firstName = RandomChoice(Names);
            var lastName = RandomChoice(LastNames);
            var birthDate = GenerateRandomDate(1950, 2025);
            var registrationDate = GenerateRandomDate(DateTime.Now.AddYears(-3).Year, DateTime.Now.Year);
            var lastLoginDate = GenerateRandomDate(registrationDate.Year, DateTime.Now.Year);

            var user = new User
            {
                // 1. Identifiers
                Id = startId + i, // Ensure unique IDs
                FirstName = firstName,
                LastName = lastName,
                Gender = Rnd.Next(2) == 0 ? "Male" : "Female",
                Login = $"{firstName.ToLower()[0]}{lastName.ToLower()}{Rnd.Next(10, 99)}",

                // 2. Contact Information
                Email = $"{firstName.ToLower()}.{lastName.ToLower()[0]}@{RandomChoice(Domains)}",
                Phone = $"+12{Rnd.Next(100, 999)}{Rnd.Next(100, 999)}{Rnd.Next(1000, 9999)}",
                Country = RandomChoice(Countries),
                City = RandomChoice(Cities),
                ZipCode = Rnd.Next(10000, 99999).ToString(),

                // 3. Dates and Time
                DateOfBirth = birthDate,
                RegistrationDate = registrationDate,
                LastLogin = lastLoginDate,
                TimeUTC = TimeSpan.FromHours(Rnd.Next(0, 24)),

                // 4. Numeric Metrics
                BalanceUSD = Math.Round((decimal)Rnd.NextDouble() * 15000m, 2),
                PurchaseCount = Rnd.Next(0, 200),
                Rating = Math.Round(Rnd.NextDouble() * 2.0 + 3.0, 1),
                DiscountPercent = Rnd.Next(0, 5) * 5,

                // 5. Status and Logic
                Status = RandomChoice(Statuses),
                IsVIPClient = Rnd.Next(10) < 3,
                EmailConfirmed = Rnd.Next(10) < 9,
                SubscriptionActive = Rnd.Next(10) < 7,
                ReceiveNewsletter = Rnd.Next(10) < 6,

                // 6. Additional Data
                Priority = RandomChoice(Priorities),
                Language = RandomChoice(Languages),
                TrafficSource = RandomChoice(TrafficSources),
                LastProduct = RandomChoice(Products),
                Comment = RandomChoice(Comments)
            };
            users.Add(user);
        }
        return users;
    }

    private static T RandomChoice<T>(T[] array)
    {
        return array[Rnd.Next(array.Length)];
    }

    private static DateTime GenerateRandomDate(int minYear, int maxYear)
    {
        var start = new DateTime(minYear, 1, 1);
        var end = new DateTime(maxYear, 12, 31);
        var timeSpan = end - start;
        var newSpan = new TimeSpan(0, Rnd.Next(0, (int)timeSpan.TotalMinutes), 0);
        return start + newSpan;
    }
}