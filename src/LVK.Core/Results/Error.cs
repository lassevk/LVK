namespace LVK.Core;

public record Error
{
    public static readonly Error None = new();

    protected Error()
    {
    }
}