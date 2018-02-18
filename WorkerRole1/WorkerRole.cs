using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace WorkerRole1
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent RunCompleteEvent = new ManualResetEvent(false);

        public override void Run()
        {
            Trace.TraceInformation("WorkerRole1 is running");

            try
            {
                RunAsync(CancellationTokenSource.Token).Wait();
            }
            finally
            {
                RunCompleteEvent.Set();
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

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("WorkerRole1 is stopping");

            CancellationTokenSource.Cancel();
            RunCompleteEvent.WaitOne();

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
                        Image.GetThumbnailImageAbort myCallback = ThumbnailCallback;
                        //get photo as stream
                        MemoryStream memStream = new MemoryStream();
                        var blob = GetContainer("photo").GetBlockBlobReference(msg.AsString);

                        blob.DownloadToStream(memStream);

                        //create a bitmap from photo
                        Bitmap myBitmap = new Bitmap(memStream);
                        //create thumbnail
                        Image myThumbnail = myBitmap.GetThumbnailImage(40, 40, myCallback, IntPtr.Zero);

                        var ms = new MemoryStream();
                        myThumbnail.Save(ms, ImageFormat.Jpeg);

                        // If you're going to read from the stream, you may need to reset the position to the start
                        ms.Position = 0;

                        SaveThumb(msg.AsString, ms);

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

        private CloudBlobContainer GetContainer(string type)
        {
            // Get a handle on account, create a blob service client and get container proxy
            var account = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            var client = account.CreateCloudBlobClient();

            var container = type == "photo" ? client.GetContainerReference(RoleEnvironment.GetConfigurationSettingValue("ContainerName") + "-photo")
                : client.GetContainerReference(RoleEnvironment.GetConfigurationSettingValue("ContainerName") + "-thumb");
            container.CreateIfNotExists();
            return container;
        }

        private void SaveThumb(string name, Stream fileStream)
        {
            // Create a blob in container and upload image bytes to it
            var blob = GetContainer("thumb").GetBlockBlobReference(name);

            blob.UploadFromStream(fileStream);
        }

        public bool ThumbnailCallback()
        {
            return false;
        }
    }
}