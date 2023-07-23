using AuthServer.Core.DTOs;
using FluentValidation;

namespace AuthServer.API.Validations
{
    public class CreateUserDtoValidator: AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(c => c.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .EmailAddress()
                .WithMessage("You must fit email format");

            RuleFor(c => c.Password)
                .NotEmpty()
                .WithMessage("Password is required");

            RuleFor(c => c.UserName)
                .NotEmpty()
                .WithMessage("UserName is required");
        }
    }
}
