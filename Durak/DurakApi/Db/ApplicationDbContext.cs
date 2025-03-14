using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using DurakApi.Models.Db;

namespace DurakApi.Db;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<IdentityUser>(options)
{
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<GameRule> GameRules { get; set; }

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    modelBuilder.Entity<Room>()
    //        .HasMany(r => r.Users)
    //        .WithOne(r => r.Room)
    //        .HasForeignKey(r => r.RoomId)
    //        .IsRequired(false);
    //}
}