using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using XDevkit;
using JRPC_Client;
using System.Windows.Forms;
using System.Windows.Media;
using MessageBox = System.Windows.Forms.MessageBox;
using System.Windows.Media.Imaging;
using DiscordRPC;

namespace x360Tool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Our console object. Should get initialized on connect.
        private IXboxConsole _myConsole;
        private bool isConnected = true; // Keep a global state of connection status.
        private readonly FontFamily _infoFontFamily = new FontFamily("SimSun");
        private readonly GameTitleManager _gameTitleManager = new GameTitleManager();
        private GameTitle _currentTitle;
        public DiscordRpcClient client;

        public MainWindow()
        {
            InitializeComponent();
            client = new DiscordRpcClient("891182357896384542");  //Creates the client
            client.Initialize();
            client.SetPresence(new RichPresence()
            {
                Details = "Not Connected",
                State = "Main Menu",
                Assets = new Assets()
                {
                    LargeImageKey = "zephyr",
                    LargeImageText = "Zephyr Services",
                },
                Timestamps = new Timestamps()
                {
                    Start = DateTime.UtcNow,
                }
            });
        }

        /// <summary>
        /// Console connect button click. This is the main program logic.
        /// </summary>
        private async void ConnectButton_OnClick(object sender, RoutedEventArgs e)
        {
            connectProgressRing.IsActive = true;
            connectProgressRing.Visibility = Visibility.Visible;

            await Task.Delay(100); // This delay is here just so the progress ring can properly render first.

            if (JRPC.Connect(_myConsole, out _myConsole, addyInput.Text != "" ? addyInput.Text : "default"))
            {
                MessageBox.Show("Your console was connected successfully!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.None);
                addyInput.Text = "default";
                Title = "ZephyrRTE | Connected";
                connectButton.IsEnabled = false;
                isConnected = true;

                JRPC.XNotify(_myConsole, "ZephyrRTE: Successfully Connected!");

                UpdateTitleBox();
                client.SetPresence(new RichPresence()
                {
                    Details = "Connected",
                    State = $"Currently Playing: {_currentTitle.Title}",
                    Assets = new Assets()
                    {
                        LargeImageKey = "zephyr",
                        LargeImageText = "Zephyr Services",
                    },
                    Timestamps = new Timestamps()
                    {
                        Start = DateTime.UtcNow,
                    }
                });
            }
            else
            {
                MessageBox.Show("Could not connect on this IP.", "Connection Failed!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                connectProgressRing.IsActive = false;
                connectProgressRing.Visibility = Visibility.Hidden;
                return;
            }

            connectProgressRing.IsActive = false;
            connectProgressRing.Visibility = Visibility.Hidden;

            // Keep a constant loop to retrieve console and title information.
            for (;;)
            {
                consoleInfoBox.Items[0] = new ComboBoxItem() { FontFamily = _infoFontFamily, FontSize = 16, Content = $"CPU Temp: {JRPC.GetTemperature(_myConsole, JRPC.TemperatureType.CPU)} °C" };
                consoleInfoBox.Items[1] = new ComboBoxItem() { FontFamily = _infoFontFamily, FontSize = 16, Content = $"GPU Temp: {JRPC.GetTemperature(_myConsole, JRPC.TemperatureType.GPU)} °C" };
                consoleInfoBox.Items[2] = new ComboBoxItem() { FontFamily = _infoFontFamily, FontSize = 16, Content = $"RAM Temp: {JRPC.GetTemperature(_myConsole, JRPC.TemperatureType.EDRAM)} °C" };
                consoleInfoBox.Items[3] = new ComboBoxItem() { FontFamily = _infoFontFamily, FontSize = 16, Content = $"MB Temp: {JRPC.GetTemperature(_myConsole, JRPC.TemperatureType.MotherBoard)} °C" };

                // Title change checks.
                if (_currentTitle != null && _gameTitleManager.TitleChanged(ref _myConsole, ref _currentTitle))
                {
                    UpdateTitleBox();
                    client.SetPresence(new RichPresence()
                    {
                        Details = "Connected",
                        State = $"Currently Playing: {_currentTitle.Title}",
                        Assets = new Assets()
                        {
                            LargeImageKey = "zephyr",
                            LargeImageText = "Zephyr Services",
                        },
                        Timestamps = new Timestamps()
                        {
                            Start = DateTime.UtcNow,
                        }
                    });
                }

                await Task.Delay(2000); // Updates information every 2 seconds, efficiency reasons.
            }
        }

        /// <summary>
        /// Main window close event, do cleanup here mostly.
        /// </summary>
        private void Window_Close(object sender, CancelEventArgs e)
        {
            client.Dispose();
        }

        /// <summary>
        /// XNotify button click event to send a message to the console.
        /// </summary>
        private void notifyBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Button click event to manually refresh the current title.
        /// </summary>
        private void refreshTitleBtn_Click(object sender, RoutedEventArgs e)
        {
            UpdateTitleBox();
        }

        /// <summary>
        /// Updates the title information in the box with the most recent initialized title.
        /// </summary>
        private void UpdateTitleBox()
        {
            _currentTitle = _gameTitleManager.GetTitleById(JRPC.XamGetCurrentTitleId(_myConsole));

            if (_currentTitle == null)
                _currentTitle = _gameTitleManager.GetTitleById(0);

            titleName.Text = _currentTitle.Title;
            titleId.Text = _currentTitle.ID.ToString();
            titleImage.Source = new BitmapImage(new Uri($"/x360Tool;component/{_currentTitle.IconPath}", UriKind.Relative));
        }
    }
}