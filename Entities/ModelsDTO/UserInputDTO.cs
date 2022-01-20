using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.ModelsDTO
{
    public class UserInputDTO
    {      
        [Required]
        [StringLength(maximumLength: 100)]              
        public string Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email address is not valid.")]
        public string Email { get; set; }

        [Url]
        public string ImageURL { get; set; }
    }
}
