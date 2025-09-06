namespace CrystaLearn.Shared.Dtos.Crysta;

public class CrystaProgramLightDto
{
    public Guid Id { get; set; }
    public required string Code { get; set; }
    public virtual required string Title { get; set; }
}
