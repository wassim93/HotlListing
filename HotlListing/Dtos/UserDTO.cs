﻿using System.ComponentModel.DataAnnotations;

namespace HotlListing.Dtos
{

    public class LoginUserDtO
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "Your password is limied to {2} to {1} characters", MinimumLength = 8)]
        public string Password { get; set; }
    }
    public class UserDTO : LoginUserDtO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "Your password is limied to {2} to {1} characters", MinimumLength = 8)]
        public string Password { get; set; }

        public ICollection<string> Roles { get; set; }

    }
}
