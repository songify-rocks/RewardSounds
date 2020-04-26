using RewardSounds.Models;
using RewardSounds.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace RewardSounds
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<SoundObject> soundObjects = new ObservableCollection<SoundObject>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.twitch_autoconnect)
                Twitch.BotConnect();

            LoadConfig();
            AddItems(soundObjects);
            soundObjects.CollectionChanged += SoundObjects_CollectionChanged;
        }

        private void SoundObjects_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SaveConfig();
        }

        private void SaveConfig()
        {
            Config.WriteToJsonFile("config.json", soundObjects);
            AddItems(soundObjects);
        }

        public void AddItems(ObservableCollection<SoundObject> soundObjects)
        {
            dgv_sounds.Items.Clear();
            foreach (SoundObject soundObject in soundObjects)
            {
                dgv_sounds.Items.Add(soundObject);
            }
        }

        private void LoadConfig()
        {
            soundObjects = Config.ReadFromJsonFile<ObservableCollection<SoundObject>>("config.json");
        }

        private void Dgv_Delete(object sender, RoutedEventArgs e)
        {
            soundObjects.Remove((SoundObject)dgv_sounds.SelectedItem);
        }

        private void Dgv_Add(object sender, RoutedEventArgs e)
        {
            Window_SoundAdd window_SoundAdd = new Window_SoundAdd();
            window_SoundAdd.ShowDialog();
        }

        private void Dgv_EnableDisable(object sender, RoutedEventArgs e)
        {
            if (dgv_sounds.SelectedItem == null)
                return;
            soundObjects.Where(so => so.Name == (dgv_sounds.SelectedItem as SoundObject).Name).Select(usr => { usr.IsActive = !usr.IsActive; return usr; }).ToList();
            SaveConfig();
        }

        private void Dgv_Modify(object sender, RoutedEventArgs e)
        {
            SoundObject soundObject = (SoundObject)dgv_sounds.SelectedItem;
            Window_SoundAdd window_SoundAdd = new Window_SoundAdd(soundObject);
            window_SoundAdd.ShowDialog();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Window_Settings _Settings = new Window_Settings();
            _Settings.ShowDialog();
        }

        private void btn_TwitchConnect(object sender, RoutedEventArgs e)
        {
            if (Twitch.IsConnected())
            {
                Twitch._client.Disconnect();
            }
            else
            {
                Twitch.BotConnect();
            }
        }

        private void MenuItem_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!Twitch.IsConnected())
            {
                btn_twitchConnect.IsEnabled = true;
                btn_twitchDisconnect.IsEnabled = false;
            }
            else
            {
                btn_twitchConnect.IsEnabled = false;
                btn_twitchDisconnect.IsEnabled = true;
            }
        }

        private void dgv_Context_Opened(object sender, RoutedEventArgs e)
        {
            if (dgv_sounds.SelectedItem == null)
            {
                (dgv_Context.Items[1] as MenuItem).IsEnabled = false;
                (dgv_Context.Items[2] as MenuItem).IsEnabled = false;
            }
            else
            {
                foreach (MenuItem item in dgv_Context.Items)
                {
                    item.IsEnabled = true;
                }
            }
        }
    }
}
