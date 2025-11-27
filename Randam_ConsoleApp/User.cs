using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Randam_ConsoleApp;


public class User
{
    // 1. Identifiers and Core Data (5)
    public int Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;

    // 2. Contact Information (5)
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;

    // 3. Dates and Time (5)
    public DateTime RegistrationDate { get; set; }
    public DateTime LastLogin { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int BirthYear => DateOfBirth.Year;
    public TimeSpan TimeUTC { get; set; }

    // 4. Numeric Metrics (5)
    public int Age => DateTime.Now.Year - DateOfBirth.Year;
    public decimal BalanceUSD { get; set; }
    public int PurchaseCount { get; set; }
    public double Rating { get; set; }
    public int DiscountPercent { get; set; }

    // 5. Status and Logic (5)
    public string Status { get; set; } = string.Empty;
    public bool IsVIPClient { get; set; }
    public bool EmailConfirmed { get; set; }
    public bool SubscriptionActive { get; set; }
    public bool ReceiveNewsletter { get; set; }

    // 6. Additional Data (5)
    public string Priority { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public string TrafficSource { get; set; } = string.Empty;
    public string LastProduct { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
}
