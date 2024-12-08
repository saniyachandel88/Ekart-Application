using Ekart_web_Application.DTO;

namespace Ekart_Application.IServices
{
    public interface IAuthService
    {
        UserWithToken AddUser(CreateUserDto createUserDto);
        string Login(LoginDto loginRequest);
    }
}
