namespace LVK.Typed;

[Flags]
public enum NameOfTypeOptions
{
    None = 0,

    UseCSharpKeywords = 1,
    UseShorthandSyntax = 2,
    IncludeNamespaces = 4,
    SpaceAfterCommas = 8,

    Default = UseCSharpKeywords | UseShorthandSyntax | IncludeNamespaces | SpaceAfterCommas
}