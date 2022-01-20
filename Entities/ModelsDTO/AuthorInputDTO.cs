using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.ModelsDTO
{
    public class AuthorInputDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Nationality { get; set; }

        [Required]        
        public DateTime DateOfBirth { get; set; }
    }
}
