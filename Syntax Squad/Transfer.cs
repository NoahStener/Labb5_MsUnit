﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syntax_Squad
{
    public class Transfer
    {
        //Simon Ståhl SUT23
        private List<BankAccount> transferAccounts = BankAccount.bankAccounts;
        


        /*public void WithdrawFromAccount(int fromAccountNumber, double amount, string Userpassword)
        {


            var fromAccount = GetBankAccount(fromAccountNumber);
            if (fromAccount.Balance == null || fromAccount.Balance < 0 && password == Userpassword)
            {
                Console.WriteLine("Insufficient fund on selected Account.");
                return;
            }
            fromAccount.Balance = amount;

            Console.WriteLine($"Withdraw request successfull. Please take your money.");
            Console.WriteLine($"Remaining Balance for {fromAccount}: {fromAccount.Balance}");
        }*/



        /// <summary>
        /// Metod för överföring emellan egna konton. 
        /// PIN behövs inte för egen överföring då vi loggat in en gång redan.
        /// 
        /// </summary>

        public void TransferBetweenOwnAccounts(User user)
        {
            Console.Clear();
            int fromAccountNumber;
            int toAccountNumber;
            double amount;
            List<int> loggedInUserAccountNumber = loggedInAccountList(user);
                        
            try
            {

                Console.WriteLine("Insert Account number to transfer from: ");
                fromAccountNumber = int.Parse(Console.ReadLine());

                Console.WriteLine("Insert Account number to transfer to: ");
                toAccountNumber = int.Parse(Console.ReadLine());

                Console.WriteLine("Enter the amount you wish to transfer: ");
                amount = double.Parse(Console.ReadLine());

                var fromAccount = GetBankAccount(fromAccountNumber);
                var toAccount = GetBankAccount(toAccountNumber);

               
                if (fromAccount.Balance > amount && loggedInUserAccountNumber.Contains(fromAccountNumber))
                {
                    fromAccount.Balance -= amount;
                    toAccount.Balance += amount;
                    Console.WriteLine($"Transfer successful. New balance for {fromAccount.AccountName}: {fromAccount.Balance}");
                    Console.WriteLine($"New balance for {toAccount.AccountName}: {toAccount.Balance}");

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid input, please try again.");
            }




        }

        /// <summary>
        /// Metod för överföring från egna konton till andra användares konton. 
        /// PIN kontroll för att säkerställa att man vill föra över till annan användare. 
        /// </summary>

        public void TransferBetweenOtherAccounts(User user)
        {
            Console.Clear();
            int fromAccountNumber;
            int toAccountNumber;
            double amount;
            List<int> loggedInUserAccountNumber = loggedInAccountList(user);
           

            try
            {
                Console.WriteLine("Insert Account number to transfer from: ");
                fromAccountNumber = int.Parse(Console.ReadLine());

                Console.WriteLine("Insert Account number to transfer to: ");
                toAccountNumber = int.Parse(Console.ReadLine());

                Console.WriteLine("Enter the amount you wish to transfer: ");
                amount = double.Parse(Console.ReadLine());

                Console.WriteLine("Please enter your Password to confirm the transaction:");
                string password = Console.ReadLine();

                var fromAccount = GetBankAccount(fromAccountNumber);
                var toAccount = GetBankAccount(toAccountNumber);

            if (fromAccount == null || toAccount == null || password != user.Password) // fungerar verkligen detta?????
            {
                Console.WriteLine("Invalid account number.");
                return;
            }

            if (fromAccount.Balance < amount)
            {
                Console.WriteLine("Insufficient funds.");
                return;
            }

            if (fromAccount != null && toAccount != null && password == user.Password)
            {
                fromAccount.Balance -= amount;
                toAccount.Balance += amount;
                Console.WriteLine($"Transfer successful. New balance for {fromAccount.AccountName}: {fromAccount.Balance}");

                }
            }
            catch (Exception ex)
            {

            }

            


        }


        public BankAccount GetBankAccount(int AccountNumber)
        {
            return BankAccount.bankAccounts.Find(a => a.AccountNumber == AccountNumber);
                       
        }

        public List<int> loggedInAccountList(User user)
        {
            List<int> loggedInUserAccountNumber = new List<int>();

            foreach (var account in BankAccount.bankAccounts)
            {
                if (account.Owner == user.Name)
                {
                    Console.WriteLine($"Account Name: {account.AccountName}");
                    Console.WriteLine($"Account number: {account.AccountNumber} Balance: {account.Balance}{account.Currency}");
                    loggedInUserAccountNumber.Add(account.AccountNumber);
                }
            }
            return loggedInUserAccountNumber;
        }

    }
}