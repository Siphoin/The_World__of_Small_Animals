public static class TypeExtensions
{
    public static void CopyAll<T>(this T source, T target)
    {
        var type = typeof(T);
        foreach (var sourceProperty in type.GetProperties())
        {
            var targetProperty = type.GetProperty(sourceProperty.Name);
            targetProperty.SetValue(target, sourceProperty.GetValue(source, null), null);
        }
        foreach (var sourceField in type.GetFields())
        {
            var targetField = type.GetField(sourceField.Name);
            targetField.SetValue(target, sourceField.GetValue(source));
        }
    }

    public static bool IsNumber(this object value)
    {
        return value is sbyte
                || value is byte
                || value is short
                || value is ushort
                || value is int
                || value is uint
                || value is long
                || value is ulong
                || value is float
                || value is double
                || value is decimal;
    }

    public static bool IsString(this object value)
    {
        return value is string;
    }


}