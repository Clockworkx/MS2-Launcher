using System;
using System.Collections.Generic;
using System.Linq;
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

namespace MS2_Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
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
            //using TcpClient tcpClient = new TcpClient();
            //try
            //{
            //    tcpClient.BeginConnect()
            //}
        }

        //private bool IsPortOpen(string host, int port, TimeSpan timeout)
        //{
        //    using TcpClient tcpClient = new TcpClient();
        //    tcpClient.BeginConnect(host, port);
        //}

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
           
           System.Diagnostics.Process.Start("C:/Development/MS2/Maplestory 2 Client/appdata/MapleStory2.exe", "127.0.0.1 30000 -ip -port --nxapp=nxl --lc=EN");
        
        }
    }
}
