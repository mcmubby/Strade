using System.ComponentModel.DataAnnotations;
namespace Strade.Web.Models
{
    public class LoginViewModel
    {
        [Required]
        public string MatricNo { get; set; }
        [Required]
        public string Password { get; set; }
    }
}