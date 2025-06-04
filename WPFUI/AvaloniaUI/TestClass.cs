using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using AvaloniaUI.Models;

namespace AvaloniaUI
{
    public class TestClass
    {
        public static List<Account> CreateList()
        {
            var random = new Random();
            List<Account> accounts = new List<Account>();
            for (int i = 1; i <= 202; i++)
            {
                accounts.Add(
                new Account


                {
                    AccountName = $"User{i}",
                    Name = $"Name{i}",
                    Group = $"Group{i % 3}",
                    Password = $"Pass{i}",
                    TwoFA = $"2FA{i}",
                    Cookie = $"Cookie{i}",
                    Token = $"Token{i}",
                    Email = $"user{i}@example.com",
                    EmailPassword = $"EmailPass{i}",
                    RecoveryEmail = $"recovery{i}@example.com",
                    State = random.Next(0, 4) switch
                    {
                        0 => "LIVE",
                        1 => "DIE",
                        2 => "PENDING",
                        _ => ""
                    },
                    LastActivity = $"2025-05-27 {i % 24}:00",
                    Result = "Success",
                    Status = "Running",
                    IsSelected = false
                }
                );


            }
            return accounts;
        }
    }
}
