namespace litter_tracker.Objects.ApiObjects
{
    public class UploadImageRequest
    {
        public string Base64Image { get; set; } 
        public string UploadedByUid { get; set; } 
        public long MarkerDatastoreId { get; set; }
    }
}
