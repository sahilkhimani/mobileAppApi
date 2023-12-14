﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTO
{
    public class UserDTO
    {

    }
    public class SendOTPDTO
    {
        public string UserEmail { get; set; }
    }
    public class LoginDTO
    {
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
    } 
    public class JWTTokenGeneration
    {
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public  string Token { get; set; }
    }

    public class SignupDTO
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
    }

    public class UpdateUserPasswordDTO
    {
        public string UserEmail { get; set; }
        public string Otp { get; set; }
        public string UserPassword { get; set; }
        public string UserConfirmPassword { get; set; }

    }

    public class OTPDTO
    {
        public string To { get; set; }
        public string From { get; set; }
        public string MessageBody { get; set; }
        public string Subject { get; set; }
        public string RandomCode { get; set; }
        public string Password { get; set; }
        public string Otp { get; set; }
    }
    public class ValidateOTPDTO
    {
        public string UserEmail { get; set; }
        public string Otp { get; set; }
    }


}
