using MediatR;
using Microsoft.AspNetCore.Mvc;
using ScientificReportService.App.ApiRequests;
using ScientificReportService.App.Domain.Entities;
using ScientificReportService.App.Domain.Models;
using ScientificReportService.App.Features.CreateReport;
using ScientificReportService.App.Features.DeleteReport;
using ScientificReportService.App.Features.DownloadReport;
using ScientificReportService.App.Features.GetReportAuthor;
using ScientificReportService.App.Features.ListReports;
using Shared.ResultPattern.Errors;

namespace ScientificReportService.App.Controllers;

[ApiController]
[Route("reports")]
public class ScientificReportController : ControllerBase
{
    private readonly IMediator _mediator;

    public ScientificReportController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("upload")]
    public async Task<ActionResult<UploadFileResult>> UploadFile(
        IFormFile file,
        [FromForm]UploadRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateReportCommand(file, request.Title, request.Description, request.Author,
            request.AuthorId, request.Tags);
        
        var result = await _mediator.Send(command, cancellationToken);

        if (result.Succeeded)
        {
            return CreatedAtAction("DownloadFile", new {
                fileId = result.Value.FileId
            }, result.Value);
        }

        return BadRequest(result.Error.Message);
    }

    [HttpGet("download/{fileId}", Name = "DownloadFile")]
    public async Task<ActionResult<Stream>> DownloadFile([FromRoute] string fileId,
        CancellationToken cancellationToken = default)
    {
        var request = new DownloadReportRequest(fileId);
        
        var result = await _mediator.Send(request, cancellationToken);
        
        if (result.Succeeded)
        {
            return File(result.Value.FileStream, result.Value.ContentType, result.Value.FileName);
        }
        
        return result.Error.ErrorCode switch
        {
            ErrorCode.NotFound => NotFound(result.Error.Message),
            _ => BadRequest(result.Error.Message)
        };
    }

    [HttpGet]
    public async Task<ActionResult<List<ScientificReport>>> ListAsync(
        int page = 1,
        int maxPageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var request = new ListReportsRequest(page, maxPageSize);
        
        var result = await _mediator.Send(request, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id}/author")]
    public async Task<ActionResult<Author>> GetAuthorAsync(
        [FromRoute] string id,
        CancellationToken cancellationToken)
    {
        var request = new GetReportAuthorRequest(id);

        var result = await _mediator.Send(request, cancellationToken);

        if (result.Succeeded)
        {
            return Ok(result.Value);
        }

        return result.Error.ErrorCode switch
        {
            ErrorCode.NotFound => NotFound(result.Error.Message),
            _ => BadRequest(result.Error.Message)
        };
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteFile([FromRoute] string id, CancellationToken cancellationToken = default)
    {
        var command = new DeleteReportCommand(id);

        var result = await _mediator.Send(command, cancellationToken);

        if (result.Succeeded)
        {
            return NoContent();
        }

        return result.Error.ErrorCode switch
        {
            ErrorCode.NotFound => NotFound(result.Error.Message),
            _ => BadRequest(result.Error.Message)
        };
    }
}