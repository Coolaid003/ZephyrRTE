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

namespace x360Tool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IXboxConsole _myConsole;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void ConnectButton_OnClick(object sender, RoutedEventArgs e)
        {
            progress1.IsActive = true;
            progress1.Visibility = Visibility.Visible;

            if (addyInput.Text != "")   // Non-empty ip input means use the input.
            {
                await Task.Delay(1000); // This delay is here just so the progress ring can properly render first.

                if (JRPC.Connect(_myConsole, out _myConsole, addyInput.Text))
                    System.Windows.Forms.MessageBox.Show("Your console was connected successfully!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.None);
                else
                    System.Windows.Forms.MessageBox.Show("Could not connect on this IP.", "Connection Failed!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                progress1.IsActive = false;
                progress1.Visibility = Visibility.Collapsed;
            }
            else    // Empty ip input means use default console.
                System.Windows.Forms.MessageBox.Show("Your console was connected successfully!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.None);

            progress1.IsActive = false;
            progress1.Visibility = Visibility.Collapsed;
        }
    }
}
