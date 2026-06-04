using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using VeloSpace.Model.InspectionShi;
using VeloSpace.Model.Launch;
using VeloSpace.Model.OperatorShi;
using VeloSpace.Model.PayloadShi;
using VeloSpace.Model.RocketShi;
using VeloSpace.Model.SatelliteShi;
using VeloSpace.Model.ShipperShi;
using VeloSpace.Model.User;

namespace VeloSpace.Context;

public class VeloSpaceContext : DbContext
{
    private readonly IConfiguration _configuration;

    public VeloSpaceContext(DbContextOptions<VeloSpaceContext> options, IConfiguration configuration) : base(options)
    {
    }
    
    public DbSet<Inspection> Inspection { get; set; }
    public DbSet<LaunchProvider> LaunchProvider { get; set; }
    public DbSet<Operator> Operator { get; set; }
    public DbSet<OperatorStatus> OperatorStatus { get; set; }
    public DbSet<Rocket> Rocket { get; set; }
    public DbSet<RocketStatus> RocketStatus { get; set; }
    public DbSet<Satellite> Satellite { get; set; }
    public DbSet<SatellitePriority> SatellitePriority { get; set; }
    public DbSet<SatelliteStatus> SatelliteStatus { get; set; }
    public DbSet<Shipper> Shipper { get; set; }
    public DbSet<UserAccount> UserAccount { get; set; }
    public DbSet<UserRole> UserRole { get; set; }
}