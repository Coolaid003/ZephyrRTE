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
using XDevkit;
using JRPC_Client;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;

namespace x360Tool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Our console object. Should get initialized on connect.
        private IXboxConsole _myConsole;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void ConnectButton_OnClick(object sender, RoutedEventArgs e)
        {
            connectProgressRing.IsActive = true;
            connectProgressRing.Visibility = Visibility.Visible;

            await Task.Delay(100); // This delay is here just so the progress ring can properly render first.

            if (JRPC.Connect(_myConsole, out _myConsole, addyInput.Text != "" ? addyInput.Text : "default")) // Probably have XNotify's here sooner or later.
                MessageBox.Show("Your console was connected successfully!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.None);
            else
                MessageBox.Show("Could not connect on this IP.", "Connection Failed!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            connectProgressRing.IsActive = false;
            connectProgressRing.Visibility = Visibility.Collapsed;
        }
    }
}
