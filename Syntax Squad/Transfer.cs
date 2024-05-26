using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static System.TimeZoneInfo;

namespace Syntax_Squad
{
    public class Transfer
    {
        //Simon Ståhl SUT23
        private List<BankAccount> transferAccounts;

        public static List<Transfer> transactctionHistory = new List<Transfer>();
        public ExchangeRateManager exchange = new ExchangeRateManager();
        private double amount { get; set; }
        private string Currency { get; set; }
        private int fromAccountNumber { get; set; }
        private int toAccountNumber { get; set; }
        private DateTime transactionTime { get; set; }

        public void SetAccounts(List<BankAccount> accounts)
        {
            transferAccounts = accounts;
        }

        /// <summary>
        /// Metod för överföring emellan egna konton. 
        /// PIN behövs inte för egen överföring då vi loggat in en gång redan.
        /// 
        /// </summary>

        public void TransferBetweenOwnAccounts(User user, int fromAccountNumber,int toAccountNumber, double amount)
        {
            //Console.Clear();
            List<int> loggedInUserAccountNumber = LoggedInAccountList(user);

            try
            {

                //Console.Write("\n\tInsert Account number to transfer from: ");
                //fromAccountNumber = int.Parse(Console.ReadLine());

                //Console.Write("\n\tInsert Account number to transfer to: ");
                //toAccountNumber = int.Parse(Console.ReadLine());

                //Console.Write("\n\tEnter the amount you wish to transfer: ");
                //amount = double.Parse(Console.ReadLine());

                var fromAccount = GetBankAccount(fromAccountNumber);
                var toAccount = GetBankAccount(toAccountNumber);

                if (fromAccount == null || toAccount == null)
                {
                    throw new ArgumentException("Invalid account number");
                }


                if (user.TransferLimit >= amount || user.TransferLimit == 0)
                {
                    if (fromAccount.Balance >= amount && loggedInUserAccountNumber.Contains(fromAccountNumber))
                    {
                        if (fromAccount.Currency != toAccount.Currency)
                        {

                            PerformCurrencyTransfer(fromAccount, toAccount, amount);

                            //if (exchange.exchangeRates.ContainsKey(fromAccount.Currency) && exchange.exchangeRates.ContainsKey(toAccount.Currency))
                            //{
                            //    var fromRate = Convert.ToDouble(exchange.exchangeRates[fromAccount.Currency]);
                            //    var toRate = Convert.ToDouble(exchange.exchangeRates[toAccount.Currency]);
                            //    var convertedAmount = amount * (1 / fromRate) * toRate;
                            //    fromAccount.Balance -= amount;
                            //    toAccount.Balance += convertedAmount;
                            //    TransferHistory(fromAccountNumber, toAccountNumber, Currency, amount);
                            //    Console.WriteLine($"\tTransfer successful. New balance for {fromAccount.AccountName}: {fromAccount.Balance} {fromAccount.Currency}");
                            //    Console.WriteLine($"\tNew balance for {toAccount.AccountName}: {toAccount.Balance} {toAccount.Currency}");

                            //}
                        }

                        else
                        {
                            PerformSameCurrencyTransfer(fromAccount, toAccount, amount);
                            //fromAccount.Balance -= amount;
                            //toAccount.Balance += amount;
                            //Console.WriteLine($"\tTransfer successful. New balance for {fromAccount.AccountName}: {fromAccount.Balance} {fromAccount.Currency}");
                            //Console.WriteLine($"\tNew balance for {toAccount.AccountName}: {toAccount.Balance} {toAccount.Currency}");
                            //TransferHistory(fromAccountNumber, toAccountNumber, Currency, amount);

                        }
                    }
                }
                else
                {
                    Console.WriteLine($"\tYou have exceeded your transaction limit.");
                    //Console.ReadKey();
                    //return;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Invalid amount",ex);
            }
            //Console.ReadKey();

        }


        //Funktion för att hantera överföring med olika valuta
        public void PerformCurrencyTransfer(BankAccount fromAccount, BankAccount toAccount, double amount)
        {

            if(amount <= 0)
            {
                throw new ArgumentException("Invalid amount");
            }


            if (exchange.exchangeRates.ContainsKey(fromAccount.Currency) && exchange.exchangeRates.ContainsKey(toAccount.Currency))
            {
                var fromRate = Convert.ToDouble(exchange.exchangeRates[fromAccount.Currency]);
                var toRate = Convert.ToDouble(exchange.exchangeRates[toAccount.Currency]);
                var convertedAmount = amount * (1 / fromRate) * toRate;
                fromAccount.Balance -= amount;
                toAccount.Balance += convertedAmount;
                TransferHistory(fromAccountNumber, toAccountNumber, Currency, amount);
                Console.WriteLine($"\tTransfer successful. New balance for {fromAccount.AccountName}: {fromAccount.Balance} {fromAccount.Currency}");
                Console.WriteLine($"\tNew balance for {toAccount.AccountName}: {toAccount.Balance} {toAccount.Currency}");

            }
            else
            {
                Console.WriteLine("\tExchange rates not available.");
            }

        }

        public void PerformSameCurrencyTransfer(BankAccount fromAccount, BankAccount toAccount, double amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Invalid amount");
            }
            else
            {
                fromAccount.Balance -= amount;
                toAccount.Balance += amount;
                Console.WriteLine($"\tTransfer successful. New balance for {fromAccount.AccountName}: {fromAccount.Balance} {fromAccount.Currency}");
                Console.WriteLine($"\tNew balance for {toAccount.AccountName}: {toAccount.Balance} {toAccount.Currency}");
                TransferHistory(fromAccountNumber, toAccountNumber, Currency, amount);

            }
        }


