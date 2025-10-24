namespace Shared.Entities;

public class Letter : BaseEntity
{
    public string Body { get; set; } = null!;

    public override string ToString()
    {
        return Body;
    }
}