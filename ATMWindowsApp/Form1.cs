using ATMApp;

namespace ATMWindowsApp
{
    public partial class Form1 : Form
    {
        private Bank bank;
        private AutomatedTellerMachine atm;
        private Account currentAccount;

        public Form1()
        {
            InitializeComponent();
            InitializeATM();
        }

        private void InitializeATM()
        {
            bank = new Bank("My Bank");
            atm = new AutomatedTellerMachine("ATM001", "Main Street", 10000);
            bank.AddATM(atm);

            atm.OperationEvent += message => ShowNotification(message);

            currentAccount = new Account("1234567890123456", "1234", "John Doe", 1000);
        }

        private void btnAuthenticate_Click(object sender, EventArgs e)
        {
            string cardNumber = txtCardNumber.Text;
            string pin = txtPin.Text;

            if (AuthenticateAccount(cardNumber, pin))
            {
                DisplayAuthenticationSuccess();
            }
            else
            {
                ShowErrorMessage("Authentication failed. Please check your card number and PIN.");
            }
        }

        private bool AuthenticateAccount(string cardNumber, string pin)
        {
            return cardNumber == currentAccount.CardNumber && atm.Authenticate(currentAccount, pin);
        }

        private void DisplayAuthenticationSuccess()
        {
            ShowNotification($"Welcome, {currentAccount.FullName}!\nYour current balance is: {currentAccount.Balance:C}");
            UpdateAccountInfo();
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Error");
        }

        private void UpdateAccountInfo()
        {
            lblWelcome.Text = $"Welcome, {currentAccount.FullName}";
            lblBalance.Text = $"Balance: {currentAccount.Balance:C}";
            txtAmount.Text = currentAccount.Balance.ToString("F2");
        }

        private void btnCheckBalance_Click_1(object sender, EventArgs e)
        {
            ProcessBalanceCheck();
        }

        private void ProcessBalanceCheck()
        {
            decimal balance = atm.CheckBalance(currentAccount);
            ShowNotification($"Your current balance is: {balance:C}");
            txtAmount.Text = balance.ToString("F2");
        }

        private void btnDeposit_Click_1(object sender, EventArgs e)
        {
            ProcessDeposit();
        }

        private void ProcessDeposit()
        {
            decimal amount = ParseAmount();
            atm.Deposit(currentAccount, amount);
            ShowNotification($"Successfully deposited {amount:C}. New balance: {currentAccount.Balance:C}");
            UpdateBalance();
        }

        private void btnWithdraw_Click_1(object sender, EventArgs e)
        {
            ProcessWithdrawal();
        }

        private void ProcessWithdrawal()
        {
            decimal amount = ParseAmount();
            if (atm.Withdraw(currentAccount, amount))
            {
                ShowNotification($"Successfully withdrew {amount:C}. New balance: {currentAccount.Balance:C}");
                UpdateBalance();
            }
        }

        private decimal ParseAmount()
        {
            try
            {
                return decimal.Parse(txtAmount.Text);
            }
            catch
            {
                ShowErrorMessage("Invalid amount. Please enter a valid number.");
                return 0;
            }
        }

        private void btnTransfer_Click_1(object sender, EventArgs e)
        {
            ProcessTransfer();
        }

        private void ProcessTransfer()
        {
            decimal amount = ParseAmount();
            Account receiverAccount = GetReceiverAccount();

            if (atm.Transfer(currentAccount, receiverAccount, amount))
            {
                ShowNotification($"Successfully transferred {amount:C} to {receiverAccount.FullName}.");
                UpdateBalance();
            }
            else
            {
                ShowErrorMessage("Transfer failed. Please check the details and try again.");
            }
        }

        private Account GetReceiverAccount()
        {
            return new Account("9876543210987654", "5678", "Jane Smith", 500);
        }

        private void UpdateBalance()
        {
            lblBalance.Text = $"Balance: {currentAccount.Balance:C}";
            txtAmount.Text = currentAccount.Balance.ToString("F2");
        }

        private void ShowNotification(string message)
        {
            MessageBox.Show(message, "ATM Notification");
        }
    }
}
