using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms; // Add this for SendKeys alternative

namespace RiotAccounts.utils
{
    public class riotlauncher
    {
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_CHAR = 0x0102;
        private const int VK_TAB = 0x09;
        private const int VK_RETURN = 0x0D;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [SupportedOSPlatform("windows")]
        private static string FindRiotByRegistryKeys()
        {
            try
            {
                using var key = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.ClassesRoot, Microsoft.Win32.RegistryView.Registry64)
                    .OpenSubKey(@"riotclient\DefaultIcon");
                return key?.GetValue("") is string defaultIcon ? defaultIcon.Split(',')[0] : string.Empty;
            }
            catch { return string.Empty; }
        }

        [SupportedOSPlatform("windows")]
        private static void SendTextSafely(string text)
        {
            SendKeys.SendWait(text);
            Thread.Sleep(100);
        }

        [SupportedOSPlatform("windows")]
        private static void SendSpecialKeySafely(string key)
        {
            SendKeys.SendWait(key);
            Thread.Sleep(100);
        }

        [SupportedOSPlatform("windows")]
        public void LaunchRiotClient()
        {
            var account = SettingsHandler.GetSelectedAccount();
            if (account == null)
            {
                System.Windows.MessageBox.Show("No se encontró una cuenta seleccionada.");
                return;
            }

            bool valorantStarted = false;
            int attempts = 0;

            while (!valorantStarted && attempts++ < 5)
            {
                var hwnd = FindWindow(null, "Riot Client");
                if (hwnd != IntPtr.Zero)
                {
                    if (!SetForegroundWindow(hwnd))
                    {
                        Thread.Sleep(1000);
                        continue;
                    }

                    Thread.Sleep(3000);

                    // Verificar si la ventana activa sigue siendo Riot Client  
                    hwnd = FindWindow(null, "Riot Client");
                    if (hwnd == IntPtr.Zero || !SetForegroundWindow(hwnd))
                    {
                        Thread.Sleep(1000);
                        continue;
                    }

                    // Use the improved method for sending text
                    SendTextSafely(account.Username);
                    SendSpecialKeySafely("{TAB}");
                    SendTextSafely(account.Password);
                    SendSpecialKeySafely("{ENTER}");

                    Thread.Sleep(3000);

                    var process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = FindRiotByRegistryKeys(),
                            Arguments = "--launch-product=valorant --launch-patchline=live",
                            UseShellExecute = false,
                            CreateNoWindow = true
                        }
                    };
                    process.Start();
                    valorantStarted = true;
                }
                else if (attempts == 1)
                {
                    try
                    {
                        new Process
                        {
                            StartInfo = new ProcessStartInfo
                            {
                                FileName = FindRiotByRegistryKeys(),
                                UseShellExecute = false,
                                CreateNoWindow = true
                            }
                        }.Start();
                    }
                    catch { }
                }
                if (!valorantStarted) Thread.Sleep(5000);
            }

            if (!valorantStarted) throw new InvalidOperationException("No se pudo iniciar Valorant.");
        }
    }
}