using System.Text;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

var s3Client = new AmazonS3Client();

// await UploadImage(s3Client);
await DownloadFileAsync();

Console.ReadLine();

async Task DownloadFileAsync()
{
    try
    {
        var request = new GetObjectRequest
        {
            BucketName = "anishbucket5416",
            Key = "images/my.jpg",
        };

        using (GetObjectResponse response = await s3Client.GetObjectAsync(request))
        {
            // Extract just the file name from the key
            string fileName = Path.GetFileName(response.Key);

            // Output the file name (or use it in your logic)
            Console.WriteLine($"File name extracted: {fileName}");

            // Define the download folder path
            string currentDirectory = Directory.GetCurrentDirectory();
            string downloadFolderPath = Path.Combine(currentDirectory, "downloads");

            // Ensure the downloads directory exists
            if (!Directory.Exists(downloadFolderPath))
            {
                Directory.CreateDirectory(downloadFolderPath);
            }

            // Set the full file path in the downloads folder
            string filePath = Path.Combine(downloadFolderPath, fileName);

            using (Stream responseStream = response.ResponseStream)
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                await responseStream.CopyToAsync(fileStream);
            }

            Console.WriteLine($"File downloaded successfully to {filePath}");
        }
    }
    catch (AmazonS3Exception e)
    {
        Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
    }
    catch (Exception e)
    {
        Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
    }
}

async Task DownloadImage()
{
    var getObjectResult = new GetObjectRequest()
    {
        BucketName = "anishbucket5416",
        Key = "images/my.jpg",
    };

    var response = await s3Client.GetObjectAsync(getObjectResult);
    TransferUtility utility = new TransferUtility(s3Client);

    await utility.DownloadAsync(new TransferUtilityDownloadRequest()
    {
        BucketName = "anishbucket5416",
        Key = "images/my.jpg",
        FilePath = $"./downloads/my.jpg"
    });
}


async Task UploadImage(AmazonS3Client amazonS3Client)
{
    await using var inputStream = new FileStream("./images/me.jpg", FileMode.Open, FileAccess.Read);

    var putObjectRequest = new PutObjectRequest()
    {
        BucketName = "anishbucket5416",
        Key = "images/my.jpg",
        ContentType = "image/jpeg",
        InputStream = inputStream
    };

    await amazonS3Client.PutObjectAsync(putObjectRequest);
}