﻿using System.ComponentModel.DataAnnotations;

namespace Blog.Models.ViewModels
{
    public class RegisterVIewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(6, ErrorMessage = "Password has to be at least 6 characters")]
        public string Password { get; set; }

    }
}
