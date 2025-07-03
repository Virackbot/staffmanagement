using System.Net;
using System.Net.Http.Json;
using System.Text;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using App.Db;
using Domain.Models;
using Domain.OutfaceModels;
using StaffManagement.IntegrationTests.Infrastructure;

namespace StaffManagement.IntegrationTests.Controllers;

public class StaffControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>, IDisposable
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;
    private readonly IServiceScope _scope;
    private readonly StaffManagementDbContext _context;

    public StaffControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _scope = _factory.Services.CreateScope();
        _context = _scope.ServiceProvider.GetRequiredService<StaffManagementDbContext>();
    }

    public void Dispose()
    {
        _context?.Dispose();
        _scope?.Dispose();
        _client?.Dispose();
        GC.SuppressFinalize(this);
    }

    [Fact]
    public async Task CreateStaff_ValidData_ShouldReturnCreatedStaff()
    {
        // Arrange
        var createRequest = new CreateStaffRequestModel
        {
            StaffId = "ST001",
            FullName = "John Doe",
            Birthday = new DateOnly(1990, 1, 1),
            Gender = 1
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/staff", createRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var responseContent = await response.Content.ReadAsStringAsync();
        var staffResponse = JsonConvert.DeserializeObject<StaffResponseModel>(responseContent);

        staffResponse.Should().NotBeNull();
        staffResponse!.StaffId.Should().Be(createRequest.StaffId);
        staffResponse.FullName.Should().Be(createRequest.FullName);
        staffResponse.Birthday.Should().Be(createRequest.Birthday);
        staffResponse.Gender.Should().Be(createRequest.Gender);

        // Verify data was persisted
        var savedStaff = await _context.Staff.FindAsync(createRequest.StaffId);
        savedStaff.Should().NotBeNull();
        savedStaff!.FullName.Should().Be(createRequest.FullName);
    }

    [Fact]
    public async Task CreateStaff_DuplicateStaffId_ShouldReturnBadRequest()
    {
        // Arrange - Create initial staff
        var existingStaff = new Staff
        {
            StaffId = "ST002",
            FullName = "Existing Staff",
            Birthday = new DateOnly(1985, 5, 5),
            Gender = 2,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Staff.Add(existingStaff);
        await _context.SaveChangesAsync();

        var duplicateRequest = new CreateStaffRequestModel
        {
            StaffId = "ST002", // Same ID
            FullName = "New Staff",
            Birthday = new DateOnly(1990, 1, 1),
            Gender = 1
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/staff", duplicateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetStaff_ExistingId_ShouldReturnStaff()
    {
        // Arrange
        var staff = new Staff
        {
            StaffId = "ST003",
            FullName = "Jane Smith",
            Birthday = new DateOnly(1992, 3, 15),
            Gender = 2,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Staff.Add(staff);
        await _context.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync($"/api/staff/{staff.StaffId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseContent = await response.Content.ReadAsStringAsync();
        var staffResponse = JsonConvert.DeserializeObject<StaffResponseModel>(responseContent);

        staffResponse.Should().NotBeNull();
        staffResponse!.StaffId.Should().Be(staff.StaffId);
        staffResponse.FullName.Should().Be(staff.FullName);
        staffResponse.Birthday.Should().Be(staff.Birthday);
        staffResponse.Gender.Should().Be(staff.Gender);
    }

    [Fact]
    public async Task GetStaff_NonExistingId_ShouldReturnNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/staff/NONEXISTENT");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateStaff_ExistingStaff_ShouldReturnUpdatedStaff()
    {
        // Arrange
        var staff = new Staff
        {
            StaffId = "ST004",
            FullName = "Original Name",
            Birthday = new DateOnly(1990, 1, 1),
            Gender = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Staff.Add(staff);
        await _context.SaveChangesAsync();

        var updateRequest = new UpdateStaffRequestModel
        {
            FullName = "Updated Name",
            Birthday = new DateOnly(1990, 2, 2),
            Gender = 2
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/staff/{staff.StaffId}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseContent = await response.Content.ReadAsStringAsync();
        var staffResponse = JsonConvert.DeserializeObject<StaffResponseModel>(responseContent);

        staffResponse.Should().NotBeNull();
        staffResponse!.StaffId.Should().Be(staff.StaffId);
        staffResponse.FullName.Should().Be(updateRequest.FullName);
        staffResponse.Birthday.Should().Be(updateRequest.Birthday);
        staffResponse.Gender.Should().Be(updateRequest.Gender);

        // Verify data was persisted
        var updatedStaff = await _context.Staff.FindAsync(staff.StaffId);
        updatedStaff!.FullName.Should().Be(updateRequest.FullName);
        updatedStaff.Birthday.Should().Be(updateRequest.Birthday);
        updatedStaff.Gender.Should().Be(updateRequest.Gender);
    }

    [Fact]
    public async Task UpdateStaff_NonExistingStaff_ShouldReturnNotFound()
    {
        // Arrange
        var updateRequest = new UpdateStaffRequestModel
        {
            FullName = "Updated Name",
            Birthday = new DateOnly(1990, 2, 2),
            Gender = 2
        };

        // Act
        var response = await _client.PutAsJsonAsync("/api/staff/NONEXISTENT", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteStaff_ExistingStaff_ShouldReturnNoContent()
    {
        // Arrange
        var staff = new Staff
        {
            StaffId = "ST005",
            FullName = "To Be Deleted",
            Birthday = new DateOnly(1990, 1, 1),
            Gender = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Staff.Add(staff);
        await _context.SaveChangesAsync();

        // Act
        var response = await _client.DeleteAsync($"/api/staff/{staff.StaffId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify data was deleted
        var deletedStaff = await _context.Staff.FindAsync(staff.StaffId);
        deletedStaff.Should().BeNull();
    }

    [Fact]
    public async Task DeleteStaff_NonExistingStaff_ShouldReturnNotFound()
    {
        // Act
        var response = await _client.DeleteAsync("/api/staff/NONEXISTENT");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task SearchStaff_WithFilters_ShouldReturnFilteredResults()
    {
        // Arrange - Create test data
        var staff1 = new Staff
        {
            StaffId = "ST006",
            FullName = "Male Staff 1",
            Birthday = new DateOnly(1990, 1, 1),
            Gender = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var staff2 = new Staff
        {
            StaffId = "ST007",
            FullName = "Female Staff 1",
            Birthday = new DateOnly(1995, 6, 15),
            Gender = 2,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Staff.AddRange(staff1, staff2);
        await _context.SaveChangesAsync();

        // Act - Search for male staff
        var response = await _client.GetAsync("/api/staff/search?gender=1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseContent = await response.Content.ReadAsStringAsync();
        var searchResults = JsonConvert.DeserializeObject<List<StaffResponseModel>>(responseContent);

        searchResults.Should().NotBeNull();
        searchResults!.Should().HaveCount(1);
        searchResults[0].StaffId.Should().Be("ST006");
        searchResults[0].Gender.Should().Be(1);
    }
}
