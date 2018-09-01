using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobManagement
{
    public class BlobManager
    {
        CloudBlobClient client;

        public async Task InitializeAsync()
        {
            await Task.Factory.StartNew(() =>
            {
                StorageCredentials storageCredentials = new StorageCredentials(ConfigurationManager.AppSettings["SASToken"]);
                var storageEndpointSuffix = ConfigurationManager.AppSettings["StorageEndpointSuffix"];
                var cdnEndpointSuffix = ConfigurationManager.AppSettings["CdnEndpointSuffix"];
                var endpointName = ConfigurationManager.AppSettings["EndPointName"];
                var blobUri = new Uri($"https://{endpointName}.{cdnEndpointSuffix}");
                var account = new CloudStorageAccount(storageCredentials, blobUri, null, null, null);
                client = account.CreateCloudBlobClient();
            });
        }

        public async Task UploadToBlobAsync(string containerName, string filePath)
        {
            var blobName = Path.GetFileName(filePath);
            var container = client.GetContainerReference(containerName.ToLower());

            //If Exists Throws Exception, Hence Check Is Required
            if (!container.Exists())
            {
                container.CreateIfNotExists();
            }
            var blob = container.GetBlockBlobReference(blobName);
            await blob.UploadFromFileAsync(filePath);
        }

        public async Task<Tuple<byte[],string>> DownloadBlobAsync(string containerName, string blobName)
        {
            using (var stream = new MemoryStream())
            {
                var container = client.GetContainerReference(containerName);
                var blob = container.GetBlockBlobReference(blobName);
                
                //Fetches All Metadatas, User & System
                blob.FetchAttributes();

                long fileByteLength = blob.Properties.Length;
                byte[] fileContent = new byte[fileByteLength];
                for (int i = 0; i < fileByteLength; i++)
                {
                    fileContent[i] = 0x20;
                }

                await blob.DownloadToByteArrayAsync(fileContent, 0);

                var lastDownloaded = string.Empty;
                if(!blob.Metadata.ContainsKey("lastdownloaded"))
                {
                    blob.Metadata.Add("lastdownloaded", DateTime.Now.ToString());
                }
                else
                {
                    lastDownloaded = blob.Metadata["lastdownloaded"];
                    blob.Metadata["lastdownloaded"] = DateTime.Now.ToString();
                }

                //Saves Metadata
                await blob.SetMetadataAsync();

                return new Tuple<byte[], string> (fileContent, lastDownloaded);
            }
        }

        public async Task<IEnumerable<Model>> GetBlobsAsync()
        {
            return await Task.Factory.StartNew(
                () => from container in client.ListContainers()
                    select new Model()
                    {
                        Container = container.Name,
                        Images = (from blob in container.ListBlobs()
                                select new ImageInfo()
                                {
                                    ContainerName = container.Name,
                                    Name = Path.GetFileName(blob.Uri.AbsolutePath),
                                }).ToArray()
                    });
        }

        public async Task DeleteContainerAsync(string name)
        {
            var container = client.ListContainers().FirstOrDefault(c => c.Name == name);
            if (container.Exists())
            {
                await container.DeleteIfExistsAsync();
            }
        }

        public async Task DeleteBlobAsync(string containerName, string blobName)
        {
            var container = client.GetContainerReference(containerName);
            if(container!=null)
            {
                var blob = container.GetBlockBlobReference(blobName);
                if (blob.Exists())
                {
                    await blob.DeleteIfExistsAsync();
                }
            }
        }
    }
}
