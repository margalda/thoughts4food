using System;
using System.Web.UI;

namespace WebFacade
{
    public partial class Site : MasterPage
    {
        public static string Username;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtUsername.Text = Request.QueryString["username"];
                Username = txtUsername.Text;
            }
        }
    }
}