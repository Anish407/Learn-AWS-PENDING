using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
string bucketName = "anishbucket5416";
app.UseHttpsRedirection();
app.MapGet("/downloadFile", async (IAmazonS3 amazonS3) =>
    {
        try
        {
            var getObjectRequest = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = "images/my.jpg"
            };

            using var response = await amazonS3.GetObjectAsync(getObjectRequest);

            return Results.Stream(response.ResponseStream, response.Headers.ContentType);
        }
        catch (AmazonS3Exception ex) when (ex.Message.Contains("The specified key does not exist"))
        {
            return Results.NotFound();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.StatusCode(500); // Internal Server Error
        }
    })
    .WithName("downloadFiles");

app.MapPost("/uploadFile", async (IAmazonS3 amazonS3, IFormFile file) =>
    {
        try
        {
            if (file.Length > 0)
            {
                using var newMemoryStream = new MemoryStream();
                await file.CopyToAsync(newMemoryStream);

                var uploadRequest = new PutObjectRequest
                {
                    InputStream = newMemoryStream,
                    BucketName = bucketName,
                    Key = "images/" + file.FileName, // Store file with original name
                    ContentType = file.ContentType
                };

                var response = await amazonS3.PutObjectAsync(uploadRequest);

                return Results.Ok(new { message = "File uploaded successfully", response.ETag });
            }

            return Results.BadRequest(new { message = "File is empty" });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.StatusCode(500); // Internal Server Error
        }
    })
    .WithName("uploadFile")
    .WithOpenApi();

app.MapDelete("/deleteFile", async (IAmazonS3 amazonS3, string fileName) =>
    {
        try
        {
            var deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = bucketName,
                Key = "images/" + fileName
            };

            await amazonS3.DeleteObjectAsync(deleteObjectRequest);

            return Results.Ok(new { message = "File deleted successfully" });
        }
        catch (AmazonS3Exception ex) when (ex.Message.Contains("The specified key does not exist"))
        {
            return Results.NotFound(new { message = "File not found" });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.StatusCode(500); // Internal Server Error
        }
    })
    .WithName("deleteFile")
    .WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}