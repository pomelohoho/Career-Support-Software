// Controllers/H1BController.cs
using CareerSupportSoftwareServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareerSupportSoftware.Server.Controllers;

//[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class H1BController : ControllerBase
{
    private readonly ExcelImportService _importService;

    public H1BController(ExcelImportService importService)
    {
        _importService = importService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadH1BData(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded");

        if (Path.GetExtension(file.FileName).ToLower() != ".xlsx")
            return BadRequest("Only .xlsx files are supported");

        using var stream = file.OpenReadStream();
        await _importService.ImportH1BData(stream);

        return Ok($"Imported {file.FileName} successfully");
    }
}