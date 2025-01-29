using CrystaLearn.Client.Web;

namespace CrystaLearn.Server.Web;

public partial class ServerWebSettings : ClientWebSettings
{
    public ForwardedHeadersOptions ForwardedHeaders { get; set; } = default!;

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var validationResults = base.Validate(validationContext).ToList();

        Validator.TryValidateObject(ForwardedHeaders, new ValidationContext(ForwardedHeaders), validationResults, true);

        return validationResults;
    }
}
