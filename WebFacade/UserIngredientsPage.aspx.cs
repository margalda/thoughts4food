using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Label = System.Web.UI.WebControls.Label;
using TextBox = System.Web.UI.WebControls.TextBox;

namespace WebFacade
{
    public partial class UserIngredientsPage : Page
    {
        private string _currentUser;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                _currentUser = Request.QueryString["username"];
                if (!IsPostBack)
                {
                    //_currentUser = Request.QueryString["username"];

                    SetInitialRow();
                }

                CloudStorageAccount storageAccount =
                    CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
                // Create the table client.
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                // Create the table if it doesn't exist.
                CloudTable table = tableClient.GetTableReference("usersIngredients");
                table.CreateIfNotExists();

                RefreshIngredients();
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

        protected void CategoryList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropDown = (DropDownList) sender;
            GridViewRow gvRow = (GridViewRow) dropDown.NamingContainer;
            int rowID = gvRow.RowIndex;

            Label unit = (Label) Gridview1.Rows[rowID].Cells[4].FindControl("txtUnit");
            unit.Text = dropDown.Text == "SaucesAndSpices" || dropDown.Text == "Baking" ? "(teaspoons)" : "(grams)";
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            LinkButton lb = (LinkButton) sender;
            GridViewRow gvRow = (GridViewRow) lb.NamingContainer;
            int rowID = gvRow.RowIndex + 1;
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dt = (DataTable) ViewState["CurrentTable"];
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
            DataTable dt = (DataTable) ViewState["CurrentTable"];
            if (dt?.Rows.Count > 0)
            {
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    TextBox box1 = (TextBox) Gridview1.Rows[rowIndex].Cells[1].FindControl("txtName");
                    DropDownList dropDown =
                        (DropDownList) Gridview1.Rows[rowIndex].Cells[2].FindControl("categoryList");
                    TextBox box2 = (TextBox) Gridview1.Rows[rowIndex].Cells[3].FindControl("txtQuantity");
                    Label label = (Label) Gridview1.Rows[rowIndex].Cells[4].FindControl("txtUnit");

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
                DataTable dtCurrentTable = (DataTable) ViewState["CurrentTable"];
                DataRow drCurrentRow = null;
                if (dtCurrentTable.Rows.Count > 0)
                {
                    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                    {
                        //extract the TextBox values
                        TextBox box1 = (TextBox) Gridview1.Rows[rowIndex].Cells[1].FindControl("txtName");
                        DropDownList dropDown =
                            (DropDownList) Gridview1.Rows[rowIndex].Cells[2].FindControl("categoryList");
                        TextBox box2 = (TextBox) Gridview1.Rows[rowIndex].Cells[3].FindControl("txtQuantity");
                        Label label = (Label) Gridview1.Rows[rowIndex].Cells[4].FindControl("txtUnit");

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

        // Hide the Remove Button at the last row of the GridView
        protected void Gridview1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataTable dt = (DataTable) ViewState["CurrentTable"];
                LinkButton lb = (LinkButton) e.Row.FindControl("LinkButton1");
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

        private void RefreshIngredients()
        {
            GridView2.DataSource = GetCurrentUserIngredients();
            GridView2.DataBind();
        }

        private List<UserIngredient> GetCurrentUserIngredients()
        {
            List<UserIngredient> ingredients =
                new List<UserIngredient>();

            CloudTable table = CloudHelpers.GetTable("usersIngredients");
            TableOperation retrieveOperation =
                TableOperation.Retrieve<DynamicTableEntity>(_currentUser[0].ToString(), _currentUser);
            TableResult retrievedResult = table.Execute(retrieveOperation);

            if (retrievedResult.Result != null)
            {
                foreach (var item in ((DynamicTableEntity) retrievedResult.Result).Properties.Values)
                {
                    string[] ingredientDetails = item.StringValue.Split('_');
                    ingredients.Add(new UserIngredient
                    {
                        Name = ingredientDetails[1],
                        Category = ingredientDetails[0],
                        Quantity = ingredientDetails[2],
                        Units = ingredientDetails[0] == "SaucesAndSpices" || ingredientDetails[0] == "Baking"
                            ? "(teaspoons)"
                            : "(grams)",
                    });
                }
            }

            return ingredients;
        }

        protected void Upload_Click(object sender, EventArgs e)
        {
            var newIngredients = new List<string>();
            foreach (var ingredient in GetIngredientsFromDataGrid())
            {
                newIngredients.Add($"{ingredient.Category}_{ingredient.Name}_{ingredient.Quantity}");
            }

            SaveIngredients(
                newIngredients
            );
        }

        private void SaveIngredients(IReadOnlyList<string> ingredients)
        {
            CloudTable table = CloudHelpers.GetTable("usersIngredients");
            TableOperation retrieveOperation =
                TableOperation.Retrieve<DynamicTableEntity>(_currentUser[0].ToString(), _currentUser);
            TableResult retrievedResult = table.Execute(retrieveOperation);

            // Assign the result to a CustomerEntity object.
            DynamicTableEntity updateEntity = (DynamicTableEntity) retrievedResult.Result;

            if (updateEntity != null)
            {
                for (int i = 0; i < ingredients.Count; i++)
                {
                    string id = Guid.NewGuid().ToString().Replace("-", "");
                    updateEntity.Properties.Add($"ingredient{id}", new EntityProperty(ingredients[i]));
                }

                // Create the Replace TableOperation.
                TableOperation updateOperation = TableOperation.Replace(updateEntity);

                // Execute the operation.
                table.Execute(updateOperation);
            }
            else
            {
                // object to place into table
                string partitionKey = _currentUser[0].ToString();
                string rowKey = _currentUser;
                var properties = new Dictionary<string, EntityProperty>();
                for (int i = 0; i < ingredients.Count; i++)
                {
                    string id = Guid.NewGuid().ToString().Replace("-", "");
                    properties.Add($"ingredient{id}", new EntityProperty(ingredients[i]));
                }

                //create the entity
                var userIngredients = new DynamicTableEntity(partitionKey, rowKey, "*", properties);
                // Build insert operation.
                TableOperation insertOperation = TableOperation.Insert(userIngredients);
                // Execute the insert operation.
                table.Execute(insertOperation);
            }

            //send to queue
            CloudHelpers.SendToQueue(_currentUser);

            ClientScript.RegisterStartupScript(GetType(), "alert", "alert('Ingredients added successfully!');", true);
            RefreshIngredients();
        }

        private List<UserIngredient> GetIngredientsFromDataGrid()
        {
            var res = new List<UserIngredient>();

            for (int i = 0; i < Gridview1.Rows.Count; i++)
            {
                TextBox box1 = (TextBox) Gridview1.Rows[i].Cells[1].FindControl("txtName");
                DropDownList dropDown = (DropDownList) Gridview1.Rows[i].Cells[2].FindControl("categoryList");
                TextBox box2 = (TextBox) Gridview1.Rows[i].Cells[3].FindControl("txtQuantity");

                res.Add(new UserIngredient
                {
                    Name = box1.Text,
                    Category = dropDown.Text,
                    Quantity = box2.Text,
                    Units = dropDown.Text == "SaucesAndSpices" || dropDown.Text == "Baking" ? "(teaspoons)" : "(grams)",
                });
            }

            return res;
        }

        private void RemoveIngredient(string ingredient)
        {
            CloudTable table = CloudHelpers.GetTable("usersIngredients");
            TableOperation retrieveOperation =
                TableOperation.Retrieve<DynamicTableEntity>(_currentUser[0].ToString(), _currentUser);
            TableResult retrievedResult = table.Execute(retrieveOperation);

            // Assign the result to a CustomerEntity object.
            DynamicTableEntity updateEntity = (DynamicTableEntity) retrievedResult.Result;
            foreach (var item in updateEntity.Properties.Where(kvp => kvp.Value.StringValue == ingredient).ToList())
            {
                updateEntity.Properties.Remove(item.Key);
            }

            // Create the Replace TableOperation.
            TableOperation updateOperation = TableOperation.Replace(updateEntity);

            // Execute the operation.
            table.Execute(updateOperation);

            //send to queue
            CloudHelpers.SendToQueue(_currentUser);

            ClientScript.RegisterStartupScript(GetType(), "alert", "alert('Ingredient deleted successfully!');", true);
            RefreshIngredients();
        }

        protected void GridView2_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var rowCells = GridView2.Rows[e.RowIndex].Cells;
            RemoveIngredient($"{rowCells[2].Text}_{rowCells[1].Text}_{rowCells[3].Text}");
        }
    }
}