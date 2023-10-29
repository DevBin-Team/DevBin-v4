using API.Authentication;
using API.Models;
using Common.Data;
using Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Common.Models.ApiToken;
using Paste = Common.Models.Paste;
using ApiPaste = API.Models.Paste;
using API.Utils;

namespace API.Controllers;

[Route("v4/[controller]")]
[ApiController]
public class PastesController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public PastesController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<string> Random()
    {
        return await _dbContext.GeneratePasteCodeAsync();
    }

    [HttpGet("{code}")]
    [ApiToken(APIPermissions.GetPaste)]
    public async Task<ActionResult<ApiPaste>> GetPaste(string code)
    {
        var paste = await _dbContext.Pastes.FirstOrDefaultAsync(p => p.Code == code);
        if (paste == null)
            return NotFound();

        var user = await _userManager.GetUserAsync(User);
        if (paste.Exposure == Exposure.Private)
        {
            if (paste.User != user)
                return Forbid();
        }

        var apiPaste = new ApiPaste
        {
            User = paste.User?.UserName,
            Code = paste.Code,
            Name = paste.Name,
            CreationDate = paste.CreationDate,
            ExpireDate = paste.ExpireDate,
            ModifiedDate = paste.ModifiedDate,
            Exposure = paste.Exposure,
            FolderId = paste.FolderId,
            Hash = paste.Hash,
            Syntax = paste.Syntax,
            TagString = paste.TagString
        };

        return apiPaste;
    }

    [HttpPost]
    [ApiToken(APIPermissions.GetPaste)]
    public async Task<ActionResult<ApiPaste>> CreatePaste([FromBody] UserPaste paste)
    {
        var user = await _userManager.GetUserAsync(User);
        if(user == null)
            return NotFound();

        var newPaste = new Paste
        {
            Code = await _dbContext.GeneratePasteCodeAsync(),
            Name = paste.Name.Truncate(255),
            CreationDate = DateTime.UtcNow,
            ExpireDate = paste.ExpireDate,
            Exposure = paste.Exposure,
            FolderId = paste.FolderId,
            Syntax = paste.Syntax,
            UserId = paste.AsGuest ? null : user.Id,
            TagString = string.Join(",", paste.Tags.Select(q => q.Truncate(50).ToLower())),
            
        };

        return Ok();
    }
}
