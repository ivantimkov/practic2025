using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly EmailService _emailService;

        public AutomatedTellerMachine(string atmId, string address, decimal totalCash)
        {
            ATMId = atmId;
            Address = address;
            TotalCash = totalCash;
            _emailService = new EmailService();
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
            _emailService.SendEmail(toEmail, subject, body);
        }
    }

    public class EmailService
    {
        private readonly string _smtpServer;
        private readonly int _port;
        private readonly string _email;
        private readonly string _password;

        public EmailService()
        {
            _smtpServer = Environment.GetEnvironmentVariable("SMTP_SERVER") ?? "smtp.gmail.com";
            _port = int.Parse(Environment.GetEnvironmentVariable("SMTP_PORT") ?? "587");
            _email = Environment.GetEnvironmentVariable("SMTP_EMAIL") ?? "your-email@gmail.com";
            _password = Environment.GetEnvironmentVariable("SMTP_PASSWORD") ?? "your-password";
        }

        public void SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                var smtpClient = new SmtpClient(_smtpServer)
                {
                    Port = _port,
                    Credentials = new NetworkCredential(_email, _password),
                    EnableSsl = true
                };
                smtpClient.Send(_email, toEmail, subject, body);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
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
