using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using VeloSpace.Model.InspectionShi;
using VeloSpace.Model.Launch;
using VeloSpace.Model.OperatorShi;
using VeloSpace.Model.PayloadShi;
using VeloSpace.Model.RocketShi;
using VeloSpace.Model.SatelliteShi;
using VeloSpace.Model.ShipperShi;

namespace VeloSpace.Context;

public class VeloSpaceContext : DbContext
{
    private readonly IConfiguration _configuration;

    public VeloSpaceContext(DbContextOptions<VeloSpaceContext> options, IConfiguration configuration) : base(options)
    {
    }
    
    public DbSet<LaunchProvider> LaunchProvider { get; set; }
    public DbSet<OperatorStatus> HandlerStatus { get; set; }
    public DbSet<Operator> PayloadHandler { get; set; }
    public DbSet<Satellite> Payload { get; set; }
    public DbSet<SatellitePriority> PayloadPriority { get; set; }
    public DbSet<SatelliteStatus> PayloadStatus { get; set; }
    public DbSet<Rocket> Rocket { get; set; }
    public DbSet<RocketStatus> RocketStatus { get; set; }
    public DbSet<Inspection> Screening { get; set; }
    public DbSet<Shipper> ShipperPf{ get; set; }
}