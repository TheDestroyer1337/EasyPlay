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
using System.Windows.Media;

namespace EasyPlay
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MediaPlayer Player;
        public MainWindow()
        {
            InitializeComponent();

            TitelGrid.Visibility = Visibility.Visible;
            PlaylistGrid.Visibility = Visibility.Hidden;
            InterpretenGrid.Visibility = Visibility.Hidden;
            AlbenGrid.Visibility = Visibility.Hidden;
            
            Player = new MediaPlayer();
        }

        private void TitelButton_Clicked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Titel button is clicked.");
            TitelGrid.Visibility = Visibility.Visible;
            PlaylistGrid.Visibility = Visibility.Hidden;
            AlbenGrid.Visibility = Visibility.Hidden;
            InterpretenGrid.Visibility = Visibility.Hidden;
        }

        private void PlaylistsButton_Clicked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Playlists button is clicked.");
            TitelGrid.Visibility = Visibility.Hidden;
            PlaylistGrid.Visibility = Visibility.Visible;
            AlbenGrid.Visibility = Visibility.Hidden;
            InterpretenGrid.Visibility = Visibility.Hidden;
        }

        private void AlbenButton_Clicked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Alben button is clicked.");
            TitelGrid.Visibility = Visibility.Hidden;
            PlaylistGrid.Visibility = Visibility.Hidden;
            AlbenGrid.Visibility = Visibility.Visible;
            InterpretenGrid.Visibility = Visibility.Hidden;
        }

        private void InterpretenButton_Clicked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Interpreten button is clicked.");
            TitelGrid.Visibility = Visibility.Hidden;
            PlaylistGrid.Visibility = Visibility.Hidden;
            AlbenGrid.Visibility = Visibility.Hidden;
            InterpretenGrid.Visibility = Visibility.Visible;
        }

        private void WartelisteButton_Clicked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Warteliste button is clicked.");
            TitelGrid.Visibility = Visibility.Visible;
            PlaylistGrid.Visibility = Visibility.Hidden;
            AlbenGrid.Visibility = Visibility.Hidden;
            InterpretenGrid.Visibility = Visibility.Hidden;
        }

        private void Beenden_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
