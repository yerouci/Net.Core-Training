using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ModelsDTO
{
    public class ReviewDTO
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public string Opinion { get; set; }
        public int Qualification { get; set; }
        public int BookId { get; set; }
    }
}
