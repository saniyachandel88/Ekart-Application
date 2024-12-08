using Ekart_Web_App.Models;

namespace Ekart_web_Application.DTO
{
    public class UserWithToken
    {
        public User User { get; set; }
        public string Token { get; set; }
    }
}
