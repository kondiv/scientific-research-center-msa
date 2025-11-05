using System.Net;
using System.Text;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using ScientificReportService.App.Common;
using ScientificReportService.App.Domain.Configurations;
using ScientificReportService.App.Domain.Errors;
using ScientificReportService.App.Domain.Models;
using Shared.ResultPattern;
using Shared.ResultPattern.Errors;

namespace ScientificReportService.App.Infrastructure.CloudStorage;

public class YandexCloudFileStorage : IFileStorage
{
    private readonly AmazonS3Client _client;
    private readonly string _bucketName;

    public YandexCloudFileStorage(IOptions<YandexCloudSettings> settings)
    {
        _bucketName = settings.Value.BucketName;

        var config = new AmazonS3Config 
        {
            ServiceURL = settings.Value.Url
        };

        _client = new AmazonS3Client(
            settings.Value.AccessKey,
            settings.Value.SecretKey, 
            config
        );
    }
    
    public async Task<Result<UploadFileResult>> Upload(IFormFile file, string? uploadedBy = null,
        CancellationToken cancellationToken = default)
    {
        var fileId = Guid.NewGuid().ToString();
        var objectKey = $"files/{fileId}";
        
        await using var stream = file.OpenReadStream();

        var putRequest = new PutObjectRequest()
        {
            BucketName = _bucketName,
            Key = objectKey,
            InputStream = stream,
            ContentType = file.ContentType
        };
        
        putRequest.Metadata.Add("original-filename", Convert.ToBase64String(Encoding.UTF8.GetBytes(file.FileName)));
        putRequest.Metadata.Add("uploaded-by", Convert.ToBase64String(Encoding.UTF8.GetBytes(uploadedBy)) ?? "anonymous");
        putRequest.Metadata.Add("content-type", file.ContentType);
        putRequest.Metadata.Add("uploaded-at", DateTime.UtcNow.ToString("O"));

        try
        {
            await _client.PutObjectAsync(putRequest, cancellationToken);

            var fileUrl = await GeneratePresignedUrlAsync(objectKey);

            return Result<UploadFileResult>.Success(new UploadFileResult
            {
                ContentType = file.ContentType,
                FileId = fileId,
                FileName = file.FileName,
                Size = file.Length,
                Url = fileUrl,
                UploadedAt = DateTime.UtcNow
            });
        }
        catch (Exception e) when (e is not OperationCanceledException or TaskCanceledException) 
        {
            return Result<UploadFileResult>.Failure(
                new CloudStorageError("Cannot upload file. Try again later"), e);
        }
    }

    public async Task<Result<Stream>> Download(string fileId, CancellationToken cancellationToken = default)
    {
        var objectKey = $"files/{fileId}";

        var request = new GetObjectRequest
        {
            BucketName = _bucketName,
            Key = objectKey,
        };

        try
        {
            var response = await _client.GetObjectAsync(request, cancellationToken);

            if (response.HttpStatusCode == HttpStatusCode.NotFound)
            {
                return Result<Stream>.Failure(new NotFoundError("Object does not exist"));
            }
            
            return Result<Stream>.Success(response.ResponseStream);
        }
        catch (Exception e) when (e is not OperationCanceledException or TaskCanceledException)
        {
            return Result<Stream>.Failure(new CloudStorageError("Cannot download file. Try again later"));
        }
    }

    public async Task<Result> Delete(string fileId, CancellationToken cancellationToken = default)
    {
        var objectKey = $"files/{fileId}";

        var request = new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = objectKey,
        };

        try
        {
            await _client.DeleteObjectAsync(request, cancellationToken);
            return Result.Success();
        }
        catch (Exception e) when (e is not OperationCanceledException or TaskCanceledException)
        {
            return Result.Failure(new CloudStorageError("Cannot delete file. Try again later"), e);
        }
    }

    public async Task<Result<FileInfoResult>> GetFileInfo(string fileId, CancellationToken cancellationToken = default)
    {
        var objectKey = $"files/{fileId}";

        var request = new GetObjectMetadataRequest
        {
            BucketName = _bucketName,
            Key = objectKey,
        };

        try
        {
            var result = await _client.GetObjectMetadataAsync(request, cancellationToken);

            if (result is null)
            {
                return Result<FileInfoResult>.Failure(new NotFoundError($"Cannot find file with id: {fileId}"));
            }

            return Result<FileInfoResult>.Success(new FileInfoResult()
            {
                ContentType = result.ContentType,
                FileId = fileId,
                FileName = Encoding.UTF8.GetString(Convert.FromBase64String(result.Metadata["original-filename"])),
                Size = result.Headers.ContentLength,
                UploadedAt = DateTime.Parse(result.Metadata["uploaded-at"])
            });
        }
        catch (Exception e) when (e is not OperationCanceledException or TaskCanceledException)
        {
            return Result<FileInfoResult>.Failure(new CloudStorageError("Cannot get file info. Try again later"));
        }
    }
    
    private async Task<string> GeneratePresignedUrlAsync(string objectKey, int expirationHours = 3)
    {
        var urlRequest = new GetPreSignedUrlRequest
        {
            BucketName = _bucketName,
            Key = objectKey,
            Expires = DateTime.UtcNow.AddHours(expirationHours),
            Verb = HttpVerb.GET
        };
        
        return await _client.GetPreSignedURLAsync(urlRequest);
    }
}