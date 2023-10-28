using System.ComponentModel;

namespace ConsoleApplication.Handler;

public static class EnumDescription
{
    public static string GetDescription(this Enum value)
    {
        var type = value.GetType();
        var name = Enum.GetName(type, value);
        if (string.IsNullOrWhiteSpace(name)) return "";

        var field = type.GetField(name);
        if (field == null) return name;

        return Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is not DescriptionAttribute attr
            ? name
            : attr.Description;

    }
}