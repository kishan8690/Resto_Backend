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

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            try
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.FileName, stream),
                        PublicId = Guid.NewGuid().ToString()
                    };

                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                    return uploadResult.SecureUrl.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Image Upload Error: {ex.Message}");
                return string.Empty;
            }
        }

    }


}
