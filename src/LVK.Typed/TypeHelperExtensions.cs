namespace LVK.Typed
{
    public static class TypeHelperExtensions
    {
        public static string NameOf(this ITypeHelper typeHelper, Type type, NameOfTypeOptions options = NameOfTypeOptions.Default)
        {
            ArgumentNullException.ThrowIfNull(typeHelper);
            ArgumentNullException.ThrowIfNull(type);

            return typeHelper.TryGetNameOf(type, options) ?? type.FullName!;
        }

        public static string NameOf<T>(this ITypeHelper typeHelper, NameOfTypeOptions options = NameOfTypeOptions.Default) => NameOf(typeHelper, typeof(T), options);
    }
}