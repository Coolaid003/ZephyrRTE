using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using XDevkit;
using JRPC_Client;
using System.Windows.Forms;
using System.Windows.Media;
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
        private FontFamily infoFontFamily = new FontFamily("SimSun");

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void ConnectButton_OnClick(object sender, RoutedEventArgs e)
        {
            connectProgressRing.IsActive = true;
            connectProgressRing.Visibility = Visibility.Visible;

            await Task.Delay(100); // This delay is here just so the progress ring can properly render first.

            if (JRPC.Connect(_myConsole, out _myConsole,
                addyInput.Text != "" ? addyInput.Text : "default")) // Probably have XNotify's here sooner or later.
            {
                MessageBox.Show("Your console was connected successfully!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.None);
                Title = "ZephyrRTE | Connected";
            }
            else
            {
                MessageBox.Show("Could not connect on this IP.", "Connection Failed!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                connectProgressRing.IsActive = false;
                connectProgressRing.Visibility = Visibility.Collapsed;
                return;
            }

            connectProgressRing.IsActive = false;
            connectProgressRing.Visibility = Visibility.Collapsed;

            for (;;)
            {
                consoleInfoBox.Items[0] = new ComboBoxItem() { FontFamily = infoFontFamily, FontSize = 16, Content = $"CPU Temp: {JRPC.GetTemperature(_myConsole, JRPC.TemperatureType.CPU)} °C" };
                consoleInfoBox.Items[1] = new ComboBoxItem() { FontFamily = infoFontFamily, FontSize = 16, Content = $"GPU Temp: {JRPC.GetTemperature(_myConsole, JRPC.TemperatureType.GPU)} °C" };
                consoleInfoBox.Items[2] = new ComboBoxItem() { FontFamily = infoFontFamily, FontSize = 16, Content = $"RAM Temp: {JRPC.GetTemperature(_myConsole, JRPC.TemperatureType.EDRAM)} °C" };
                consoleInfoBox.Items[3] = new ComboBoxItem() { FontFamily = infoFontFamily, FontSize = 16, Content = $"MB Temp: {JRPC.GetTemperature(_myConsole, JRPC.TemperatureType.MotherBoard)} °C" };
                await Task.Delay(2000); // Update console information every 2 seconds.
            }
        }
    }
}
