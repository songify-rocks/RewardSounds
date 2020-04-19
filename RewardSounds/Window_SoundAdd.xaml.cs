using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using System;
using System.Windows;
using NAudio;
using NAudio.Wave;
using RewardSounds.Models;

namespace RewardSounds
{
    /// <summary>
    /// Interaktionslogik für Window_SoundAdd.xaml
    /// </summary>
    public partial class Window_SoundAdd : Window
    {
        IWavePlayer waveOutDevice;
        AudioFileReader AudioFileReader;

        public Window_SoundAdd()
        {
            InitializeComponent();
        }

        private void btn_searchfile_Click(object sender, RoutedEventArgs e)
        {
            // OpenfileDialog with settings initialdirectory is the path were the exe is located
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
                Filter = @"MP3 (*.mp3)|*.mp3|All files (*.*)|*.*"
            };

            // Opening the dialog and when the user hits "OK" the following code gets executed
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tb_filepath.Text = openFileDialog.FileName;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void btn_play_Click(object sender, RoutedEventArgs e)
        {
            waveOutDevice = new WaveOut();
            AudioFileReader = new AudioFileReader(tb_filepath.Text);
            waveOutDevice.PlaybackStopped += WaveOutDevice_PlaybackStopped;
            waveOutDevice.Init(AudioFileReader);
            waveOutDevice.Play();
        }

        private void WaveOutDevice_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            waveOutDevice?.Stop();
            AudioFileReader?.Dispose();
            waveOutDevice?.Dispose();
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).dgv_sounds.Items.Add(
                new SoundObject
                {
                    Name = tb_name.Text,
                    Path = tb_filepath.Text,
                    RewardID = tb_rewardid.Text,
                    IsActive = true
                });
            Close();
        }
    }
}
