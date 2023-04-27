using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
using System.Windows.Threading;

namespace MS2_Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer = new DispatcherTimer();
        private Properties.Settings settings = Properties.Settings.Default;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            timer.Interval = TimeSpan.FromSeconds(25);
            timer.Tick += Timer_Tick;
            CheckOnlineStatus();
            timer.Start();
            ApplyDefaults();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            CheckOnlineStatus();
            timer.Start();
        }

        private async void CheckOnlineStatus()
        {
            var isOnlinePortOpen = await IsPortOpen(Properties.Settings.Default.OnlineIp, Properties.Settings.Default.OnlinePort);
            var isLocalPortOpen = await IsPortOpen(Properties.Settings.Default.LocalIp, Properties.Settings.Default.LocalPort);

            if (isOnlinePortOpen)
                OnlineStatus.Fill = Brushes.Green;
            else
                OnlineStatus.Fill = Brushes.Red;
            if (isLocalPortOpen)
                LocalStatus.Fill = Brushes.Green;
            else
                LocalStatus.Fill = Brushes.Red;
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
            //SettingsWindow settingsWindow = new SettingsWindow();
            //settingsWindow.Owner = Application.Current.MainWindow;
            //settingsWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            //settingsWindow.Show();
        }

        private async Task<bool> IsPortOpen(string host, int port)
        {
            using var client = new TcpClient();
            try
            {
                await client.ConnectAsync(host, port);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {

            if (settings.SelectedServer.Length == 0)
            {
                MessageBox.Show("Select a server to start");
                return;
            }
            if (settings.ClientLocation.Length == 0)
            {
                string messageBoxText = "Client location not set.\nDo you want to set it now?";
                if (MessageBox.Show(messageBoxText, "Starting Error", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    SetClientLocation();
                }
                return;
            }

            //string serverIp;
            int serverPort;
            string commonParams = "-ip -port --nxapp=nxl --lc=EN";
            List<string> orionConfigurationContent = new List<string> { "[Settings]" };

            if (settings.SelectedServer == "Online")
            {
                //serverIp = settings.OnlineIp;
                serverPort = settings.OnlinePort;
                orionConfigurationContent.Add($"ClientIP={settings.OnlineIp}");
            }
            else
            {
                //serverIp = settings.LocalIp;
                serverPort = settings.LocalPort;
                orionConfigurationContent.Add($"ClientIP={settings.LocalIp}");
            }

            if (await WriteOrionConfiguration(orionConfigurationContent) == false)
            {
                return;
            }
            string startParams = string.Join(" ", serverPort, commonParams);
            try
            {
                Process process = System.Diagnostics.Process.Start(settings.ClientLocation + "\\MapleStory2.exe", startParams);
                if (process == null)
                {
                    HandleStartError();
                }
            }
            catch (Exception ex)
            {

                HandleStartError();
                return;
            }
            Close();
        }

        private async Task<bool> WriteOrionConfiguration(List<string> content)
        {
            //StreamReader reader = new StreamReader(settings.ClientLocation + "\\Orion2.ini");
            Trace.WriteLine("File created");
            Trace.WriteLine($"content {content}");
            try
            {
                await File.WriteAllLinesAsync($"{settings.ClientLocation}\\Orion.ini", content);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not write to Orion.ini", "Startup Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

        }

        private void HandleStartError()
        {
            string messageBoxText = "Could not start Maple Story2\nDo you want to change MapleStory2.exe`s location?";
            if (MessageBox.Show(messageBoxText, "Starting Error", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                SetClientLocation();
            }
            return;
        }

        private void LocalButton_Click(object sender, RoutedEventArgs e)
        {
            settings.SelectedServer = "Local";
            settings.Save();
            VisualiseServerSelection("Local");
            //string buttonName = (sender as Button).Name;
        }

        private void OnlineButton_Click(object sender, RoutedEventArgs e)
        {
            settings.SelectedServer = "Online";
            settings.Save();
            VisualiseServerSelection("Online");
        }

        private void VisualiseServerSelection(string server)
        {
            if (server == "Online")
            {
                OnlineButtonImage.Opacity = 1;
                LocalButtonImage.Opacity = 0.4;
                return;
            }
            LocalButtonImage.Opacity = 1;
            OnlineButtonImage.Opacity = 0.4;
        }

        private void ApplyDefaults()
        {
            VisualiseServerSelection(settings.SelectedServer);
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow SettingsWindow = new SettingsWindow();
            SettingsWindow.Owner = this; //Application.Current.MainWindow;
            SettingsWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            SettingsWindow.Show();
        }

        private void SetClientLocation()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.FileName = "MapleStory2";
            fileDialog.DefaultExt = ".exe";
            fileDialog.Filter = "MapleStory2 Executable|MapleStory2.exe";
            if (fileDialog.ShowDialog() == true)
            {
                string clientPath = fileDialog.FileName.Remove(fileDialog.FileName.IndexOf("\\MapleStory2.exe"));
                settings.ClientLocation = clientPath; //.Replace("\\", "/");
                settings.Save();
                return;
            }
            MessageBox.Show("Could not set client location.");
        }
    }
}
