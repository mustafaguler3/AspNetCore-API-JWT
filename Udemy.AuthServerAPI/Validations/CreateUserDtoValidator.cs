using FluentValidation;
using UdemyAuthServer.Core.Dtos;

namespace Udemy.AuthServerAPI.Validations
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(i=>i.Email).NotEmpty().WithMessage("Email is required").EmailAddress().WithMessage("Email is wrong");
            RuleFor(i => i.Password).NotEmpty().WithMessage("Password is required");
            RuleFor(i => i.UserName).NotEmpty().WithMessage("Username is required");
        }
    }
}
