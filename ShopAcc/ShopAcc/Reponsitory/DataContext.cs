using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShopAcc.Models;

namespace ShopAcc.Reponsitory
{
    public class DataContext : IdentityDbContext<AppUserModel>
    {
        public DataContext(DbContextOptions<DataContext> options) :base(options) { }

        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<ServerModel> Servers { get; set; }
        public DbSet<CharacterModel> Characters { get; set; }
        public DbSet<NickModel> Nicks { get; set; }
    }
}
