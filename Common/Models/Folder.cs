using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
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

    public virtual ICollection<Paste> Pastes { get; set; }
}
