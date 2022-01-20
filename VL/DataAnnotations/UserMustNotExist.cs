using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;
using VL.Contracts;

namespace Entities.DataAnnotations
{
    class UserMustNotExistAttribute : CustomDataAnnotationBaseAttribute
    {
        private const string MESSAGE = "The username already exists.";
        private readonly RequiredAttribute _requiredAtt = new RequiredAttribute { AllowEmptyStrings = false };

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {

            if (!_requiredAtt.IsValid(value))
            {
                return new ValidationResult("Required");
            }

            ValidationResult validationResult;
            var username = value as string;
            var service = (IUserService)context.GetService(typeof(IUserService));
            try
            {
                if (service.VerifyUsername(username))
                {
                    validationResult = ValidationResult.Success;
                }
                else
                {
                    validationResult = new ValidationResult(MESSAGE);
                }
            }
            catch (System.Exception e)
            {
                throw new System.Exception(e.Message);
            }

            return validationResult;
        }
    }
}
