using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Google.Cloud.Storage.V1;
using litter_tracker.Objects.ApiObjects;
using litter_tracker.Objects.InternalObjects;
using Microsoft.Extensions.Options;
using Object = Google.Apis.Storage.v1.Data.Object;

namespace litter_tracker.Services.GoogleCloudStorage
{
    /*
    Service agent class for interfacing with Google cloud Storage API. 
    Used specifically for uploading and deleting images.
    */
    public class GoogleCloudStorage : IGoogleCloudStorage
    {
        private readonly string _bucketName;
        private readonly Task<StorageClient> _client;

        public GoogleCloudStorage(IOptions<ConnectionStrings> connectionStrings)
        {
            _bucketName = connectionStrings.Value.BucketName;
            _client = StorageClient.CreateAsync();
        }

        public async Task<string> UploadFile(UploadImageRequest request)
        {
            MemoryStream imageStream = new MemoryStream(Convert.FromBase64String(request.Base64Image));

            var storage = await _client;

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

        public async Task DeleteFiles(List<string> fileNames)
        {
            var storage = await _client;
            foreach (var file in fileNames)
            {
                await storage.DeleteObjectAsync(_bucketName, file);
            }
        }
    }
}
