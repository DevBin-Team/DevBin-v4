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
    public IEnumerable<Models.Paste> GetUserPastes(ApplicationUser user, bool publicOnly)
    {
        var pastes = _dbContext.Pastes
            .Include(p => p.User)
            .Where(q => q.UserId == user.Id).AsQueryable();
        if (publicOnly)
            return pastes.Where(p => p.Exposure == Exposure.Public).Select(PastesController.ToApiPaste);
        else
            return pastes.Select(PastesController.ToApiPaste);
    }

    /// <summary>
    /// Information about your user.
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Information about an user.
    /// </summary>
    /// <param name="username">The username.</param>
    /// <returns></returns>
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


    /// <summary>
    /// A list of all the pastes you created.
    /// </summary>
    /// <returns></returns>
    [HttpGet("me/pastes")]
    [RequireApiToken(ApiToken.APIPermissions.GetPaste | ApiToken.APIPermissions.GetMe)]
    public async Task<ActionResult<IEnumerable<Models.Paste>>> GetUserPastes()
    {
        var me = await _userManager.GetUserAsync(User);
        if (me == null)
            return NotFound();

        var pastes = GetUserPastes(me, false);

        return Ok(pastes);
    }

    /// <summary>
    /// A list of all the public pastes the user created.
    /// </summary>
    /// <param name="username">Username</param>
    /// <returns></returns>
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

        var pastes = GetUserPastes(user, true);

        return Ok(pastes);
    }

    [HttpPost("me/folders")]
    [RequireApiToken(ApiToken.APIPermissions.CreateFolder)]
    public async Task<ActionResult<Models.Folder>> CreateFolder(string folderName)
    {
        var me = await _userManager.GetUserAsync(User);
        if (me == null)
            return NotFound();

        var slug = Common.Models.Folder.SanitizeName(folderName);
        if(await _dbContext.Folders.AnyAsync(q => q.UserId == me.Id && q.Slug == slug))
        {
            return Conflict("A folder with a similar name already exists");
        }

        var folder = new Common.Models.Folder()
        {
            Name = folderName[0..Math.Min(folderName.Length, 250)],
            Slug = slug,
            UserId = me.Id,
        };

        _dbContext.Folders.Add(folder);
        await _dbContext.SaveChangesAsync();

        return new Models.Folder()
        {
            Id = folder.Id,
            Name = folder.Name,
            Slug = folder.Slug,
        };
    }

    [HttpDelete("me/folders/{id:int}")]
    [RequireApiToken(ApiToken.APIPermissions.CreateFolder)]
    public async Task<ActionResult<Models.Folder>> DeleteFolder(int id)
    {
        var me = await _userManager.GetUserAsync(User);
        if (me == null)
            return NotFound();

        return Ok();
    }
}
