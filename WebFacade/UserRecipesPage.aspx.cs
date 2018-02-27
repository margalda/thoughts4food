using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;

namespace WebFacade
{
    public partial class UserRecipesPage : Page
    {
        private string _currentUser;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                _currentUser = Request.QueryString["username"];
                if (!IsPostBack)
                {
                    foreach (var cuisine in Enum.GetValues(typeof(Cuisine)))
                    {
                        CloudHelpers.EnsureContainerExists(cuisine.ToString());
                    }
                }

                CloudHelpers.EnsureTableExists("usersRecipes");

                RefreshRecipes();
            }
            catch (WebException we)
            {
                string status = "Network error: " + we.Message;
                if (we.Status == WebExceptionStatus.ConnectFailure)
                {
                    status += "<br />Please check if the blob service is running at " +
                              ConfigurationManager.AppSettings["storageEndpoint"];
                    Console.WriteLine(status);
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

        private void RefreshRecipes()
        {
            images.DataSource = GetUserRecipes();
            images.DataBind();
        }

        private List<IListBlobItem> GetUserRecipes()
        {
            var res = new List<IListBlobItem>();
            //get recipes names from azure table
            CloudTable table = CloudHelpers.GetTable("usersRecipes");
            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation =
                TableOperation.Retrieve<DynamicTableEntity>(_currentUser[0].ToString(), _currentUser);
            // Execute the retrieve operation.
            TableResult retrievedResult = table.Execute(retrieveOperation);

            if (retrievedResult.Result != null)
            {
                List<string> userRecipes = new List<string>();
                foreach (var recipeProperty in ((DynamicTableEntity) retrievedResult.Result).Properties)
                {
                    userRecipes.Add(recipeProperty.Value.StringValue);
                }

                //add relevant blobs to res list
                foreach (var recipe in userRecipes)
                {
                    var recipeParts = recipe.Split('_');
                    var blobIntems = CloudHelpers.GetContainer(recipeParts[0])
                        .ListBlobs(null, true, BlobListingDetails.All, new BlobRequestOptions());

                    foreach (var item in blobIntems)
                    {
                        var blob = (CloudBlockBlob) item;
                        if (blob.Name == recipeParts[1])
                        {
                            res.Add(blob);
                        }
                    }
                }
            }

            return res;
        }
    }
}