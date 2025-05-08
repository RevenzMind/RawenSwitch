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

namespace RiotAccounts.routes
{
    /// <summary>
    /// Lógica de interacción para accountsmanager.xaml
    /// </summary>
    public partial class Accountsmanager : Page
    {
        public Accountsmanager()
        {
            InitializeComponent();
            renderAccountList();
            SettingsHandler.SettingsUpdated += renderAccountList; 
        }
        private void renderAccountList()
        {
            var settings = utils.SettingsHandler.LoadSettings();
            AccountList.Items.Clear();
            foreach (var account in settings.Accounts)
            {
                var accountCard = new components.accountcomponent.accountcard
                {
                    RiotTag = account.Tag
                };
                AccountList.Items.Add(accountCard);
            }
        }
        private void AddAccount(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TagInput.Text) || string.IsNullOrWhiteSpace(UserInput.Text) || string.IsNullOrWhiteSpace(PassInput.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }
            var account = new utils.Account
            {
                Tag = TagInput.Text,
                Username = UserInput.Text,
                Password = PassInput.Text
            };
            utils.SettingsHandler.AddAccount(account);
            TagInput.Clear();
            UserInput.Clear();
            PassInput.Clear();
            MessageBox.Show("Account added successfully.");
        }
    }
}
