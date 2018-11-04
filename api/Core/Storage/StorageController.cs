using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

// http://dotnetdetail.net/how-to-upload-file-using-angular-6-and-asp-net-core-2-1-web-api/
namespace Api.Core.Storage
{
  [Route(RouteUrl), ApiController]
  public class StorageController : ControllerBase
  {
    private const string RouteUrl = "uploads";
    private readonly string[] acceptedFileTypes = {".jpg", ".jpeg", ".png"};
    private readonly IHostingEnvironment host;

    public StorageController(IHostingEnvironment host)
    {
      this.host = host;
    }

    [HttpPost]
    [AddSwaggerFileUploadButton]
    public async Task<IActionResult> Upload(IFormFile file)
    {
      if (file == null || file.Length == 0)
      {
        return BadRequest("Empty or null file");
      }

      if (file.Length > 10 * 1024 * 1024)
      {
        return BadRequest("Max file size exceeded.");
      }

      if (acceptedFileTypes.All(s => s != Path.GetExtension(file.FileName).ToLower()))
      {
        return BadRequest("Invalid file type.");
      }

      var uploadPath = Path.Combine(host.WebRootPath, "uploads");
      var listeningPath = Path.Combine(Request.Host.Value, "uploads");
      
      if (!Directory.Exists(uploadPath))
      {
        Directory.CreateDirectory(uploadPath);
      }
        
      var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
      var filePath = Path.Combine(uploadPath, fileName);
      using (var stream = new FileStream(filePath, FileMode.Create))
      {
        await file.CopyToAsync(stream);
      }

      var apiFilePath = Path.Combine(listeningPath, fileName);
      return Ok(apiFilePath);
    }
  }
}