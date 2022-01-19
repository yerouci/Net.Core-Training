using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    public class BookParameters: QueryStringParameters
    {
        public int? AuthorId { get; set; } = null;
        public string EditorialName { get; set; } = null;
        public DateTime? Before { get; set; } = null;
        public DateTime? After { get; set; } = null;
        public bool? Sort { get; set; } = null;
    }
}
