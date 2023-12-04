﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syntax_Squad
{
    public class LoanMenu : Menu
    {
        private bool run = true;
        public override void ShowMenu(User user)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Loan loan = new Loan();
            do
            {
                Console.WriteLine("\t---|| Loan Menu ||---");
                Console.WriteLine("\t1: Take loan \n\t2: See Loans \n\t3: Return to menu");

                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "1":
                        loan.TakeOutLoan();
                        break;
                    case "2":
                        loan.SeeLoans();
                        break;
                    case "3":
                        run = false;
                        break;
                    default:
                        break;
                }
            } while (run);
        }
    }
}
