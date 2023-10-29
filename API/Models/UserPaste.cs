using Common.Models;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class UserPaste
    {
        public int? FolderId { get; set; }
        [MaxLength(255)]
        public string Name { get; set; }
        public string Content { get; set; }
        public DateTime ExpireDate { get; set; }
        public bool AsGuest { get; set; }
        [MaxLength(64)]
        public string Syntax { get; set; } = "none";
        public Exposure Exposure { get; set; } = Exposure.Unlisted;
        public IEnumerable<string> Tags { get; set; } = Enumerable.Empty<string>();
    }
}
