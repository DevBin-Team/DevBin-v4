using Common.Models;
using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class Paste
{
    [MaxLength(32)]
    public string Code { get; set; }
    public string? User { get; set; }
    public int? FolderId { get; set; }

    public DateTime CreationDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public DateTime? ExpireDate { get; set; }
    [MaxLength(255)]
    public string Name { get; set; }
    [MaxLength(64)]
    public string Hash { get; set; }
    [MaxLength(64)]
    public string Syntax { get; set; }
    public Exposure Exposure { get; set; }
    [MaxLength(255)]
    public string TagString { get; set; } = "";
    public IEnumerable<string> Tags => TagString.Split(',');
}
