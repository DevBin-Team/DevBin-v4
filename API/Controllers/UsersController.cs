using API.Authentication;
using API.Models;
using Common.Data;
using Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Route("v4/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public UsersController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    [NonAction]
    public async Task<IEnumerable<Models.Paste>> GetUserPastes(ApplicationUser user, bool publicOnly)
    {
        var pastes = _dbContext.Pastes
            .Include(p => p.User)
            .Where(q => q.UserId == user.Id).AsQueryable();
        if (publicOnly)
            return pastes.Where(p => p.Exposure == Exposure.Public).Select(PastesController.ToApiPaste);
        else
            return pastes.Select(PastesController.ToApiPaste);
    }

    [HttpGet("me")]
    [RequireApiToken(ApiToken.APIPermissions.GetMe)]
    public async Task<ActionResult<User>> GetMe()
    {
        var me = await _userManager.GetUserAsync(User);
        if (me == null)
            return NotFound();

        var folders = _dbContext.Folders.Where(q => q.UserId == me.Id).Select(f => new Models.Folder
        {
            Id = f.Id,
            Name = f.Name,
            Slug = f.Slug,
        });

        return new User
        {
            Username = me.UserName!,
            CreationDate = me.CreationDate,
            Folders = folders,
        };
    }

    [HttpGet("{username}")]
    [RequireApiToken(ApiToken.APIPermissions.GetUser)]
    public async Task<ActionResult<User>> GetUser(string username)
    {
        var me = await _userManager.GetUserAsync(User);
        if (me == null)
            return NotFound();

        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
            return NotFound();

        var folders = _dbContext.Folders.Where(q => q.UserId == user.Id).Select(f => new Models.Folder
        {
            Id = f.Id,
            Name = f.Name,
            Slug = f.Slug,
        });

        return new User
        {
            Username = user.UserName!,
            CreationDate = user.CreationDate,
            Folders = folders,
        };
    }

    [HttpGet("me/pastes")]
    [RequireApiToken(ApiToken.APIPermissions.GetPaste | ApiToken.APIPermissions.GetMe)]
    public async Task<ActionResult<IEnumerable<Models.Paste>>> GetUserPastes()
    {
        var me = await _userManager.GetUserAsync(User);
        if (me == null)
            return NotFound();

        var pastes = await GetUserPastes(me, false);

        return Ok(pastes);
    }

    [HttpGet("{username}/pastes")]
    [RequireApiToken(ApiToken.APIPermissions.GetPaste | ApiToken.APIPermissions.GetUser)]
    public async Task<ActionResult<IEnumerable<Models.Paste>>> GetUserPastes(string username)
    {
        var me = await _userManager.GetUserAsync(User);
        if (me == null)
            return NotFound();

        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
            return NotFound();

        var pastes = await GetUserPastes(user, true);

        return Ok(pastes);
    }
}
