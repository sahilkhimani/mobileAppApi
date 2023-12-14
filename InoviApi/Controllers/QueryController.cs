using DTO;
using InoviDataTransferObject.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InoviWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class QueryController : ControllerBase
    {
  
        private IRepositoryWrapper _reposWrapper;
        public QueryController(IRepositoryWrapper reposWrapper)
        {
            _reposWrapper = reposWrapper;
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GetQueryList()
        {
            try
            {
                return Ok(await _reposWrapper.QueryRepo.GetQueryList());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Retrieving Data!");

            }

        }

        [HttpPost("upload")]
        public IActionResult UploadImages([FromForm] List<IFormFile> files)
        {
            try
            {
                List<AttachmentLinkList> uploadedFiles = new List<AttachmentLinkList>();

                foreach (var file in files)
                {
                    if (file == null || file.Length == 0)
                        return BadRequest("Invalid file");

                    // Process each file, generate attachment link ID
                    var attachmentLinkId = Guid.NewGuid().ToString();
                    var filePath = "E:\\Daniyal\\Test\\Test" + attachmentLinkId + "_" + file.FileName;

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    // Create model for the uploaded file
                    var uploadedFile = new AttachmentLinkList
                    {
                        FileName = file.FileName,
                        AttachmentLink = attachmentLinkId
                    };

                    uploadedFiles.Add(uploadedFile);
                }

                return Ok(uploadedFiles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
