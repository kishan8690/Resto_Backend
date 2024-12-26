using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System;
using System.IO;
using System.Threading.Tasks;
namespace Resto_Backend.Utils
{


    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IConfiguration configuration)
        {
            var cloudName = configuration["CloudinarySettings:CloudName"];
            var apiKey = configuration["CloudinarySettings:ApiKey"];
            var apiSecret = configuration["CloudinarySettings:ApiSecret"];

            _cloudinary = new Cloudinary(new Account(cloudName, apiKey, apiSecret))
            {
                Api = { Secure = true }
            };
        }

        public async Task<string> UploadFileAsync(string localFilePath)
        {
            try
            {
                if (string.IsNullOrEmpty(localFilePath) || !File.Exists(localFilePath))
                    throw new ArgumentException("Invalid file path.");

                // Use RawUploadParams for resource_type:auto
                var uploadParams = new RawUploadParams
                {
                    File = new FileDescription(localFilePath)
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                // Delete the local file after upload
                File.Delete(localFilePath);

                // Return the secure URL of the uploaded file
                return uploadResult.SecureUrl?.ToString();
            }
            catch (Exception ex)
            {
                // Handle error (e.g., log it)
                Console.WriteLine($"Upload failed: {ex.Message}");

                // Delete the local file if upload fails
                if (File.Exists(localFilePath))
                    File.Delete(localFilePath);

                return null; // Return null if the upload fails
            }
        }

    }


}
