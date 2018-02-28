using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace CalcWorker
{
    public class WorkerRole : RoleEntryPoint
    {
        public enum Cuisine
        {
            Italian,
            Chinese,
            American,
            Israeli,
            French,
            Mexican,
            Thai,
            Indian,
            Japanese,
            Vietnamese,
        }

        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent _runCompleteEvent = new ManualResetEvent(false);

        public override void Run()
        {
            Trace.TraceInformation("WorkerRole1 is running");

            try
            {
                RunAsync(_cancellationTokenSource.Token).Wait();
            }
            finally
            {
                _runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at https://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("WorkerRole1 has been started");

            EnsureTableExists("usersRecipes");
            EnsureTableExists("recipes");
            EnsureTableExists("usersIngredients");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("WorkerRole1 is stopping");

            _cancellationTokenSource.Cancel();
            _runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("WorkerRole1 has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // initialize the account information
            var storageAccount =
                CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            // retrieve a reference to the messages queue
            var queueClient = storageAccount.CreateCloudQueueClient();
            var queue = queueClient.GetQueueReference("messagequeue");

            while (!cancellationToken.IsCancellationRequested)
            {
                if (queue.Exists())
                {
                    queue.FetchAttributes();
                    Trace.TraceInformation("queue size is {0}.", queue.ApproximateMessageCount);

                    var msg = queue.GetMessage();
                    if (msg != null)
                    {
                        string username = msg.AsString;
                        List<string> recipes = CalcRecipes(username);
                        // object to place into table
                        string partitionKey = username[0].ToString();
                        string rowKey = username;
                        var properties = new Dictionary<string, EntityProperty>();
                        for (int i = 0; i < recipes.Count; i++)
                        {
                            string id = Guid.NewGuid().ToString().Replace("-", "");
                            properties.Add($"recipe{id}", new EntityProperty(recipes[i]));
                        }

                        CloudTable table = GetTable("usersRecipes");
                        //create the entity
                        var usersRecipes = new DynamicTableEntity(partitionKey, rowKey, "*", properties);
                        // Build insert operation.
                        TableOperation insertOperation = TableOperation.InsertOrReplace(usersRecipes);
                        // Execute the insert operation.
                        table.Execute(insertOperation);

                        queue.DeleteMessage(msg);

                        Trace.TraceInformation($"Message '{msg.AsString}' processed.");
                    }
                }
                else
                {
                    await Task.Delay(1000, cancellationToken);
                }
            }
        }

        private List<string> CalcRecipes(string username)
        {
            List<string> res = new List<string>();
            Tuple<string, int> preferences = GetUserPreferences(username);
            Dictionary<string, double> ingredients = GetUserIngredients(username);

            int i = 0;
            CloudTable table = GetTable("recipes");
            foreach (var cuisine in Enum.GetValues(typeof(Cuisine)))
            {
                if (((preferences.Item2 >> i) & 1) == 1)
                {
                    TableQuery<DynamicTableEntity> query = new TableQuery<DynamicTableEntity>()
                        .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal,
                            cuisine.ToString()));
                    foreach (DynamicTableEntity entity in table.ExecuteQuery(query))
                    {
                        if (preferences.Item1 == "" || double.Parse(preferences.Item1) >=
                            entity.Properties["preperationTime"].DoubleValue)
                        {
                            if (CompareIngredients(ingredients, entity))
                            {
                                res.Add($"{entity.PartitionKey}_{entity.RowKey}");
                            }
                        }
                    }
                }

                i++;
            }

            return res;
        }

        private bool CompareIngredients(Dictionary<string, double> ingredients, DynamicTableEntity entity)
        {
            foreach (var recipeIngredient in entity.Properties.Where(kvp => kvp.Key.StartsWith("ingredient")).ToList())
            {
                var recipeIngredientParts = recipeIngredient.Value.StringValue.Split('_');
                var key = $"{recipeIngredientParts[0]}_{recipeIngredientParts[1]}";
                if (!ingredients.ContainsKey(key) ||
                    ingredients[key] < double.Parse(recipeIngredientParts[2]))
                {
                    return false;
                }
            }

            return true;
        }

        private Dictionary<string, double> GetUserIngredients(string username)
        {
            Dictionary<string, double> res = new Dictionary<string, double>();
            CloudTable table = GetTable("usersIngredients");
            TableOperation retrieveOperation =
                TableOperation.Retrieve<DynamicTableEntity>(username[0].ToString(), username);
            // Execute the retrieve operation.
            TableResult retrievedResult = table.Execute(retrieveOperation);
            // Print the phone number of the result.
            if (retrievedResult.Result != null)
            {
                foreach (var ingredient in ((DynamicTableEntity) retrievedResult.Result).Properties.Values)
                {
                    var ingredientsParts = ingredient.StringValue.Split('_');
                    res.Add($"{ingredientsParts[0]}_{ingredientsParts[1]}", double.Parse(ingredientsParts[2]));
                }
            }

            return res;
        }

        private Tuple<string, int> GetUserPreferences(string username)
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
                    sb.Append($"SELECT * FROM Users WHERE Username = '{username}';");
                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            reader.Read();
                            return new Tuple<string, int>(reader.GetString(4), reader.GetInt32(5));
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

        private void EnsureTableExists(string name)
        {
            CloudStorageAccount storageAccount =
                CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            // Create the table if it doesn't exist.
            CloudTable table = tableClient.GetTableReference(name);
            table.CreateIfNotExists();
        }

        private CloudTable GetTable(string name)
        {
            CloudStorageAccount storageAccount =
                CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            // Create the table if it doesn't exist.
            return tableClient.GetTableReference(name);
        }

        //private void SaveThumb(string name, Stream fileStream)
        //{
        //    Image.GetThumbnailImageAbort myCallback = ThumbnailCallback;
        //    //get photo as stream
        //    MemoryStream memStream = new MemoryStream();
        //    var blob = GetContainer("photo").GetBlockBlobReference(name);

        //    blob.DownloadToStream(memStream);

        //    //create a bitmap from photo
        //    Bitmap myBitmap = new Bitmap(memStream);
        //    //create thumbnail
        //    Image myThumbnail = myBitmap.GetThumbnailImage(40, 40, myCallback, IntPtr.Zero);

        //    var ms = new MemoryStream();
        //    myThumbnail.Save(ms, ImageFormat.Jpeg);

        //    // If you're going to read from the stream, you may need to reset the position to the start
        //    ms.Position = 0;

        //    // Create a blob in container and upload image bytes to it
        //    blob = GetContainer("thumb").GetBlockBlobReference(name);

        //    blob.UploadFromStream(fileStream);
        //}

        //public bool ThumbnailCallback()
        //{
        //    return false;
        //}

        //private CloudBlobContainer GetContainer(string type)
        //{
        //    // Get a handle on account, create a blob service client and get container proxy
        //    var account = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
        //    var client = account.CreateCloudBlobClient();

        //    var container = type == "photo" ? client.GetContainerReference(RoleEnvironment.GetConfigurationSettingValue("ContainerName") + "-photo")
        //        : client.GetContainerReference(RoleEnvironment.GetConfigurationSettingValue("ContainerName") + "-thumb");
        //    container.CreateIfNotExists();
        //    return container;
        //}
    }
}