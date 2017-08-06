using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foodbook.MobileApp.Tools
{
    public static class UploadFileToAzureBlob
    {
        /// <summary>
        /// Basic operations to work with block blobs
        /// </summary>
        /// <returns>Task<returns>
        public static async Task<string> BasicStorageBlockBlobOperationsAsync(Stream fileStream, string fileName)
        {
            string photoUrl = null;
            string containerName = "photos";
            try
            {

                if (fileStream != null)
                {
                    // Retrieve storage account information
                    CloudStorageAccount storageAccount = CreateStorageAccount();

                    // Create a blob client for interacting with the blob service.
                    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                    // Create a container for organizing blobs within the storage account.
                    CloudBlobContainer container = blobClient.GetContainerReference(containerName);
                   

                    string blobName = string.Format("{0}", fileName);

                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
                    //await blockBlob.UploadFromByteArrayAsync(photo, 0, photo.Length);
                    await blockBlob.UploadFromStreamAsync(fileStream, fileStream.Length);

                    photoUrl = blockBlob.Uri.ToString();

                    fileStream.Dispose();
                }

            }
            catch (Exception ex)
            {
                //System.Diagnostics.Trace.TraceError(ex.Message);
                photoUrl = null;
            }

            return photoUrl;
        }

        public static void DeleteBlob(List<string> files)
        {
            try
            {
                // Retrieve storage account from connection string.
                CloudStorageAccount storageAccount = CreateStorageAccount();

                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve reference to a previously created container.
                CloudBlobContainer container = blobClient.GetContainerReference("photos");

                // Retrieve reference to a blob named "myblob.txt".
                foreach (var item in files)
                {
                    var array = item.Split('/');
                    string blobName = item;
                    if (array.Any())
                    {
                        blobName = item.Split('/').Reverse().FirstOrDefault();
                    }
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
                    blockBlob.DeleteAsync();
                }

            }
            catch (Exception)
            {
            }

        }

        /// <summary>
        /// Validates the connection string information in app.config and throws an exception if it looks like 
        /// the user hasn't updated this to valid values. 
        /// </summary>
        /// <param name="storageConnectionString">The storage connection string</param>
        /// <returns>CloudStorageAccount object</returns>
        private static CloudStorageAccount CreateStorageAccount()
        {
            CloudStorageAccount storageAccount;
            try
            {
                string AZURE_STORAGE_CONNECTION_STRING = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", AzureStorageSettings.ACCOUNT_NAME, AzureStorageSettings.KEY_VALUE);
                StorageCredentials credentials = new StorageCredentials(AzureStorageSettings.ACCOUNT_NAME, AzureStorageSettings.KEY_VALUE);
                //storageAccount = CloudStorageAccount.Parse(AZURE_STORAGE_CONNECTION_STRING);
                storageAccount = new CloudStorageAccount(credentials, false);
            }
            catch (FormatException ex)
            {
                int i = 2;
                //Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the sample.");
                //Console.ReadLine();
                throw;
            }
            catch (ArgumentException exc)
            {
                int i = 2;
                //Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the sample.");
                //Console.ReadLine();
                throw;
            }

            return storageAccount;
        }
    }
}
