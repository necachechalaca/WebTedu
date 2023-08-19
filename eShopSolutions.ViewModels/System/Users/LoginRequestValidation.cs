using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolutions.ViewModels.System.Users
{
    public class LoginRequestValidation : AbstractValidator<LoginRequest>   
    {
        public LoginRequestValidation() 
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("User name is required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Passsword is emty");
        }
    }
}
