using System;
using System.Collections.Generic;

namespace ATMApp
{
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
            var currentAccount = AuthenticateUser(atm, accounts);

            if (currentAccount == null)
            {
                Console.WriteLine("Authentication failed. Exiting...");
                return;
            }

            RunATMOperations(atm, currentAccount, accounts);
            Console.WriteLine("Thank you for using the ATM. Goodbye!");
        }

        private static Account AuthenticateUser(AutomatedTellerMachine atm, List<Account> accounts)
        {
            Console.Write("Enter your card number: ");
            string cardNumber = Console.ReadLine();

            var currentAccount = ATMService.GetAccountByCardNumber(accounts, cardNumber);
            if (currentAccount == null)
            {
                Console.WriteLine("Invalid card number.");
                return null;
            }

            Console.Write("Enter your PIN: ");
            string pin = Console.ReadLine();

            if (!atm.Authenticate(currentAccount, pin))
            {
                Console.WriteLine("Authentication failed.");
                return null;
            }

            return currentAccount;
        }

        private static void RunATMOperations(AutomatedTellerMachine atm, Account currentAccount, List<Account> accounts)
        {
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
                        HandleWithdraw(atm, currentAccount);
                        break;
                    case "3":
                        HandleDeposit(atm, currentAccount);
                        break;
                    case "4":
                        HandleTransfer(atm, currentAccount, accounts);
                        break;
                    case "5":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private static void HandleWithdraw(AutomatedTellerMachine atm, Account account)
        {
            Console.Write("Enter amount to withdraw: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
            {
                atm.Withdraw(account, amount);
            }
            else
            {
                Console.WriteLine("Invalid amount. Please enter a positive number.");
            }
        }

        private static void HandleDeposit(AutomatedTellerMachine atm, Account account)
        {
            Console.Write("Enter amount to deposit: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
            {
                atm.Deposit(account, amount);
            }
            else
            {
                Console.WriteLine("Invalid amount. Please enter a positive number.");
            }
        }

        private static void HandleTransfer(AutomatedTellerMachine atm, Account senderAccount, List<Account> accounts)
        {
            Console.Write("Enter receiver card number: ");
            string receiverCard = Console.ReadLine();
            var receiverAccount = ATMService.GetAccountByCardNumber(accounts, receiverCard);

            if (receiverAccount == null)
            {
                Console.WriteLine("Invalid receiver card number.");
                return;
            }

            Console.Write("Enter amount to transfer: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
            {
                atm.Transfer(senderAccount, receiverAccount, amount);
            }
            else
            {
                Console.WriteLine("Invalid amount. Please enter a positive number.");
            }
        }
    }
}