using System;
using System.Collections.Generic;
using ATMApp;

class Program
{
    static void Main(string[] args)
    {
        var bank = new Bank("My Bank");
        var atm = new AutomatedTellerMachine("ATM001", "Main Street", 10000);
        bank.AddATM(atm);

        var accounts = new List<Account>
        {
            new Account("1234567890123456", "1234", "John Doe", 5000),
            new Account("9876543210987654", "5678", "Jane Smith", 3000)
        };

        atm.OperationEvent += Console.WriteLine;

        Console.WriteLine("Welcome to the ATM!");
        Console.Write("Enter your card number: ");
        string cardNumber = Console.ReadLine();
        
        Account currentAccount = ATMService.GetAccountByCardNumber(accounts, cardNumber);

        if (currentAccount == null)
        {
            Console.WriteLine("Invalid card number.");
            return;
        }

        Console.Write("Enter your PIN: ");
        string pin = Console.ReadLine();

        if (!atm.Authenticate(currentAccount, pin))
        {
            Console.WriteLine("Authentication failed.");
            return;
        }

        bool running = true;
        while (running)
        {
            Console.WriteLine("\n1. Check Balance\n2. Withdraw\n3. Deposit\n4. Transfer\n5. Exit");
            Console.Write("Choose an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    atm.CheckBalance(currentAccount);
                    break;
                case "2":
                    Console.Write("Enter amount to withdraw: ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal withdrawAmount))
                    {
                        atm.Withdraw(currentAccount, withdrawAmount);
                    }
                    break;
                case "3":
                    Console.Write("Enter amount to deposit: ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal depositAmount))
                    {
                        atm.Deposit(currentAccount, depositAmount);
                    }
                    break;
                case "4":
                    Console.Write("Enter receiver card number: ");
                    string receiverCard = Console.ReadLine();
                    Account receiverAccount = ATMService.GetAccountByCardNumber(accounts, receiverCard);

                    if (receiverAccount == null)
                    {
                        Console.WriteLine("Invalid receiver card number.");
                        break;
                    }

                    Console.Write("Enter amount to transfer: ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal transferAmount))
                    {
                        atm.Transfer(currentAccount, receiverAccount, transferAmount);
                    }
                    break;
                case "5":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }

        Console.WriteLine("Thank you for using the ATM. Goodbye!");
    }
}