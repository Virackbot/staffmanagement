using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.Json.Serialization;
using App.Db;
using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Domain.Interfaces;

namespace App.Logics;

/// <summary>
/// Base logic class providing core database and validation functionality
/// </summary>
public class MasterLogic
{
    protected readonly StaffManagementDbContext _db;
    protected readonly IHttpContextScope _current;

    public MasterLogic(StaffManagementDbContext db, IHttpContextScope current)
    {
        _db = db;
        _current = current;
    }

    public async Task<T> WrapAsync<T>(Func<Task<T>> action, int timeout = 10 * 60)
        => await _db.WrapAsync(action, timeout);

    public TLogic GetLogic<TLogic>() where TLogic : class
    {
        return _current.Provider.GetService<TLogic>()
            ?? throw new Exception($"Service {nameof(TLogic)} not registered.");
    }

    protected static List<string> ValidateRequest<T>(T payload, List<string> fieldNames)
    {
        var missingFields = new List<string>();

        foreach (var fieldName in fieldNames)
        {
            var property = typeof(T).GetProperty(fieldName);
            if (property != null)
            {
                var jsonPropertyAttribute = property.GetCustomAttribute<JsonPropertyNameAttribute>();
                string displayName = jsonPropertyAttribute?.Name ?? property.Name;

                var value = property.GetValue(payload);
                if (IsRequiredField(value))
                {
                    missingFields.Add(displayName);
                }
            }
        }

        return missingFields;
    }

    private static bool IsRequiredField(object? value)
    {
        return value switch
        {
            null => true,
            string s => string.IsNullOrEmpty(s),
            DateTime dt => dt == default || dt == DateTime.MinValue,
            decimal d => d == 0m,
            double d => d == 0d,
            float f => f == 0f,
            _ => false
        };
    }
}