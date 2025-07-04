using Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace App.Configs;

public class StaffConfig : IEntityTypeConfiguration<Staff>
{
    public void Configure(EntityTypeBuilder<Staff> builder)
    {
        builder.ToTable("staff");

        // Primary Key
        builder.HasKey(e => e.StaffId);

        // Properties configuration
        builder.Property(e => e.StaffId)
            .HasColumnName("staff_id")
            .HasMaxLength(8)
            .IsRequired();

        builder.Property(e => e.FullName)
            .HasColumnName("full_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.Birthday)
            .HasColumnName("birthday")
            .HasColumnType("date")
            .IsRequired();

        builder.Property(e => e.Gender)
            .HasColumnName("gender")
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp")
            .IsRequired(); builder.Property(e => e.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("timestamp")
            .IsRequired();

        // Temporarily removed indexes to fix migration issue
        // Will add back in a separate migration
        // builder.HasIndex(e => e.FullName)
        //     .HasDatabaseName("IX_Staff_FullName");

        // builder.HasIndex(e => e.Birthday)
        //     .HasDatabaseName("IX_Staff_Birthday");
    }
}
