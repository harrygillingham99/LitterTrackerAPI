using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Storage.v1.Data;
using Google.Cloud.Storage.V1;
using litter_tracker.Objects.ApiObjects;
using Microsoft.AspNetCore.Http;
using Object = Google.Apis.Storage.v1.Data.Object;

namespace litter_tracker.Services.GoogleCloudStorage
{
    public class GoogleCloudStorage : IGoogleCloudStorage
    {
        public async Task UploadFile(string uploadedByUid, long markerDatastoreId, string base64Image)
        {
            byte[] imgBytes = Convert.FromBase64String(base64Image);

            MemoryStream imageStream = new MemoryStream(imgBytes);

            var storage = await StorageClient.CreateAsync();

            var fileDestination = new Object
            {
                Bucket = "litter-tracker.appspot.com",
                ContentType = "image/jpg" ,
                Name = $"{markerDatastoreId}-{Guid.NewGuid()}-upload",
                Metadata = new Dictionary<string, string>()
                {
                    {"LitterPinId", markerDatastoreId.ToString()}, 
                    {"UploadedBy", uploadedByUid},
                },

            };

            await storage.UploadObjectAsync(fileDestination, imageStream);
        }
    }
}
