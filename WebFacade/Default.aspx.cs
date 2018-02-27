using System;
using System.Web.UI;

namespace WebFacade
{
    public partial class Default : Page
    {
        private string _nextPageName = "UserRecipesPage.aspx";

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Login_Click(object sender, EventArgs e)
        {
            if (QueriesRunner.UserValid(txtUsername.Text, txtPassword.Text))
            {
                if (txtUsername.Text == "admin")
                {
                    _nextPageName = "AdminPage.aspx";
                }

                ClientScript.RegisterStartupScript(GetType(), "alert",
                    $"alert('Welcome {txtUsername.Text}!'); window.open('{_nextPageName}?username={txtUsername.Text}','_self');",
                    true);
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "alert",
                    "alert('Invalid username/password');", true);
            }
        }
    }
}