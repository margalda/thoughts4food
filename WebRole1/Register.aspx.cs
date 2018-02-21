using System;
using System.Collections.Generic;
using System.Web.UI;

namespace WebRole1
{
    public partial class Register : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Register_Click(object sender, EventArgs e)
        {
            if (!QueriesRunner.ValueExists("Users", "Username", txtUsername.Text))
            {
                ClientScript.RegisterStartupScript(GetType(), "alert",
                QueriesRunner.InsertToTable(
                    "Users",
                    new List<string> { txtUsername.Text, txtPassword.Text, txtEmail.Text, txtAge.Text }
                ) ? "alert('User registered successfuly!');" : "alert('User registration failed');"
                    , true);
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "alert", "alert('Username already exists.');", true);
            }

        }
    }
}