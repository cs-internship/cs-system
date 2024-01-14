using Microsoft.AspNetCore.Components.Web;

namespace CrystallineSociety.Client.Core.Components.Layout;

public partial class MainLayout : IDisposable
{
    private bool _disposed;
    private bool _isMenuOpen;
    private bool _isUserAuthenticated;
    private ErrorBoundary ErrorBoundaryRef = default!;

    [AutoInject] private IExceptionHandler _exceptionHandler = default!;

    [AutoInject] private AuthenticationManager _authStateProvider = default!;

    [AutoInject] private AppStateDto _appStateDto = default!;

    private void ToggleMenuHandler()
    {
        _isMenuOpen = !_isMenuOpen;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        _disposed = true;
    }
}
