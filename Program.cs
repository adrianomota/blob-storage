using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace blob_storage
{
    class Program
    {

        const string name = "fotosglobais";

        static async Task Main(string[] args)
        {

            var storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=photoswebappapi;AccountKey=IzZr19NI92nQbw2J+IdVNOG38Jw6FfnCPWQ6FH707RrGd9J0YTkqJtWo1A9e6PnkR5DpUxMyNVKS2JVhZY4nsw==;EndpointSuffix=core.windows.net");

            var blobClient = storageAccount.CreateCloudBlobClient();

            await ListarContainers(blobClient);

            await CriarContainer(blobClient);

            await CriarBlobClient(blobClient);

            await DownloadBlob(blobClient);

            await CriarSASTken(blobClient);

        }

       

        static async Task ListarContainers(CloudBlobClient blobClient)
        {
            var containers = await blobClient.ListContainersSegmentedAsync(null);

            foreach (var item in containers.Results)
            {
                Console.WriteLine(item.Name);
            }
        }

        static async Task CriarContainer(CloudBlobClient blobClient)
        {
            var container = blobClient.GetContainerReference(name);
            await container.CreateIfNotExistsAsync();
            Console.WriteLine($"Container {name}");
        }


        static async Task CriarBlobClient(CloudBlobClient blobClient)
        {
            var container = blobClient.GetContainerReference(name);
            var blob = container.GetBlockBlobReference("curso.txt");

            var exist = await blob.ExistsAsync();

            if(!exist)
            {
                await blob.UploadTextAsync("Hello blobs com Azure!!");
            }


            Console.WriteLine($"Url para download: {blob.Uri}");
        }

        static async Task DownloadBlob(CloudBlobClient blobClient)
        {
            var container = blobClient.GetContainerReference(name);
            var blob = container.GetBlockBlobReference("curso.txt");
            var content = await blob.DownloadTextAsync();

            Console.WriteLine($"Conteudo: {content} Type: {blob.Properties.ContentType}");
        }

        private static  void CriarSASTken(CloudBlobClient blobClient)
        {
            var container = blobClient.GetContainerReference(name);
            var blob = container.GetBlockBlobReference("curso.txt");
            var token = blob.GetSharedAccessSignature(new SharedAccessBlobPolicy
            {
                SharedAccessStartTime = DateTime.Today.AddDays(-1),
                SharedAccessExpiryTime = DateTime.Today.AddDays(1),
                Permissions = SharedAccessBlobPermissions.Read

            });

            Console.WriteLine($"Sas token: {token}");

        }

    }
}
