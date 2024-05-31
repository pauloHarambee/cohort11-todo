using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models.DTOs
{
    public class RegisterDTO
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string UserName {  get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber {  get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set;}
    }
}
