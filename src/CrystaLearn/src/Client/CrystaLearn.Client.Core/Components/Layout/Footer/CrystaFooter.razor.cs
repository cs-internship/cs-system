namespace CrystaLearn.Client.Core.Components.Layout.Footer;
public partial class CrystaFooter
{
    [Parameter] public BitAppShell? AppShell { get; set; }

    private async Task GoToTop()
    {
        if (AppShell is null) return;

        await AppShell.GoToTop(BitScrollBehavior.Instant);
    }
}
