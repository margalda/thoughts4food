using System;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using Quartz;

namespace NightWorker
{
    public class DailyJob : IJob
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

        public Task Execute(IJobExecutionContext context)
        {
            CloudTable recipes = GetTable("recipes");
            string[] info = GetUserInfo("admin");
            const double minRate = 2;
            int minNumOfReters = Convert.ToInt32(info[5]);

            foreach (var cuisine in Enum.GetValues(typeof(Cuisine)))
            {
                // Create the batch operation.
                TableBatchOperation batchOperation = new TableBatchOperation();
                EnsureContainerExists(cuisine.ToString());
                CloudBlobContainer container = GetContainer(cuisine.ToString());

                TableQuery<DynamicTableEntity> query = new TableQuery<DynamicTableEntity>().Where(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, cuisine.ToString()));
                foreach (DynamicTableEntity entity in recipes.ExecuteQuery(query))
                {
                    if (entity.Properties["rating"].DoubleValue < minRate &
                        entity.Properties["numOfRaters"].Int32Value > minNumOfReters)
                    {
                        batchOperation.Delete(entity);
                        CloudBlockBlob blockBlob = container.GetBlockBlobReference(entity.RowKey);
                        blockBlob.Delete();
                    }
                }

                if (batchOperation.Count > 0)
                    recipes.ExecuteBatch(batchOperation);
            }

            return null;
        }

        private void EnsureContainerExists(string name)
        {
            var container = GetContainer(name);
            container.CreateIfNotExists();
            var permissions = container.GetPermissions();
            permissions.PublicAccess = BlobContainerPublicAccessType.Container;
            container.SetPermissions(permissions);
        }


        public static CloudBlobContainer GetContainer(string name)
        {
            // Get a handle on account, create a blob service client and get container proxy
            var account = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            var client = account.CreateCloudBlobClient();

            return client.GetContainerReference(RoleEnvironment.GetConfigurationSettingValue("ContainerName") +
                                                $"-{name.ToLower()}");
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

        private string[] GetUserInfo(string username)
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
                            string[] ans = new string[6];
                            for (int i = 0; i < 6; i++)
                            {
                                if (!reader.IsDBNull(i))
                                {
                                    if (i == 3 | i == 5)
                                        ans[i] = reader.GetInt32(i).ToString();
                                    else ans[i] = reader.GetString(i);
                                }
                                else if (i == 4)
                                {
                                    ans[i] = "";
                                }
                                else if (i == 5)
                                {
                                    ans[i] = "1023";
                                }
                                else
                                    ans[i] = "0";
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
    }
}