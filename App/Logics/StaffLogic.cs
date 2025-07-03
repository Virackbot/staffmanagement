using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using App.Db;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using App.Logics;
using Domain.Interfaces;
using Domain.Enums;
using Domain.OutfaceModels;
using Domain.Exceptions;
using Sentry;

namespace App.Logics;

public class StaffLogic : MasterLogic
{
    public StaffLogic(StaffManagementDbContext db, IHttpContextScope current)
        : base(db, current)
    {
    }

    /// <summary>
    /// Create a new staff member
    /// </summary>
    public async Task<StaffResponseModel> CreateAsync(CreateStaffRequestModel model)
    {
        try
        {
            // Check if staff ID already exists
            var existingStaff = await _db.Staff.FirstOrDefaultAsync(x => x.StaffId == model.StaffId);
            if (existingStaff != null)
            {
                throw new LogicException($"Staff with ID '{model.StaffId}' already exists.");
            }

            var staff = new Staff
            {
                StaffId = model.StaffId,
                FullName = model.FullName,
                Birthday = model.Birthday,
                Gender = model.Gender,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _db.Staff.AddAsync(staff);
            await _db.SaveChangesAsync();

            return MapToResponseModel(staff);
        }
        catch (Exception ex)
        {
            SentrySdk.CaptureException(ex);
            throw;
        }
    }

    /// <summary>
    /// Get staff by ID
    /// </summary>
    public async Task<StaffResponseModel?> GetByIdAsync(string staffId)
    {
        var staff = await _db.Staff.FirstOrDefaultAsync(x => x.StaffId == staffId);
        return staff != null ? MapToResponseModel(staff) : null;
    }

    /// <summary>
    /// Update existing staff
    /// </summary>
    public async Task<StaffResponseModel> UpdateAsync(string staffId, UpdateStaffRequestModel model)
    {
        try
        {
            var staff = await _db.Staff.FirstOrDefaultAsync(x => x.StaffId == staffId);
            if (staff == null)
            {
                throw new LogicException($"Staff with ID '{staffId}' not found.");
            }

            staff.FullName = model.FullName;
            staff.Birthday = model.Birthday;
            staff.Gender = model.Gender;
            staff.UpdatedAt = DateTime.UtcNow;

            _db.Staff.Update(staff);
            await _db.SaveChangesAsync();

            return MapToResponseModel(staff);
        }
        catch (Exception ex)
        {
            SentrySdk.CaptureException(ex);
            throw;
        }
    }

    /// <summary>
    /// Delete staff by ID
    /// </summary>
    public async Task<bool> DeleteAsync(string staffId)
    {
        try
        {
            var staff = await _db.Staff.FirstOrDefaultAsync(x => x.StaffId == staffId);
            if (staff == null)
            {
                return false;
            }

            _db.Staff.Remove(staff);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            SentrySdk.CaptureException(ex);
            throw;
        }
    }

    /// <summary>
    /// Search staff with advanced filters
    /// </summary>
    public async Task<List<StaffResponseModel>> SearchAsync(StaffSearchRequestModel searchModel)
    {
        try
        {
            var query = _db.Staff.AsNoTracking();

            // Filter by Staff ID
            if (!string.IsNullOrEmpty(searchModel.StaffId))
            {
                query = query.Where(x => x.StaffId.Contains(searchModel.StaffId));
            }

            // Filter by Full Name
            if (!string.IsNullOrEmpty(searchModel.FullName))
            {
                query = query.Where(x => x.FullName.Contains(searchModel.FullName));
            }

            // Filter by Gender
            if (searchModel.Gender.HasValue)
            {
                query = query.Where(x => x.Gender == searchModel.Gender.Value);
            }

            // Filter by Birthday Range
            if (searchModel.BirthdayFrom.HasValue)
            {
                query = query.Where(x => x.Birthday >= searchModel.BirthdayFrom.Value);
            }

            if (searchModel.BirthdayTo.HasValue)
            {
                query = query.Where(x => x.Birthday <= searchModel.BirthdayTo.Value);
            }

            // Filter by Age Range (calculated)
            if (searchModel.MinAge.HasValue || searchModel.MaxAge.HasValue)
            {
                var today = DateOnly.FromDateTime(DateTime.Today);

                if (searchModel.MinAge.HasValue)
                {
                    var maxBirthDateForMinAge = today.AddYears(-searchModel.MinAge.Value);
                    query = query.Where(x => x.Birthday <= maxBirthDateForMinAge);
                }

                if (searchModel.MaxAge.HasValue)
                {
                    var minBirthDateForMaxAge = today.AddYears(-searchModel.MaxAge.Value - 1);
                    query = query.Where(x => x.Birthday > minBirthDateForMaxAge);
                }
            }

            var staffList = await query.OrderBy(x => x.StaffId).ToListAsync();
            return staffList.Select(MapToResponseModel).ToList();
        }
        catch (Exception ex)
        {
            SentrySdk.CaptureException(ex);
            throw;
        }
    }

    /// <summary>
    /// Get all staff
    /// </summary>
    public async Task<List<StaffResponseModel>> GetAllAsync()
    {
        try
        {
            var staffList = await _db.Staff.AsNoTracking().OrderBy(x => x.StaffId).ToListAsync();
            return staffList.Select(MapToResponseModel).ToList();
        }
        catch (Exception ex)
        {
            SentrySdk.CaptureException(ex);
            throw;
        }
    }

    /// <summary>
    /// Map Staff entity to response model
    /// </summary>
    private StaffResponseModel MapToResponseModel(Staff staff)
    {
        return new StaffResponseModel
        {
            StaffId = staff.StaffId,
            FullName = staff.FullName,
            Birthday = staff.Birthday,
            Gender = staff.Gender,
            CreatedAt = staff.CreatedAt,
            UpdatedAt = staff.UpdatedAt
        };
    }
}