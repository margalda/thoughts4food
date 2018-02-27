using System;
using System.Collections.Generic;
using System.Web.UI;

namespace WebFacade
{
    public partial class RegisterPage : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Register_Click(object sender, EventArgs e)
        {
            if (!QueriesRunner.ValueExists("Users", "Username", txtUsername.Text))
            {
                ClientScript.RegisterStartupScript(GetType(), "alert", QueriesRunner.InsertToTable(
                    "Users",
                    new List<string> {txtUsername.Text, txtPassword.Text, txtEmail.Text, txtAge.Text, null, "1023"}
                )
                    ? "alert('User registered successfully!'); window.open('LoginPage.aspx','_self');"
                    : "alert('User registration failed');", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "alert", "alert('Username already exists');", true);
            }
        }
    }
}