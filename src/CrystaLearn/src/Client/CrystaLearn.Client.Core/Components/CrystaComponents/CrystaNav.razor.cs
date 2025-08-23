namespace CrystaLearn.Client.Core.Components.CrystaComponents;
public partial class CrystaNav
{
    [Parameter] public BitNavItem SelectedItem { get; set; }
    [Parameter] public List<BitNavItem> Items { get; set; }
    [Parameter] public EventCallback<BitNavItem> OnSelectItem { get; set; }

    private Task HandleSelectItem(BitNavItem item)
        => OnSelectItem.InvokeAsync(item);
}
