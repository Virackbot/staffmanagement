using App.Db;
using Domain.Helpers;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Data.Common;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Transactions;

namespace App.Db;

public class StaffManagementDbContext : DbContext
{
    public StaffManagementDbContext()
    {
    }

    public StaffManagementDbContext(DbContextOptions<StaffManagementDbContext> options)
        : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    protected StaffManagementDbContext(DbContextOptions opt) : base(opt) { }

    public async Task<T> WrapAsync<T>(Func<Task<T>> action, int timeout = 10 * 60)
    {
        Database.SetCommandTimeout(TimeSpan.FromSeconds(timeout));
        var strategy = Database.CreateExecutionStrategy();
        return await strategy.Execute(async () =>
        {
            using var tran = new TransactionScope(
                scopeOption: TransactionScopeOption.Required,
                transactionOptions: new TransactionOptions()
                {
                    IsolationLevel = IsolationLevel.ReadCommitted,
                    Timeout = TimeSpan.FromSeconds(timeout)
                },
                asyncFlowOption: TransactionScopeAsyncFlowOption.Enabled);

            if (Database.GetDbConnection().State != System.Data.ConnectionState.Open)
            {
                Database.GetDbConnection().Open();
            }

            T t = await action.Invoke();
            tran.Complete();
            return t;
        });
    }

    // Staff table
    public DbSet<Staff> Staff => Set<Staff>();

    // Note: BankApi removed as it's not part of Staff Management requirements
    // If needed for future integration, add back with proper separation of concerns

    // end of database table 
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply all entity configurations from the Configs folder
        modelBuilder.ApplyAllConfigurations();
    }


    public async Task<object?> ExecuteScalarAsync(string Sql, int connectionTimeOut = 0)
    {
        object? value;
        var cn = Database.GetDbConnection();
        using (var cmd = cn.CreateCommand())
        {
            if (cn.State == System.Data.ConnectionState.Closed) { cn.Open(); }
            cmd.CommandText = Sql;
            if (connectionTimeOut != 0)
            {
                cmd.CommandTimeout = connectionTimeOut;
            }
            value = await cmd.ExecuteScalarAsync();
        }
        if (cn.State == System.Data.ConnectionState.Open) { cn.Close(); }
        return value;
    }

    public async Task<T> ExecuteRawSqlQuery<T>(string Sql, int connectionTimeOut = 0) where T : new()
    {
        var jsonResult = await ExecuteScalarAsync(Sql, connectionTimeOut);
        var result = new T();
        if (jsonResult is not null)
        {
            result = JsonHelper.DeserializeObject<T>(jsonResult.ToString());
        }

        return result ?? new T();
    }
}