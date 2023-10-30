using Common.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models;

public class Paste
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public int? FolderId { get; set; }

    [MaxLength(32)]
    public string Code { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public DateTime? ExpireDate { get; set; }
    [MaxLength(255)]
    public string Name { get; set; }

    [ForeignKey(nameof(PasteContent))]
    [MaxLength(64)]
    public string Hash { get; set; }
    [MaxLength(64)]
    public string Syntax { get; set; }
    public Exposure Exposure { get; set; }
    [MaxLength(255)]
    public string TagString { get; set; } = "";
    public IEnumerable<string> Tags => TagString.Split(',');
    public bool Deleted { get; set; } = false;

    public virtual ApplicationUser? User { get; set; }
    public virtual Folder? Folder { get; set; }
    public virtual PasteContent PasteContent { get; set; }
    public string Content => PasteContent.Content;

    public string UpdateHash(string content)
    {
        Hash = GetHash(content);
        return Hash;
    }

    public static string GetHash(string content)
    {
        var bytes = Encoding.UTF8.GetBytes(content);
        var digest = SHA256.HashData(bytes);
        return Convert.ToHexString(digest);
    }

    public static string GetTagString(IEnumerable<string> tags)
    {
        return string.Join(",", tags.Select(q => q.Truncate(50).ToLower()));
    }
}
