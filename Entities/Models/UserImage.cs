using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Models
{
    public class UserImage
    {
        [Url]
        public string Url { get; set; }
    }
}
