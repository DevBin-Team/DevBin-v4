using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models;

public class Syntax
{
    [Key]
    [MaxLength(255)]
    public string Code { get; set; }
    [MaxLength(255)]
    public string Name { get; set; }
    public bool Priority { get; set; } = false;


}
