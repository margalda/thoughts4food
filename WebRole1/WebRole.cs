using Microsoft.Azure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace WebRole1
{
    public class WebRole : RoleEntryPoint
    {
        public override bool OnStart()
        {
            // For information on handling configuration changes
            // see the MSDN topic at https://go.microsoft.com/fwlink/?LinkId=166357.

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            // Create the table if it doesn't exist.
            CloudTable table = tableClient.GetTableReference("recipes");
            table.CreateIfNotExists();
            table = tableClient.GetTableReference("ingredients");
            table.CreateIfNotExists();

            return base.OnStart();
        }
    }
}