using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;

namespace ATMApp
{
    public class Account
    {
        public string CardNumber { get; }
        private string PinCode { get; }
        public string FullName { get; }
        public decimal Balance { get; private set; }

        public Account(string cardNumber, string pinCode, string fullName, decimal balance)
        {
            CardNumber = cardNumber;
            PinCode = pinCode;
            FullName = fullName;
            Balance = balance;
        }

        public bool ValidatePin(string enteredPin)
        {
            return CryptographicOperations.FixedTimeEquals(
                System.Text.Encoding.UTF8.GetBytes(PinCode),
                System.Text.Encoding.UTF8.GetBytes(enteredPin)
            );
        }

        public bool Withdraw(decimal amount)
        {
            if (amount <= 0 || amount > Balance) return false;
            Balance -= amount;
            return true;
        }

        public void Deposit(decimal amount)
        {
            if (amount > 0) Balance += amount;
        }
    }

    public interface IEmailService
    {
        void SendEmail(string toEmail, string subject, string body);
    }

    public class EmailService : IEmailService
    {
        private readonly string _smtpServer;
        private readonly int _port;
        private readonly string _email;
        private readonly string _password;

        public EmailService()
        {
            _smtpServer = Environment.GetEnvironmentVariable("SMTP_SERVER") ?? throw new InvalidOperationException("SMTP_SERVER is not set");
            _port = int.Parse(Environment.GetEnvironmentVariable("SMTP_PORT") ?? throw new InvalidOperationException("SMTP_PORT is not set"));
            _email = Environment.GetEnvironmentVariable("SMTP_EMAIL") ?? throw new InvalidOperationException("SMTP_EMAIL is not set");
            _password = Environment.GetEnvironmentVariable("SMTP_PASSWORD") ?? throw new InvalidOperationException("SMTP_PASSWORD is not set");
        }

        public void SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                using var smtpClient = new SmtpClient(_smtpServer)
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

    public class AutomatedTellerMachine
    {
        public string ATMId { get; }
        public string Address { get; }
        public decimal TotalCash { get; private set; }
        private readonly IEmailService _emailService;

        public AutomatedTellerMachine(string atmId, string address, decimal totalCash, IEmailService emailService)
        {
            ATMId = atmId;
            Address = address;
            TotalCash = totalCash;
            _emailService = emailService;
        }

        public event Action<string> OperationEvent;

        public bool Authenticate(Account account, string enteredPin)
        {
            bool isAuthenticated = account.ValidatePin(enteredPin);
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
            if (amount > 0)
            {
                account.Deposit(amount);
                TotalCash += amount;
                OperationEvent?.Invoke($"Deposit successful. Amount: {amount:C}");
            }
            else
            {
                OperationEvent?.Invoke("Deposit failed: Amount must be positive.");
            }
        }

        public bool Transfer(Account sender, Account receiver, decimal amount)
        {
            if (!sender.Withdraw(amount)) return false;

            receiver.Deposit(amount);
            OperationEvent?.Invoke($"Transfer successful. Amount: {amount:C}");
            return true;
        }

        public void SendEmailNotification(string toEmail, string subject, string body)
        {
            _emailService.SendEmail(toEmail, subject, body);
        }
    }

    public class Bank
    {
        public string Name { get; }
        private readonly List<Account> _accounts;
        public List<AutomatedTellerMachine> ATMs { get; }

        public Bank(string name)
        {
            Name = name;
            ATMs = new List<AutomatedTellerMachine>();
            _accounts = new List<Account>();
        }

        public void AddATM(AutomatedTellerMachine atm)
        {
            ATMs.Add(atm);
        }

        public void AddAccount(Account account)
        {
            _accounts.Add(account);
        }

        public Account GetAccountByCardNumber(string cardNumber)
        {
            return _accounts.FirstOrDefault(a => a.CardNumber == cardNumber);
        }
    }
}
