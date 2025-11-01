using MediatR;
using Microsoft.AspNetCore.Mvc;
using ScientificReportService.App.ApiRequests;
using ScientificReportService.App.Domain.Models;
using ScientificReportService.App.Features.CreateReport;

namespace ScientificReportService.App.Controllers;

[ApiController]
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
            request.Tags);
        
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
    public async Task<IActionResult> DownloadFile(string fileId)
    {
        return StatusCode(405);
    }
    
    [HttpGet("info/{fileId}")]
    public async Task<ActionResult<FileInfoResult>> GetFileInfo(string fileId)
    {
        return StatusCode(405);
    }
    
    [HttpDelete("{fileId}")]
    public async Task<ActionResult> DeleteFile(string fileId)
    {
        return StatusCode(405);
    }
}