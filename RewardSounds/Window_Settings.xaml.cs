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
using System.Windows.Shapes;
using RewardSounds.Properties;

namespace RewardSounds
{
    /// <summary>
    /// Interaktionslogik für Window_Settings.xaml
    /// </summary>
    public partial class Window_Settings : Window
    {
        public Window_Settings()
        {
            InitializeComponent();
        }

        private void Chk_Autoconnect_Checked(object sender, RoutedEventArgs e)
        {
            Settings.Default.twitch_autoconnect = (bool)Chk_Autoconnect.IsChecked;
        }

        private void tb_Twitchname_TextChanged(object sender, TextChangedEventArgs e)
        {
            Settings.Default.twitch_account = tb_Twitchname.Text;
        }

        private void tb_TwitchOauth_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Settings.Default.twitch_oauth = tb_TwitchOauth.Password;

        }

        private void tb_TwitchChannel_TextChanged(object sender, TextChangedEventArgs e)
        {
            Settings.Default.twitch_channel = tb_TwitchChannel.Text;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetControls();
        }

        private void SetControls()
        {
            tb_TwitchChannel.Text = Settings.Default.twitch_channel;
            tb_Twitchname.Text = Settings.Default.twitch_account;
            tb_TwitchOauth.Password = Settings.Default.twitch_oauth;
            Chk_Autoconnect.IsChecked = Settings.Default.twitch_autoconnect;
        }
    }
}
