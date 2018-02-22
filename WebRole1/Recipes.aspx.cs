using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Azure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace WebRole1
{
    public partial class Recipes : Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    EnsureContainerExists();
                }
                RefreshRecipes();
            }
            catch (WebException we)
            {
                status.Text = "Network error: " + we.Message;
                if (we.Status == WebExceptionStatus.ConnectFailure)
                {
                    status.Text += "<br />Please check if the blob service is running at " +
                                   ConfigurationManager.AppSettings["storageEndpoint"];
                }
            }
            catch (StorageException se)
            {
                Console.WriteLine("Storage service error: " + se.Message);
            }
        }

        protected void OnBlobDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var metadataRepeater = e.Item.FindControl("blobMetadata") as Repeater;
                var blob = ((ListViewDataItem)e.Item).DataItem as CloudBlockBlob;
                // If this blob is a snapshot, rename button to "Delete Snapshot"
                if (blob != null)
                {
                    if (metadataRepeater != null)
                    {
                        //bind to metadata
                        metadataRepeater.DataSource = from key in blob.Metadata.Keys
                                                      select new
                                                      {
                                                          Name = key,
                                                          Value = blob.Metadata[key]
                                                      };
                        metadataRepeater.DataBind();
                    }
                }
            }
        }

        protected void OnDeleteImage(object sender, CommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Delete")
                {
                    var blobUri = (string)e.CommandArgument;
                    var blob = GetContainer().GetBlockBlobReference(blobUri);
                    blob.DeleteIfExists();
                    status.Text = "";
                }
            }
            catch (StorageException se)
            {
                status.Text = "Storage client error: " + se.Message;
            }

            RefreshRecipes();
        }

        protected void OnCopyImage(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Copy")
            {
                // Prepare an Id for the copied blob
                var newId = Guid.NewGuid();
                // Get source blob
                var blobUri = (string)e.CommandArgument;
                var srcBlob = GetContainer().GetBlockBlobReference(blobUri);
                // Create new blob
                var newBlob = GetContainer().GetBlockBlobReference(newId.ToString());
                // Copy content from source blob
                newBlob.StartCopy(srcBlob.Uri);
                // Explicitly get metadata for new blob
                newBlob.FetchAttributes();
                // Change metadata on the new blob to reflect this is a copy via UI
                newBlob.Metadata["ImageName"] = "Copy of \"" + newBlob.Metadata["ImageName"] + "\"";
                newBlob.Metadata["Id"] = newId.ToString();
                newBlob.SetMetadata();
                // Render all blobs
                RefreshRecipes();
            }
        }

        private void EnsureContainerExists()
        {
            var container = GetContainer();
            container.CreateIfNotExists();
            var permissions = container.GetPermissions();
            permissions.PublicAccess = BlobContainerPublicAccessType.Container;
            container.SetPermissions(permissions);
        }

        private CloudBlobContainer GetContainer()
        {
            // Get a handle on account, create a blob service client and get container proxy
            var account = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            var client = account.CreateCloudBlobClient();

            return client.GetContainerReference(RoleEnvironment.GetConfigurationSettingValue("ContainerName") + "-photo");
        }

        private void RefreshRecipes()
        {
            images.DataSource = GetContainer().ListBlobs(null, true, BlobListingDetails.All, new BlobRequestOptions());
            images.DataBind();
        }
    }
}