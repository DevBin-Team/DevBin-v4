using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class Folder
{
    public int Id { get; set; }
    [MaxLength(255)]
    public string Name { get; set; }
    [MaxLength(255)]
    public string Slug { get; set; }
}
