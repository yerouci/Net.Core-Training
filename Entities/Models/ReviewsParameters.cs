using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    public class ReviewsParameters: QueryStringParameters
    {        
        public int? reviewType { get; set; } = null;
        public bool? Sort { get; set; } = null;        
    }
}
