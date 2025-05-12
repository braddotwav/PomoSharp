using System.Windows;
using System.Windows.Controls;

namespace PomoSharp.UserControls;

public partial class ReportCard : UserControl
{
    public string Value
    {
        get { return (string)GetValue(ValueProperty); }
        set { SetValue(ValueProperty, value); }
    }

    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register("Value", typeof(string), typeof(ReportCard));

    public string Title
    {
        get { return (string)GetValue(TitleProperty); }
        set { SetValue(TitleProperty, value); }
    }

    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register("Title", typeof(string), typeof(ReportCard));

    public ReportCard()
    {
        InitializeComponent();
    }

}