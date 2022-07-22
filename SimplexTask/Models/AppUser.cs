using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplexTask.Models
{
    public class AppUser : IdentityUser
    {
        public Guid BookId { get; set; }
    }
}
