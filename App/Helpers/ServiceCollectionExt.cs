using App.Db;
using App.Logics;
using Domain.OutfaceModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace App.Helpers;

public static class ServiceCollectionExt
{
    //public static IServiceCollection InjectAllLogics(this IServiceCollection services, Type? type = null)
    //{
    //    Type classType = type ?? typeof(MasterLogic);
    //    foreach (var logic in classType.Assembly
    //                                .GetTypes()
    //                                .Where(x => x.Name.EndsWith("Logic")))
    //    {
    //        services.AddTransient(logic);
    //    }

    //    //services.AddTransient(typeof(IInquiryService<,>), typeof(InquiryService<,>));
    //    //services.AddTransient(typeof(ISingleOfInquiryService<>), typeof(SingleOfInquiryService<>));

    //    return services.AddTransient<BankGateWayClient>();
    //}

    public static IServiceCollection AddDbContext(
        this IServiceCollection services,
        DbConnection db,
        bool applyReadOnlyCtx = true)
    {
        services.AddDbContextPool<StaffManagementDbContext>(opt => opt.ApplyDbContextOptions(db.PrimaryConnection));
        if (applyReadOnlyCtx)
        {
            services.AddDbContextPool<ReadOnlyStaffManagementDbContext>(opt => opt.ApplyDbContextOptions(db.ReadOnlyConnection));
        }

        // Add logging
        //services.AddLogging(builder =>
        //{
        //    builder.AddConsole();
        //});

        return services;
    }

    public static DbContextOptionsBuilder ApplyDbContextOptions(
        this DbContextOptionsBuilder builder, string connectString)
    {
        return builder.UseNpgsql(connectString, npgsqlOptionsAction: sql =>
        {
            sql.CommandTimeout(DbOptionConst.DbCommandTimeout);
            sql.EnableRetryOnFailure(DbOptionConst.DbMaxRetryCount, DbOptionConst.DbRetryDelay, null);
        });
    }

    public static void ApplyDbMigration(this IServiceProvider service, bool isAutoMigrate)
    {
        if (isAutoMigrate)
        {
            using var scope = service.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<StaffManagementDbContext>();
            context.Database.Migrate();
        }
    }
}

