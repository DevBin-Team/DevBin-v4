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

    public static ApiPaste ToApiPaste(Paste paste)
    {
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

    /// <summary>
    /// Get information about a paste.
    /// </summary>
    /// <param name="code">The unique code of the paste.</param>
    /// <returns></returns>
    [HttpGet("{code}")]
    [RequireApiToken(APIPermissions.GetPaste)]
    public async Task<ActionResult<ApiPaste>> GetPaste(string code)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound();

        var paste = await _dbContext.Pastes.FirstOrDefaultAsync(p => p.Code == code);
        if (paste == null)
            return NotFound();

        if (paste.Exposure == Exposure.Private)
        {
            if (paste.UserId != user.Id)
                return Forbid();
        }

        return ToApiPaste(paste);
    }

    /// <summary>
    /// Get the content of a paste.
    /// </summary>
    /// <param name="code">The unique code of the paste.</param>
    /// <returns></returns>
    [HttpGet("{code}/content")]
    [RequireApiToken(APIPermissions.GetPaste)]
    public async Task<ActionResult<string>> GetPasteContent(string code)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound();

        var paste = await _dbContext.Pastes
            .Include(p => p.PasteContent)
            .FirstOrDefaultAsync(p => p.Code == code);

        if (paste == null)
            return NotFound();

        if (paste.Exposure == Exposure.Private)
        {
            if (paste.UserId != user.Id)
                return Forbid();
        }

        return paste.Content;
    }

    /// <summary>
    /// Create a paste.
    /// Exposure is an integer.
    /// </summary>
    /// <param name="paste">The paste information to submit.</param>
    /// <returns></returns>
    [HttpPost]
    [RequireApiToken(APIPermissions.CreatePaste)]
    public async Task<ActionResult<ApiPaste>> CreatePaste([FromBody] UserPaste paste)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound();

        var asGuest = paste.AsGuest ?? false;

        if (asGuest && paste.FolderId != null)
        {
            return Forbid("Guests cannot have folders");
        }

        if (paste.FolderId != null && !user.Folders.Any(q => q.Id == paste.FolderId))
        {
            return NotFound($"Folder by id {paste.FolderId} not found");
        }

        if(paste.Content == null)
        {
            return BadRequest("Content field is required");
        }

        var newPaste = new Paste
        {
            Code = await _dbContext.GeneratePasteCodeAsync(),
            Name = paste.Name?.Truncate(255) ?? "Unnamed paste",
            CreationDate = DateTime.UtcNow,
            ExpireDate = paste.ExpireDate,
            Exposure = paste.Exposure ?? Exposure.Unlisted,
            FolderId = paste.FolderId,
            Syntax = paste.Syntax ?? "text",
            UserId = asGuest ? null : user.Id,
            TagString = paste.Tags != null ? Paste.GetTagString(paste.Tags) : "",
        };

        var hash = newPaste.UpdateHash(paste.Content);

        if (!await _dbContext.PasteContents.AnyAsync(q => q.Hash == hash))
        {
            await _dbContext.AddAsync(new PasteContent()
            {
                Hash = hash,
                Content = paste.Content,
            });
        }

        _dbContext.Pastes.Add(newPaste);
        await _dbContext.SaveChangesAsync();

        return ToApiPaste(newPaste);
    }

    /// <summary>
    /// Delete a paste fo yours.
    /// </summary>
    /// <param name="code">The unique code of the paste.</param>
    /// <returns></returns>
    [HttpDelete("{code}")]
    [RequireApiToken(APIPermissions.DeletePaste)]
    public async Task<ActionResult> DeletePaste(string code)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound();

        var paste = await _dbContext.Pastes.FirstOrDefaultAsync(p => p.Code == code);
        if (paste == null)
            return NotFound();

        if (paste.UserId != user.Id)
            return Forbid();

        _dbContext.Pastes.Remove(paste);
        await _dbContext.SaveChangesAsync();

        return Ok();
    }

    /// <summary>
    /// Update fields of a paste of yours.
    /// 
    /// Setting the folder to `-1` will move it out of it.
    /// </summary>
    /// <param name="code">The code of the paste.</param>
    /// <param name="userPaste">The paste with the fields to update.</param>
    /// <returns></returns>
    [HttpPatch("{code}")]
    [RequireApiToken(APIPermissions.UpdatePaste)]
    public async Task<ActionResult<ApiPaste>> UpdatePaste(string code, UserPaste userPaste)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound();

        var paste = await _dbContext.Pastes.FirstOrDefaultAsync(p => p.Code == code);
        if (paste == null)
            return NotFound();

        if (paste.UserId != user.Id)
            return Forbid();

        paste.Name = userPaste.Name?.Truncate(255) ?? paste.Name;
        paste.Exposure = userPaste.Exposure ?? paste.Exposure;
        paste.Syntax = userPaste.Syntax ?? paste.Syntax;
        paste.ExpireDate = userPaste.ExpireDate ?? paste.ExpireDate;

        if (userPaste.Tags != null)
        {
            paste.TagString = Paste.GetTagString(userPaste.Tags);
        }

        if (userPaste.FolderId != null)
        {
            if (userPaste.FolderId == -1)
            {
                paste.FolderId = null;
            }
            else if (user.Folders.Any(q => q.Id == userPaste.FolderId))
            {
                paste.FolderId = userPaste.FolderId;
            }
            else
            {
                return NotFound($"Folder by id {userPaste.FolderId} not found");
            }
        }

        if (userPaste.Content != null)
        {
            var hash = paste.UpdateHash(userPaste.Content);
            if (!await _dbContext.PasteContents.AnyAsync(q => q.Hash == hash))
            {
                await _dbContext.PasteContents.AddAsync(new()
                {
                    Hash = hash,
                    Content = userPaste.Content
                });
            }
        }

        _dbContext.Update(paste);
        await _dbContext.SaveChangesAsync();

        return Ok();
    }

    /// <summary>
    /// Get all syntaxes that the service knows about.
    /// </summary>
    /// <returns></returns>
    [HttpGet("syntaxes")]
    [RequireApiToken(APIPermissions.None)]
    public async Task<ActionResult<IEnumerable<Syntax>>> GetSyntaxes()
    {
        return await _dbContext.Syntaxes.ToListAsync();
    }
}
