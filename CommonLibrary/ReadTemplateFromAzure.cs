using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public class ReadTemplateFromAzure
    {


        public string getHtmlContentFromAzure(string accountname, string accesskey, string containerName, string fileName, int MI_Id)
        {

            StorageCredentials cre = new StorageCredentials(accountname, accesskey);
            CloudStorageAccount acc = new CloudStorageAccount(cre, useHttps: true);
            CloudBlobClient blobClient = acc.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);
            CloudBlob blockBlob = container.GetBlobReference(fileName);
            string html = "";
            using (var stream = blockBlob.OpenReadAsync().Result)
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        var n = reader.ReadLine();
                        html += n;
                    }
                }
            }
            return html;
        }
    }
}
