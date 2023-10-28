using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models;

public class ApiTokens
{
    [Flags]
    public enum APIPermissions
    {
        None = 0,

        GetPaste = 1,
        CreatePaste = 2,
        UpdatePaste = 4,
        DeletePaste = 8,
        GetFolder = 16,
        DeleteFolder = 32,
        GetMe = 64,
        GetUser = 128,

        ReadOnly = GetPaste | GetFolder | GetMe | GetUser,
        All = GetPaste | CreatePaste | UpdatePaste | DeletePaste | GetFolder | DeleteFolder | GetMe | GetUser,
    }

    public int Id { get; set; }
    public int UserId { get; set; }
    [MaxLength(255)]
    public string Name { get; set; }
    [MaxLength(255)]
    public string Token { get; set; }
    public APIPermissions Permissions { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime ModifiedDate { get; set; }

    public virtual ApplicationUser User { get; set; }
}
