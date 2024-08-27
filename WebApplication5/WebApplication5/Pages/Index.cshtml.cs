using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace WebApplication5.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public string AccountType { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType { get; set; }
        public string Message { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

        public void OnPostCreateAccount(string accountNumber, decimal initialBalance, string accountType)
        {
            AccountNumber = accountNumber;
            Balance = initialBalance;
            AccountType = accountType;

            if (AccountType == "savings")
            {
                SavingsAccount savingsAccount = new SavingsAccount(AccountNumber, Balance, 2);
                Message = $"Savings account created with account number {AccountNumber} and initial balance {Balance:C}";
            }
            else if (AccountType == "checking")
            {
                CheckingAccount checkingAccount = new CheckingAccount(AccountNumber, Balance, 1000);
                Message = $"Checking account created with account number {AccountNumber} and initial balance {Balance:C}";
            }
        }

        public void OnPostPerformTransaction(decimal amount, string transactionType)
        {
            Amount = amount;
            TransactionType = transactionType;

            if (AccountType == "savings")
            {
                SavingsAccount savingsAccount = new SavingsAccount(AccountNumber, Balance, 2);
                if (TransactionType == "deposit")
                {
                    savingsAccount.Deposit(Amount);
                    Balance = savingsAccount.Balance;
                    Message = $"Deposited {Amount:C} into savings account {AccountNumber}. New balance is {Balance:C}";
                }
                else if (TransactionType == "withdrawal")
                {
                    try
                    {
                        savingsAccount.Withdraw(Amount);
                        Balance = savingsAccount.Balance;
                        Message = $"Withdrew {Amount:C} from savings account {AccountNumber}. New balance is {Balance:C}";
                    }
                    catch (InvalidOperationException ex)
                    {
                        Message = ex.Message;
                    }
                }
            }
            else if (AccountType == "checking")
            {
                CheckingAccount checkingAccount = new CheckingAccount(AccountNumber, Balance, 1000);
                if (TransactionType == "deposit")
                {
                    checkingAccount.Deposit(Amount);
                    Balance = checkingAccount.Balance;
                    Message = $"Deposited {Amount:C} into checking account {AccountNumber}. New balance is {Balance:C}";
                }
                else if (TransactionType == "withdrawal")
                {
                    try
                    {
                        checkingAccount.Withdraw(Amount);
                        Balance = checkingAccount.Balance;
                        Message = $"Withdrew {Amount:C} from checking account {AccountNumber}. New balance is {Balance:C}";
                    }
                    catch (InvalidOperationException ex)
                    {
                        Message = ex.Message;
                    }
                }
            }
        }
    }

    public class SavingsAccount
    {
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public int InterestRate { get; set; }

        public SavingsAccount(string accountNumber, decimal balance, int interestRate)
        {
            AccountNumber = accountNumber;
            Balance = balance;
            InterestRate = interestRate;
        }

        public void Deposit(decimal amount)
        {
            Balance += amount;
        }

        public void Withdraw(decimal amount)
        {
            if (Balance >= amount)
            {
                Balance -= amount;
            }
            else
            {
                throw new InvalidOperationException("Insufficient balance");
            }
        }
    }

    public class CheckingAccount
    {
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public decimal OverdraftLimit { get; set; }

        public CheckingAccount(string accountNumber, decimal balance, decimal overdraftLimit)
        {
            AccountNumber = accountNumber;
            Balance = balance;
            OverdraftLimit = overdraftLimit;
        }

        public void Deposit(decimal amount)
        {
            Balance += amount;
        }

        public void Withdraw(decimal amount)
        {
            if (Balance + OverdraftLimit >= amount)
            {
                Balance -= amount;
            }
            else
            {
                throw new InvalidOperationException("Insufficient balance");
            }
        }
    }
}