using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;
using System.Linq;
using RewardSounds.Properties;
using RewardSounds.Models;
using NAudio.Wave;


namespace RewardSounds.Util
{
    class Twitch
    {
        public static IWavePlayer waveOutDevice;
        public static AudioFileReader AudioFileReader;
        private SoundObject selectedItem;
        public static int userLevel = 0; // 0 = regular, 1 = sub, 2 = mod, 3 = broadcaster
        public static TwitchClient _client;
        public static bool onCooldown = false;
        public static Timer cooldownTimer = new Timer
        {
            Interval = TimeSpan.FromSeconds(5).TotalMilliseconds,
        };

        public static void BotConnect()
        {
            // Checks if twitch credentials are present
            if (string.IsNullOrEmpty(Settings.Default.twitch_account) || string.IsNullOrEmpty(Settings.Default.twitch_oauth) || string.IsNullOrEmpty(Settings.Default.twitch_channel))
            {
                return;
            }

            // creates new connection based on the credentials in settings
            ConnectionCredentials credentials = new ConnectionCredentials(Settings.Default.twitch_account, Settings.Default.twitch_oauth);
            ClientOptions clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };
            WebSocketClient customClient = new WebSocketClient(clientOptions);
            _client = new TwitchClient(customClient);
            _client.Initialize(credentials, Settings.Default.twitch_channel);

            _client.OnMessageReceived += _client_OnMessageReceived;
            _client.OnConnected += _client_OnConnected;
            _client.OnDisconnected += _client_OnDisconnected;

            _client.Connect();

            // subscirbes to the cooldowntimer elapsed event for the command cooldown
            cooldownTimer.Elapsed += CooldownTimer_Elapsed;
        }

        private static void _client_OnDisconnected(object sender, TwitchLib.Communication.Events.OnDisconnectedEventArgs e)
        {
            UpdateMainWindowStatus("Disconnected from Twitch");
        }

        private static void CooldownTimer_Elapsed(object sender, ElapsedEventArgs e)
        {

        }

        private static void _client_OnConnected(object sender, OnConnectedArgs e)
        {
            UpdateMainWindowStatus("Connected to Twitch");
        }

        private static void UpdateMainWindowStatus(string msg)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(MainWindow))
                    {
                        (window as MainWindow).lbl_Status.Content = msg;
                    }
                }
            }));
        }

        public static bool IsConnected()
        {
            return _client.IsConnected;
        }

        private static void _client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {

            // If message logging is enabled and the reward was triggered, save it to the settings (if settings window is open, write it to the textbox)
            if (e.ChatMessage.CustomRewardId != null)
            {
                if (IsWindowOpen<Window_SoundAdd>())
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(Window_SoundAdd))
                            {
                                (window as Window_SoundAdd).tb_rewardid.Text = e.ChatMessage.CustomRewardId;
                            }
                        }
                    }));
                }
                else
                {
                    userLevel = 0;

                    if (e.ChatMessage.Badges.FindAll(x => x.Key == "subscriber").Count > 0)
                        userLevel = 1;
                    if (e.ChatMessage.Badges.FindAll(x => x.Key == "moderator").Count > 0)
                        userLevel = 2;
                    if (e.ChatMessage.Badges.FindAll(x => x.Key == "broadcaster").Count > 0)
                        userLevel = 3;

                    SoundObject so = null;


                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(MainWindow))
                            {
                                List<SoundObject> soundObjects = (window as MainWindow).soundObjects.ToList<SoundObject>();
                                so = soundObjects.Find(o => o.RewardID == e.ChatMessage.CustomRewardId);
                            }
                        }
                    }));

                    if (so == null)
                        return;

                    waveOutDevice = new WaveOut();
                    AudioFileReader = new AudioFileReader(so.Path);
                    waveOutDevice.PlaybackStopped += WaveOutDevice_PlaybackStopped;
                    waveOutDevice.Init(AudioFileReader);
                    waveOutDevice.Play();
                }

            }
        }

        private static void WaveOutDevice_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            waveOutDevice?.Stop();
            AudioFileReader?.Dispose();
            waveOutDevice?.Dispose();
        }

        public static bool IsWindowOpen<T>(string name = "") where T : Window
        {
            bool b = false;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                b = string.IsNullOrEmpty(name)
                   ? Application.Current.Windows.OfType<T>().Any()
                   : Application.Current.Windows.OfType<T>().Any(w => w.Name.Equals(name));
            }));
            return b;

        }
    }
}
