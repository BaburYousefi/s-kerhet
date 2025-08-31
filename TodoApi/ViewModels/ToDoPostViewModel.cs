using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.ViewModels;

public class ToDoPostViewModel
{
    
    [Required, StringLength(80)]
    public required string Title { get; set; } 

    public required string UserId { get; set; }

}
