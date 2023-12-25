namespace LVK.Typed
{
    internal class NullableTypeNameRule : ITypeNameRule
    {
        public int Priority => 2;

        public string? TryGetNameOfType(Type type, ITypeHelper typeHelper, NameOfTypeOptions options)
        {
            if ((options & NameOfTypeOptions.UseShorthandSyntax) == 0)
            {
                return null;
            }

            if (!(type.IsGenericType))
            {
                return null;
            }

            Type openGenericType = type.GetGenericTypeDefinition();
            if (openGenericType != typeof(Nullable<>))
            {
                return null;
            }

            Type underlyingType = type.GetGenericArguments()[0];
            return typeHelper.NameOf(underlyingType) + "?";
        }
    }
}