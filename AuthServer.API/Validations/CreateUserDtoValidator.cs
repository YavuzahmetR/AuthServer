using AuthServer.Core.Dto_s;
using FluentValidation;

namespace AuthServer.API.Validations
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email field cannot be empty.").
                EmailAddress().WithMessage("Email format is wrong.");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Password field cannot be empty.");


            RuleFor(x => x.UserName).NotEmpty().WithMessage("Username field cannot be empty.");
                

        }



    }
}
