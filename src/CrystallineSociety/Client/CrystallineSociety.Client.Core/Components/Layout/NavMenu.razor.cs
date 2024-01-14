using CrystallineSociety.Shared.Dtos.Identity;
using CrystallineSociety.Client.Core.Controllers.Identity;

namespace CrystallineSociety.Client.Core.Components.Layout;

public partial class NavMenu : IDisposable
{
    private bool _disposed;
    private UserDto _user = new();
    private List<BitNavItem> _navItems = new();

    private Action _unsubscribe = default!;

    [Parameter] public bool IsMenuOpen { get; set; }

    [Parameter] public EventCallback<bool> IsMenuOpenChanged { get; set; }

    public override void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed || disposing is false) return;

        _unsubscribe?.Invoke();

        _disposed = true;
    }
}
