// Бібліотека класів
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace ATMApp
{
    public class Account
    {
        public string CardNumber { get; set; }
        public string PinCode { get; set; }
        public string FullName { get; set; }
        public decimal Balance { get; set; }

        public Account(string cardNumber, string pinCode, string fullName, decimal balance)
        {
            CardNumber = cardNumber;
            PinCode = pinCode;
            FullName = fullName;
            Balance = balance;
        }
    }

    public class AutomatedTellerMachine
    {
        public string ATMId { get; set; }
        public string Address { get; set; }
        public decimal TotalCash { get; set; }

        public AutomatedTellerMachine(string atmId, string address, decimal totalCash)
        {
            ATMId = atmId;
            Address = address;
            TotalCash = totalCash;
        }

        public event Action<string> OperationEvent;

        public bool Authenticate(Account account, string enteredPin)
        {
            bool isAuthenticated = account.PinCode == enteredPin;
            OperationEvent?.Invoke(isAuthenticated ? "Authentication successful." : "Authentication failed.");
            return isAuthenticated;
        }

        public decimal CheckBalance(Account account)
        {
            OperationEvent?.Invoke($"Balance checked. Current balance: {account.Balance:C}");
            return account.Balance;
        }

        public bool Withdraw(Account account, decimal amount)
        {
            if (amount > account.Balance || amount > TotalCash)
            {
                OperationEvent?.Invoke("Withdrawal failed: Insufficient funds.");
                return false;
            }

            account.Balance -= amount;
            TotalCash -= amount;
            OperationEvent?.Invoke($"Withdrawal successful. Amount: {amount:C}");
            return true;
        }

        public void Deposit(Account account, decimal amount)
        {
            account.Balance += amount;
            TotalCash += amount;
            OperationEvent?.Invoke($"Deposit successful. Amount: {amount:C}");
        }

        public bool Transfer(Account sender, Account receiver, decimal amount)
        {
            if (amount > sender.Balance)
            {
                OperationEvent?.Invoke("Transfer failed: Insufficient funds.");
                return false;
            }

            sender.Balance -= amount;
            receiver.Balance += amount;
            OperationEvent?.Invoke($"Transfer successful. Amount: {amount:C}");
            return true;
        }

        public void SendEmailNotification(string toEmail, string subject, string body)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("your-email@gmail.com", "your-password"),
                    EnableSsl = true
                };

                smtpClient.Send("your-email@gmail.com", toEmail, subject, body);
                OperationEvent?.Invoke("Email notification sent.");
            }
            catch (Exception ex)
            {
                OperationEvent?.Invoke($"Failed to send email: {ex.Message}");
            }
        }
    }

    public class Bank
    {
        public string Name { get; set; }
        public List<AutomatedTellerMachine> ATMs { get; set; }

        public Bank(string name)
        {
            Name = name;
            ATMs = new List<AutomatedTellerMachine>();
        }

        public void AddATM(AutomatedTellerMachine atm)
        {
            ATMs.Add(atm);
        }
    }
}
