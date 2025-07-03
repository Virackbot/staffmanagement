using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.Json.Serialization;
using App.Db;
using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Domain.Interfaces;

namespace App.Logics;

public class TDbLogic<T> : MasterLogic where T : TBaseModel<string>
{
    protected readonly bool _hasActiveColumn;

    public TDbLogic(StaffManagementDbContext db, IHttpContextScope current) : base(db, current)
    {
        _hasActiveColumn = typeof(T).GetProperties().Any(x => x.Name == "IsActive");
    }

    public virtual IQueryable<T> All()
        => _db.Set<T>().Where(x => !_hasActiveColumn || x.IsActive == true).AsNoTracking();

    public virtual IQueryable<T> AllWithInactive()
        => _db.Set<T>().AsNoTracking();

    public virtual async Task<T?> FindAsync(string? id)
        => id == null ? default : await _db.Set<T>().FindAsync(id);

    public virtual async Task<T> AddOrUpdateAsync(T entity)
    {
        string? id = entity is BaseModel baseModel ? baseModel.Id : null;
        return await (await FindAsync(id!) == null ? AddAsync(entity) : UpdateAsync(entity));
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        if (await IsExistsAsync(entity))
            throw new Exception("Entity already exists");

        if (entity is BaseModel baseModel && string.IsNullOrEmpty(baseModel.Id))
            baseModel.Id = Guid.NewGuid().ToString();

        entity.CreatedBy = _current?.FullName ?? "-";
        entity.CreatedDate = DateTime.Now;

        if (_hasActiveColumn)
            SetActive(entity, true);

        await _db.Set<T>().AddAsync(entity);
        await _db.SaveChangesAsync();

        return entity;
    }

    public virtual async Task<T> UpdateAsync(T entity)
    {
        if (await IsExistsAsync(entity))
            throw new Exception("Entity already exists");

        //entity.UpdatedBy = _current?.FullName ?? "-";
        //entity.UpdatedDate = DateTime.Now;

        _db.Entry(entity).State = EntityState.Modified;
        await _db.SaveChangesAsync();

        return entity;
    }

    public virtual async Task<T> RemoveAsync(string id)
    {
        var entity = await FindAsync(id) ?? throw new Exception("Entity not found");
        return await RemoveAsync(entity);
    }

    public virtual async Task<T> RemoveAsync(T entity)
    {
        if (_hasActiveColumn)
        {
            SetActive(entity, false);
            _db.Entry(entity).State = EntityState.Modified;
        }
        else
        {
            _db.Set<T>().Remove(entity);
        }

        await _db.SaveChangesAsync();
        return entity;
    }

    public virtual Task<bool> IsExistsAsync(T entity) => Task.FromResult(false);

    private void SetActive(T obj, bool active)
    {
        if (!_hasActiveColumn) return;
        var property = typeof(T).GetProperties().FirstOrDefault(x => x.Name == "IsActive");
        property?.SetValue(obj, active);
    }
}
