using RiotAccounts.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RiotAccounts
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            UpdateSelAccText();
            SettingsHandler.SettingsUpdated += UpdateSelAccText;
        }
        private void UpdateSelAccText()
        {
            var selectedAccount = SettingsHandler.GetSelectedAccount();
            if (selectedAccount != null)
            {
                selacc.Content = $"{selectedAccount.Tag}";
            }
            else
            {
                selacc.Content = "No account selected";
            }
        }
        private void CloseApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void MinimizeApp(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MoveApp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void SwitchPageAnimation(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var buttons = new[] { mainswitch, settingswitch };
            var icons = new Dictionary<Button, (string Normal, string Filled)> {
        { mainswitch, ("", "") },
        { settingswitch, ("", "") }
    };

            var unselectedBrush = TryFindResource("ForegroundUnselected") as SolidColorBrush;
            var selectedBrush = TryFindResource("ForegroundSelected") as SolidColorBrush;
            if (unselectedBrush == null || selectedBrush == null) return;

            foreach (var b in buttons)
            {
                bool isSelected = b == button;
                b.Content = isSelected ? icons[b].Filled : icons[b].Normal;

                if (b.Foreground is SolidColorBrush currentBrush)
                {
                    b.Foreground = new SolidColorBrush(currentBrush.Color);
                    b.Foreground.BeginAnimation(SolidColorBrush.ColorProperty,
                        new ColorAnimation
                        {
                            From = currentBrush.Color,
                            To = isSelected ? selectedBrush.Color : unselectedBrush.Color,
                            Duration = TimeSpan.FromMilliseconds(300),
                            EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
                        });
                }
            }

            current.BeginAnimation(MarginProperty, new ThicknessAnimation
            {
                From = current.Margin,
                To = new Thickness(
                    current.Margin.Left,
                    button.Margin.Top + (button.ActualHeight - current.ActualHeight) / 2,
                    current.Margin.Right,
                    current.Margin.Bottom),
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
            });
        }

        private void mainswitch_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("routes/accountsmanager.xaml", UriKind.Relative));
            SwitchPageAnimation(sender, e);

        }


        private void settingswitch_Click(object sender, RoutedEventArgs e)
        {
            SwitchPageAnimation(sender, e);

        }

        
    }
}
