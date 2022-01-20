using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ModelsDTO
{
    public class UserListDTO
    {        
        public Guid Id { get; set; }        
        public string Name { get; set; }
        public string Email { get; set; }
        public string ImageURL { get; set; }
        public DateTime CreatedAt { get; set; }
        public int AuthorsAmount { get; set; }
    }
}
