using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ReactTraining2023.Data.Models.Base;

namespace ReactTraining2023.Data.Models;

public partial class MycosReact2023TrainingContext : DbContext
{
    public MycosReact2023TrainingContext()
    {
    }

    public MycosReact2023TrainingContext(DbContextOptions<MycosReact2023TrainingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AppScore> AppScores { get; set; }

    public override int SaveChanges()
    {
        ApplyDecorationToEntity();
        return base.SaveChanges();
    }

    private void ApplyDecorationToEntity()
    {
        var allEntityBaseModels = ChangeTracker.Entries().Where(e => e.Entity is EntityBase).ToList();
        foreach (var m in allEntityBaseModels)
        {
            if (m.State == EntityState.Added) {
                ((EntityBase)m.Entity).CreatedDate = DateTime.UtcNow;
            }
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppScore>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AppScore__3214EC07AE2023C6");

            entity.ToTable("AppScore");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Ip)
                .HasMaxLength(15)
                .HasColumnName("IP");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.ProjectName).HasMaxLength(255);
            entity.Property(e => e.Score).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.TotalTime).HasColumnType("time(2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
