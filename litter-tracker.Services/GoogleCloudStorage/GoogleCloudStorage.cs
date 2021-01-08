using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Google.Cloud.Storage.V1;
using litter_tracker.Objects.ApiObjects;
using litter_tracker.Objects.InternalObjects;
using Microsoft.Extensions.Options;
using Object = Google.Apis.Storage.v1.Data.Object;

namespace litter_tracker.Services.GoogleCloudStorage
{
    public class GoogleCloudStorage : IGoogleCloudStorage
    {
        private readonly string _bucketName;

        public GoogleCloudStorage(IOptions<ConnectionStrings> connectionStrings)
        {
            _bucketName = connectionStrings.Value.BucketName;
        }

        public async Task<string> UploadFile(UploadImageRequest request)
        {
            MemoryStream imageStream = new MemoryStream(Convert.FromBase64String(request.Base64Image));

            var storage = await StorageClient.CreateAsync();

            var fileName = $"{request.MarkerDatastoreId}-{Guid.NewGuid()}-upload";

            var fileDestination = new Object
            {
                Bucket = _bucketName,
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

        public async Task DeleteImages(List<string> fileNames)
        {
            var storage = await StorageClient.CreateAsync();
            foreach (var file in fileNames)
            {
                await storage.DeleteObjectAsync(_bucketName, file);
            }
        }
    }
}