        /// <summary>
        /// Metod för överföring från egna konton till andra användares konton. 
        /// PIN kontroll för att säkerställa att man vill föra över till annan användare. 
        /// </summary>


        public void TransferBetweenOtherAccounts(User user, int fromAccountNumber, int toAccountNumber, double amount, string password)
        {
            //Console.Clear();
            List<int> loggedInUserAccountNumber = LoggedInAccountList(user);

            try
            {
                //Console.Write("\n\tInsert Account number to transfer from: ");
                //fromAccountNumber = int.Parse(Console.ReadLine());

                //Console.Write("\n\tInsert Account number to transfer to: ");
                //toAccountNumber = int.Parse(Console.ReadLine());

                var fromAccount = GetBankAccount(fromAccountNumber);
                var toAccount = GetBankAccount(toAccountNumber);

                if (fromAccount == null || toAccount == null)
                {
                    throw new ArgumentException("Invalid account number");
                    //Console.WriteLine("\tInvalid account number.");
                    //Console.ReadKey();
                    //return;
                }

                //Console.Write("\n\tPlease enter your Password to confirm the transaction:");
                //string password = Console.ReadLine();
                if (password != user.Password)
                {
                    throw new UnauthorizedAccessException("Wrong password");
                    //Console.WriteLine("\tWrong password");
                    //Console.ReadKey();
                    //return;
                }
                //Console.Write("\n\tEnter the amount you wish to transfer: ");
                //amount = double.Parse(Console.ReadLine());

                if (fromAccount.Balance < amount)
                {
                    Console.WriteLine("\tInsufficient funds.");
                    return;
                }
                if (user.TransferLimit >= amount || user.TransferLimit == 0)
                {
                    //if (fromAccount.Balance >= amount && password == user.Password)
                    
                        if (fromAccount.Currency != toAccount.Currency)
                        {
                            PerformCurrencyTransfer(fromAccount, toAccount, amount);
                            //ExchangeRateManager exchange = new ExchangeRateManager();
                            //if (exchange.exchangeRates.ContainsKey(fromAccount.Currency) && exchange.exchangeRates.ContainsKey(toAccount.Currency))
                            //{
                            //    var fromRate = Convert.ToDouble(exchange.exchangeRates[fromAccount.Currency]);
                            //    var toRate = Convert.ToDouble(exchange.exchangeRates[toAccount.Currency]);
                            //    var convertedAmount = amount * (1 / fromRate) * toRate;
                            //    fromAccount.Balance -= amount;
                            //    toAccount.Balance += convertedAmount;
                            //    TransferHistory(fromAccountNumber, toAccountNumber, Currency, amount);
                            //    Console.WriteLine($"\tTransfer successful. New balance for {fromAccount.AccountName}: {fromAccount.Balance} {fromAccount.Currency}");
                            //    Console.ReadKey();

                            //}

                        }
                        else
                        {
                            PerformSameCurrencyTransfer(fromAccount, toAccount, amount);
                            //fromAccount.Balance -= amount;
                            //toAccount.Balance += amount;
                            //Console.WriteLine($"\tTransfer successful. New balance for {fromAccount.AccountName}: {fromAccount.Balance} {fromAccount.Currency}");
                            //TransferHistory(fromAccountNumber, toAccountNumber, Currency, amount);

                        }
                    

                }
                else
                {
                    throw new InvalidOperationException("You have exceeded transaction limit");
                    //Console.WriteLine($"\tYou have exceeded your transaction limit.");
                    //Console.ReadKey();
                    //return;
                }

            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("invalid input", ex);
            }
           



        }

