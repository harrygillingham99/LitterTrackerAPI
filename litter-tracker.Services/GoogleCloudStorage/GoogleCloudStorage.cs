using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Storage.v1.Data;
using Google.Cloud.Storage.V1;
using litter_tracker.Objects.ApiObjects;
using Object = Google.Apis.Storage.v1.Data.Object;

namespace litter_tracker.Services.GoogleCloudStorage
{
    public class GoogleCloudStorage : IGoogleCloudStorage
    {
        public async Task UploadFile(ImageUploadRequest request)
        {
            var storage = await StorageClient.CreateAsync();

            var fileDestination = new Object
            {
                Bucket = "litter-tracker.appspot.com",
                ContentType = request.Image.ContentType,
                Name = $"{request.MarkerDatastoreId}-{Guid.NewGuid()}-upload",
                Metadata = new Dictionary<string, string>()
                {
                    {"LitterPinId", request.MarkerDatastoreId.ToString()}, 
                    {"UploadedBy", request.UploadedByUid},
                },

            };

            await storage.UploadObjectAsync(fileDestination, request.Image.OpenReadStream());
        }
    }
}
