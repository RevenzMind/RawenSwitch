using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace RiotAccounts.utils
{
    public class Account
    {
        public string Tag { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class Settings
    {
        public List<Account> Accounts { get; set; } = new List<Account>();
        public string SelectedAccountTag { get; set; }  // Añadimos la cuenta seleccionada por su tag
    }


    public static class SettingsHandler
    {
        public static event Action SettingsUpdated;

        private static readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");

        public static Settings LoadSettings()
        {
            if (!File.Exists(FilePath))
                return new Settings();

            try
            {
                string json = File.ReadAllText(FilePath);
                return JsonSerializer.Deserialize<Settings>(json) ?? new Settings();
            }
            catch
            {
                return new Settings();
            }
        }

        public static void SelectAccount(string tag)
        {
            var settings = LoadSettings();
            settings.SelectedAccountTag = tag;  // Establece el tag de la cuenta seleccionada
            SaveSettings(settings);
        }

        public static Account GetSelectedAccount()
        {
            var settings = LoadSettings();
            return settings.Accounts.FirstOrDefault(a => a.Tag.Equals(settings.SelectedAccountTag, StringComparison.OrdinalIgnoreCase));
        }

        public static void SaveSettings(Settings settings)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(settings, options);
            File.WriteAllText(FilePath, json);
            OnSettingsUpdated();
        }

        public static void AddAccount(Account account)
        {
            var settings = LoadSettings();
            settings.Accounts.Add(account);
            SaveSettings(settings);
        }

        public static void RemoveAccount(string tag)
        {
            var settings = LoadSettings();
            settings.Accounts.RemoveAll(a => a.Tag == tag);
            SaveSettings(settings);
        }

        public static List<Account> GetAllAccounts()
        {
            return LoadSettings().Accounts;
        }

        public static Account GetAccountByTag(string tag)
        {
            var settings = LoadSettings();
            return settings.Accounts.FirstOrDefault(a => a.Tag.Equals(tag, StringComparison.OrdinalIgnoreCase));
        }

        private static void OnSettingsUpdated()
        {
            SettingsUpdated?.Invoke();
        }
    }
}
