using System.ComponentModel.DataAnnotations;

namespace Domain.OutfaceModels
{
    public class CreateStaffRequestModel
    {
        [Required]
        [MaxLength(8)]
        public string StaffId { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public DateOnly Birthday { get; set; }

        [Required]
        [Range(1, 2, ErrorMessage = "Gender must be 1 (Male) or 2 (Female)")]
        public int Gender { get; set; }
    }

    public class UpdateStaffRequestModel
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public DateOnly Birthday { get; set; }

        [Required]
        [Range(1, 2, ErrorMessage = "Gender must be 1 (Male) or 2 (Female)")]
        public int Gender { get; set; }
    }

    public class StaffSearchRequestModel
    {
        public string? StaffId { get; set; }
        public string? FullName { get; set; }
        public int? Gender { get; set; }
        public DateOnly? BirthdayFrom { get; set; }
        public DateOnly? BirthdayTo { get; set; }
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
    }

    public class StaffResponseModel
    {
        public string StaffId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public DateOnly Birthday { get; set; }
        public int Gender { get; set; }
        public string GenderDisplay => Gender == 1 ? "Male" : "Female";
        public int Age => DateTime.Today.Year - Birthday.Year - (DateTime.Today.DayOfYear < Birthday.DayOfYear ? 1 : 0);
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}