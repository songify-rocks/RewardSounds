using RewardSounds.Models;
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

namespace RewardSounds
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static List<SoundObject> soundObjects;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            soundObjects = new List<SoundObject>();
            LoadConfig();
        }

        private void LoadConfig()
        {

        }

        private void AddSound(SoundObject soundObject)
        {
            soundObjects.Add(soundObject);
        }

        private void Dgv_Delete(object sender, RoutedEventArgs e)
        {

        }

        private void Dgv_Add(object sender, RoutedEventArgs e)
        {
            Window_SoundAdd window_SoundAdd = new Window_SoundAdd();
            window_SoundAdd.ShowDialog();
        }
    }
}
