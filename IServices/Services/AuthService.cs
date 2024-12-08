using AutoMapper;
using Ekart_Web_App.Data;
using Ekart_Web_App.Models;
using Ekart_web_Application.DTO;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ekart_Application.IServices.Services
{
    public class AuthService:IAuthService
    {
        private readonly EkartProjectContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public AuthService(EkartProjectContext dbcontextAuth, IConfiguration configuration, IMapper mapper)

        {
            _context = dbcontextAuth;
            _configuration = configuration;
            _mapper = mapper;

        }
        public UserWithToken AddUser(CreateUserDto createUserDto)
        {
            // Map DTO to the User entity
            var user = _mapper.Map<User>(createUserDto);

            // Hash the user's password using BCrypt
            user.Password = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);  // Hashing password using BCrypt

            // Save the user to the database
            var addedUser = _context.Users.Add(user);
            _context.SaveChanges();

            // Generate the token (use plain password for validation)
            var loginCredentials = new LoginDto { UserName = user.UserName, Password = createUserDto.Password };
            var token = Login(loginCredentials);

            // Return the user with the generated token
            return new UserWithToken
            {
                User = addedUser.Entity,
                Token = token
            };
        }
        public string Login(LoginDto loginRequest)
        { // Check if username/email and password are provided
            if (string.IsNullOrEmpty(loginRequest.UserName) || string.IsNullOrEmpty(loginRequest.Password))
            {
                throw new Exception("Invalid credentials: username or password is null.");
            }

            // Attempt to find the user by username
            User? user = _context.Users.SingleOrDefault(s => s.UserName == loginRequest.UserName);

            // Attempt to find the supplier by email
            Supplier? supplier = _context.Suppliers.SingleOrDefault(s => s.Email == loginRequest.UserName);

            // If neither user nor supplier is found, throw exception
            if (user == null && supplier == null)
            {
                throw new Exception("User or Supplier not found.");
            }

            // Verify password for user (hashed password)
            if (user != null && !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
            {
                throw new Exception("Invalid credentials for user.");
            }

            // Verify password for supplier (plaintext password)
            if (supplier != null && supplier.Password != loginRequest.Password)
            {
                throw new Exception("Invalid credentials for supplier.");
            }

            // JWT configuration values
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = jwtSettings["Key"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var subject = jwtSettings["Subject"];

            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience) || string.IsNullOrEmpty(subject))
            {
                throw new ArgumentNullException("JWT configuration is missing some required values.");
            }

            // Define claims for user or supplier
            var claims = new List<Claim>();

            if (user != null)
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Sub, subject));
                claims.Add(new Claim("Id", user.Id.ToString()));
                claims.Add(new Claim("UserName", user.UserName ?? string.Empty));
                claims.Add(new Claim("Role", user.Role?.ToLower() ?? "unknown"));
            }
            else if (supplier != null)
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Sub, subject));
                claims.Add(new Claim("Id", supplier.SupplierId.ToString()));
                claims.Add(new Claim("Email", supplier.Email ?? string.Empty)); // Use email for supplier
                claims.Add(new Claim("Role", "supplier")); // Assuming "Supplier" role for supplier
            }

            // Generate the JWT token
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: creds
            );

            // Return the token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
