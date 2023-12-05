﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syntax_Squad
{
    //Noah SUT23
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; } = false;
        public bool IsLoggedIn { get; set; } = false;

        public static List<User> AllTheUsers = new List<User>();

        public static List<User> AllUsers()
        {
            var users = new List<User>
            {
            new Admin("Syntax", "Squad", 1337),
            new RegularUser("Börje", "kaffe123", 101),
            new RegularUser("Stefan", "betong321", 102),
            new RegularUser("Åke", "snus444", 103)

            };

            return users;
        }

    }
}
