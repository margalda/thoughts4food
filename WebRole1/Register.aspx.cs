using System;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace WebRole1
{
    public partial class Register : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private void SendToQueue(string message)
        {
            // initialize the account information 
            var storageAccount =
                CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));

            // retrieve a reference to the messages queue 
            var queueClient = storageAccount.CreateCloudQueueClient();
            var queue = queueClient.GetQueueReference("messagequeue");

            queue.CreateIfNotExists();

            var msg = new CloudQueueMessage(message);
            queue.AddMessage(msg);
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            if (!UserExists(txtUsername.Text))
            {
                ClientScript.RegisterStartupScript(GetType(), "alert",
                    InsertUser(txtUsername.Text, txtPassword.Text, txtEmail.Text, Int32.Parse(txtAge.Text))
                        ? "alert('User registered successfuly!');"
                        : "alert('Registration failed. Please try again.');", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "alert", "alert('Username already exists.');", true);
            }

        }

        private bool UserExists(string username)
        {
            try
            {
                SqlConnectionStringBuilder builder =
                    new SqlConnectionStringBuilder
                    {
                        DataSource = "thoughts4food.database.windows.net",
                        UserID = "thoughts4food",
                        Password = "Kfc369nba",
                        InitialCatalog = "thoughts4foodSQL"
                    };

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();

                    StringBuilder sb = new StringBuilder();
                    sb.Append($"SELECT * FROM Users WHERE Username = '{username}';");
                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            return reader.Read();
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        private bool InsertUser(string username, string password, string email, int age)
        {
            try
            {
                SqlConnectionStringBuilder builder =
                    new SqlConnectionStringBuilder
                    {
                        DataSource = "thoughts4food.database.windows.net",
                        UserID = "thoughts4food",
                        Password = "Kfc369nba",
                        InitialCatalog = "thoughts4foodSQL"
                    };

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();

                    StringBuilder sb = new StringBuilder();
                    sb.Append("INSERT INTO Users ");
                    sb.Append($"VALUES('{username}', '{password}', '{email}', '{age}');");
                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }
    }
}