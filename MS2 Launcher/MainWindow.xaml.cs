using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        DispatcherTimer timer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(25);
            timer.Tick += Timer_Tick;
            CheckOnlineStatus();
            timer.Start(); 
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
            if (isOnlinePortOpen) OnlineStatus.Fill = Brushes.Green;
            else
            OnlineStatus.Fill = Brushes.Red;
            if (isLocalPortOpen) LocalStatus.Fill = Brushes.Green;
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

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
           if (Properties.Settings.Default.SelectedServer.Length == 0)
            {
                MessageBox.Show("Select a server to start");
                return;
            }

           System.Diagnostics.Process.Start("C:/Development/MS2/Maplestory 2 Client/appdata/MapleStory2.exe", "127.0.0.1 30000 -ip -port --nxapp=nxl --lc=EN");
            Close();
        }
    }
}
