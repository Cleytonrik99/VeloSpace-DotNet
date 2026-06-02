using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using VeloSpace.Model.Launch;
using VeloSpace.Model.PayloadShi;
using VeloSpace.Model.RocketShi;
using VeloSpace.Model.ScreeningShi;
using VeloSpace.Model.ShipperShi;

namespace VeloSpace.Context;

public class VeloSpaceContext : DbContext
{
    private readonly IConfiguration _configuration;

    public VeloSpaceContext(DbContextOptions<VeloSpaceContext> options, IConfiguration configuration) : base(options)
    {
    }
    
    public DbSet<LaunchProvider> LaunchProvider { get; set; }
    public DbSet<HandlerStatus> HandlerStatus { get; set; }
    public DbSet<PayloadHandler> PayloadHandler { get; set; }
    public DbSet<Payload> Payload { get; set; }
    public DbSet<PayloadPriority> PayloadPriority { get; set; }
    public DbSet<PayloadStatus> PayloadStatus { get; set; }
    public DbSet<Rocket> Rocket { get; set; }
    public DbSet<RocketStatus> RocketStatus { get; set; }
    public DbSet<Screening> Screening { get; set; }
    public DbSet<Shipper> ShipperPf{ get; set; }
}