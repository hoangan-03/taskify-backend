using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TodoAppBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttachmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly string _attachmentsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Attachments");

        public AttachmentsController(ApplicationDbContext context)
        {
            _context = context;

            if (!Directory.Exists(_attachmentsDirectory))
            {
                Directory.CreateDirectory(_attachmentsDirectory);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadAttachment([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var filePath = Path.Combine(_attachmentsDirectory, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var attachmentUrl = $"{Request.Scheme}://{Request.Host}/api/attachments/{file.FileName}";

            Console.WriteLine($"Generated attachment URL: {attachmentUrl}");

            return Ok(new { Url = attachmentUrl });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAttachment(int id)
        {
            var attachment = await _context.Attachments.FindAsync(id);
            if (attachment == null)
            {
                return NotFound("Attachment not found.");
            }

            if (string.IsNullOrEmpty(attachment.Name))
            {
                return NotFound("Attachment name is missing.");
            }

            var filePath = Path.Combine(_attachmentsDirectory, attachment.Name);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found.");
            }

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, GetContentType(filePath), attachment.Name);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAttachments()
        {
            var attachments = await _context.Attachments.ToListAsync();
            if (attachments == null || !attachments.Any())
            {
                return NotFound("No attachments found.");
            }

            return Ok(attachments);
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }


        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
    }

    
}