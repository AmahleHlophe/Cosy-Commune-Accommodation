using cosycommune.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace cosycommune.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Room> Rooms { get; set; }
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<Lease> Leases { get; set; }
    public DbSet<Maintenance> Maintenance { get; set; }
    public DbSet<Facility> Facility { get; set; }
    public DbSet<Checkout> Checkouts { get; set; }
}
