using System;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;

namespace WebRole1
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Login_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(GetType(), "alert",
                UserValid(txtUsername.Text, txtPassword.Text)
                    ? $"alert('Welcome {txtUsername.Text}!');"
                    : "alert('Invalid Username/Password');", true);
        }

        private bool UserValid(string username, string password)
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
                            return reader.Read() && reader.GetString(0) == username && reader.GetString(1) == password;
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
    }
}