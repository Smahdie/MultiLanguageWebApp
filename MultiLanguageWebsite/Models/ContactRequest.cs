using System.ComponentModel.DataAnnotations;

namespace MultiLanguageWebsite.Models
{
    public class ContactRequest
    {
        [Display(Name = "Name")]
        [MaxLength(50)]
        [RegularExpression(@"[\u0600-\u06FFa-zA-Z\s]+", ErrorMessage = "Use only Persian or English letters.")]
        public string Name { get; set; }

        [Display(Name = "Phone Number")]
        [MaxLength(11)]
        [RegularExpression("[(0-9)(۰-۹)]*", ErrorMessage = "{0} must contain maximum 11 digits.")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Message text")]
        [Required(ErrorMessage = "{0} is required")]
        [RegularExpression(@"(\n)*[-_\u0600-\u06FFa-zA-Z0-9۰-۹\s]+(\n|-_\u0600-\u06FFa-zA-Z0-9۰-۹\s)*", ErrorMessage = "You have not entered {0} or it contains forbidden characters.")]
        [MaxLength(200)]
        public string Message { get; set; }
    }
}
