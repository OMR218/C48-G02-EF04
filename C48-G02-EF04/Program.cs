using C48_G02_EF04.Models;
using Microsoft.EntityFrameworkCore;

namespace C48_G02_EF04
{
    internal class Program
    {
        static async Task Main()
        {
            await using var dbContext = new BankDbContext();


            // =============================================
            // Main Menu
            // =============================================

            async Task RunMenuAsync()
            {
                bool isRunning = true;

                while (isRunning)
                {
                    Console.Clear();

                    PrintMenu();

                    Console.Write("Enter choice: ");

                    string? choice = Console.ReadLine();

                    try
                    {
                        switch (choice)
                        {
                            case "1":
                                await AddCustomerAsync();
                                break;

                            case "2":
                                await OpenAccountAsync();
                                break;

                            case "3":
                                await UpdateAccountStatusAsync();
                                break;

                            case "4":
                                await RemoveAccountFromCustomerAsync();
                                break;

                            case "5":
                                await ListCustomersAsync();
                                break;

                            case "0":
                                isRunning = false;

                                Console.WriteLine(
                                    "\nThank you for using National Bank Management."
                                );

                                continue;

                            default:
                                WriteError("Invalid choice. Please select from 0 to 5.");
                                break;
                        }
                    }
                    catch (DbUpdateException ex)
                    {
                        WriteError(
                            $"Database Error: {ex.GetBaseException().Message}"
                        );
                    }
                    catch (Exception ex)
                    {
                        WriteError(ex.GetBaseException().Message);
                    }

                    Console.WriteLine(
                        "\nPress any key to return to the menu..."
                    );

                    Console.ReadKey(true);
                }
            }


            // =============================================
            // Display Menu
            // =============================================

            void PrintMenu()
            {
                Console.ForegroundColor = ConsoleColor.Cyan;

                Console.WriteLine("==============================================");
                Console.WriteLine("          National Bank - Management");
                Console.WriteLine("==============================================");

                Console.ResetColor();

                Console.WriteLine("  1) Add a new Customer");

                Console.WriteLine("  2) Open a new Account for a Customer");

                Console.WriteLine("  3) Update Account Status (Active / Closed)");

                Console.WriteLine("  4) Remove an Account from a Customer");

                Console.WriteLine("  5) List all Customers (with accounts)");

                Console.WriteLine("  0) Exit");

                Console.WriteLine("----------------------------------------------");
            }


            // =============================================
            // Option 1: Add New Customer
            // =============================================

            async Task AddCustomerAsync()
            {
                Console.WriteLine("\n--- Add New Customer ---");

                string fullName = ReadRequiredText("Full Name: ");

                string nationalId = ReadRequiredText("National ID: ");

                DateTime dateOfBirth = ReadDate(
                    "Date of Birth (yyyy-MM-dd): "
                );

                string email = ReadEmail("Email: ");

                string phone = ReadRequiredText("Phone: ");

                string address = ReadRequiredText("Address: ");

                CustomerType customerType = ReadEnumChoice(
                    "Customer Type:\n" +
                    "  1) Individual\n" +
                    "  2) Business\n" +
                    "Choice: ",

                    new Dictionary<int, CustomerType>
                    {
            { 1, CustomerType.Individual },
            { 2, CustomerType.Business }
                    }
                );

                // Check National ID

                bool nationalIdExists = await dbContext.Customers
                    .AnyAsync(c => c.NationalId == nationalId);

                if (nationalIdExists)
                {
                    WriteError("This National ID already exists.");
                    return;
                }

                // Check Email

                bool emailExists = await dbContext.Customers
                    .AnyAsync(c => c.Email == email);

                if (emailExists)
                {
                    WriteError("This Email already exists.");
                    return;
                }

                // Create Customer

                var customer = new Customer
                {
                    FullName = fullName,
                    NationalId = nationalId,
                    DateOfBirth = dateOfBirth,
                    Email = email,
                    PhoneNumber = phone,
                    Address = address,
                    CustomerType = customerType
                };

                await dbContext.Customers.AddAsync(customer);

                await dbContext.SaveChangesAsync();

                WriteSuccess(
                    $"Customer created successfully. CustomerId = {customer.Id}"
                );
            }


            // =============================================
            // Option 2: Open New Account
            // =============================================

            async Task OpenAccountAsync()
            {
                Console.WriteLine("\n--- Open New Account ---");

                string accountNumber = ReadRequiredText(
                    "Account Number: "
                );

                AccountType accountType = ReadEnumChoice(
                    "Account Type:\n" +
                    "  1) Savings\n" +
                    "  2) Current\n" +
                    "  3) Business\n" +
                    "Choice: ",

                    new Dictionary<int, AccountType>
                    {
            { 1, AccountType.Savings },
            { 2, AccountType.Current },
            { 3, AccountType.Business }
                    }
                );

                string branchCode = ReadRequiredText(
                    "Branch Code: "
                ).ToUpperInvariant();

                int customerId = ReadPositiveInt("Customer Id: ");

                OwnershipType ownershipType = ReadEnumChoice(
                    "Ownership Role:\n" +
                    "  1) Primary\n" +
                    "  2) CoHolder\n" +
                    "Choice: ",

                    new Dictionary<int, OwnershipType>
                    {
            { 1, OwnershipType.Primary },
            { 2, OwnershipType.CoHolder }
                    }
                );

                // Check Account Number

                bool accountExists = await dbContext.Accounts
                    .AnyAsync(a => a.AccountNumber == accountNumber);

                if (accountExists)
                {
                    WriteError("This Account Number already exists.");
                    return;
                }

                // Check Branch

                var branch = await dbContext.Branches
                    .FirstOrDefaultAsync(b => b.Code == branchCode);

                if (branch == null)
                {
                    WriteError($"Branch '{branchCode}' does not exist.");
                    return;
                }

                // Check Customer

                var customer = await dbContext.Customers
                    .FirstOrDefaultAsync(c => c.Id == customerId);

                if (customer == null)
                {
                    WriteError($"Customer with ID {customerId} does not exist.");
                    return;
                }

                // Create Account

                var account = new Account
                {
                    AccountNumber = accountNumber,
                    AccountType = accountType,
                    BranchCode = branchCode,
                    CurrentBalance = 0,
                    OpeningDate = DateTime.Now
                };

                // Create CustomerAccount Relationship

                var customerAccount = new CustomerAccount
                {
                    AccountNumber = accountNumber,
                    CustomerId = customerId,
                    OwnershipType = ownershipType,
                    OwnershipStartDate = DateTime.Today,
                    AccountStatus = AccountStatus.Active
                };

                // Save Both Entities in One Transaction

                await using var transaction =
                    await dbContext.Database.BeginTransactionAsync();

                await dbContext.Accounts.AddAsync(account);

                await dbContext.CustomerAccounts.AddAsync(customerAccount);

                await dbContext.SaveChangesAsync();

                await transaction.CommitAsync();

                WriteSuccess(
                    $"Account '{accountNumber}' created and linked to customer {customerId} as {ownershipType} owner."
                );
            }


            // =============================================
            // Option 3: Update Account Status
            // =============================================

            async Task UpdateAccountStatusAsync()
            {
                Console.WriteLine("\n--- Update Account Status ---");

                string accountNumber = ReadRequiredText(
                    "Account Number: "
                );

                int customerId = ReadPositiveInt("Customer Id: ");

                AccountStatus newStatus = ReadEnumChoice(
                    "New Status:\n" +
                    "  1) Active\n" +
                    "  2) Closed\n" +
                    "Choice: ",

                    new Dictionary<int, AccountStatus>
                    {
            { 1, AccountStatus.Active },
            { 2, AccountStatus.Closed }
                    }
                );

                // Find CustomerAccount

                var customerAccount = await dbContext.CustomerAccounts
                    .FirstOrDefaultAsync(ca =>
                        ca.AccountNumber == accountNumber &&
                        ca.CustomerId == customerId
                    );

                if (customerAccount == null)
                {
                    WriteError(
                        "This account is not linked to the specified customer."
                    );

                    return;
                }

                // Update Status

                customerAccount.AccountStatus = newStatus;

                await dbContext.SaveChangesAsync();

                WriteSuccess($"Status updated to {newStatus}.");
            }


            // =============================================
            // Option 4: Remove Account From Customer
            // =============================================

            async Task RemoveAccountFromCustomerAsync()
            {
                Console.WriteLine("\n--- Remove Account From Customer ---");

                string accountNumber = ReadRequiredText(
                    "Account Number: "
                );

                int customerId = ReadPositiveInt("Customer Id: ");

                // Find CustomerAccount

                var customerAccount = await dbContext.CustomerAccounts
                    .FirstOrDefaultAsync(ca =>
                        ca.AccountNumber == accountNumber &&
                        ca.CustomerId == customerId
                    );

                if (customerAccount == null)
                {
                    WriteError(
                        "This account is not linked to the specified customer."
                    );

                    return;
                }

                // Remove Relationship

                dbContext.CustomerAccounts.Remove(customerAccount);

                await dbContext.SaveChangesAsync();

                // Check Remaining Owners

                bool hasOtherOwners = await dbContext.CustomerAccounts
                    .AnyAsync(ca => ca.AccountNumber == accountNumber);

                if (!hasOtherOwners)
                {
                    // Delete Account If It Has No Owners

                    var account = await dbContext.Accounts
                        .Include(a => a.Transactions)
                        .FirstOrDefaultAsync(a =>
                            a.AccountNumber == accountNumber
                        );

                    if (account != null)
                    {
                        dbContext.Accounts.Remove(account);

                        await dbContext.SaveChangesAsync();
                    }

                    WriteSuccess(
                        $"Ownership link deleted. Account '{accountNumber}' was also removed because it had no remaining owners."
                    );
                }
                else
                {
                    WriteSuccess(
                        "Ownership link deleted. The account still has other owners."
                    );
                }
            }


            // =============================================
            // Option 5: List All Customers With Accounts
            // =============================================

            async Task ListCustomersAsync()
            {
                Console.WriteLine("\n--- All Customers ---");

                // Eager Loading

                var customers = await dbContext.Customers
                    .AsNoTracking()
                    .Include(c => c.CustomerAccounts)
                        .ThenInclude(ca => ca.Account)
                            .ThenInclude(a => a.Branch)
                    .OrderBy(c => c.Id)
                    .ToListAsync();

                if (customers.Count == 0)
                {
                    Console.WriteLine("No customers found.");
                    return;
                }

                foreach (var customer in customers)
                {
                    Console.WriteLine(
                        $"\n#{customer.Id} {customer.FullName} ({customer.CustomerType})"
                    );

                    if (customer.CustomerAccounts.Count == 0)
                    {
                        Console.WriteLine("   (no accounts)");
                        continue;
                    }

                    foreach (var customerAccount in customer.CustomerAccounts)
                    {
                        var account = customerAccount.Account;

                        Console.WriteLine(
                            $"   {account.AccountNumber,-12} " +
                            $"{account.AccountType,-10} " +
                            $"Balance: {account.CurrentBalance,12:N2} " +
                            $"{customerAccount.OwnershipType,-8} " +
                            $"{customerAccount.AccountStatus,-8} " +
                            $"@ {account.Branch.Name}"
                        );
                    }
                }
            }


            // =============================================
            // Input Validation Methods
            // =============================================

            string ReadRequiredText(string prompt)
            {
                while (true)
                {
                    Console.Write(prompt);

                    string? value = Console.ReadLine()?.Trim();

                    if (!string.IsNullOrWhiteSpace(value))
                        return value;

                    WriteError("This field is required. Please try again.");
                }
            }


            string ReadEmail(string prompt)
            {
                while (true)
                {
                    string email = ReadRequiredText(prompt);

                    try
                    {
                        _ = new System.Net.Mail.MailAddress(email);

                        return email;
                    }
                    catch
                    {
                        WriteError("Please enter a valid email address.");
                    }
                }
            }


            DateTime ReadDate(string prompt)
            {
                while (true)
                {
                    Console.Write(prompt);

                    if (DateTime.TryParse(
                        Console.ReadLine(),
                        out DateTime date) &&
                        date.Date < DateTime.Today)
                    {
                        return date.Date;
                    }

                    WriteError("Please enter a valid date in the past.");
                }
            }


            int ReadPositiveInt(string prompt)
            {
                while (true)
                {
                    Console.Write(prompt);

                    if (int.TryParse(
                        Console.ReadLine(),
                        out int value) &&
                        value > 0)
                    {
                        return value;
                    }

                    WriteError("Please enter a valid positive number.");
                }
            }


            TEnum ReadEnumChoice<TEnum>(
                string prompt,
                Dictionary<int, TEnum> choices)
                where TEnum : struct, Enum
            {
                while (true)
                {
                    Console.Write(prompt);

                    if (int.TryParse(Console.ReadLine(), out int choice) &&
                        choices.TryGetValue(choice, out TEnum value))
                    {
                        return value;
                    }

                    WriteError("Invalid selection. Please try again.");
                }
            }


            // =============================================
            // Console Formatting
            // =============================================

            void WriteSuccess(string message)
            {
                Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine(message);

                Console.ResetColor();
            }


            void WriteError(string message)
            {
                Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine(message);

                Console.ResetColor();
            }
        }
    }
}
