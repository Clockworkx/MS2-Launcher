using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MS2_Launcher
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private Properties.Settings settings = Properties.Settings.Default;

        public SettingsWindow()
        {
            InitializeComponent();
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaxmizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                return;
            }
            WindowState = WindowState.Maximized;

        }

        private void ChangeClientLocation_Click(object sender, RoutedEventArgs e)
        {
            SetClientLocation();
        }
        private void SetClientLocation()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.FileName = "MapleStory2";
            fileDialog.DefaultExt = ".exe";
            fileDialog.Filter = "MapleStory2 Executable|MapleStory2.exe";
            if (fileDialog.ShowDialog() == true)
            {
                settings.ClientLocation = fileDialog.FileName.Replace("\\", "/");
                return;
            }
            MessageBox.Show("Could not set client location.");
        }
        
        private void SaveLocalServerData_Click(object sender, RoutedEventArgs e)
        {
            if (LocalServerPort.Text.Length > 0 == true && int.TryParse(LocalServerPort.Text, out int localPort))
            {
                settings.LocalPort = localPort;
            }
            if (LocalServerIp.Text.Length > 0 == true)
            {
                settings.LocalIp = LocalServerIp.Text;
            }
            settings.Save();
        }
        private void SaveOnlineServerData_Click(object sender, RoutedEventArgs e)
        {
            if (OnlineServerPort.Text.Length > 0 == true && int.TryParse(OnlineServerPort.Text, out int onlinePort))
            {
                settings.OnlinePort = onlinePort;
            }
            if (OnlineServerIp.Text.Length > 0 == true)
            {
                settings.OnlineIp = OnlineServerIp.Text;
            }
            settings.Save();
        }
    }
}
