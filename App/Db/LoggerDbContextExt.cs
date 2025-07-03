using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace App.Db;

public static  class StaffManagementDbContextExt
{
    public static void ApplyAllConfigurations(this ModelBuilder modelBuilder)
    {
        var applyConfigurationMethodInfo = modelBuilder
            .GetType()
            .GetMethods(BindingFlags.Instance | BindingFlags.Public)
            .First(m => m.Name.Equals("ApplyConfiguration", StringComparison.OrdinalIgnoreCase));
        _ = typeof(StaffManagementDbContext).Assembly
             .GetTypes()
             .Select(t => (t, i: t.GetInterfaces().FirstOrDefault(i => i.Name.Equals(typeof(IEntityTypeConfiguration<>).Name, StringComparison.Ordinal))))
             .Where(it => it.i != null)
             .Select(it => (et: it!.i!.GetGenericArguments()[0], cfgObj: Activator.CreateInstance(it.t)))
             .Select(it => applyConfigurationMethodInfo.MakeGenericMethod(it.et).Invoke(modelBuilder, new[] { it.cfgObj }))
             .ToList();

        modelBuilder.HasPostgresExtension("pg_stat_statements").HasPostgresExtension("pldbgapi");
    }
}