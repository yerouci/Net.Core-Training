using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.ModelsDTO
{
    public class AuthorDetailsDTO
    {        
        public string Name { get; set; }
     
        public string Nationality { get; set; }
     
        public DateTime DateOfBirth { get; set; }

        public int SubscribedUsersAmount { get; set; }

        public List<BookDTO> Books { get; set; }
    }
}
