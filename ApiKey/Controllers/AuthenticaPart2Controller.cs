using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ApiKey.Data;
using ApiKey.Models;
using ApiKeyPOC.Controllers;
using ApiKeyPOC.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace ApiKey.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class AuthenticaController : CustomController
    {
        private readonly WhitelistDbContext _context;

        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticaController(WhitelistDbContext context, ILogger<AuthenticationController> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        [HttpGet("Whitelist/Entries/Data")]
        public async Task<ActionResult<IEnumerable<WhitelistEntry>>> GetWhitelist()
        {
            try
            {
                var whitelistEntry = await _context.WhitelistEntries.ToListAsync();
                return Ok(new ResponseApi<IEnumerable<WhitelistEntry>>(HttpStatusCode.OK, null, whitelistEntry));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CustomErrorStatusCode(ex);
            }
        }

        [HttpGet("Whitelist/Entry/{id}")]
        public async Task<ActionResult<WhitelistEntry>> GetWhitelistEntry(int id)
        {
            try
            {
                var whitelistEntry = await _context.WhitelistEntries.FindAsync(id);
                return whitelistEntry == null 
                    ? Ok(new ResponseApi<IActionResult>(HttpStatusCode.NotFound, "NotFound", null))
                    : Ok(new ResponseApi<WhitelistEntry>(HttpStatusCode.OK, null, whitelistEntry));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CustomErrorStatusCode(ex);
            }
        }

        [HttpPost("Whitelist/Entry")]
        public async Task<ActionResult<WhitelistEntry>> PostWhitelistEntry(WhitelistEntryDto whitelistEntry)
        {
            try
            {
                var entry = new WhitelistEntry { Value = whitelistEntry.Value, Description = whitelistEntry.Description };
                var result = _context.WhitelistEntries.Add(entry);
                await _context.SaveChangesAsync();

                CreatedAtAction(nameof(GetWhitelistEntry), new { id = result.Entity.Id }, result.Entity);
                return Ok(new ResponseApi<WhitelistEntry>(HttpStatusCode.Created, "Created", result.Entity));      
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CustomErrorStatusCode(ex);
            }
        }
            
        [HttpDelete("Whitelist/{id}")]
        public async Task<IActionResult> DeleteWhitelistEntry(int id)
        {
            try
            {
                var whitelistEntry = await _context.WhitelistEntries.FindAsync(id);
                if (whitelistEntry == null)
                    return Ok(new ResponseApi<IActionResult>(HttpStatusCode.NotFound, "NotFound", null));

                _context.WhitelistEntries.Remove(whitelistEntry);
                await _context.SaveChangesAsync();
                return Ok(new ResponseApi<IActionResult>(HttpStatusCode.NoContent, "Deleted", null));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CustomErrorStatusCode(ex);
            }
        }

        [HttpGet("Whitelist/Download")]
        public async Task<IActionResult> ExportWhitelist()
        {
            try
            {
                var entries = await _context.WhitelistEntries.ToListAsync();
                var content = string.Join("\n", entries.Select(e => e.Value));
                var bytes = Encoding.UTF8.GetBytes(content);
                return File(bytes, "text/plain", "whitelist.txt");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CustomErrorStatusCode(ex);
            }
        }
    }
}
