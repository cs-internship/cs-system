namespace CrystaLearn.Client.Core.Components;
public partial class CsLearnIcon
{
    private readonly ElementClassBuilder _classBuilder = new();
    private string? _class;
    private IconColorStyle? _colorStyle;
    private string? _iconName;

    [Parameter] public string? Title { get; set; }
    [Parameter] public bool IsVisible { get; set; } = true;

    [Parameter]
    public string? IconName
    {
        get => _iconName;
        set
        {
            if (string.Equals(_iconName, value, StringComparison.Ordinal)) return;

            _iconName = value;
            _classBuilder.Reset();
        }
    }

    [Parameter]
    public IconColorStyle? ColorStyle
    {
        get => _colorStyle;
        set
        {
            if (_colorStyle == value) return;

            _colorStyle = value;
            _classBuilder.Reset();
        }
    }

    [Parameter]
    public string? Class
    {
        get => _class;
        set
        {
            if (string.Equals(_class, value, StringComparison.Ordinal)) return;

            _class = value;
            _classBuilder.Reset();
        }
    }

    protected override Task OnInitAsync()
    {
        _classBuilder
        .Register(() => "cslearn-ico")
        .Register(() => IconName)
        .Register(() => Class)
        .Register(() => ColorStyle?.ToString().ToLowerInvariant());

        return base.OnInitAsync();
    }

    public enum IconColorStyle
    {
        Blue,
        Gray,
        Primary,
        Green,
    }
}
