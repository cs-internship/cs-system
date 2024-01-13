using CrystallineSociety.Shared.Dtos.Organization;
using CrystallineSociety.Shared.Dtos.Identity;

namespace CrystallineSociety.Client.Core.Components.Layout;

public partial class Header : IDisposable
{
    private bool _disposed;
    private bool _isUserAuthenticated;

    [Parameter] 
    public EventCallback OnToggleMenu { get; set; }

    [CascadingParameter(Name = "AppState")]
    public AppStateDto AppState { get; set; } = default!;

    private List<OrganizationDto> Organizations { get; set; } = new();

    private OrganizationDto? ActiveOrganization { get; set; }

    protected override async Task OnInitAsync()
    {
        Organizations = await HttpClient.GetFromJsonAsync<List<OrganizationDto>>("Organization/GetOrganizations");
    }

    private async Task ToggleMenu()
    {
        await OnToggleMenu.InvokeAsync();
    }

    public override void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        _disposed = true;
    }

    private void HandleOrganizationClick(OrganizationDto organization)
    {
        ActiveOrganization = organization;
        AppState.Organization = ActiveOrganization;
        NavigationManager.NavigateTo($"/o/{organization.Code}",false);
    }
}
