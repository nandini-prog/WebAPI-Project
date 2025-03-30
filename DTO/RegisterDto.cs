﻿namespace SmartTaskApi.DTO
{
    public class RegisterDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Role { get; set; }  // 🔹 Accept role during registration
    }
}
