using System.Threading.Tasks;
using litter_tracker.Objects.ApiObjects;

namespace litter_tracker.Services.GoogleCloudStorage
{
    public interface IGoogleCloudStorage
    {
        Task<string> UploadFile(UploadImageRequest request);
    }
}