using System;
using System.Data.SqlClient;
using AjaxControlToolkit;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using Microsoft.Azure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Text;

namespace WebRole1
{
    public partial class Recipe : System.Web.UI.Page
    {
        private double realRating;
        private int numOfRaters;

        protected void Page_Load(object sender, EventArgs e)
        {

            SqlDataReader recipe = getFromSQL("Name", "Recipes", "Burger");
            string name = "";
            string description = "";
            double preperationTime = 0;
            string cuisine = "";
            int rating = 3;
            if (recipe != null)
            {
                recipe.Read();
                name = recipe.GetString(0);
                description = recipe.GetString(1);
                preperationTime = Convert.ToDouble(recipe.GetString(2));
                cuisine = recipe.GetString(3);
                rating = Convert.ToInt32(recipe.GetString(4));
                realRating = Convert.ToDouble(recipe.GetString(4));
                numOfRaters = Convert.ToInt32(recipe.GetString(5));
            }
            else
            {
                name = "Burger";
                description = "cool burger with double cheess";
                preperationTime = 10;
                cuisine = "Amrican";
                realRating = 3.0;
                numOfRaters = 0;
            }
            Label1.Text = name;
            Image1.ImageUrl = getImage("Garlic Bolognese");
            Rating1.CurrentRating = rating;
            SqlDataReader ingredients = getFromSQL("RecipeName", "RecipesIngredients", "Burger");
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Ingredients: ");
            if (ingredients != null)
            {
                while (ingredients.Read())
                {
                    sb.AppendLine("- " + ingredients.GetString(2) + " " + ingredients.GetString(0) + "\n");
                }
            }
            Label2.Text = "Cuisine: " + cuisine;
            Label3.Text = "Preperation Time: " + preperationTime + " Minutes.";
            Label4.Text = sb.ToString().Replace(Environment.NewLine, "<br />");
            Label5.Text = "Description: " + description;
        }

        private string getImage(string name)
        {

            CloudBlobContainer container = GetContainer();

            // Retrieve reference to a blob named "photo1.jpg".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(name);
            return blockBlob.Uri.AbsoluteUri;
        }

        protected SqlDataReader getFromSQL(string rowName, string tableName, string keyName)
        {
            try
            {
                SqlConnectionStringBuilder builder =
                    new SqlConnectionStringBuilder
                    {
                        DataSource = "thoughts4food.database.windows.net",
                        UserID = "thoughts4food",
                        Password = "Kfc369nba",
                        InitialCatalog = "thoughts4foodSQL"
                    };

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append($"SELECT * FROM  '{tableName}'  WHERE '{rowName}'  = '{keyName}';");
                    String sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            return reader;
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        protected void OnRatingChanged(object sender, RatingEventArgs e)
        {
            double helper = realRating * numOfRaters;
            double oldRating = realRating;
            helper = helper + Convert.ToInt32(e.Value);
            numOfRaters++;
            realRating = helper / numOfRaters;
            Rating1.CurrentRating = Convert.ToInt32(realRating);
            try
            {
                SqlConnectionStringBuilder builder =
                    new SqlConnectionStringBuilder
                    {
                        DataSource = "thoughts4food.database.windows.net",
                        UserID = "thoughts4food",
                        Password = "Kfc369nba",
                        InitialCatalog = "thoughts4foodSQL"
                    };

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append($"UPDATE Recipes SET Rating = '{realRating}' NumOfRaters ='{numOfRaters}' WHERE Name  = '{Label1.Text}';");
                    String sql = sb.ToString();
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.ExecuteReader();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
                realRating = oldRating;
                numOfRaters--;
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


    }
}