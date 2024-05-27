using Moq;
using Syntax_Squad;

namespace MsTest_Syntax_Squad
{
    [TestClass]
    public class TransferTests
    {

        //Tester som bör finnas med
        //Överföring mellan konton med samma valuta / negativa värden
        //Överföring mellan konton med olika valuta / negativa värden
        //Överföring till annan användare 
        

        [TestMethod]
        public void TransferBetweenOwnAccounts_SameCurrency_ShouldTransfer()
        {
            //Arrange
            var accounts = new List<BankAccount>
            {
                new BankAccount ("test1", "test", 123, "user", 1, "SEK", 200),
                new BankAccount ("test2", "test", 321, "user", 2, "SEK", 200),
            };
            var transfer = new Transfer();
            transfer.SetAccounts (accounts);
            var user = new User
            {
                Name = "user",
                TransferLimit = 1000
            };

            //Act
            transfer.TransferBetweenOwnAccounts(user, 123, 321, 100);
            var expectedFromAccountBalance = 100m;
            var expectedToAccountBalance = 300m;

            //Assert
            Assert.AreEqual(expectedFromAccountBalance,(decimal) accounts[0].Balance);
            Assert.AreEqual(expectedToAccountBalance,(decimal) accounts[1].Balance);
            

        }

        [TestMethod]
        public void TransferBetweenOwnAccounts_DifferentCurrency_ShouldTransfer_GBP_to_SEK()
        {
            //Arrange
            var accounts = new List<BankAccount>
            {
                new BankAccount ("test1", "test", 123, "user", 1, "GBP", 200),
                new BankAccount ("test2", "test", 321, "user", 2, "SEK", 200),
            };
            var transfer = new Transfer();
            transfer.SetAccounts (accounts);
            var user = new User
            {
                Name = "user",
                TransferLimit = 1000
            };

            //Act
            transfer.TransferBetweenOwnAccounts(user, 123, 321, 100);

            //Assert
            var expectedFromAccountBalance = 100m;
            Assert.AreEqual(expectedFromAccountBalance, (decimal)accounts[0].Balance);
            Assert.IsTrue((decimal)accounts[1].Balance > 200m);

        }

        [TestMethod]
        public void TransferBetweenOwnAccounts_SameCurrency_ShouldNotTransfer_NegativeAmount()
        {
            //Arrange
            var accounts = new List<BankAccount>
            {
                new BankAccount ("test1", "test", 123, "user", 1, "SEK", 200),
                new BankAccount ("test2", "test", 321, "user", 2, "SEK", 200),
            };
            var transfer = new Transfer();
            transfer.SetAccounts(accounts);
            var user = new User
            {
                Name = "user",
                TransferLimit = 1000
            };

            //Act
            transfer.TransferBetweenOwnAccounts(user, 123, 321, -100);

            //Assert
            var expectedFromAccountBalance = 200m;
            var expectedToAccountBalance = 200m;

            Assert.AreEqual(expectedFromAccountBalance, (decimal)accounts[0].Balance);
            Assert.AreEqual(expectedToAccountBalance, (decimal)accounts[1].Balance);
        }

        [TestMethod]
        public void TransferBetweenOwnAccounts_DifferentCurrency_ShouldNotTransfer_NegativeAmount()
        {
            //Arrange
            var accounts = new List<BankAccount>
            {
                new BankAccount ("test1", "test", 123, "user", 1, "GBP", 200),
                new BankAccount ("test2", "test", 321, "user", 2, "SEK", 200),
            };
            var transfer = new Transfer();
            transfer.SetAccounts(accounts);
            var user = new User
            {
                Name = "user",
                TransferLimit = 1000
            };

            //Act
            transfer.TransferBetweenOwnAccounts(user, 123, 321, -100);

            //Assert
            var expectedFromAccountBalance = 200m;
            var expectedToAccountBalance = 200m;

            Assert.AreEqual(expectedFromAccountBalance, (decimal)accounts[0].Balance);
            Assert.AreEqual(expectedToAccountBalance, (decimal)accounts[1].Balance);
        }

        [TestMethod]
        public void TransferBetweenOtherAccounts_SameCurrency_ShouldTransfer()
        {
            var accounts = new List<BankAccount>
            {
                new BankAccount("test1", "test1", 123, "user1",1,"SEK", 200),
                new BankAccount("test2", "test2", 321, "user2",2,"SEK", 200)
            };
            var transfer = new Transfer();
            transfer.SetAccounts(accounts);
            var user1 = new User { Name = "user1", Password = "password1", TransferLimit = 1000 };  
            var user2 = new User { Name = "user2", Password = "password2", TransferLimit = 1000 };


            //ACT
            transfer.TransferBetweenOtherAccounts(user1, 123, 321,100, "password1");

            var expectedFromAccountBalance = 100m;
            var expectedToAccountBalance = 300m;
            

            Assert.AreEqual(expectedFromAccountBalance,(decimal) accounts[0].Balance);
            Assert.AreEqual(expectedToAccountBalance, (decimal)accounts[1].Balance);
        }

        [TestMethod]
        public void TransferBetweenOtherAccounts_DifferentCurrency_ShouldTransfer_GBP_to_SEK()
        {
            var accounts = new List<BankAccount>
            {
                new BankAccount("test1", "test1", 123, "user1",1,"GBP", 200),
                new BankAccount("test2", "test2", 321, "user2",2,"SEK", 200)
            };
            var transfer = new Transfer();
            transfer.SetAccounts(accounts);
            var user1 = new User { Name = "user1", Password = "password1", TransferLimit = 1000 };
            var user2 = new User { Name = "user2", Password = "password2", TransferLimit = 1000 };


            //ACT
            transfer.TransferBetweenOtherAccounts(user1, 123, 321, 100, "password1");

            var expectedFromAccountBalance = 100m;

            Assert.AreEqual(expectedFromAccountBalance, (decimal)accounts[0].Balance);
            Assert.IsTrue((decimal)accounts[1].Balance > 200);

        }

        
    }
}