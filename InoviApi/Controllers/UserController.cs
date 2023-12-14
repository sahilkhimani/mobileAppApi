using DTO;
using DTO.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using InoviWebApi.Helper;
using InoviDataTransferObject.DTO;

namespace InoviApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IRepositoryWrapper _reposWrapper;
        private readonly IConfiguration _configuration;
        ResponseDTO resp = new ResponseDTO();
        public UserController(IRepositoryWrapper reposWrapper, IConfiguration configuration)
        {
            _reposWrapper = reposWrapper;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> SignupUser([FromBody] SignupDTO req)
        {
            try
            {
                if (req == null || string.IsNullOrWhiteSpace(req.UserEmail) || string.IsNullOrWhiteSpace(req.UserPassword) || string.IsNullOrWhiteSpace(req.Name) || string.IsNullOrWhiteSpace(req.Username))
                {
                    return StatusCode(StatusCodes.Status204NoContent, "Values cannot be Null!");
                }
                else
                {
                    var result = await _reposWrapper.UserRepo.SignupUser(req);
                    if (result == false)
                    {
                        return StatusCode(StatusCodes.Status208AlreadyReported, "User with the same email already exist!");
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status201Created, "Signup Successfully!");
                    }
                }

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Inserting Data!");
            }
        }

        [HttpPost]
        public async Task<IActionResult> LoginUser([FromBody] LoginDTO req)
        {
            try
            {
                if (req == null || string.IsNullOrWhiteSpace(req.UserEmail) || string.IsNullOrWhiteSpace(req.UserPassword))
                {
                    return StatusCode(StatusCodes.Status204NoContent, "Values cannot be Null!");
                }
                else
                {
                    var result = await _reposWrapper.UserRepo.LoginUser(req);
                    if (result == false)
                    {
                        return StatusCode(StatusCodes.Status401Unauthorized, "User Not Authenticate!");
                    }
                    else
                    {
                        string Token = GenerateJWT(req);
                        return StatusCode(StatusCodes.Status200OK, Token);
                    }
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Login Data!");
            }

        }

        [HttpPost]
        public async Task<IActionResult> SendOTP([FromBody] SendOTPDTO req)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(req.UserEmail))
                {
                    return StatusCode(StatusCodes.Status204NoContent, "Value cannot be Null!");
                }
                else
                {
                    var result = await _reposWrapper.UserRepo.SendOTP(req);
                    if (result == null)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, "Email not Exist!");
                    }
                    else
                    {
                        OTPDTO otp = new OTPDTO();
                        otp.To = result;
                        otp.From = _configuration["Gmail:Email"];
                        otp.Password = _configuration["Gmail:Password"];
                        otp.Otp = (new Random()).Next(9999).ToString();
                        otp.MessageBody = "Your OTP is:" + otp.Otp;
                        otp.Subject = "Password Resetting Code";

                        MailMessage mailMessage = new MailMessage();
                        mailMessage.To.Add(otp.To);
                        mailMessage.From = new MailAddress(otp.From);
                        mailMessage.Body = otp.MessageBody;
                        mailMessage.Subject = otp.Subject;

                        SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                        smtp.EnableSsl = true;
                        smtp.Port = 587;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.Credentials = new NetworkCredential(otp.From, otp.Password);
                        await smtp.SendMailAsync(mailMessage);

                        //HttpContext.Session.SetString("code", otp.Otp);

                        return StatusCode(StatusCodes.Status200OK, "OTP Send Succesfully '" + otp.Otp);
                    }
                }

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Sending Data!");
            }
        }

        [HttpPost]
        public async Task<ActionResult> ValidateOTP([FromBody] ValidateOTPDTO req)
        {
            try
            {
                string val = HttpContext.Session.GetString("code");
                var result = await _reposWrapper.UserRepo.ValidateOTP(req);
                if (result == false)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "User Not Found!");
                }

                else
                {
                    if (req.Otp == val)
                    {
                        return StatusCode(StatusCodes.Status200OK, "OTP Matched!");
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status404NotFound, "OTP Not Matched!");
                    }
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Validating Data!");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserPassword([FromBody] UpdateUserPasswordDTO req)
        {
            try
            {
                if (req == null || string.IsNullOrWhiteSpace(req.UserEmail) || string.IsNullOrWhiteSpace(req.Otp) || string.IsNullOrWhiteSpace(req.UserPassword) || string.IsNullOrWhiteSpace(req.UserConfirmPassword))
                {
                    return StatusCode(StatusCodes.Status204NoContent, "Values cannot be Null!");
                }
                else
                {
                    if (req.UserPassword == req.UserConfirmPassword)
                    {
                        var result = await _reposWrapper.UserRepo.UpdateUserPassword(req);
                        if (result == false)
                        {
                            return StatusCode(StatusCodes.Status404NotFound, "User Not Found!");
                        }
                        else
                        {
                            return StatusCode(StatusCodes.Status200OK, "Password Updated Successfully!");
                        }
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status404NotFound, "Password Not Matched!");
                    }
                }
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error Updating Data!");
            }
        }


        private string GenerateJWT(LoginDTO req)
        {

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
         new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
         new Claim(JwtRegisteredClaimNames.Email, req.UserEmail),
         new Claim(JwtRegisteredClaimNames.Sid, req.UserPassword),
         new Claim("Date", DateTime.Now.ToString()),
         };

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
              _configuration["Jwt:Audiance"],
              claims,
              expires: DateTime.Now.AddMinutes(2),
              signingCredentials: credentials);
            string Token = new JwtSecurityTokenHandler().WriteToken(token);
            return Token;
        }

    }
}
