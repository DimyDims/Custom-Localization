using System;

public static class LocalizationUtils
{
    public static string GetKey<T>(T element, string attribute = null)
        where T : Enum
    {
        string name = typeof(T).Name;
        attribute = string.IsNullOrWhiteSpace(attribute) ? string.Empty : $".{attribute}";
        return $"{name}.{element}{attribute}";
    }
}
