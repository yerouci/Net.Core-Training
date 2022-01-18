using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Models
{
    [Table("book")]
    public class Book
    {
        [Key]        
        public string Title { get; set; }

        [Required]
        public Author Author { get; set; }

        [Required]
        public string EditorialName { get; set; }

        [Required]
        public int Pages { get; set; }

        [Required]
        public string URL { get; set; }

        [Required]
        public string ISBN { get; set; }

        public int Qualification { get; set; }
    }
}
