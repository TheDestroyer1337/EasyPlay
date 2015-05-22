using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Threading;
using Microsoft.Win32;
using System.Windows.Forms;

namespace EasyPlay
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Klassenvariabeln
        private MediaPlayer Player;
        private List<Playlist> Playlists;
        private Bibliothek Biblio;
        private Lied AktuelleWiedergabe;
        private Warteliste Wartelist;
        private DispatcherTimer Timer;
        private TimeSpan Pause;
        private Playlist Playlist;
        private bool IsPlaylist;
        
        private enum MyType
        {
            Titel, Interpret, Album, Playlists, Warteliste
        }

        public MainWindow()
        {
            InitializeComponent();

            string saveFile = AppDomain.CurrentDomain.BaseDirectory + "easyplay.txt";
            if (!File.Exists(saveFile))
            {
                FolderBrowserDialog ofd = new FolderBrowserDialog();
                ofd.Description = "Ordner öffnen";
                ofd.ShowNewFolderButton = false;
                bool result = false;
                result = Convert.ToBoolean(ofd.ShowDialog());
                Biblio = new Bibliothek(ofd.SelectedPath);
                this.displayData(MyType.Titel);
            }
            Player = new MediaPlayer();
            BtnPlay.Visibility = Visibility.Visible;
            BtnPause.Visibility = Visibility.Hidden;
            BtnNeuePlaylist.Visibility = Visibility.Hidden;
            BtnPlaylistSpeichern.Visibility = Visibility.Hidden;

            ListViewTitel.Visibility = Visibility.Visible;
            ListViewPlaylists.Visibility = Visibility.Hidden;
            ListViewInterpreten.Visibility = Visibility.Hidden;
            ListViewAlben.Visibility = Visibility.Hidden;

            Playlists = new List<Playlist>();
            Player = new MediaPlayer();   
        }

        private void TitelButton_Clicked(object sender, RoutedEventArgs e)
        {
            ListViewTitel.Visibility = Visibility.Visible;
            ListViewPlaylists.Visibility = Visibility.Hidden;
            ListViewAlben.Visibility = Visibility.Hidden;
            ListViewInterpreten.Visibility = Visibility.Hidden;
            BtnNeuePlaylist.Visibility = Visibility.Hidden;
        }

        private void PlaylistsButton_Clicked(object sender, RoutedEventArgs e)
        {
            ListViewTitel.Visibility = Visibility.Hidden;
            ListViewPlaylists.Visibility = Visibility.Visible;
            ListViewAlben.Visibility = Visibility.Hidden;
            ListViewInterpreten.Visibility = Visibility.Hidden;
            BtnNeuePlaylist.Visibility = Visibility.Visible;
        }

        private void AlbenButton_Clicked(object sender, RoutedEventArgs e)
        {
            ListViewTitel.Visibility = Visibility.Hidden;
            ListViewPlaylists.Visibility = Visibility.Hidden;
            ListViewAlben.Visibility = Visibility.Visible;
            ListViewInterpreten.Visibility = Visibility.Hidden;
            BtnNeuePlaylist.Visibility = Visibility.Hidden;
        }

        private void InterpretenButton_Clicked(object sender, RoutedEventArgs e)
        {
            ListViewTitel.Visibility = Visibility.Hidden;
            ListViewPlaylists.Visibility = Visibility.Hidden;
            ListViewAlben.Visibility = Visibility.Hidden;
            ListViewInterpreten.Visibility = Visibility.Visible;
            BtnNeuePlaylist.Visibility = Visibility.Hidden;
        }

        private void WartelisteButton_Clicked(object sender, RoutedEventArgs e)
        {
            ListViewTitel.Visibility = Visibility.Visible;
            ListViewPlaylists.Visibility = Visibility.Hidden;
            ListViewAlben.Visibility = Visibility.Hidden;
            ListViewInterpreten.Visibility = Visibility.Hidden;
            BtnNeuePlaylist.Visibility = Visibility.Hidden;
        }

        private void Beenden_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnPause_Click(object sender, RoutedEventArgs e)
        {
            Player.Pause();
            BtnPause.Visibility = Visibility.Hidden;
            BtnPlay.Visibility = Visibility.Visible;
        }

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            Player.Play();
            BtnPlay.Visibility = Visibility.Hidden;
            BtnPause.Visibility = Visibility.Visible;
        }

        private void BtnNeuePlaylist_Click(object sender, RoutedEventArgs e)
        {
            InputBox.Visibility = Visibility.Visible;
        }

        private void WeiterButton_Click(object sender, RoutedEventArgs e)
        {
            Playlist = new Playlist(null, InputTextBox.Text);
            InputBox.Visibility = Visibility.Collapsed;
            BtnNeuePlaylist.Visibility = Visibility.Hidden;
            BtnPlaylistSpeichern.Visibility = Visibility.Visible;
            ListViewPlaylists.Visibility = Visibility.Hidden;
            ListViewTitel.Visibility = Visibility.Visible;
            IsPlaylist = true;
        }

        private void AbbrechenButton_Click(object sender, RoutedEventArgs e)
        {
            InputBox.Visibility = Visibility.Collapsed;
        }

        private void BtnPlaylistSpeichern_Click(object sender, RoutedEventArgs e)
        {
            Playlists.Add(Playlist);
            displayData(MyType.Playlists);
//            ListViewPlaylists.Items.Add(new displayPlaylist { Name = Playlist.getName(), AnzLieder = Playlist.getAlllLieder().Count });
            ListViewPlaylists.Visibility = Visibility.Visible;
            ListViewTitel.Visibility = Visibility.Hidden;
            BtnPlaylistSpeichern.Visibility = Visibility.Hidden;
            BtnNeuePlaylist.Visibility = Visibility.Visible;
        }

        private void displayData(MyType type)
        {
            switch (type)
            {
                case MyType.Titel:
                    List<Lied> lieder = Biblio.getAlllLieder();
                    foreach (Lied l in lieder)
                    {
                        ListViewTitel.Items.Add(new displayTitel { Titel = l.getTitel(), Album = l.getAlbum(), Interpret = l.getInterpret(), Dauer = l.getLaenge(), Pfad = l.getPfad() });
                    }
                    break;
                case MyType.Album:
                    break;
                case MyType.Interpret:
                    break;
                case MyType.Playlists:
                    foreach (Playlist p in Playlists)
                    {
                        ListViewTitel.Items.Add(new displayPlaylist { Name = p.getName(), AnzLieder = p.getAlllLieder().Count });
                    }
                    break;
                case MyType.Warteliste:
                    break;
            }
        }

        private void ListViewTitel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsPlaylist)
            {
                displayTitel item = new displayTitel();
                item = (displayTitel)ListViewTitel.SelectedItem;
                foreach (Lied l in Biblio.getAlllLieder())
                {
                    if (item.Pfad == l.getPfad())
                    {
                        Playlist.addLied(l);
                    }
                }
            }
        }

        internal class displayTitel
        {
            public string Titel { get; set; }
            public string Interpret { get; set; }
            public string Album { get; set; }
            public string Dauer { get; set; }
            public string Pfad { get; set; }
        }

        internal class displayPlaylist
        {
            public string Name { get; set; }
            public int AnzLieder { get; set; }
        }

        internal class displayAblum
        {
            public string Album { get; set; }
            public string Interpret { get; set; }
            public int AnzLieder { get; set; }
        }

        internal class displayInterpret
        {
            public string Interpret { get; set; }
            public int AnzLieder { get; set; }
        }
    }
}
