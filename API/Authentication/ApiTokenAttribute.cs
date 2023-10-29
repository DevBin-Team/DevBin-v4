using Common.Data;
using Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Security;
using static Common.Models.ApiToken;

namespace API.Authentication;

public class ApiTokenAttribute : TypeFilterAttribute
{
    public ApiTokenAttribute(APIPermissions permissions = APIPermissions.None) : base(typeof(ApiTokenFilter))
    {
        Arguments = [permissions];
    }
}

public class ApiTokenFilter : IAsyncAuthorizationFilter
{
    private readonly ApplicationDbContext _dbContext;
    private readonly APIPermissions _permission;
    private readonly SignInManager<ApplicationUser> _signInManager;
    public ApiTokenFilter(ApplicationDbContext dbContext, APIPermissions permissions, SignInManager<ApplicationUser> signInManager)
    {
        _dbContext = dbContext;
        _permission = permissions;
        _signInManager = signInManager;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var apiTokenKey = context.HttpContext.Request.Headers.Authorization.ToString();
        var apiToken = await _dbContext.ApiTokens
            .Include(q => q.User)
            .FirstOrDefaultAsync(q => q.Token == apiTokenKey);
        if (apiToken == null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if((apiToken.Permissions & _permission) != _permission)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var userPrincipal = await _signInManager.CreateUserPrincipalAsync(apiToken.User);
        context.HttpContext.User = userPrincipal;
    }
}