        public BankAccount GetBankAccount(int AccountNumber)
        {
            var account = transferAccounts.FirstOrDefault(account =>  account.AccountNumber == AccountNumber);
            if (account == null)
            {
                Console.WriteLine($"Account with {AccountNumber} not found");
            }
            return account;
        }

        public virtual List<int> LoggedInAccountList(User user)
        {
            List<int> loggedInUserAccountNumber = new List<int>();

            foreach (var account in transferAccounts)
            {
                if (account.Owner == user.Name)
                {
                    Console.WriteLine($"\tAccount Name: {account.AccountName}");
                    Console.WriteLine($"\tAccount number: {account.AccountNumber} Balance: {account.Balance}{account.Currency}");
                    loggedInUserAccountNumber.Add(account.AccountNumber);
                }
            }
            return loggedInUserAccountNumber;
        }

        public static void PrintTransferHistoryAdmin()
        {

            Console.WriteLine("\tTransaction History:");

            foreach (var transaction in transactctionHistory)
            {
                Console.WriteLine($"\tTransaction: From {transaction.fromAccountNumber} to {transaction.toAccountNumber} - Amount: {transaction.amount} {transaction.Currency} - Time: {transaction.transactionTime}");
            }

            Console.WriteLine($"\tPress enter to return to menu");


            Console.ReadKey();
        }

        public static void PrintTransactionHistoryUser(User user)
        {
            Console.Clear();
            foreach (var account in BankAccount.bankAccounts)
            {
                if (account.Owner == user.Name)
                {
                    foreach (var transaction in transactctionHistory)
                    {
                        if (transaction.fromAccountNumber == account.AccountNumber || transaction.toAccountNumber == account.AccountNumber)
                        {
                            Console.WriteLine($"\tTransaction: From {transaction.fromAccountNumber} to {transaction.toAccountNumber} - " +
                                $"Amount: {transaction.amount} {transaction.Currency} - Time: {transaction.transactionTime}");
                        }

                    }
                    break;
                }
            }

            Console.WriteLine($"\tPress enter to return to menu");



            Console.ReadKey();

        }

        public virtual void TransferHistory(int fromAccount, int toAccount, string currency, double Amount)
        {
            Transfer transactionHistory = new Transfer
            {
                amount = Amount,
                Currency = currency,
                fromAccountNumber = fromAccount,
                toAccountNumber = toAccount,
                transactionTime = DateTime.Now
            };
            transactctionHistory.Add(transactionHistory);

        }

    }
}
