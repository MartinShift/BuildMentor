using Azure.Storage.Blobs;
using BuildMentor.Database;
using BuildMentor.Database.Entities;
using BuildMentor.Services.Base;
using Microsoft.Extensions.Configuration;

namespace BuildMentor.Services
{
	public class ImageService : BaseDbService<Image>
	{
		private readonly IConfiguration _configuration;
		public ImageService(BuildContext context, IConfiguration configuration) : base(context)
        {
            _configuration = configuration;
        }
        public async Task<Image> Upload(IFormFile file)
		{
            var connectionString = _configuration.GetValue<string>("Azure:BlobStorage:ConnectionString");

            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            string containerName = "images";
            BlobContainerClient containerClient;

            if (blobServiceClient.GetBlobContainers().Any(x => x.Name == containerName))
            {
                containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            }
            else
            {
                containerClient = blobServiceClient.CreateBlobContainer(containerName);
            }
            var blobFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var blobClient = containerClient.GetBlobClient(blobFileName);
            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream);
            }

            var dbFile = new Image()
			{
				Filename = blobClient.Uri.AbsoluteUri,
				RootDirectory = "images",
			};
			dbSet.Add(dbFile);
			await context.SaveChangesAsync();
			return dbFile;
		}
	}
}
