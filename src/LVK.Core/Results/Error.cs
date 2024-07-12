namespace LVK.Core;

public record Error
{
    protected Error()
    {
    }

    public static readonly Error None = new();
}