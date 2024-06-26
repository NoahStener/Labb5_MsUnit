﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Syntax_Squad
{
    public class Loan
    {

        // Max SUT23

        public double loanAmount { get; set; }
        private int accountID { get; set; }
        private int accountNumber { get; set; }

        private static double totalMoneyAmount;
        private static double loanSize;
        private static List<Loan> loans = new List<Loan>();
        //private static Transfer getBankInfo = new Transfer();


        //private static int toAcc;
        private static ExchangeRateManager exchangeRate = new ExchangeRateManager();


        /// <summary>
        /// This method checks how big of a loan the user wants and if they are eligable for it
        /// </summary>
        //public static void TakeOutLoan(User user)
        //{
        //    var convertedAmount = 0.0;
        //    Console.Clear();
        //    //toaltalMoneyAmount = 0;
        //    //foreach (BankAccount bankAccount in BankAccount.bankAccounts)
        //    //{
        //    //    if (bankAccount.ID == user.UserId)
        //    //    {
        //    //        var fromRate = Convert.ToDouble(exchangeRate.exchangeRates[bankAccount.Currency]);
        //    //        var toRate = Convert.ToDouble(exchangeRate.exchangeRates["SEK"]);
        //    //        convertedAmount = bankAccount.Balance * (1 / fromRate) * toRate;
        //    //       /* toaltalMoneyAmount*/ += convertedAmount;
        //    //    }
        //    //}
        //    //Console.WriteLine($"\nYou can only take out a loan that is 5 times bigger than your total balance" +
        //    //    $"\n\t Your toatal balance is {toaltalMoneyAmount} SEK" +
        //    //    $"\n\t And the loan will have a 4.5% intrest" +
        //    //    $"\n How big of a loan do you want to take? (SEK)");

        //    double.TryParse(Console.ReadLine(), out loanSize);

        //    if (loanSize <= convertedAmount * 5)
        //    {
        //        List<int> accNr = getBankInfo.LoggedInAccountList(user);
        //        Console.WriteLine("Insert Account number of the account you want the money: ");
        //        toAcc = int.Parse(Console.ReadLine());
        //        var toAccount = getBankInfo.GetBankAccount(toAcc);
        //        if (accNr.Contains(toAcc))
        //        {
        //            var toRate = Convert.ToDouble(exchangeRate.exchangeRates[toAccount.Currency]);
        //            var convertedLoan = loanSize * toRate;
        //            toAccount.Balance += convertedLoan;
        //            Console.WriteLine($"You have now taken a loan of {loanSize}");
        //            AllLoanes(loanSize, user.UserId, toAcc);
        //        }
        //    }
        //    else Console.WriteLine("You can not borrow that much money with the amount of money you have.");
        //    Console.ReadKey();
        //}

        public static void TakeOutLoan(User user, double loanSize, int toAcc)
        {
            if(loanSize <= 0)
            {
                throw new ArgumentException("Amount must be positive");
            }

            var convertedAmount = 0.0;
            totalMoneyAmount = 0;

            foreach(BankAccount bankAccount in BankAccount.bankAccounts)
            {
                if(bankAccount.ID == user.UserId)
                {
                    var fromRate = Convert.ToDouble(exchangeRate.exchangeRates[bankAccount.Currency]);
                    var toRate = Convert.ToDouble(exchangeRate.exchangeRates["SEK"]);
                    convertedAmount = bankAccount.Balance * (1 / fromRate) * toRate;
                    totalMoneyAmount += convertedAmount;
                }
            }

            if(loanSize <= totalMoneyAmount * 5)
            {
                //List<int> accNr = getBankInfo.LoggedInAccountList(user);
                var toAccount = GetBankAccount(toAcc);
                if(toAccount != null && toAccount.Owner == user.Name)
                {
                    var toRate = Convert.ToDouble(exchangeRate.exchangeRates[toAccount.Currency]);
                    var convertedLoan = loanSize * toRate;
                    toAccount.Balance += convertedLoan;
                    AllLoans(loanSize, user.UserId, toAcc);
                    Console.WriteLine($"You have now taken a loan of {loanSize}. New balance for account {toAcc} is {toAccount.Balance}."); 
                }
                else
                {
                    throw new ArgumentException("Invalid account number");
                }

            }
            else
            {
                throw new InvalidOperationException("You cant borrow that much money");
            }
        }


        private static BankAccount GetBankAccount (int accountNumber)
        {
            return BankAccount.bankAccounts.FirstOrDefault(account => account.AccountNumber == accountNumber);
        }


        /// <summary>
        /// Makes it so the user can see what outgoing loans they have
        /// </summary>
        public static void SeeUserLoans(User user)
        {
            Console.Clear();
            foreach (Loan loan in loans)
            {
                if (loan.accountID == user.UserId)
                {
                    Console.Write($"\n\t You have a loan on: {loan.loanAmount}");
                }
            }
            Console.ReadKey();
        }

        public static void SeeAllLoans()
        {
            Console.Clear();
            foreach (Loan loan in loans)
            {
                Console.Write($"\n\t Loan of: {loan.loanAmount} by {loan.accountID} to account {loan.accountNumber}");
            }
            Console.ReadKey();
        }
        /// <summary>
        /// Adds the new loan to a list were all loans are keept
        /// </summary>
        /// <param name="loanS"></param>
        /// <param name="accountId"></param>
        //public static void AllLoanes(double loanS, int accountId, int toAcc)
        //{
        //    Loan newLoan = new Loan
        //    {
        //        loanAmount = loanS,
        //        accountID = accountId,
        //        accountNumber = toAcc
        //    };
        //    loans.Add(newLoan);
        //}

        public static List<Loan> GetAllLoans()
        {
            return new List<Loan>(loans);
        }

        private static void AllLoans(double loanAmount, int accountID, int accountNumber)
        {
            loans.Add(new Loan { loanAmount = loanAmount, accountID = accountID, accountNumber = accountNumber });
        }

        public static List<Loan> GetUserLoans(User user)
        {
            List<Loan>userLoans = new List<Loan>();
            foreach(Loan loan in loans)
            {
                if(loan.accountID == user.UserId)
                {
                    userLoans.Add(loan);
                }
            }
            return userLoans;
        }

    }
}
