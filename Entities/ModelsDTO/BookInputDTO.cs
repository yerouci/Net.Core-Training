using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.ModelsDTO
{    
    public class BookInputDTO
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string EditorialName { get; set; }

        [Required]        
        public int Pages { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [Url]
        public string URL { get; set; }

        [Required]
        public string ISBN { get; set; }
    }
}
