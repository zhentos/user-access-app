using FluentValidation;
using UserAccessApp.WebApi.Dtos;

namespace UserAccessApp.WebApi.Validators
{
    public class LoginDtoValidator : AbstractValidator<UserLoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Email)
           .NotEmpty().WithMessage("Email can't be null or empty.")
           .EmailAddress().WithMessage("A valid email address is required.")
           .Length(2, 60).WithMessage("Email must be between 3 and 60 characters long.");

            RuleFor(x => x.Password)
           .NotEmpty().WithMessage("Password can't be null or empty.")
           .Length(8, 20).WithMessage("Password must be between 8 and 20 characters long.");
        }
    }
}
