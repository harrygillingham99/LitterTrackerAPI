using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Google.Cloud.Storage.V1;
using litter_tracker.Objects.ApiObjects;
using Object = Google.Apis.Storage.v1.Data.Object;

namespace litter_tracker.Services.GoogleCloudStorage
{
    public class GoogleCloudStorage : IGoogleCloudStorage
    {
        public async Task<string> UploadFile(UploadImageRequest request)
        {
            MemoryStream imageStream = new MemoryStream(Convert.FromBase64String(request.Base64Image));

            var storage = await StorageClient.CreateAsync();

            var fileName = $"{request.MarkerDatastoreId}-{Guid.NewGuid()}-upload";

            var fileDestination = new Object
            {
                Bucket = "litter-tracker.appspot.com",
                ContentType = "image/jpg" ,
                Name = fileName,
                Metadata = new Dictionary<string, string>()
                {
                    {"LitterPinId", request.MarkerDatastoreId.ToString()}, 
                    {"UploadedBy", request.UploadedByUid},
                },
            };

            await storage.UploadObjectAsync(fileDestination, imageStream);

            return fileName;
        }
    }
}
