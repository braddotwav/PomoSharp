﻿using System.Globalization;
using System.Windows.Data;

namespace PomoSharp.Converters;

internal class EnumToBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
            return false;

        string parameterString = parameter.ToString()!;
        if (!Enum.IsDefined(value.GetType(), value))
            return false;

        var enumValue = Enum.Parse(value.GetType(), parameterString);
        return enumValue.Equals(value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
}
