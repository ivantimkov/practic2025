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

            atm.OperationEvent += message => MessageBox.Show(message, "ATM Notification");

            // Add a sample account
            currentAccount = new Account("1234567890123456", "1234", "John Doe", 1000);
        }


        private Account GetAccountByCardNumber(string cardNumber)
        {
            return accounts.FirstOrDefault(acc => acc.CardNumber == cardNumber);
        } 



        private void btnAuthenticate_Click(object sender, EventArgs e)
        {
            string cardNumber = txtCardNumber.Text;
            string pin = txtPin.Text;

            currentAccount = GetAccountByCardNumber(cardNumber);

            // �������� �����
            if (currentAccount != null && atmService.Authenticate(currentAccount, pin))
            {
                MessageBox.Show($"Welcome, {currentAccount.FullName}!\nYour current balance is: {currentAccount.Balance:C}", "Authentication Successful");

                // ��������� ������ � ���������
                lblWelcome.Text = $"Welcome, {currentAccount.FullName}";
                lblBalance.Text = $"Balance: {currentAccount.Balance:C}";
                txtAmount.Text = currentAccount.Balance.ToString("F2");
            }
            else
            {
                MessageBox.Show("Authentication failed. Please check your card number and PIN.", "Error");
            }
        }

        private Account GetReceiverAccount()
        {
            // Dummy implementation for receiver's account
            return new Account("9876543210987654", "5678", "Jane Smith", 500);
        }

        private void UpdateBalance()
        {
            lblBalance.Text = $"Balance: {currentAccount.Balance:C}";
            txtAmount.Text = currentAccount.Balance.ToString("F2");
        }

        private void btnCheckBalance_Click_1(object sender, EventArgs e)
        {
            if (currentAccount != null)
            {
                decimal balance = atm.CheckBalance(currentAccount);
                MessageBox.Show($"Your current balance is: {balance:C}", "ATM");
                txtAmount.Text = balance.ToString("F2");
            }
            else
            {
                MessageBox.Show("Please authenticate first.", "ATM");
            }
        }

        private void btnDeposit_Click_1(object sender, EventArgs e)
        {
            if (currentAccount != null)
            {
                try
                {
                    decimal amount = decimal.Parse(txtAmount.Text);
                    atm.Deposit(currentAccount, amount);
                    MessageBox.Show($"Successfully deposited {amount:C}. New balance: {currentAccount.Balance:C}", "ATM");
                    UpdateBalance();
                    txtAmount.Text = "";
                }
                catch
                {
                    MessageBox.Show("Invalid amount. Please enter a valid number.", "ATM");
                }
            }
            else
            {
                MessageBox.Show("Please authenticate first.", "ATM");
            }
        }

        private void btnWithdraw_Click_1(object sender, EventArgs e)
        {
            if (currentAccount != null)
            {
                try
                {
                    decimal amount = decimal.Parse(txtAmount.Text);

                    if (amount <= 0) {
                        MessageBox.Shop("The amount cannot be less than 0!" , "ATM");
                        return;
                    }

                    if (atm.Withdraw(currentAccount, amount))
                    {
                        MessageBox.Show($"Successfully withdrew {amount:C}. New balance: {currentAccount.Balance:C}", "ATM");
                        UpdateBalance();
                        txtAmount.Text = "";
                    }
                }
                catch
                {
                    MessageBox.Show("Invalid amount. Please enter a valid number.", "ATM");
                }
            }
            else
            {
                MessageBox.Show("Please authenticate first.", "ATM");
            }
        }

        private void btnTransfer_Click_1(object sender, EventArgs e)
        {
            if (currentAccount != null)
            {
                try
                {
                    decimal amount = decimal.Parse(txtAmount.Text);
                    // Assuming you have a method to get receiver's account
                    Account receiverAccount = GetReceiverAccount();

                    if (receiverAccount != null && atm.Transfer(currentAccount, receiverAccount, amount))
                    {
                        MessageBox.Show($"Successfully transferred {amount:C} to {receiverAccount.FullName}.", "ATM");
                        UpdateBalance();
                        txtAmount.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("Transfer failed. Please check the details and try again.", "ATM");
                    }
                }
                catch
                {
                    MessageBox.Show("Invalid amount. Please enter a valid number.", "ATM");
                }
            }
            else
            {
                MessageBox.Show("Please authenticate first.", "ATM");
            }
        }
    }
}

    

    
