﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Azure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;

namespace WebRole1
{
    public partial class AddRecipes : Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    List<ListItem> cuisines = new List<ListItem>();
                    foreach (var cuisine in Enum.GetValues(typeof(Cuisine)))
                    {
                        cuisines.Add(new ListItem(cuisine.ToString(), cuisine.ToString()));
                    }

                    cuisineList.DataTextField = "Text";
                    cuisineList.DataValueField = "Value";
                    cuisineList.DataSource = cuisines;
                    cuisineList.DataBind();

                    SetInitialRow();

                    EnsureContainerExists();
                }
            }
            catch (WebException we)
            {
                string status = "Network error: " + we.Message;
                if (we.Status == WebExceptionStatus.ConnectFailure)
                {
                    status += "<br />Please check if the blob service is running at " + ConfigurationManager.AppSettings["storageEndpoint"];
                }
                Console.WriteLine(status);
            }
            catch (StorageException se)
            {
                Console.WriteLine("Storage service error: " + se.Message);
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

        protected void Upload_Click(object sender, EventArgs e)
        {
            //check if recipe exists
            if (QueriesRunner.ValueExists("Recipes", "Name", recipeName.Text))
            {
                ClientScript.RegisterStartupScript(GetType(), "alert", "alert('Recipe already exists');", true);
            }
            else
            {
                //add recipe pic to blob
                SaveImage(
                    Guid.NewGuid().ToString(),
                    recipeName.Text,
                    imageFile.FileName,
                    imageFile.PostedFile.ContentType,
                    imageFile.PostedFile.InputStream
                );

                //add recipe to sql table
                const string initRating = "3";
                const string initNumOfRaters = "0";

                bool recipeInserted = QueriesRunner.InsertToTable(
                    "Recipes",
                    new List<string> { recipeName.Text, recipeDescription.Text, timeBox.Text, cuisineList.Text, initRating, initNumOfRaters }
                    );

                //add ingredients to sql table
                bool ingredientInserted = true;
                bool recipeingredientInserted = true;
                List<Tuple<string, string, string>> ingredients = GetAllIngredients();

                foreach (var ingredient in ingredients)
                {
                    //check if ingredient exists
                    if (!QueriesRunner.ValueExists("Ingredients", "Name", ingredient.Item1))
                    {
                        ingredientInserted = QueriesRunner.InsertToTable(
                            "Ingredients",
                            new List<string> { ingredient.Item1, ingredient.Item2 }
                            );
                    }
                    recipeingredientInserted = QueriesRunner.InsertToTable(
                        "RecipesIngredients",
                        new List<string> { ingredient.Item1, recipeName.Text, ingredient.Item3 }
                    );
                }

                if (recipeInserted && ingredientInserted && recipeingredientInserted)
                {
                    ClientScript.RegisterStartupScript(GetType(), "alert", "alert('Recipe successfully added');", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(GetType(), "alert", "alert('Failed adding recipe');", true);
                }
            }
        }

        private List<Tuple<string, string, string>> GetAllIngredients()
        {
            var res = new List<Tuple<string, string, string>>();

            for (int i = 0; i < Gridview1.Rows.Count; i++)
            {
                TextBox box1 = (TextBox)Gridview1.Rows[i].Cells[1].FindControl("txtName");
                DropDownList dropDown = (DropDownList)Gridview1.Rows[i].Cells[2].FindControl("categoryList");
                TextBox box2 = (TextBox)Gridview1.Rows[i].Cells[3].FindControl("txtQuantity");

                res.Add(new Tuple<string, string, string>(box1.Text, dropDown.Text, box2.Text));
            }

            return res;
        }

        private CloudBlobContainer GetContainer()
        {
            // Get a handle on account, create a blob service client and get container proxy

            var account = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            var client = account.CreateCloudBlobClient();
            return client.GetContainerReference(RoleEnvironment.GetConfigurationSettingValue("ContainerName") + "-photo");
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

        protected void CategoryList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropDown = (DropDownList)sender;
            GridViewRow gvRow = (GridViewRow)dropDown.NamingContainer;
            int rowID = gvRow.RowIndex;

            Label unit = (Label)Gridview1.Rows[rowID].Cells[4].FindControl("txtUnit");
            unit.Text = dropDown.Text == "SaucesAndSpices" || dropDown.Text == "Baking" ? "(teaspoons)" : "(grams)";
        }

        private void SetInitialRow()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
            dt.Columns.Add(new DataColumn("Column1", typeof(string)));
            dt.Columns.Add(new DataColumn("Column2", typeof(string)));
            dt.Columns.Add(new DataColumn("Column3", typeof(string)));
            dt.Columns.Add(new DataColumn("Column4", typeof(string)));
            var dr = dt.NewRow();
            dr["RowNumber"] = 1;
            dr["Column1"] = string.Empty;
            dr["Column2"] = string.Empty;
            dr["Column3"] = string.Empty;
            dr["Column4"] = string.Empty;
            dt.Rows.Add(dr);

            //Store the DataTable in ViewState
            ViewState["CurrentTable"] = dt;

            Gridview1.DataSource = dt;
            Gridview1.DataBind();
        }

        // Hide the Remove Button at the last row of the GridView
        protected void Gridview1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataTable dt = (DataTable)ViewState["CurrentTable"];
                LinkButton lb = (LinkButton)e.Row.FindControl("LinkButton1");
                if (lb != null)
                {
                    if (dt.Rows.Count > 1)
                    {
                        if (e.Row.RowIndex == dt.Rows.Count - 1)
                        {
                            lb.Visible = false;
                        }
                    }
                    else
                    {
                        lb.Visible = false;
                    }
                }
            }
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            LinkButton lb = (LinkButton)sender;
            GridViewRow gvRow = (GridViewRow)lb.NamingContainer;
            int rowID = gvRow.RowIndex + 1;
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["CurrentTable"];
                if (dt.Rows.Count > 1)
                {
                    if (gvRow.RowIndex < dt.Rows.Count - 1)
                    {
                        //Remove the Selected Row data
                        dt.Rows.Remove(dt.Rows[rowID]);
                    }
                }
                //Store the current data in ViewState for future reference
                ViewState["CurrentTable"] = dt;
                //Re bind the GridView for the updated data
                Gridview1.DataSource = dt;
                Gridview1.DataBind();
            }

            //Set Previous Data on Postbacks
            SetPreviousData();
        }

        private void SetPreviousData()
        {
            int rowIndex = 0;
            DataTable dt = (DataTable)ViewState["CurrentTable"];
            if (dt?.Rows.Count > 0)
            {
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    TextBox box1 = (TextBox)Gridview1.Rows[rowIndex].Cells[1].FindControl("txtName");
                    DropDownList dropDown = (DropDownList)Gridview1.Rows[rowIndex].Cells[2].FindControl("categoryList");
                    TextBox box2 = (TextBox)Gridview1.Rows[rowIndex].Cells[3].FindControl("txtQuantity");
                    Label label = (Label)Gridview1.Rows[rowIndex].Cells[4].FindControl("txtUnit");

                    box1.Text = dt.Rows[i]["Column1"].ToString();
                    dropDown.SelectedValue = dt.Rows[i]["Column2"].ToString();
                    box2.Text = dt.Rows[i]["Column3"].ToString();
                    label.Text = dt.Rows[i]["Column4"].ToString();

                    rowIndex++;

                }
            }
        }

        protected void ButtonAdd_Click(object sender, EventArgs e)
        {
            int rowIndex = 0;

            if (ViewState["CurrentTable"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                DataRow drCurrentRow = null;
                if (dtCurrentTable.Rows.Count > 0)
                {
                    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                    {
                        //extract the TextBox values
                        TextBox box1 = (TextBox)Gridview1.Rows[rowIndex].Cells[1].FindControl("txtName");
                        DropDownList dropDown = (DropDownList)Gridview1.Rows[rowIndex].Cells[2].FindControl("categoryList");
                        TextBox box2 = (TextBox)Gridview1.Rows[rowIndex].Cells[3].FindControl("txtQuantity");
                        Label label = (Label)Gridview1.Rows[rowIndex].Cells[4].FindControl("txtUnit");

                        drCurrentRow = dtCurrentTable.NewRow();

                        drCurrentRow["RowNumber"] = i + 1;
                        drCurrentRow["Column1"] = box1.Text;
                        drCurrentRow["Column2"] = dropDown.SelectedValue;
                        drCurrentRow["Column3"] = box2.Text;
                        drCurrentRow["Column4"] = label.Text;

                        rowIndex++;
                    }

                    dtCurrentTable.Rows.Add(drCurrentRow);
                    ViewState["CurrentTable"] = dtCurrentTable;

                    Gridview1.DataSource = dtCurrentTable;
                    Gridview1.DataBind();
                }
            }
            else
            {
                Response.Write("ViewState is null");
            }

            //Set Previous Data on Postbacks
            SetPreviousData();
        }
    }
}