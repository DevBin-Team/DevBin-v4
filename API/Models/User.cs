using Common.Models;

namespace API.Models;

public class User
{
    public string Username { get; set; }
    public DateTime CreationDate { get; set; }
    public IEnumerable<Folder> Folders { get; set; }
}
