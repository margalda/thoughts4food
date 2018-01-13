using System;
using System.Configuration;
using System.IO;
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
        private const string PICS_DIR = @"D:\blob_pics";

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    EnsureContainerExists();
                    //InitBlob(); //for testing, set PICS_DIR before using 
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

        private void InitBlob()
        {
            foreach (string picPath in Directory.GetFiles(PICS_DIR))
            {
                using (var fileStream = File.OpenRead(picPath))
                {
                    SaveImage(
                        Guid.NewGuid().ToString(),
                        Path.GetFileNameWithoutExtension(picPath),
                        string.Format("a picture of a {0}", Path.GetFileNameWithoutExtension(picPath)),
                        string.Format("food, {0}", Path.GetFileNameWithoutExtension(picPath)),
                        picPath,
                        "food",
                        fileStream
                    );
                }
            }
        }

        /// <summary>
        ///     Cast out blob instance and bind it's metadata to metadata repeater
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnBlobDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var metadataRepeater = e.Item.FindControl("blobMetadata") as Repeater;
                var blob = ((ListViewDataItem) e.Item).DataItem as CloudBlockBlob;
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

        /// <summary>
        ///     Delete an image blob by Uri
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnDeleteImage(object sender, CommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Delete")
                {
                    var blobUri = (string) e.CommandArgument;
                    var blob = GetContainer().GetBlockBlobReference(blobUri);
                    bool result = blob.DeleteIfExists();
                    status.Text = "";
                }
            }
            catch (StorageException se)
            {
                status.Text = "Storage client error: " + se.Message;
            }
            catch (Exception)
            {
            }

            RefreshRecipes();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnCopyImage(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Copy")
            {
                // Prepare an Id for the copied blob
                var newId = Guid.NewGuid();
                // Get source blob
                var blobUri = (string) e.CommandArgument;
                var srcBlob = GetContainer().GetBlockBlobReference(blobUri);
                // Create new blob
                var newBlob = GetContainer().GetBlockBlobReference(newId.ToString());
                // Copy content from source blob
                newBlob.StartCopy(srcBlob.Uri);
                // Explicitly get metadata for new blob
                newBlob.FetchAttributes();
                // Change metadata on the new blob to reflect this is a copy via UI
                newBlob.Metadata["ImageName"] = "Copy of \"" +
                                                newBlob.Metadata["ImageName"] + "\"";
                newBlob.Metadata["Id"] = newId.ToString();
                newBlob.SetMetadata();
                // Render all blobs
                RefreshRecipes();
            }
        }


        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnListItemDeleting(object sender, EventArgs e)
        {
        }

        protected void images_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        #region

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
            return client.GetContainerReference(RoleEnvironment.GetConfigurationSettingValue("ContainerName"));
        }


        private void RefreshRecipes()
        {
            images.DataSource =
                GetContainer().ListBlobs(null, true, BlobListingDetails.All, new BlobRequestOptions(), null);
            images.DataBind();
        }

        private void SaveImage(string id, string name, string description, string tags, string fileName,
            string contentType, Stream fiileStream)
        {
            // Create a blob in container and upload image bytes to it
            var blob = GetContainer().GetBlockBlobReference(name);
            blob.Properties.ContentType = contentType;
            // Create some metadata for this image
            blob.Metadata.Add("Id", id);
            blob.Metadata.Add("Filename", fileName);
            blob.Metadata.Add("ImageName", string.IsNullOrEmpty(name) ? "unknown" : name);
            blob.Metadata.Add("Description", string.IsNullOrEmpty(description) ? "unknown" : description);
            blob.Metadata.Add("Tags", string.IsNullOrEmpty(tags) ? "unknown" : tags);

            blob.UploadFromStream(fiileStream);
            blob.SetMetadata();
        }

        #endregion
    }
}