using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models;

public class ApplicationUser : IdentityUser<int>
{
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    //public virtual ICollection<Paste> Pastes { get; set; }
    public virtual ICollection<Folder> Folders { get; set; }
}
