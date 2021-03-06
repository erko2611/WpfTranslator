﻿using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Reflection;
using Microsoft.Win32;

namespace WpfTranslator
{
    class WinApi
    {
        const int MOD_ALT = 0x1;
        const int MOD_CONTROL = 0x2;
        const int MOD_SHIFT = 0x4;
        const int MOD_WIN = 0x8;
        const int WM_HOTKEY = 0x312;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public static void RegisterAppHotKey(IntPtr hWnd)
        {
            RegisterHotKey(hWnd, Constants.TranslateHotKeyId, MOD_ALT | MOD_SHIFT, (int)Keys.Q);
            RegisterHotKey(hWnd, Constants.SpellCheckHotKeyId, MOD_ALT | MOD_SHIFT, (int)Keys.W);
            RegisterHotKey(hWnd, Constants.PronounceHotKeyId, MOD_ALT | MOD_SHIFT, (int)Keys.E);
        }

        public static void UnregisterAppHotKey(IntPtr hWnd)
        {
            UnregisterHotKey(hWnd, Constants.TranslateHotKeyId);
            UnregisterHotKey(hWnd, Constants.PronounceHotKeyId);
            UnregisterHotKey(hWnd, Constants.SpellCheckHotKeyId);
        }

        public static bool IsAddedToStartup()
        {
            using (var rk = OpenRegistry())
            {
                return rk?.GetValue(Properties.Resources.AppName) != null;
            }
        }

        public static void AddToStartup()
        {
            using (var rk = OpenRegistry())
            {
                rk?.SetValue(Properties.Resources.AppName, Assembly.GetExecutingAssembly().Location);
            }
        }

        public static void DeleteFromStartup()
        {
            using (var rk = OpenRegistry())
            {
                rk?.DeleteValue(Properties.Resources.AppName, false);
            }
        }

        private static RegistryKey OpenRegistry() => Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
    }
}
