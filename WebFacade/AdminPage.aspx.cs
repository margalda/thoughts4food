using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;

namespace WebFacade
{
    public partial class AdminPage : Page
    {
        private string _adminName;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    _adminName = "admin";

                    string[] info = QueriesRunner.GetUserInfo(_adminName);
                    if (info != null)
                    {
                        ratingBox.Text = info[4];
                        numOfRatersBox.Text = info[5];
                    }

                    txtUsers.Text = QueriesRunner.GetNumOfUsers();
                    txtRecipes.Text = GetNumOfRecipes();
                    txtIngridients.Text = GetNumOfIngridients();

                    foreach (var cuisine in Enum.GetValues(typeof(Cuisine)))
                    {
                        CloudHelpers.EnsureContainerExists(cuisine.ToString());
                    }
                }

                RefreshRecipes();
            }
            catch (StorageException se)
            {
                Console.WriteLine("Storage service error: " + se.Message);
            }
        }

        private string GetNumOfIngridients()
        {
            CloudTable recipes = CloudHelpers.GetTable("ingredients");
            int ans = 0;
            foreach (var cuisine in Enum.GetValues(typeof(Category)))
            {
                TableQuery<DynamicTableEntity> query = new TableQuery<DynamicTableEntity>().Where(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, cuisine.ToString()));
                foreach (DynamicTableEntity entity in recipes.ExecuteQuery(query))
                {
                    ans++;
                }
            }

            return ans.ToString();
        }

        private string GetNumOfRecipes()
        {
            CloudTable recipes = CloudHelpers.GetTable("recipes");
            int ans = 0;
            foreach (var cuisine in Enum.GetValues(typeof(Cuisine)))
            {
                TableQuery<DynamicTableEntity> query = new TableQuery<DynamicTableEntity>().Where(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, cuisine.ToString()));
                foreach (DynamicTableEntity entity in recipes.ExecuteQuery(query))
                {
                    ans++;
                }
            }

            return ans.ToString();
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

        protected void OnDeleteImage(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                var blobUri = (string) e.CommandArgument;
                string[] rowAndPartition = blobUri.Split(',');

                CloudTable recipes = CloudHelpers.GetTable("recipes");

                string cuisine = rowAndPartition[0].Split('-')[1];
                var blob = CloudHelpers.GetContainer(cuisine).GetBlockBlobReference(rowAndPartition[1]);
                blob.DeleteIfExists();
                cuisine = cuisine.First().ToString().ToUpper() + cuisine.Substring(1);
                TableOperation retrieveOperation =
                    TableOperation.Retrieve<DynamicTableEntity>(cuisine, rowAndPartition[1]);

                // Execute the operation.
                TableResult retrievedResult = recipes.Execute(retrieveOperation);
                // Assign the result to a CustomerEntity.
                DynamicTableEntity deleteEntity = (DynamicTableEntity) retrievedResult.Result;

                // Create the Delete TableOperation.
                if (deleteEntity != null)
                {
                    TableOperation deleteOperation = TableOperation.Delete(deleteEntity);

                    // Execute the operation.
                    recipes.Execute(deleteOperation);
                    Console.WriteLine("Entity deleted.");
                }
            }

            //send to queue
            foreach (var user in QueriesRunner.GetAllUsernames())
            {
                CloudHelpers.SendToQueue(user);
            }

            RefreshRecipes();
        }

        private void RefreshRecipes()
        {
            var allBlobs = new List<IListBlobItem>();
            foreach (var cuisine in Enum.GetValues(typeof(Cuisine)))
            {
                allBlobs.AddRange(CloudHelpers.GetContainer(cuisine.ToString())
                    .ListBlobs(null, true, BlobListingDetails.All, new BlobRequestOptions()));
            }

            //images.DataSource = GetContainer().ListBlobs(null, true, BlobListingDetails.All, new BlobRequestOptions());
            images.DataSource = allBlobs;
            images.DataBind();
        }

        protected void Save_Click(object sender, EventArgs e)
        {
            List<string> userInfo = new List<string>();
            List<string> usersColumns = new List<string>();
            userInfo.Add(_adminName);
            userInfo.Add("kfc369nba");
            userInfo.Add("admin@admin.admin");
            userInfo.Add("200");
            userInfo.Add(ratingBox.Text);
            userInfo.Add(numOfRatersBox.Text);
            usersColumns.Add("Username");
            usersColumns.Add("Password");
            usersColumns.Add("Email");
            usersColumns.Add("Age");
            usersColumns.Add("TimePreferences");
            usersColumns.Add("CuisinePreferences");
            ClientScript.RegisterStartupScript(GetType(), "alert",
                QueriesRunner.UpdateTable("Users", "admin", userInfo, usersColumns)
                    ? "alert('Reating And Number Of Reters Update successfuly!');"
                    : "alert('Reating And Number Of Reters Update failed');"
                , true);
        }
    }
}