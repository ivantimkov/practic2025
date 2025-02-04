using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace ATMApp
{
    public class Account
    {
        public string CardNumber { get; }
        public string PinCode { get; private set; }
        public string FullName { get; }
        private decimal balance;

        public decimal Balance => balance;

        public Account(string cardNumber, string pinCode, string fullName, decimal balance)
        {
            CardNumber = cardNumber;
            PinCode = pinCode;
            FullName = fullName;
            this.balance = balance;
        }

        public bool Withdraw(decimal amount)
        {
            if (amount > balance)
                return false;

            balance -= amount;
            return true;
        }

        public void Deposit(decimal amount)
        {
            balance += amount;
        }
        public bool Transfer(Account receiver, decimal amount)
        {
            if (!Withdraw(amount)) return false;

            receiver.Deposit(amount);
            return true;
        }

    }

    public class ATMService
{
    public static Account GetAccountByCardNumber(List<Account> accounts, string cardNumber)
    {
        return accounts.FirstOrDefault(a => a.CardNumber == cardNumber);
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
            if (!account.Withdraw(amount) || amount > TotalCash)
            {
                OperationEvent?.Invoke("Withdrawal failed: Insufficient funds.");
                return false;
            }

            TotalCash -= amount;
            OperationEvent?.Invoke($"Withdrawal successful. Amount: {amount:C}");
            return true;
        }

        public void Deposit(Account account, decimal amount)
        {
            account.Deposit(amount);
            TotalCash += amount;
            OperationEvent?.Invoke($"Deposit successful. Amount: {amount:C}");
        }

        public bool Transfer(Account sender, Account receiver, decimal amount)
        {
            if (!sender.Transfer(receiver, amount))
            {
                OperationEvent?.Invoke("Transfer failed: Insufficient funds.");
                return false;
            }

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
