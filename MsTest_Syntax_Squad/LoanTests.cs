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


            //Assert
            var expectedBalance = 700; // 200 + 500
            Assert.AreEqual(expectedBalance, accounts[0].Balance);
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
            var user = new User { Name="user", UserId = 1 };

            double loanSize = 2000;
            int toAcc = 123;

            
            // Act & Assert
            Assert.ThrowsException<InvalidOperationException>(() => Loan.TakeOutLoan(user, loanSize, toAcc));

        }

        [TestMethod]
        public void GetUserLoans_ShouldReturnLoans()
        {
            //Arrange
            var accounts = new List<BankAccount>
            {
                new BankAccount("test1", "test", 123, "user", 1, "SEK", 200)
            };
            BankAccount.bankAccounts = accounts;
            var user = new User { Name = "user", UserId = 1 };


            //Act
            Loan.TakeOutLoan(user, 400, 123);
            var loans = Loan.GetUserLoans(user);

            //Assert
            Assert.AreEqual(1, loans.Count);
            Assert.AreEqual(400, loans[0].loanAmount);
        }

    }
}
