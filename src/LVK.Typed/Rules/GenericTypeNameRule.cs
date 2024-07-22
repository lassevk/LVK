using System.Text;

namespace LVK.Typed.Rules;

internal class GenericTypeNameRule : ITypeNameRule
{
    public int Priority => 3;

    public string? TryGetNameOfType(Type type, ITypeHelper typeHelper, NameOfTypeOptions options)
    {
        if (!type.IsGenericType)
        {
            return null;
        }

        Type[] arguments = type.GetGenericArguments();
        var argumentTypeNames = arguments.Select(t => typeHelper.NameOf(t, options)).ToList();

        string baseName = type.Name.Substring(0, type.Name.IndexOf('`'));

        var sb = new StringBuilder();
        if ((options & NameOfTypeOptions.IncludeNamespaces) != 0)
        {
            sb.Append(type.Namespace).Append('.');
        }

        sb.Append(baseName).Append('<');
        string delimiter = (options & NameOfTypeOptions.SpaceAfterCommas) != 0 ? ", " : ",";
        sb.Append(string.Join(delimiter, argumentTypeNames));
        sb.Append('>');

        return sb.ToString();
    }
}