using CrystallineSociety.Shared.Dtos.Organization;

namespace CrystallineSociety.Client.Shared;

public partial class Header : IDisposable
{
    private bool _disposed;
    private bool _isUserAuthenticated;

    [Parameter] public EventCallback OnToggleMenu { get; set; }

    private List<OrganizationDto> Organizations { get; set; } = new();

    private OrganizationDto? ActiveOrganization { get; set; }

    protected override async Task OnInitAsync()
    {
        AuthenticationStateProvider.AuthenticationStateChanged += VerifyUserIsAuthenticatedOrNot;

        _isUserAuthenticated = await StateService.GetValue($"{nameof(Header)}-isUserAuthenticated", AuthenticationStateProvider.IsUserAuthenticatedAsync);

        Organizations = await HttpClient.GetFromJsonAsync<List<OrganizationDto>>("Organization/GetOrganizations");

        await base.OnInitAsync();
    }

    async void VerifyUserIsAuthenticatedOrNot(Task<AuthenticationState> task)
    {
        try
        {
            _isUserAuthenticated = await AuthenticationStateProvider.IsUserAuthenticatedAsync();
        }
        catch (Exception ex)
        {
            ExceptionHandler.Handle(ex);
        }
        finally
        {
            StateHasChanged();
        }
    }

    private async Task ToggleMenu()
    {
        await OnToggleMenu.InvokeAsync();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        AuthenticationStateProvider.AuthenticationStateChanged -= VerifyUserIsAuthenticatedOrNot;

        _disposed = true;
    }

    private void HandleOrganizationClick(OrganizationDto organization)
    {
        ActiveOrganization = organization;
        NavigationManager.NavigateTo($"/o/{organization.Code}",false);
    }
}
