using Microsoft.AspNetCore.Identity;

namespace Strade.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string MatricNo { get; set; }
        
    }
}