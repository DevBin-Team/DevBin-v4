using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
    public string Content { get; set; }
    [MaxLength(64)]
    public string Hash { get; set; }
    [MaxLength(64)]
    public string Syntax { get; set; }
    public Exposure Exposure { get; set; }
    [MaxLength(255)]
    public string TagString { get; set; } = "";
    public IEnumerable<string> Tags => TagString.Split(',');

    public virtual ApplicationUser? User { get; set; }
    public virtual Folder? Folder { get; set; }
}
