using System.ComponentModel.DataAnnotations;

namespace Ekart_web_Application.DTO
{
    public class CategoryCreateDto
    {
        [Required(ErrorMessage = "Category Name is required")]
        public string CategoryName { get; set; }

        public string? Description { get; set; }

        // Handle picture as base64 string
        [Required(ErrorMessage = "Picture is required")]
        public string? Picture { get; set; }

        // Utility method to convert base64 string to byte array
        public byte[]? GetPictureBytes()
        {
            if (string.IsNullOrWhiteSpace(Picture))
                return null;

            // Remove the base64 prefix if present
            var base64Data = Picture.Contains(",")
                ? Picture.Split(',')[1]
                : Picture;

            try
            {
                return Convert.FromBase64String(base64Data);
            }
            catch (FormatException)
            {
                throw new ArgumentException("Invalid base64 string for picture");
            }
        }
    }
}
