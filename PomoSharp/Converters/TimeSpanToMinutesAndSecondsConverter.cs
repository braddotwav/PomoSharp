using System.Globalization;
using System.Windows.Data;

namespace PomoSharp.Converters;

public class TimeSpanToMinutesAndSecondsConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is TimeSpan time)
        {
            int totalMinutes = (int)time.TotalMinutes;
            int seconds = time.Seconds;
            return $"{totalMinutes:D2}:{seconds:D2}";
        }

        return "00:00";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
}