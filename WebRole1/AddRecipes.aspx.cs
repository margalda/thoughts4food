using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Azure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;

namespace WebRole1
{
    public partial class AddRecipes : Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            List<ListItem> times = new List<ListItem>();
            foreach (var time in Enum.GetValues(typeof(Time)))
            {
                times.Add(new ListItem(time.ToString(), time.ToString()));
            }

            List<ListItem> cuisines = new List<ListItem>();
            foreach (var cuisine in Enum.GetValues(typeof(Cuisine)))
            {
                cuisines.Add(new ListItem(cuisine.ToString(), cuisine.ToString()));
            }

            List<ListItem> kinds = new List<ListItem>();
            foreach (var kind in Enum.GetValues(typeof(Kind)))
            {
                kinds.Add(new ListItem(kind.ToString(), kind.ToString()));
            }

            List<ListItem> measurements = new List<ListItem>();
            foreach (var measurement in Enum.GetValues(typeof(Cuisine)))
            {
                measurements.Add(new ListItem(measurement.ToString(), measurement.ToString()));
            }

            timeList.DataTextField = "Text";
            timeList.DataValueField = "Value";
            timeList.DataSource = times;
            timeList.DataBind();

            kindList.DataTextField = "Text";
            kindList.DataValueField = "Value";
            kindList.DataSource = kinds;
            kindList.DataBind();

            cuisineList.DataTextField = "Text";
            cuisineList.DataValueField = "Value";
            cuisineList.DataSource = cuisines;
            cuisineList.DataBind();

            measurementList1.DataTextField = "Text";
            measurementList1.DataValueField = "Value";
            measurementList1.DataSource = measurements;
            measurementList1.DataBind();

            measurementList2.DataTextField = "Text";
            measurementList2.DataValueField = "Value";
            measurementList2.DataSource = measurements;
            measurementList2.DataBind();

            measurementList3.DataTextField = "Text";
            measurementList3.DataValueField = "Value";
            measurementList3.DataSource = measurements;
            measurementList3.DataBind();

            measurementList4.DataTextField = "Text";
            measurementList4.DataValueField = "Value";
            measurementList4.DataSource = measurements;
            measurementList4.DataBind();

            measurementList5.DataTextField = "Text";
            measurementList5.DataValueField = "Value";
            measurementList5.DataSource = measurements;
            measurementList5.DataBind();

            measurementList6.DataTextField = "Text";
            measurementList6.DataValueField = "Value";
            measurementList6.DataSource = measurements;
            measurementList6.DataBind();

            measurementList7.DataTextField = "Text";
            measurementList7.DataValueField = "Value";
            measurementList7.DataSource = measurements;
            measurementList7.DataBind();

            measurementList8.DataTextField = "Text";
            measurementList8.DataValueField = "Value";
            measurementList8.DataSource = measurements;
            measurementList8.DataBind();

            try
            {
                if (!IsPostBack)
                {
                    EnsureContainerExists();
                    EnsureTableExists();
                }
            }
            catch (WebException we)
            {
                status.Text = "Network error: " + we.Message;
                if (we.Status == WebExceptionStatus.ConnectFailure)
                {
                    status.Text += "<br />Please check if the blob service is running at " + ConfigurationManager.AppSettings["storageEndpoint"];
                }
            }
            catch (StorageException se)
            {
                Console.WriteLine("Storage service error: " + se.Message);
            }
        }

        private void EnsureTableExists()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            // Create the table if it doesn't exist.
            CloudTable table = tableClient.GetTableReference("recipes");
            table.CreateIfNotExists();
        }

        private void EnsureContainerExists()
        {
            var container = GetContainer();
            container.CreateIfNotExists();
            var permissions = container.GetPermissions();
            permissions.PublicAccess = BlobContainerPublicAccessType.Container;
            container.SetPermissions(permissions);
        }

        protected void Upload_Click(object sender, EventArgs e)
        {
            if (imageFile.HasFile)
            {
                SaveImage(
                    Guid.NewGuid().ToString(),
                    recipeName.Text,
                    imageFile.FileName,
                    imageFile.PostedFile.ContentType,
                    imageFile.PostedFile.InputStream
                );
                status.Text = "Inserted [" + imageFile.FileName + "] - Content Type [" +
                              imageFile.PostedFile.ContentType + "] - Length [" +
                              imageFile.PostedFile.ContentLength + "]";
            }
            else
            {
                status.Text = "No image file";
            }
        }

        private CloudBlobContainer GetContainer()
        {
            // Get a handle on account, create a blob service client and get container proxy

            var account = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            var client = account.CreateCloudBlobClient();
            return client.GetContainerReference(RoleEnvironment.GetConfigurationSettingValue("ContainerName") + "-photo");
        }

        private CloudTable GetTable()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            // Create the table if it doesn't exist.
            return tableClient.GetTableReference("recipes");
        }

        private void SaveImage(string id, string name, string fileName, string contentType, Stream fileStream)
        {
            // Create a blob in container and upload image bytes to it
            var blob = GetContainer().GetBlockBlobReference(name);
            blob.Properties.ContentType = contentType;
            // Create some metadata for this image
            blob.Metadata.Add("Id", id);
            blob.Metadata.Add("Filename", fileName);
            blob.Metadata.Add("RecipeName", name);

            blob.UploadFromStream(fileStream);
            blob.SetMetadata();
            //send to queue
            SendToQueue(name);
        }

        private void SendToQueue(string name)
        {
            // initialize the account information 
            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));

            // retrieve a reference to the messages queue 
            var queueClient = storageAccount.CreateCloudQueueClient();
            var queue = queueClient.GetQueueReference("messagequeue");

            queue.CreateIfNotExists();

            var msg = new CloudQueueMessage(name);
            queue.AddMessage(msg);
        }
    }
}