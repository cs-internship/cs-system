namespace CrystallineSociety.Shared.Dtos.BadgeSystem;

public class BadgeSystemValidationDto
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public BadgeSystemValidationLevel Level { get; set; }
    public string? RefBadge { get; set; }

    public BadgeSystemValidationDto(BadgeSystemValidationLevel level, string title, string? description = null,
        string? refBadge = null)
    {
        Title = title;
        Description = description;
        RefBadge = refBadge;
        Level = level;
    }

    public override string ToString()
    {
        return Title;
    }

    public static BadgeSystemValidationDto Error(string title, string? description = null, string? refBadge = null)
    {
        return new BadgeSystemValidationDto(BadgeSystemValidationLevel.Error, title, description, refBadge);
    }

    public static BadgeSystemValidationDto Warning(string title, string? description = null, string? refBadge = null)
    {
        return new BadgeSystemValidationDto(BadgeSystemValidationLevel.Warning, title, description, refBadge);
    }

    public static BadgeSystemValidationDto Information(string title, string? description = null, string? refBadge = null)
    {
        return new BadgeSystemValidationDto(BadgeSystemValidationLevel.Information, title, description, refBadge);
    }

    
}