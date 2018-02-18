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
            string[] recipe = getRecipeFromSQL("Burger");
            string name = "";
            string description = "";
            double preperationTime = 0;
            string cuisine = "";
            int rating = 3;
            if (recipe != null)
            {
                name = recipe[0];
                description = recipe[1];
                preperationTime = Convert.ToDouble(recipe[2]);
                cuisine = recipe[3];
                rating = Convert.ToInt32(recipe[4]);
                realRating = Convert.ToDouble(recipe[4]);
                numOfRaters = Convert.ToInt32(recipe[5]);
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
            Image1.ImageUrl = getImage(name);
            Rating1.CurrentRating = rating;
            string[,] ingredients = getIngredientsFromSQL(name);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Ingredients: ");
            if(ingredients != null)
            {
                int ingrLen = ingredients.Length / 3;
                for (int j = 0; j < ingrLen; j++)
                {
                    string unit = ingredients[j, 2] == "SaucesAndSpices" || ingredients[j, 2] == "Baking" ? "teaspoons" : "grams";
                    sb.AppendLine("- " + ingredients[j, 1] + " " + unit + " " + ingredients[j, 0] + "\n");
                }
            }
            Label2.Text = "Cuisine: " + cuisine;
            Label3.Text = "Preperation Time: " + preperationTime + " Miniuts.";
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

        protected string[] getRecipeFromSQL(string keyName)
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
                    sb.Append($"SELECT * FROM  Recipes  WHERE Name  = '{keyName}';");
                    String sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            reader.Read();
                            string[] ans = new string[6];
                            for (int i = 0; i < 6; i++)
                            {
                                if (i == 0 | i == 1 | i == 3)
                                    ans[i] = reader.GetString(i);
                                else if (i != 5)
                                    ans[i] = reader.GetDecimal(i).ToString();
                                else ans[i] = reader.GetInt32(i).ToString();
                            }
                            return ans;
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

        protected string[,] getIngredientsFromSQL(string keyName)
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
                    sb.Append($"SELECT * FROM  RecipesIngredients JOIN Ingredients ON RecipesIngredients.IngredientName = Ingredients.Name WHERE RecipeName  = '{keyName}';");
                    StringBuilder sb1 = new StringBuilder();
                    sb1.Append($"SELECT COUNT(*) FROM  RecipesIngredients WHERE RecipeName  = '{keyName}';");
                    int readerLangth = 0;
                    String sql = sb1.ToString();
                    using (SqlCommand command1 = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader1 = command1.ExecuteReader())
                        {
                            reader1.Read();
                            readerLangth = reader1.GetInt32(0);
                        }
                    }
                    sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            reader.Read();
                            string[,] ans = new string[readerLangth, 3];
                            for (int j = 0; j < readerLangth; j++)
                            {
                                ans[j, 0] = reader.GetString(0);
                                ans[j, 1] = reader.GetDecimal(2).ToString();
                                ans[j, 2] = reader.GetString(4);
                                reader.Read();
                            }
                            return ans;
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
