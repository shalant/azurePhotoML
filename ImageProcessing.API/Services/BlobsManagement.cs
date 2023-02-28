using ImageProcessing.API.Services.Interfaces;

namespace ImageProcessing.API.Services;

public class BlobsManagement : IBlobsManagement
{
    public async Task<string> UploadFile(string containerName, string fileName, byte[] file, string connectionString)
    {
        //
    }
}