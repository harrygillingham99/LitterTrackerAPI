using Microsoft.AspNetCore.Http;

namespace litter_tracker.Objects.ApiObjects
{
    public class ImageUploadRequest
    {
        public IFormFile Image { get; set; }
        public string UploadedByUid { get; set; }
        public long MarkerDatastoreId { get; set; }
    }
}