using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.ModelsDTO
{    
    public class BookDTO
    {        
        public int Id { get; set; }
        public string Title { get; set; }        
        public string AuthorName { get; set; }        
        public string EditorialName { get; set; }        
        public string ISBN { get; set; }      
        public int Pages { get; set; } 
        public int AuthorId { get; set; }
        public DateTime Date { get; set; }
        public int Qualification { get; set; }
    }
}
