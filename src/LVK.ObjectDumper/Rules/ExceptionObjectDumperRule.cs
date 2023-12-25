namespace LVK.ObjectDumper.Rules;

public class ExceptionObjectDumperRule : IObjectDumperRule
{
    public Type[] GetKnownSupportedTypes() => [];

    public bool SupportsType(Type type) => typeof(Exception).IsAssignableFrom(type);

    public void Dump(IObjectDumperContext context, string name, object value, bool recursiveDump)
    {
        var ex = (Exception)value;
        context.Writer.WriteFormatted(name, value, $"{context.Writer.FormatType(ex.GetType())}: {ex.Message}", out bool isFirstTime);
        if (!isFirstTime)
            return;

        var blockName = "caught exception";
        if (ex.InnerException != null)
            context.Writer.BeginBlock();

        while (ex != null)
        {
            context.Writer.BeginBlock(blockName);
            context.Dump("Type", ex.GetType(), true);
            context.Dump("Message", ex.Message, false);

            if (!string.IsNullOrWhiteSpace(ex.StackTrace))
            {
                context.Writer.BeginBlock("stack trace");
                using var reader = new StringReader(ex.StackTrace);
                while (reader.ReadLine() is { } line)
                    context.Writer.WriteLine(line);

                context.Writer.EndBlock();
            }

            context.Writer.EndBlock();

            ex = ex.InnerException;
            blockName = "inner exception";
        }

        if (((Exception)value).InnerException != null)
            context.Writer.EndBlock();
    }
}