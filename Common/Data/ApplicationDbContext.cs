using Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Common.Data;
public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
{
    public DbSet<Paste> Pastes { get; set; }
    public DbSet<Syntax> Syntaxes { get; set; }
    public DbSet<Folder> Folders { get; set; }
    public DbSet<ApiToken> ApiTokens { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public const string PasteCodeCharset = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    public static readonly char[] Charset = PasteCodeCharset.ToCharArray();
    public async Task<string> GeneratePasteCodeAsync(int length = 8)
    {
        string code;
        do
        {
            var chars = RandomNumberGenerator.GetItems<char>(Charset, length);
            code = new string(chars);
        } while (await Pastes.AnyAsync(q => q.Code == code));

        return code;
    }
}
