namespace ATM_project;

public class FULL
{
    static string connectionString = @"Server=(localdb)\MSSQLLocalDB";

    static void Full()
    {
        // viewAccounts();

        // viewTransaction();

        // getUser();
        
        logIn();
    }

    static void logIn()
    {
        Console.WriteLine($"Welcome to TopaZ Banking! \n Enter your bank account number:");
        string inputID = Console.ReadLine();
        Console.WriteLine("Enter your pin:");
        string inputPIN = Console.ReadLine();
        getUser(inputID, inputPIN);
        Console.WriteLine();
        //return userID;
    }

    static void mainMenu()
    {
    }

    static void viewAccounts()
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var selectAllAccounts = connection.CreateCommand();
            selectAllAccounts.CommandText = $"SELECT * FROM TopazBanking.dbo.Accounts";
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
                Console.WriteLine($"{account.userID} {account.accountID} {account.accountNumber} {account.balance:c}");
            }
        }
    }

    static void getUser(string ID, string PIN)
    {
        string uID;
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var userQ = connection.CreateCommand();
            userQ.CommandText = $"SELECT * FROM TopazBanking.dbo.Users WHERE userID = '{ID}' and userPIN ='{PIN}'";
            List<User> tableUser = new();
            SqlDataReader reader = userQ.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableUser.Add(
                        new User
                        {
                            userID = reader.GetString(0),
                            userName = reader.GetString(1),
                            userPIN = reader.GetString(2)
                        });
                }
            }
            else
            {
                Console.WriteLine("No user found");
            }
            connection.Close();
            foreach (var user in tableUser)
            {
                Console.WriteLine($"Welcome, {user.userName}! ");
            }
            
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
