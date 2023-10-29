using API.Authentication;
using API.Models;
using Common.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Common.Models.ApiToken;
using ApiPaste = API.Models.Paste;

namespace API.Controllers;

[Route("v4/[controller]")]
[ApiController]
[ApiToken]
public class PastesController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    public PastesController(ApplicationDbContext dbContext) {
        _dbContext = dbContext;
    }

    [HttpGet]
    [ApiToken(APIPermissions.GetPaste)]
    public async Task<ActionResult<ApiPaste>> GetPaste(string code)
    {
        var paste = await _dbContext.Pastes.FirstOrDefaultAsync(p => p.Code == code);
        if(paste == null)
            return NotFound();

        if(paste.Exposure.HasFlag(Common.Models.Exposure.Private))
        {

        }

        var apiPaste = new ApiPaste
        {
            User = paste.User?.UserName,
            Code = paste.Code,
            Name = paste.Name,
            CreationDate = paste.CreationDate,
            ExpirationDate = paste.ExpirationDate,
            ModifiedDate = paste.ModifiedDate,
            Exposure = paste.Exposure,
            FolderId = paste.FolderId,
            Hash = paste.Hash,
            Syntax = paste.Syntax,
            TagString = paste.TagString
        };

        return apiPaste;
    }
}
