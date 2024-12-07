
//test
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using NetflixApi.Model;
using NetflixApi.Services;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using System.Threading.Tasks;
using System.Net;

namespace MongoWithDotNetAPI.Controllers
{
    [Route("api/UsersAuth")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserServices _userService;
        private readonly IConfiguration _configuration;
        protected APIResponse _response;
        private readonly string secretKey;

        public UserController(UserServices userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
            _response = new();
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }

        [HttpPost("signup")]
        [AllowAnonymous]
        public async Task<IActionResult> Signup([FromBody] RegistrationRequest newUser)
        {

            try
            {  // check if there is any existing users 
                var existingUser = await _userService.GetAsync(newUser.Email);
                if (existingUser != null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Username already exists");
                    return BadRequest(_response);
                }


                // Hashing the user's password before storing it
                //string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
                //newUser.Password = hashedPassword;

                //add the users 
                var user = await _userService.SignupAsync(newUser);
                if (user == null)
                {

                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Error while registering");
                    return BadRequest(_response);

                }
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok(_response);




                //var token = GenerateJwtToken(user);
                //return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            try
            { // input 
                var user = await _userService.LoginAsync(loginModel.Email, loginModel.Password);
                if (user == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Username or password is incorrect");
                    return BadRequest(_response);
                }

                //verify the hashed password 
                //bool isvalid = BCrypt.Net.BCrypt.Verify(loginModel.Password, user.Password);
                //if(!isvalid)
                //{
                //    return Unauthorized("Invalid username or passwordss.");
                //}


                var token = GenerateJwtToken(user);
                return Ok(new { Token = token });




            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        private string GenerateJwtToken(LocalUsers user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),

                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);


            return tokenHandler.WriteToken(token);
        }

    }
}

