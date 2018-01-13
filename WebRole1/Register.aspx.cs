using System;
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

        protected void Button1_Click(object sender, EventArgs e)
        {
            SendToQueue(TextBox1.Text + " " + TextBox2.Text + " " + TextBox3.Text + " " + TextBox4.Text);
        }
    }
}