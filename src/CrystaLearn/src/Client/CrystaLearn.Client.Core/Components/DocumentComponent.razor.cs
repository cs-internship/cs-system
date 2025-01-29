using CrystaLearn.Shared.Dtos.Crysta;

namespace CrystaLearn.Client.Core.Components;

public partial class DocumentComponent
{
    [Parameter] public DocumentDto? Document { get; set; } = default!;
}
