using DurakApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DurakApi.Db;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Room> Rooms { get; set; }
    
}