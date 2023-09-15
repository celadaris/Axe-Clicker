using FluentValidation;

namespace Axe_Server.Validation
{
    public class PlayerValidator : AbstractValidator<ScoreTable>
    {
        public PlayerValidator()
        {
            RuleFor(x => x.playerName)
                .NotEmpty()
                .NotNull()
                .MinimumLength(3)
                .MaximumLength(10);

            RuleFor(x => x.highScore)
                .NotNull()
                .NotEmpty();
        }
    }

    public class LoginValidator : AbstractValidator<LoginTable>
    {
        public LoginValidator()
        {
            RuleFor(x => x.playerName)
                .NotEmpty()
                .NotNull()
                .MinimumLength(3)
                .MaximumLength(10);

            RuleFor(x => x.pwrdHash)
                .NotEmpty()
                .NotNull();
        }
    }
}
