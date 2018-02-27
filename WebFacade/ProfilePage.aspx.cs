using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFacade
{
    public partial class ProfilePage : Page
    {
        private readonly List<CheckBox> _checkBoxList = new List<CheckBox>();
        private string _currentUser;

        protected void Page_Load(object sender, EventArgs e)
        {
            _currentUser = Request.QueryString["username"];
            if (!Page.IsPostBack)
            {
                Image1.ImageUrl = @"~\Resources\user_default.png";

                Label1.Text = "Username: " + _currentUser;

                string[] userInfo = QueriesRunner.GetUserInfo(_currentUser);
                if (userInfo != null)
                {
                    txtPassword.Attributes.Add("value", userInfo[1]);
                    txtConfirmPassword.Attributes.Add("value", userInfo[1]);
                    txtEmail.Text = userInfo[2];
                    txtAge.Text = userInfo[3];
                    if (userInfo[4] != "")
                    {
                        txtTime.Text = userInfo[4];
                    }

                    _checkBoxList.Add(CheckBox1);
                    _checkBoxList.Add(CheckBox2);
                    _checkBoxList.Add(CheckBox3);
                    _checkBoxList.Add(CheckBox4);
                    _checkBoxList.Add(CheckBox5);
                    _checkBoxList.Add(CheckBox6);
                    _checkBoxList.Add(CheckBox7);
                    _checkBoxList.Add(CheckBox8);
                    _checkBoxList.Add(CheckBox9);
                    _checkBoxList.Add(CheckBox10);
                    MarkCheckBoxs(Convert.ToInt32(userInfo[5]));
                }
            }
        }

        private void MarkCheckBoxs(int checkbox)
        {
            int i = 0;
            foreach (CheckBox boxelemnt in _checkBoxList.ToList())
            {
                int k = (checkbox >> i) & 1;
                boxelemnt.Checked = k == 1;
                i++;
            }
        }

        private int GetNumberFromCheckBoxList()
        {
            int ans = 0;
            int i = 0;
            foreach (CheckBox boxelemnt in _checkBoxList.ToList())
            {
                if (boxelemnt.Checked)
                    ans += (int) Math.Pow(2, i);
                i++;
            }

            return ans;
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            List<string> userInfo = new List<string>();
            List<string> usersColumns = new List<string>();
            int usernameLen = Label1.Text.Length - 10;
            string username = Label1.Text.Substring(10, usernameLen);

            userInfo.Add(username);
            userInfo.Add(txtPassword.Text);
            userInfo.Add(txtEmail.Text);
            userInfo.Add(txtAge.Text);
            userInfo.Add(txtTime.Text);

            _checkBoxList.Add(CheckBox1);
            _checkBoxList.Add(CheckBox2);
            _checkBoxList.Add(CheckBox3);
            _checkBoxList.Add(CheckBox4);
            _checkBoxList.Add(CheckBox5);
            _checkBoxList.Add(CheckBox6);
            _checkBoxList.Add(CheckBox7);
            _checkBoxList.Add(CheckBox8);
            _checkBoxList.Add(CheckBox9);
            _checkBoxList.Add(CheckBox10);
            userInfo.Add(GetNumberFromCheckBoxList().ToString());
            usersColumns.Add("Username");
            usersColumns.Add("Password");
            usersColumns.Add("Email");
            usersColumns.Add("Age");
            usersColumns.Add("TimePreferences");
            usersColumns.Add("CuisinePreferences");

            if (QueriesRunner.UpdateTable("Users", username, userInfo, usersColumns))
            {
                CloudHelpers.SendToQueue(_currentUser);
                ClientScript.RegisterStartupScript(GetType(), "alert", "alert('User updated successfully!');", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "alert", "alert('User update failed');", true);
            }
        }
    }
}