using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common.Models;

public class Folder
{
    public int Id { get; set; }
    public int UserId { get; set; }
    [MaxLength(255)]
    public string Name { get; set; }
    [MaxLength(255)]
    public string Slug { get; set; }

    public virtual ApplicationUser User { get; set; }
    public virtual ICollection<Paste> Pastes { get; set; }

    public static string SanitizeName(string name)
    {
        name = name.ToLower();
        name = Regex.Replace(name, @"[^a-z0-9\s-]", "");
        name = Regex.Replace(name, @"\s+", " ");
        name = Regex.Replace(name, @"\s", "-");
        name = name[0..Math.Min(name.Length, 128)];
        return name;
    }
}
