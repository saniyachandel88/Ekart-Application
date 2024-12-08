using System.ComponentModel.DataAnnotations;

namespace Ekart_web_Application.DTO
{
    public class PatchCustomerDtoT
    {
        [Required(ErrorMessage = "Region is required.")]
        [StringLength(100, ErrorMessage = "Region can't be longer than 100 characters.")]
        public string Region { get; set; }
    }
}
