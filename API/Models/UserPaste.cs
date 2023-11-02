using Common.Models;
using System.ComponentModel.DataAnnotations;

namespace API.Models;

/// <summary>
/// UserPastes only contain modifiable fields that the service does not normally change.
/// </summary>
public class UserPaste
{
    public int? FolderId { get; set; }
    [MaxLength(255)]
    public string? Name { get; set; }
    public string? Content { get; set; }
    public DateTime? ExpireDate { get; set; }

    /// <summary>
    /// The field AsGuest is only used during the creation of the paste.
    /// </summary>
    public bool? AsGuest { get; set; }
    [MaxLength(64)]
    public string? Syntax { get; set; }
    public Exposure? Exposure { get; set; }

    /// <summary>
    /// Tags help you to make your pastes easier to search on the platform by other users (granted that you set the paste to public, else only you will benefit from it).
    /// </summary>
    public IEnumerable<string>? Tags { get; set; } = Enumerable.Empty<string>();
}
