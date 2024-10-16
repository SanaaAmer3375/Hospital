using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Models
{
    public class Doctor
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "You have to provide a valid full name.")]
        [MinLength(8, ErrorMessage = "Full name can't be less than 8 characters.")]
        [MaxLength(50, ErrorMessage = "Full name mustn't exceed 50 characters.")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [DataType(DataType.Time)]
        [DisplayName("Attendance Time")]
        public DateTime AttendanceTime { get; set; }

        [Required(ErrorMessage = "You have to provide a valid National Id.")]
        [MinLength(14, ErrorMessage = "National Id can't be less than 14 characters.")]
        [MaxLength(14, ErrorMessage = "National Id mustn't exceed 14 characters.")]
        [DisplayName("National Id")]
        public string NationalId { get; set; }

        [Required(ErrorMessage = "You have to provide a valid Address.")]
        [MinLength(2, ErrorMessage = "Address can't be less than 2 characters.")]
        [MaxLength(50, ErrorMessage = "Address mustn't exceed 50 characters.")]
        public string Address { get; set; }

        [DisplayName("Phone Number")]
        [RegularExpression("^01\\d{9}$", ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }

        [ValidateNever]

        public List<Patient> Patients { get; set; }

        [ValidateNever]
        public string ImagePath { get; set; }

        [ValidateNever]
        [NotMapped]
        [DisplayName("Image")]
        public IFormFile ImageFile { get; set; }
    }
}
