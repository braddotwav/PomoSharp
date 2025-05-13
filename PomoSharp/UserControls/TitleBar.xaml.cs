using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

namespace PomoSharp.UserControls;

/// <summary>
/// Interaction logic for TitleBar.xaml
/// </summary>
public partial class TitleBar : UserControl
{
    public TitleBar()
    {
        InitializeComponent();
    }

    private void OnMinimiseApplication_Clicked(object sender, RoutedEventArgs e)
    {
        Application.Current.MainWindow.WindowState = WindowState.Minimized;
    }

    private void OnExitApplication_Clicked(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void OnMaximiseApplication_Clicked(object sender, RoutedEventArgs e)
    {
        ToggleWindowMaximizedState();
    }

    private void OnTitleBar_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount == 2) 
        {
            ToggleWindowMaximizedState();
        }

        if (e.LeftButton == MouseButtonState.Pressed) 
        {
            Application.Current.MainWindow.DragMove();
        }
    }

    private void ToggleWindowMaximizedState() 
    {
        bool isMaximized = Application.Current.MainWindow.WindowState == WindowState.Maximized;
        Application.Current.MainWindow.WindowState = isMaximized ? WindowState.Normal : WindowState.Maximized;
    }
}