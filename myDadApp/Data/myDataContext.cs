using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace myDadApp.Models
{
    public class myDataContext : IdentityDbContext<IdentityUser>
    {
        public myDataContext (DbContextOptions<myDataContext> options)
            : base(options)
        {
        }

        public DbSet<myDadApp.Models.Chore> Chore { get; set; }
    }
}
