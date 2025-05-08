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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RiotAccounts.components.accountcomponent
{
    /// <summary>
    /// Lógica de interacción para accountcard.xaml
    /// </summary>
    public class Account
    {
        public string Tag { get; set; }

    }
    public partial class accountcard : UserControl
    {
        public accountcard()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public static readonly DependencyProperty RiotTagProperty =
            DependencyProperty.Register("RiotTag", typeof(string), typeof(accountcard), new PropertyMetadata(""));

        public string RiotTag
        {
            get => (string)GetValue(RiotTagProperty);
            set => SetValue(RiotTagProperty, value);
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var accountTag = RiotTag;

            if (e.ChangedButton == MouseButton.Right)
            {
                SettingsHandler.RemoveAccount(accountTag);
            }
            else if (e.ChangedButton == MouseButton.Left)
            {
                SettingsHandler.SelectAccount(accountTag);
            }
        }
    }
}
