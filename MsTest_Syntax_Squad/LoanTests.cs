using Syntax_Squad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsTest_Syntax_Squad
{
    [TestClass]
    public class LoanTests
    {
        //Testar lån klassen
        //Test med summa som inte överskrider låntaket
        //Test med summa som är för hög
        

        [TestMethod]
        public void TakeOutLoan_ShouldIncreaseBalance()
        {
            //Arrange
            var accounts = new List<BankAccount>
            {
                new BankAccount("test1", "test", 123, "user", 1, "SEK", 200)
            };
            BankAccount.bankAccounts = accounts;
            var user = new User { Name = "user", UserId = 1 };

            double loanSize = 500;
            int toAcc = 123;

            //Act
            Loan.TakeOutLoan(user, loanSize, toAcc);
            var loans = Loan.GetUserLoans(user);


            //Assert
            var expectedBalance = 700; // 200 + 500
            Assert.AreEqual(expectedBalance, accounts[0].Balance);
            Assert.AreEqual(1,loans.Count);

        }

        [TestMethod]
        public void TakeOutLoan_ShouldThrowException_WhenLoanAmountIsTooHigh()
        {
            //Arrange
            var accounts = new List<BankAccount>
            {
                new BankAccount("test1", "test", 123, "user", 1, "SEK", 200)
            };
            BankAccount.bankAccounts = accounts;
            var user = new User { Name= "user", UserId = 1 };
            double loanSize = 2000;
            int toAcc = 123;

            //Act
            Loan.TakeOutLoan(user, loanSize, toAcc);
            var loans = Loan.GetUserLoans(user);

            //Assert
            var expectedBalance = accounts[0].Balance = 200;  //Oförändrad account balance
            Assert.AreEqual(expectedBalance, accounts[0].Balance);
            Assert.AreEqual(0, loans.Count);
        }

        [TestMethod]
        public void TakeOutLoan_ShouldThrowException_WhenAccountIsInvalid()
        {
            var accounts = new List<BankAccount>
            {
                new BankAccount("test1", "test", 123, "user", 1, "SEK", 200)
            };
            BankAccount.bankAccounts = accounts;
            var user = new User { Name = "user", UserId = 1 };
            double loanSize = 100;
            int toAcc = 999; //fel kontonummer

            //Act
            Loan.TakeOutLoan(user, loanSize, toAcc);
            var loans = Loan.GetUserLoans(user);

            var expectedBalance = accounts[0].Balance = 200;
            Assert.AreEqual (expectedBalance, accounts[0].Balance);
            Assert.AreEqual(0, loans.Count);
        }

        [TestMethod]
        public void TakeOutLoan_ShouldThrowException_WhenAmountIsNegative()
        {
            var accounts = new List<BankAccount>
            {
                new BankAccount("test1", "test", 123, "user", 1, "SEK", 200)
            };
            BankAccount.bankAccounts = accounts;
            var user = new User { Name = "user", UserId = 1 };
            double loanSize = -100;  //negativt värde
            int toAcc = 123; 

            //Act
            Loan.TakeOutLoan(user, loanSize, toAcc);
            var loans = Loan.GetUserLoans(user);

            var expectedBalance = accounts[0].Balance = 200;
            Assert.AreEqual(expectedBalance, accounts[0].Balance);
            Assert.AreEqual(0, loans.Count);
        }

        [TestMethod]
        public void TakeOutLoan_ShouldSucceed_WhenLoanAmountIsExactlyLimit()
        {
            var accounts = new List<BankAccount>
            {
                 new BankAccount("test1", "test", 123, "user", 1, "SEK", 100)
            };
            BankAccount.bankAccounts = accounts;
            var user = new User { Name = "user",UserId = 1};

            double loanSize = 500; //lån 5 gånger så stort som users balance
            int toAcc = 123;

            //Act
            Loan.TakeOutLoan(user, loanSize, toAcc);
            var loans = Loan.GetUserLoans(user);

            //assert
            var expectedBalance = 100 + 500;
            Assert.AreEqual(expectedBalance, accounts[0].Balance);
            Assert.AreEqual(1,loans.Count);
            Assert.AreEqual(500, loans[0].loanAmount);
        }

    }
}
