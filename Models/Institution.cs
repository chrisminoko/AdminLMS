using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BackEnd.Models
{
    public class Institution
    {
        [Key]
        public int InstitutionID { get; set; }
        [Required]
        [RegularExpression(pattern: @"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Numbers and special characters are not allowed.")]
        [StringLength(maximumLength: 228, ErrorMessage = "Full Name must be atleast 3 characters long", MinimumLength = 3)]
        public string FullName { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }

        [Required]
        [Display(Name = "Email")]
        [DataType(dataType: DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        public string UserId { get; set; }
        [Display(Name = "Uploaded File")]
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
        public byte[] UserPhoto { get; set; }
    }
}