namespace ATMWindowsApp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtCardNumber = new TextBox();
            txtPin = new TextBox();
            btnAuthenticate = new Button();
            btnWithdraw = new Button();
            btnCheckBalance = new Button();
            btnDeposit = new Button();
            btnTransfer = new Button();
            lblWelcome = new Label();
            txtAmount = new TextBox();
            lblBalance = new Label();
            SuspendLayout();
            // 
            // txtCardNumber
            // 
            txtCardNumber.Location = new Point(53, 23);
            txtCardNumber.Name = "txtCardNumber";
            txtCardNumber.Size = new Size(140, 27);
            txtCardNumber.TabIndex = 8;
            // 
            // txtPin
            // 
            txtPin.Location = new Point(53, 84);
            txtPin.Name = "txtPin";
            txtPin.Size = new Size(140, 27);
            txtPin.TabIndex = 1;
            // 
            // btnAuthenticate
            // 
            btnAuthenticate.Location = new Point(53, 175);
            btnAuthenticate.Name = "btnAuthenticate";
            btnAuthenticate.Size = new Size(94, 29);
            btnAuthenticate.TabIndex = 2;
            btnAuthenticate.Text = "Вхід";
            btnAuthenticate.UseVisualStyleBackColor = true;
            btnAuthenticate.Click += btnAuthenticate_Click;
            // 
            // btnWithdraw
            // 
            btnWithdraw.Location = new Point(53, 325);
            btnWithdraw.Name = "btnWithdraw";
            btnWithdraw.Size = new Size(94, 29);
            btnWithdraw.TabIndex = 3;
            btnWithdraw.Text = "Вивести кошти";
            btnWithdraw.UseVisualStyleBackColor = true;
            btnWithdraw.Click += btnWithdraw_Click_1;
            // 
            // btnCheckBalance
            // 
            btnCheckBalance.Location = new Point(53, 223);
            btnCheckBalance.Name = "btnCheckBalance";
            btnCheckBalance.Size = new Size(94, 29);
            btnCheckBalance.TabIndex = 4;
            btnCheckBalance.Text = "Провірити баланс";
            btnCheckBalance.UseVisualStyleBackColor = true;
            btnCheckBalance.Click += btnCheckBalance_Click_1;
            // 
            // btnDeposit
            // 
            btnDeposit.Location = new Point(53, 275);
            btnDeposit.Name = "btnDeposit";
            btnDeposit.Size = new Size(94, 29);
            btnDeposit.TabIndex = 5;
            btnDeposit.Text = "Депозит";
            btnDeposit.UseVisualStyleBackColor = true;
            btnDeposit.Click += btnDeposit_Click_1;
            // 
            // btnTransfer
            // 
            btnTransfer.Location = new Point(53, 373);
            btnTransfer.Name = "btnTransfer";
            btnTransfer.Size = new Size(94, 29);
            btnTransfer.TabIndex = 6;
            btnTransfer.Text = "Надіслати кошти";
            btnTransfer.UseVisualStyleBackColor = true;
            btnTransfer.Click += btnTransfer_Click_1;
            // 
            // lblWelcome
            // 
            lblWelcome.AutoSize = true;
            lblWelcome.Location = new Point(273, 42);
            lblWelcome.Name = "lblWelcome";
            lblWelcome.Size = new Size(88, 20);
            lblWelcome.TabIndex = 7;
            lblWelcome.Text = "lblWelcome";
            // 
            // txtAmount
            // 
            txtAmount.Location = new Point(211, 275);
            txtAmount.Name = "txtAmount";
            txtAmount.Size = new Size(140, 27);
            txtAmount.TabIndex = 9;
            // 
            // lblBalance
            // 
            lblBalance.AutoSize = true;
            lblBalance.Location = new Point(273, 87);
            lblBalance.Name = "lblBalance";
            lblBalance.Size = new Size(51, 20);
            lblBalance.TabIndex = 10;
            lblBalance.Text = "lblWel";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(435, 450);
            Controls.Add(lblBalance);
            Controls.Add(txtAmount);
            Controls.Add(lblWelcome);
            Controls.Add(btnTransfer);
            Controls.Add(btnDeposit);
            Controls.Add(btnCheckBalance);
            Controls.Add(btnWithdraw);
            Controls.Add(btnAuthenticate);
            Controls.Add(txtPin);
            Controls.Add(txtCardNumber);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtCardNumber;
        private TextBox txtPin;
        private Button btnAuthenticate;
        private Button btnWithdraw;
        private Button btnCheckBalance;
        private Button btnDeposit;
        private Button btnTransfer;
        private Label lblWelcome;
        private TextBox txtAmount;
        private Label lblBalance;
    }
}
