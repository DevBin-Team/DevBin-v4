using System.ComponentModel.DataAnnotations;

namespace API.Models;

/// <summary>
/// Folders contain pastes.
/// </summary>
public class Folder
{
    /// <summary>
    /// Unique id of the folder
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the folder
    /// </summary>
    [MaxLength(255)]
    public string Name { get; set; }

    /// <summary>
    /// URL friendly name of the folder
    /// </summary>
    [MaxLength(255)]
    public string Slug { get; set; }
}
