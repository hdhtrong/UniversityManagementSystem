﻿namespace AuthService.API.Models
{
    public class ChangePasswordDto
    {
        public required string Username { get; set; }
        public required string CurrentPassword { get; set; }
        public required string NewPassword { get; set; }
    }
}
