using CrystaLearn.Client.Core.Components;

namespace CrystaLearn.Client.Core.Extensions;
public static class StringExtensions
{
    public static string GetLanguageIconName(this string? langCode)
        => langCode switch
        {
            "en-US" => CsLearnIconName.English,
            "fr-FR" => CsLearnIconName.French,
            "fa-IR" => CsLearnIconName.Farsi,
            "zh-CN" => CsLearnIconName.Chineese,
            _ => CsLearnIconName.English
        };

    public static string GetLanguageName(this string? langCode)
=> langCode switch
{
    "en-US" => "English",
    "fr-FR" => "French",
    "fa-IR" => "Farsi",
    "zh-CN" => "Chinese",
    _ => "English"
};
}
