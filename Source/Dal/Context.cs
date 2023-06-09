using Dal.Entities;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace Dal;

/// <inheritdoc />
public class Context : DbContext
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Only for migrations. Initializes a new instance of the <see cref="Context"/> class.
    /// </summary>
    // public Context()
    // {
    // }

    /// <summary>
    /// Initializes a new instance of the <see cref="Context"/> class.
    /// </summary>
    /// <param name="options">Context options.</param>
    public Context(DbContextOptions<Context> options/*, IConfigurationHelper configurationHelper*/)
        : base(options)
    {
        // if (configurationHelper.IsNeedToRecreate == "1")
        // {
        //     Database.EnsureDeleted();
        //     Database.EnsureCreated();
        // }
    }

    /// <summary>
    /// Benchmarks.
    /// </summary>
    public DbSet<Benchmark> Benchmarks { get; set; } = null!;

    /// <summary>
    /// Configurations.
    /// </summary>
    public DbSet<Configuration> Configurations { get; set; } = null!;

    /// <summary>
    /// Configurations recommendations.
    /// </summary>
    public DbSet<ConfigurationRecommendation> ConfigurationRecommendations { get; set; } = null!;

    /// <summary>
    /// Configurations recommendations benchmarks.
    /// </summary>
    public DbSet<ConfigurationRecommendationsBenchmark> ConfigurationRecommendationBenchmarks { get; set; } = null!;

    /// <summary>
    /// Trusted hardware.
    /// </summary>
    public DbSet<TrustedHardware> TrustedHardware { get; set; } = null!;

    /// <summary>
    /// Trusted hardware benchmarks.
    /// </summary>
    public DbSet<TrustedHardwareBenchmark> TrustedHardwareBenchmarks { get; set; } = null!;

    /// <summary>
    /// Trusted software.
    /// </summary>
    public DbSet<TrustedSoftware> TrustedSoftware { get; set; } = null!;

    /// <summary>
    /// Trusted software benchmarks.
    /// </summary>
    public DbSet<TrustedSoftwareBenchmark> TrustedSoftwareBenchmarks { get; set; } = null!;

    /// <summary>
    /// Analysis results.
    /// </summary>
    public DbSet<AnalysisScheduleRecord> AnalysisResults { get; set; } = null!;

    /// <summary>
    /// Analysis schedule records.
    /// </summary>
    public DbSet<AnalysisScheduleRecord> AnalysisScheduleRecords { get; set; } = null!;

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("public");
        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<AnalysisResult>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
        });

        modelBuilder.Entity<AnalysisScheduleRecord>(entity =>
        {
            entity.HasIndex(e => e.Name, "AK_AnalysisScheduleRecords_Name").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<Benchmark>(entity =>
        {
            entity.HasIndex(e => e.Name, "AK_Benchmarks_Name").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.Name).HasMaxLength(200);

            entity.HasOne(d => d.ConfigurationRecommendationsBenchmark).WithMany(p => p.Benchmarks)
                .HasForeignKey(d => d.ConfigurationRecommendationsBenchmarkId)
                .HasConstraintName("FK_Benchmarks_ConfigurationsRecommendationsBenchmarks");

            entity.HasOne(d => d.TrustedHardwareBenchmark).WithMany(p => p.Benchmarks)
                .HasForeignKey(d => d.TrustedHardwareBenchmarkId)
                .HasConstraintName("FK_Benchmarks_TrustedHardwareBenchmarks");

            entity.HasOne(d => d.TrustedSoftwareBenchmark).WithMany(p => p.Benchmarks)
                .HasForeignKey(d => d.TrustedSoftwareBenchmarkId)
                .HasConstraintName("FK_Benchmarks_TrustedSoftwareBenchmarks");
        });

        modelBuilder.Entity<Entities.Configuration>(entity =>
        {
            entity.HasIndex(e => e.Name, "AK_Configurations_Name").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<ConfigurationRecommendation>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.VerificationCommand).HasMaxLength(1000);
            entity.Property(e => e.VerificationResult).HasMaxLength(1000);

            entity.HasOne(d => d.Configuration).WithMany(p => p.ConfigurationRecommendations)
                .HasForeignKey(d => d.ConfigurationId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ConfigurationsRecommendations_Configurations");
        });

        modelBuilder.Entity<ConfigurationRecommendationsBenchmark>(entity =>
        {
            entity.HasIndex(e => e.Name, "AK_ConfigurationsRecommendationsBenchmarks_Name").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.Name).HasMaxLength(200);

            entity.HasMany(d => d.ConfigurationRecommendations).WithMany(p => p.ConfigurationRecommendationsBenchmarks)
                .UsingEntity<Dictionary<string, object>>(
                    "BenchmarksConfigurationsRecommendationsRelation",
                    r => r.HasOne<ConfigurationRecommendation>().WithMany()
                        .HasForeignKey("ConfigurationsRecommendationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_BenchmarksConfigsRecsRel_ConfigsRecs"),
                    l => l.HasOne<ConfigurationRecommendationsBenchmark>().WithMany()
                        .HasForeignKey("ConfigurationsRecommendationsBenchmarkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_BenchmarksConfigsRecsRel_ConfigsRecsBenchmarks"),
                    j =>
                    {
                        j.HasKey("ConfigurationsRecommendationsBenchmarkId", "ConfigurationsRecommendationId");
                    });
        });

        modelBuilder.Entity<TrustedHardware>(entity =>
        {
            entity.ToTable("TrustedHardware");

            entity.HasIndex(e => e.HardwareId, "AK_TrustedHardware_HardwareId").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.HardwareId).HasMaxLength(350);
        });

        modelBuilder.Entity<TrustedHardwareBenchmark>(entity =>
        {
            entity.HasIndex(e => e.Name, "AK_TrustedHardwareBenchmarks_Name").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.Name).HasMaxLength(200);

            entity.HasMany(d => d.TrustedHardware).WithMany(p => p.TrustedHardwareBenchmarks)
                .UsingEntity<Dictionary<string, object>>(
                    "BenchmarksTrustedHardwareRelation",
                    r => r.HasOne<TrustedHardware>().WithMany()
                        .HasForeignKey("TrustedHardwareId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_BenchmarksTrustedHardwareRel_TrustedHardware"),
                    l => l.HasOne<TrustedHardwareBenchmark>().WithMany()
                        .HasForeignKey("TrustedHardwareBenchmarkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_BenchmarksTrustedHardwareRel_TrustedHardwareBenchmarks"),
                    j =>
                    {
                        j.HasKey("TrustedHardwareBenchmarkId", "TrustedHardwareId");
                    });
        });

        modelBuilder.Entity<TrustedSoftware>(entity =>
        {
            entity.ToTable("TrustedSoftware");

            entity.HasIndex(e => new { e.Name, e.Version }, "AK_TrustedSoftware_Name_Version").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.Name).HasMaxLength(350);
            entity.Property(e => e.Version).HasMaxLength(100);
        });

        modelBuilder.Entity<TrustedSoftwareBenchmark>(entity =>
        {
            entity.HasIndex(e => e.Name, "AK_TrustedSoftwareBenchmarks_Name").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.Name).HasMaxLength(200);

            entity.HasMany(d => d.TrustedSoftware).WithMany(p => p.TrustedSoftwareBenchmarks)
                .UsingEntity<Dictionary<string, object>>(
                    "BenchmarksTrustedSoftwareRelation",
                    r => r.HasOne<TrustedSoftware>().WithMany()
                        .HasForeignKey("TrustedSoftwareId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_BenchmarksTrustedSoftRel_TrustedSoft"),
                    l => l.HasOne<TrustedSoftwareBenchmark>().WithMany()
                        .HasForeignKey("TrustedSoftwareBenchmarkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_BenchmarksTrustedSoftRel_TrustedSoftBenchmarks"),
                    j =>
                    {
                        j.HasKey("TrustedSoftwareBenchmarkId", "TrustedSoftwareId");
                    });
        });
    }

    /// <summary>
    /// Only for migrations.
    /// </summary>
    /// <param name="optionsBuilder">Options builder only for migrations.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configuration = new ConfigurationHelper();
        optionsBuilder.UseNpgsql(configuration.MainConnectionString);
    }
}