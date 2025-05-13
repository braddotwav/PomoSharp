using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace PomoSharp.UserControls;

public partial class IntegerTextBox : UserControl
{
    public int Value
    {
        get { return (int)GetValue(ValueProperty); }
        set { SetValue(ValueProperty, value); }
    }

    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register("Value", typeof(int), typeof(IntegerTextBox), new PropertyMetadata(0));

    public int MaximumValue
    {
        get { return (int)GetValue(MaximumValueProperty); }
        set { SetValue(MaximumValueProperty, value); }
    }

    public static readonly DependencyProperty MaximumValueProperty =
        DependencyProperty.Register("MaximumValue", typeof(int), typeof(IntegerTextBox), new PropertyMetadata(0));

    public int MinimumValue
    {
        get { return (int)GetValue(MinimumValueProperty); }
        set { SetValue(MinimumValueProperty, value); }
    }

    public static readonly DependencyProperty MinimumValueProperty =
        DependencyProperty.Register("MinimumValue", typeof(int), typeof(IntegerTextBox), new PropertyMetadata(0));

    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register("Text", typeof(string), typeof(IntegerTextBox));

    public int Rollback
    {
        get { return (int)GetValue(RollbackProperty); }
        set { SetValue(RollbackProperty, value); }
    }

    public static readonly DependencyProperty RollbackProperty =
        DependencyProperty.Register("Rollback", typeof(int), typeof(IntegerTextBox));

    public IntegerTextBox()
    {
        InitializeComponent();
    }

    private void OnPreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        e.Handled = !IsInputNumeric(e.Text);
    }

    private void OnTextBoxPasted(object sender, DataObjectPastingEventArgs e)
    {
        if (e.DataObject.GetDataPresent(DataFormats.Text))
        {
            string text = (string)e.DataObject.GetData(DataFormats.Text);
            if (!IsInputNumeric(text)) 
            {
                e.CancelCommand();
            }
        }
        else
        {
            e.CancelCommand();
        }
    }

    private bool IsInputNumeric(string text) 
    {
        return int.TryParse(text, out _);
    }

    private void OnTextBoxLostFocus(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(ValueElement.Text)) 
        {
            ValueElement.Text = Rollback.ToString();
        }

        Value = Math.Clamp(Value, MinimumValue, MaximumValue);
    }
}
