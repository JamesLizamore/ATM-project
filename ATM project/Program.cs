﻿namespace ATM_project;

class Program
{
    static string connectionString = @"Server=(localdb)\MSSQLLocalDB";

    static void Main(string[] args)
    {
        //viewAccounts();

        // viewTransaction();

        // getUser();
        
         logIn();
         
    }

    static void logIn()
    {
        Console.WriteLine($"Welcome to TopaZ Banking! \n Enter your User ID:");
        string inputID = Console.ReadLine();
        Console.WriteLine("Enter your pin:");
        string inputPIN = Console.ReadLine();
        
        getUser(inputID, inputPIN);
    }

    static void accMenu(string accNum)
    {
        Console.WriteLine($@"Choose an option for Acc Num: {accNum}
        1) Withdraw money
        2) Deposit money
        3) View balance");
        Console.WriteLine($"\n\n Enter 0 to return to previous menu");
        var accOption = Console.ReadLine();
        switch (accOption)
        {
            case "1":
                break;
            case "2":
                break;
            case "3":
                break;
            default:
                accMenu(accNum);
                break;
        }
    }

    static void viewAccounts(string userID)
    {
        
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var selectAllAccounts = connection.CreateCommand();
            selectAllAccounts.CommandText = $"SELECT * FROM TopazBanking.dbo.Accounts WHERE userID = '{userID}'";
            List<Account> tableData = new();
            SqlDataReader reader = selectAllAccounts.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(
                        new Account
                        {
                            accountID = reader.GetString(0),
                            userID = reader.GetString(1),
                            accountNumber = reader.GetString(2),
                            balance = reader.GetDecimal(3),
                        });
                }
            }
            else
            {
                Console.WriteLine("No accounts found");
            }

            connection.Close();

            foreach (var account in tableData)
            {
                Console.WriteLine($"{account.accountID} {account.accountNumber} {account.balance:c}\n");
            }
            
            Console.WriteLine($"\n\t Enter acc no to proceed to operations menu");
            string accNum = Console.ReadLine();
            accMenu(accNum);
        }
    }

    static void getUser(string ID, string PIN)
    {
        string uID;
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var userQ = connection.CreateCommand();
            userQ.CommandText = $"SELECT userID FROM TopazBanking.dbo.Users WHERE userID = '{ID}' and userPIN ='{PIN}'";

            string verifiedID = userQ.ExecuteScalar() as string;
            
            
            
            connection.Close();

            if (verifiedID == ID) viewAccounts(verifiedID);
            else
            {
                Console.Clear();
                Console.WriteLine("Invalid credentials");
                logIn();
            };
           // if (verifiedID == ID) return verifiedID;
            
            // List<User> tableUser = new();
            //SqlDataReader reader = userQ.ExecuteReader();

            // if (reader.HasRows)
            // {
            //     while (reader.Read())
            //     {
            //         tableUser.Add(
            //             new User
            //             {
            //                 userID = reader.GetString(0),
            //                 userName = reader.GetString(1),
            //                 userPIN = reader.GetString(2)
            //             });
            //     }
            // }
            // else
            // {
            //     Console.WriteLine("No user found");
            // }
            // connection.Close();
            // foreach (var user in tableUser)
            // {
            //     Console.WriteLine($"Welcome, {user.userName}! ");
            // }
        }
    }

    static void viewTransaction()
    {
        using (var trans = new SqlConnection(connectionString))
        {
            trans.Open();
            var selectAllTransaction = trans.CreateCommand();
            selectAllTransaction.CommandText = $"SELECT * FROM TopazBanking.dbo.Transactions";
            List<Transaction> tableTrans = new();
            SqlDataReader reader = selectAllTransaction.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableTrans.Add(
                        new Transaction
                        {
                            transactionID = reader.GetString(0),
                            accountID = reader.GetString(1),
                            transactionType = reader.GetString(2),
                            amount = reader.GetDecimal(3),
                            transactionDateTime = reader.GetDateTime(4),
                        });
                }
            }
            else
            {
                Console.WriteLine("No transactions found");
            }

            trans.Close();

            foreach (var transaction in tableTrans)
            {
                Console.WriteLine(
                    $"{transaction.transactionID} {transaction.accountID} {transaction.transactionType} {transaction.amount:c} {transaction.transactionDateTime}");
            }
        }
    }

    public class Account
    {
        public string accountID { get; set; }
        public string userID { get; set; }
        public string accountNumber { get; set; }
        public decimal balance { get; set; }
    }

    public class User
    {
        public string userID { get; set; }
        public string userName { get; set; }
        public string userPIN { get; set; }
    }

    public class Transaction
    {
        //AccountID for transactions

        public string transactionID { get; set; }
        public string accountID { get; set; }
        public string transactionType { get; set; }
        public decimal amount { get; set; }
        public DateTime transactionDateTime { get; set; }
    }
}