using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.DataAnnotations
{
    public class CustomDataAnnotationBaseAttribute : ValidationAttribute
    {
        public ValidationResult IsValidAlternative(object value, ValidationContext context)
        {
            return IsValid(value, context);
        }
    }
}
