﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syntax_Squad
{
    public class LoanMenu : UserMenu
    {
        public override void ShowMenu(User user)
        {
            bool validChoice = false;
            do
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\t---|| Loan menu ||---");
                Console.WriteLine("\t1: Take Loan \n\t2: See Loans");

                string userInput = Console.ReadLine();
                switch (userInput)
                {
                    case "1":
                        
                        validChoice = true;
                        break;
                    case "2":
                        //See loans metod kommer här
                        validChoice = true;
                        break;
                    default:
                        Console.WriteLine("choose 1 or 2 please");
                        break;
                }


            } while (!validChoice);

        }
    }
}
