﻿using System;
using System.Security.Principal;
using ATMApp;

namespace ATMConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Ініціалізація банку та банкомата
            var bank = new Bank("My Bank");
            var atm = new AutomatedTellerMachine("ATM001", "Main Street", 10000);
            bank.AddATM(atm);

            // Ініціалізація облікових записів
            var account1 = new Account("1234567890123456", "1234", "John Doe", 5000);
            var account2 = new Account("9876543210987654", "5678", "Jane Smith", 3000);

            // Підписка на події
            atm.OperationEvent += Console.WriteLine;

            Console.WriteLine("Welcome to the ATM!");

            Console.Write("Enter your card number: ");
            string cardNumber = Console.ReadLine();

            Account currentAccount = null;

            // Вибір акаунта за номером картки
            if (cardNumber == account1.CardNumber)
                currentAccount = account1;
            else if (cardNumber == account2.CardNumber)
                currentAccount = account2;
            else
                Console.WriteLine("Invalid card number.");

            if (currentAccount != null)
            {
                Console.Write("Enter your PIN: ");
                string pin = Console.ReadLine();

                if (atm.Authenticate(currentAccount, pin))
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
                                Account receiverAccount = null;

                                if (receiverCard == account1.CardNumber)
                                    receiverAccount = account1;
                                else if (receiverCard == account2.CardNumber)
                                    receiverAccount = account2;
                                else
                                    Console.WriteLine("Invalid receiver card number.");

                                if (receiverAccount != null)
                                {
                                    Console.Write("Enter amount to transfer: ");
                                    if (decimal.TryParse(Console.ReadLine(), out decimal transferAmount))
                                    {
                                        atm.Transfer(currentAccount, receiverAccount, transferAmount);
                                    }
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
                }
                else
                {
                    Console.WriteLine("Authentication failed.");
                }
            }

            Console.WriteLine("Thank you for using the ATM. Goodbye!");
        }
    }
}
