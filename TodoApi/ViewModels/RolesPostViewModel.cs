using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Ganss.Xss;

namespace TodoApi.ViewModels;
public class RolesPostViewModel
{
    private static readonly HtmlSanitizer _htmlSanitizer = new HtmlSanitizer();

    [Required(ErrorMessage = "Namn krävs.")]
    [MaxLength(13, ErrorMessage = "Namn får inte vara längre än 12 tecken.")]
    [MinLength(8, ErrorMessage = "Namn måste vara minst 8 tecken.")]
    public required string ItemNumber { get; set; }

    // Example properties for demonstration
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string RoleName { get; set; }

    public void Sanitize()
    {
        FirstName = _htmlSanitizer.Sanitize(FirstName);
        LastName = _htmlSanitizer.Sanitize(LastName);
        Email = _htmlSanitizer.Sanitize(Email);
        Password = _htmlSanitizer.Sanitize(Password);
        RoleName = _htmlSanitizer.Sanitize(RoleName);
    }
}

