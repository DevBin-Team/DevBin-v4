using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models;

public class PasteContent
{
    [Key]
    [MaxLength(64)]
    public string Hash { get; set; }
    public string Content { get; set; }

    public virtual ICollection<Paste> Pastes { get; set;}
}
