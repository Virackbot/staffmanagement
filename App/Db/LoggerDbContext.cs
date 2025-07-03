using App.Db;
using Microsoft.EntityFrameworkCore;

namespace App.Db;

public class ReadOnlyStaffManagementDbContext : StaffManagementDbContext
{
    public ReadOnlyStaffManagementDbContext()
    {

    }

    public ReadOnlyStaffManagementDbContext(DbContextOptions<ReadOnlyStaffManagementDbContext> options)
        : base(options)
    {
        ChangeTracker.AutoDetectChangesEnabled = false;
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }
}