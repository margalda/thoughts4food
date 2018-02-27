using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AjaxControlToolkit;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;

namespace WebFacade
{
    public partial class RecipePage : System.Web.UI.Page
    {
        private string _recipeName;
        private string _recipeCuisine;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _recipeName = Request.QueryString["name"];
                _recipeCuisine = Request.QueryString["cuisine"].Split('-')[1].First().ToString().ToUpper()
                                 + Request.QueryString["cuisine"].Split('-')[1].Substring(1);

                List<string> recipe = GetRecipe(_recipeName, _recipeCuisine);

                if (recipe != null)
                {
                    var description = recipe[0];
                    var preperationTime = Convert.ToDouble(recipe[1]);
                    var rating = Convert.ToDouble(recipe[3]);
                    txtRating.Text = recipe[3];
                    txtNumOfRaters.Text = recipe[4];

                    Label1.Text = _recipeName;
                    Image1.ImageUrl = GetImage(_recipeName, _recipeCuisine);
                    Rating1.CurrentRating = Convert.ToInt32(rating);
                    List<string[]> ingredients = new List<string[]>();
                    for (int j = 5; j < recipe.Count; j++)
                    {
                        string[] ingredient = recipe[j].Split('_');
                        ingredients.Add(ingredient);
                    }

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Ingredients: ");
                    int ingrLen = recipe.Count - 5;
                    for (int j = 0; j < ingrLen; j++)
                    {
                        string unit = ingredients[j][0] == "SaucesAndSpices" || ingredients[j][0] == "Baking"
                            ? "teaspoons"
                            : "grams";
                        sb.AppendLine("- " + ingredients[j][1] + " " + ingredients[j][2] + " " + unit + "\n");
                    }

                    Label2.Text = "Cuisine: " + _recipeCuisine;
                    Label3.Text = "Preperation Time: " + preperationTime + " minutes.";
                    Label4.Text = sb.ToString().Replace(Environment.NewLine, "<br />");
                    Label5.Text = "Description: " + description;
                }
            }
        }

        private string GetImage(string name, string containerName)
        {
            CloudBlobContainer container = CloudHelpers.GetContainer(containerName);

            // Retrieve reference to a blob named "photo1.jpg".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(name);
            return blockBlob.Uri.AbsoluteUri;
        }

        protected List<string> GetRecipe(string keyName, string partitionKey)
        {
            CloudTable recipes = CloudHelpers.GetTable("recipes");
            TableOperation retrieveOperation = TableOperation.Retrieve<DynamicTableEntity>(partitionKey, keyName);
            // Execute the retrieve operation.
            TableResult retrievedResult = recipes.Execute(retrieveOperation);
            List<string> recipe = new List<string>();
            foreach (var recipeProperty in ((DynamicTableEntity) retrievedResult.Result).Properties)
            {
                recipe.Add("");
            }

            int i = 5;
            foreach (var recipeProperty in ((DynamicTableEntity) retrievedResult.Result).Properties)
            {
                if (recipeProperty.Value.PropertyType == EdmType.Int32)
                {
                    if (recipeProperty.Key == "numOfRaters")
                        recipe[4] = recipeProperty.Value.Int32Value.ToString();
                }
                else if (recipeProperty.Value.PropertyType == EdmType.Double)
                {
                    if (recipeProperty.Key == "rating")
                        recipe[3] = recipeProperty.Value.DoubleValue.ToString();
                    else if (recipeProperty.Key == "preperationTime")
                        recipe[1] = recipeProperty.Value.DoubleValue.ToString();
                }

                else
                {
                    if (recipeProperty.Key == "cuisine")
                        recipe[2] = recipeProperty.Value.StringValue;
                    else if (recipeProperty.Key == "description")
                        recipe[0] = recipeProperty.Value.StringValue;
                    else
                    {
                        recipe[i] = recipeProperty.Value.StringValue;
                        i++;
                    }
                }
            }

            return recipe;
        }

        protected void OnRatingChanged(object sender, RatingEventArgs e)
        {
            double helper = Convert.ToDouble(txtRating.Text) * Convert.ToInt32(txtNumOfRaters.Text);
            //double oldRating = Convert.ToDouble(txtRating.Text);
            helper = helper + Convert.ToInt32(e.Value);
            int numOfRaters = Convert.ToInt32(txtNumOfRaters.Text) + 1;
            txtNumOfRaters.Text = numOfRaters.ToString();
            double realRating = helper / numOfRaters;
            txtRating.Text = realRating.ToString();
            UpdateRatingInRecipe(realRating, numOfRaters);
            Rating1.CurrentRating = Convert.ToInt32(realRating);
        }

        private void UpdateRatingInRecipe(double rating, int numOfRaters)
        {
            CloudTable table = CloudHelpers.GetTable("recipes");
            TableOperation retrieveOperation =
                TableOperation.Retrieve<DynamicTableEntity>(Label2.Text.Substring(9, (Label2.Text.Length - 9)),
                    Label1.Text);
            TableResult retrievedResult = table.Execute(retrieveOperation);

            // Assign the result to a CustomerEntity object.
            DynamicTableEntity updateEntity = (DynamicTableEntity) retrievedResult.Result;
            updateEntity.Properties["rating"].DoubleValue = rating;
            updateEntity.Properties["numOfRaters"].Int32Value = numOfRaters;
            // Create the Replace TableOperation.
            TableOperation updateOperation = TableOperation.Replace(updateEntity);

            // Execute the operation.
            table.Execute(updateOperation);
        }
    }
}