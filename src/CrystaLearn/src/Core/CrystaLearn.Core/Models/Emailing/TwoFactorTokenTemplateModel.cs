namespace CrystaLearn.Core.Models.Emailing;

public partial class TwoFactorTokenTemplateModel
{
    public required string DisplayName { get; set; }

    public required string Token { get; set; }
}
