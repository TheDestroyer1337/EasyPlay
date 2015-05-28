using System;
using System.Collections;
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
using System.Runtime.Serialization.Formatters.Binary;

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
        private bool IsInterpret;
        private bool IsAlbum;
        private bool mouseCaptured;
        private double Volume;
        
        private enum MyType
        {
            Titel, Interpret, Album, Playlists, Warteliste
        }

        public MainWindow()
        {
            InitializeComponent();

            string saveFile = AppDomain.CurrentDomain.BaseDirectory + "easyplay.bin";
            if (!File.Exists(saveFile))
            {
                FolderBrowserDialog ofd = new FolderBrowserDialog();
                ofd.Description = "Ordner öffnen";
                ofd.ShowNewFolderButton = false;
                ofd.ShowDialog();
                if (ofd.SelectedPath != "")
                    Biblio = new Bibliothek(ofd.SelectedPath);
                else 
                {
                    this.Close();
                    return;
                }
            }

            BtnPlay.Visibility = Visibility.Visible;
            BtnPause.Visibility = Visibility.Hidden;
            BtnNeuePlaylist.Visibility = Visibility.Hidden;
            BtnPlaylistSpeichern.Visibility = Visibility.Hidden;
            BtnPlaylistLoeschen.Visibility = Visibility.Hidden;
            BtnZuWarteliste.Visibility = Visibility.Visible;
            BtnLaut.Visibility = Visibility.Hidden;
            BtnStumm.Visibility = Visibility.Visible;
            BtnPlaylistWiederholen.Visibility = Visibility.Hidden;
            
            ListViewTitel.Visibility = Visibility.Visible;
            ListViewPlaylists.Visibility = Visibility.Hidden;
            ListViewInterpreten.Visibility = Visibility.Hidden;
            ListViewAlben.Visibility = Visibility.Hidden;

            VolumeSlider.Maximum = 100;
            VolumeSlider.Value = 50;
            mouseCaptured = false;

            Playlists = new List<Playlist>();
            Player = new MediaPlayer();
            Timer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Tick += new EventHandler(PlayTime_Tick);
            Wartelist = new Warteliste(null);
            Player = new MediaPlayer();
            IsInterpret = false;
            IsAlbum = false;
            this.load();
            this.displayData(MyType.Titel);
            Wartelist.setSpielend(false);
            foreach (Playlist p in Playlists)
            {
                p.setSpielend(false);
            }
            Biblio.setSpielend(true);
        }

        private void Player_MediaEnded()
        {
            if (Biblio.getSpielend())
            {
                Lied next = null;
                foreach (Lied l in Biblio.getAllLieder())
                {
                    if (IsInterpret)
                    {
                        if (next != null && next != l && l.getInterpret().Equals(next.getInterpret()))
                        {
                            play(l.getPfad());
                            return;
                        }
                        else if (l.getWiederholen())
                        {
                            play(l.getPfad());
                            return;
                        }
                        if (l.getSpielt() && !l.getWiederholen() && next == null)
                        {
                            next = l;
                            Timer.Stop();
                            l.setSpielt(false);
                        }
                    }
                    else if (IsAlbum)
                    {
                        if (next != null && next != l && l.getInterpret().Equals(next.getInterpret()) && l.getAlbum().Equals(next.getAlbum()))
                        {
                            play(l.getPfad());
                            return;
                        }
                        else if (l.getWiederholen())
                        {
                            play(l.getPfad());
                            return;
                        }
                        if (l.getSpielt() && !l.getWiederholen() && next == null)
                        {
                            next = l;
                            Timer.Stop();
                            l.setSpielt(false);
                        }
                    }
                    else
                    {
                        if (next != null && next != l)
                        {
                            play(l.getPfad());
                            return;
                        }
                        else if (l.getWiederholen())
                        {
                            play(l.getPfad());
                            return;
                        }
                        if (l.getSpielt() && !l.getWiederholen() && next == null)
                        {
                            next = l;
                            Timer.Stop();
                            l.setSpielt(false);
                        }
                    }
                }
                next = null;
            }
            else if (Wartelist.getSpielend())
            {
                Lied next = null;
                foreach (Lied l in Biblio.getAllLieder())
                {
                    if (l.getWartend())
                    {
                        if (next != null)
                        {
                            play(l.getPfad());
                            next = null;
                        }
                        else if (l.getSpielt() && next == null)
                        {
                            next = l;
                            l.setSpielt(false);
                            l.setWartend(false);
                            displayData(MyType.Warteliste);
                            Timer.Stop();
                        }
                    }
                }
            }
            else
            {
                foreach(Playlist p in Playlists)
                {
                    if (p.getSpielend())
                    {
                        Lied next = null;
                        foreach (Lied l in p.getAllLieder())
                        {
                            if (next != null)
                            {
                                play(l.getPfad());
                                next = null;
                            }
                            if (l.getSpielt())
                            {
                                next = l;
                                l.setSpielt(false);
                            }
                        }
                    }
                }
            }
        }

        private void PlayTime_Tick(object sender, EventArgs e)
        {
            LiedSlider.Value += 1;
            if (LiedSlider.Value == LiedSlider.Maximum)
            {
                Player_MediaEnded();
            }
        }

        private void TitelButton_Clicked(object sender, RoutedEventArgs e)
        {
            ListViewTitel.Visibility = Visibility.Visible;
            ListViewPlaylists.Visibility = Visibility.Hidden;
            ListViewAlben.Visibility = Visibility.Hidden;
            ListViewInterpreten.Visibility = Visibility.Hidden;
            BtnNeuePlaylist.Visibility = Visibility.Hidden;
            BtnPlaylistLoeschen.Visibility = Visibility.Hidden;
            BtnZuWarteliste.Visibility = Visibility.Visible;
            BtnPlaylistWiederholen.Visibility = Visibility.Hidden;
            displayData(MyType.Titel);
            Wartelist.setSpielend(false);
            foreach (Playlist p in Playlists)
            {
                p.setSpielend(false);
            }
            Biblio.setSpielend(true);
            IsInterpret = false;
            IsAlbum = false;
        }

        private void PlaylistsButton_Clicked(object sender, RoutedEventArgs e)
        {
            ListViewTitel.Visibility = Visibility.Hidden;
            ListViewPlaylists.Visibility = Visibility.Visible;
            ListViewAlben.Visibility = Visibility.Hidden;
            ListViewInterpreten.Visibility = Visibility.Hidden;
            BtnNeuePlaylist.Visibility = Visibility.Visible;
            BtnPlaylistLoeschen.Visibility = Visibility.Visible;
            BtnZuWarteliste.Visibility = Visibility.Hidden;
            displayData(MyType.Playlists);
            IsInterpret = false;
            IsAlbum = false;
        }

        private void AlbenButton_Clicked(object sender, RoutedEventArgs e)
        {
            ListViewTitel.Visibility = Visibility.Hidden;
            ListViewPlaylists.Visibility = Visibility.Hidden;
            ListViewAlben.Visibility = Visibility.Visible;
            ListViewInterpreten.Visibility = Visibility.Hidden;
            BtnNeuePlaylist.Visibility = Visibility.Hidden;
            BtnPlaylistLoeschen.Visibility = Visibility.Hidden;
            BtnZuWarteliste.Visibility = Visibility.Hidden;
            displayData(MyType.Album);
            Wartelist.setSpielend(false);
            foreach (Playlist p in Playlists)
            {
                p.setSpielend(false);
            }
            Biblio.setSpielend(true);
            IsInterpret = false;
            IsAlbum = true;
        }

        private void InterpretenButton_Clicked(object sender, RoutedEventArgs e)
        {
            ListViewTitel.Visibility = Visibility.Hidden;
            ListViewPlaylists.Visibility = Visibility.Hidden;
            ListViewAlben.Visibility = Visibility.Hidden;
            ListViewInterpreten.Visibility = Visibility.Visible;
            BtnNeuePlaylist.Visibility = Visibility.Hidden;
            BtnPlaylistLoeschen.Visibility = Visibility.Hidden;
            BtnZuWarteliste.Visibility = Visibility.Hidden;
            displayData(MyType.Interpret);
            Wartelist.setSpielend(false);
            foreach (Playlist p in Playlists)
            {
                p.setSpielend(false);
            }
            Biblio.setSpielend(true);
            IsInterpret = true;
            IsAlbum = false;
        }

        private void WartelisteButton_Clicked(object sender, RoutedEventArgs e)
        {
            ListViewTitel.Visibility = Visibility.Visible;
            ListViewPlaylists.Visibility = Visibility.Hidden;
            ListViewAlben.Visibility = Visibility.Hidden;
            ListViewInterpreten.Visibility = Visibility.Hidden;
            BtnNeuePlaylist.Visibility = Visibility.Hidden;
            BtnPlaylistLoeschen.Visibility = Visibility.Hidden;
            BtnZuWarteliste.Visibility = Visibility.Hidden;
            BtnPlaylistWiederholen.Visibility = Visibility.Hidden;
            displayData(MyType.Warteliste);
            Wartelist.setSpielend(true);
            foreach (Playlist p in Playlists)
            {
                p.setSpielend(false);
            }
            Biblio.setSpielend(false);
            IsInterpret = false;
            IsAlbum = false;
        }

        private void Beenden_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnPause_Click(object sender, RoutedEventArgs e)
        {
            Player.Pause();
            Timer.Stop();
            BtnPause.Visibility = Visibility.Hidden;
            BtnPlay.Visibility = Visibility.Visible;
        }

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            Player.Play();
            Timer.Start();
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
            BtnPlaylistLoeschen.Visibility = Visibility.Hidden;
            BtnPlaylistSpeichern.Visibility = Visibility.Visible;
            ListViewPlaylists.Visibility = Visibility.Hidden;
            ListViewTitel.Visibility = Visibility.Visible;
            BtnZuWarteliste.Visibility = Visibility.Hidden;
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
            ListViewPlaylists.Visibility = Visibility.Visible;
            ListViewTitel.Visibility = Visibility.Hidden;
            BtnPlaylistSpeichern.Visibility = Visibility.Hidden;
            BtnNeuePlaylist.Visibility = Visibility.Visible;
            BtnPlaylistLoeschen.Visibility = Visibility.Visible;
            BtnZuWarteliste.Visibility = Visibility.Hidden;
            IsPlaylist = false;
        }

        private void displayData(MyType type)
        {
            switch (type)
            {
                case MyType.Titel:
                    ListViewTitel.Items.Clear();
                    foreach (Lied l in Biblio.getAllLieder())
                        ListViewTitel.Items.Add(new displayTitel { Titel = l.getTitel(), Album = l.getAlbum(), Interpret = l.getInterpret(), Dauer = l.getLaenge(), Pfad = l.getPfad() });
                    break;
                case MyType.Album:
                    ListViewAlben.Items.Clear();
                    List<string> alben = new List<string>();
                    Hashtable aInterpret = new Hashtable();
                    int count = 0;
                    foreach (Lied l in Biblio.getAllLieder())
                    {
                        try 
                        {
                            alben.Add(l.getAlbum());
                            aInterpret.Add(l.getAlbum(), l.getInterpret());
                        }
                        catch 
                        {

                        }
                    }
                    foreach (DictionaryEntry de in aInterpret)
                    {
                        count = 0;
                        foreach (Lied l in Biblio.getAllLieder())
                        {
                            if(de.Key.Equals(l.getAlbum()) && de.Value.Equals(l.getInterpret()))
                                count++;
                        }
                        ListViewAlben.Items.Add(new displayAlbum { Album = de.Key.ToString(), AnzLieder = count, Interpret = de.Value.ToString() });
                    }
                    break;
                case MyType.Interpret:
                    ListViewInterpreten.Items.Clear();
                    List<string> interpreten = new List<string>();
                    Hashtable anzInterpreten = new Hashtable();
                    bool existInt = false;
                    int countInt = 0;
                    foreach (Lied l in Biblio.getAllLieder())
                    {
                        foreach (string s in interpreten) 
                        {
                            if (s.Equals(l.getInterpret()))
                                existInt = true;
                        }
                        if(!existInt)
                            interpreten.Add(l.getInterpret());
                        existInt = false;
                    }
                    foreach (string s in interpreten)
                    {
                        countInt = 0;
                        foreach (Lied l in Biblio.getAllLieder())
                        {
                            if(l.getInterpret().Equals(s))
                                countInt++;
                        }
                        ListViewInterpreten.Items.Add(new displayInterpret { AnzLieder = countInt, Interpret = s });
                    }
                    break;
                case MyType.Playlists:
                    ListViewPlaylists.Items.Clear();
                    foreach (Playlist p in Playlists)
                        ListViewPlaylists.Items.Add(new displayPlaylist { Name = p.getName(), AnzLieder = p.getAllLieder().Count });
                    break;
                case MyType.Warteliste:
                    ListViewTitel.Items.Clear();
                    foreach (Lied l in Biblio.getAllLieder())
                    {
                        if(l.getWartend())
                            ListViewTitel.Items.Add(new displayTitel { Titel = l.getTitel(), Album = l.getAlbum(), Interpret = l.getInterpret(), Dauer = l.getLaenge(), Pfad = l.getPfad() });
                    }
                    break;
            }
        }

        private void ListViewTitel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsPlaylist)
            {
                displayTitel item = new displayTitel();
                item = (displayTitel)ListViewTitel.SelectedItem;
                foreach (Lied l in Biblio.getAllLieder())
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

        internal class displayAlbum
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

        private void ListViewPlaylists_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            displayPlaylist item = new displayPlaylist();
            item = (displayPlaylist)ListViewPlaylists.SelectedItem;
            foreach (Playlist p in Playlists)
            {
                if (item.Name == p.getName())
                {
                    BtnPlaylistWiederholen.Visibility = Visibility.Visible;
                    ListViewTitel.Items.Clear();
                    ListViewPlaylists.Visibility = Visibility.Hidden;
                    ListViewTitel.Visibility = Visibility.Visible;
                    BtnNeuePlaylist.Visibility = Visibility.Hidden;
                    BtnPlaylistLoeschen.Visibility = Visibility.Hidden;
                    BtnZuWarteliste.Visibility = Visibility.Visible;
                    p.setSpielend(true);
                    foreach (Lied l in p.getAllLieder())
                    {
                        ListViewTitel.Items.Add(new displayTitel { Titel = l.getTitel(), Album = l.getAlbum(), Interpret = l.getInterpret(), Dauer = l.getLaenge(), Pfad = l.getPfad() });
                    }
                    Wartelist.setSpielend(false);
                    p.setSpielend(true);
                    Biblio.setSpielend(false);
                }
            }
        }

        private void save()
        {
            if (Biblio != null && Playlists != null && Wartelist != null) 
            {
                string saveFile = AppDomain.CurrentDomain.BaseDirectory + "easyplay.bin";
                FileStream fs = new FileStream(saveFile, FileMode.Create);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, Biblio);
                formatter.Serialize(fs, Playlists);
                formatter.Serialize(fs, Wartelist);
                fs.Close();
            }
        }

        private void load()
        {
            string saveFile = AppDomain.CurrentDomain.BaseDirectory + "easyplay.bin";
            if (File.Exists(saveFile))
            {
                FileStream fs = new FileStream(saveFile, FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                Biblio = (Bibliothek)formatter.Deserialize(fs);
                Playlists = (List<Playlist>)formatter.Deserialize(fs);
                Wartelist = (Warteliste)formatter.Deserialize(fs);
                fs.Close();
                foreach(Lied l in Biblio.getAllLieder())
                {
                    l.setWiederholen(false);
                }
            }
        }
        private void BtnPlaylistLoeschen_Click(object sender, RoutedEventArgs e)
        {
            displayPlaylist item = new displayPlaylist();
            item = (displayPlaylist)ListViewPlaylists.SelectedItem;
            foreach (Playlist p in Playlists)
            {
                if (item.Name == p.getName())
                {
                    Playlists.Remove(p);
                    displayData(MyType.Playlists);
                    break;
                }
            }
        }

        private void BtnZuWarteliste_Click(object sender, RoutedEventArgs e)
        {
            displayTitel item = new displayTitel();
            item = (displayTitel)ListViewTitel.SelectedItem;
            foreach (Lied l in Biblio.getAllLieder())
            {
                if (item.Pfad == l.getPfad())
                {
                    Wartelist.addLied(l);
                    l.setWartend(true);
                }
            }
        }

        private void FrmMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            save();
        }

        private void ListViewTitel_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            displayTitel item = new displayTitel();
            item = (displayTitel)ListViewTitel.SelectedItem;
            play(item.Pfad);
            foreach (Lied l in Biblio.getAllLieder())
            {
                if (l.getPfad() == item.Pfad)
                    l.setSpielt(true);
            }
        }

        private void play(string pfad)
        {
            Player.Open(new Uri(pfad));
            foreach (Lied l in Biblio.getAllLieder())
            {
                if (l.getPfad() == pfad)
                {
                    l.setSpielt(true);
                    LblLiedName.Content = l.getTitel();
                }
            }
            Player.Play();
            while (!Player.NaturalDuration.HasTimeSpan)
            {

            }
            Duration d = Player.NaturalDuration;
            string Splittit = d.ToString();
            string[] splitD = Splittit.Split(new char[] { ':', '.' });
            int std = Convert.ToInt16(splitD[0]);
            int min = Convert.ToInt16(splitD[1]);
            int sec = Convert.ToInt16(splitD[2]);
            std = std * 3600;
            min = min * 60;
            sec += min + std;
            LiedSlider.Maximum = sec;
            Timer.Start();
            if (Pause != new TimeSpan(0))
            {
                Player.Position = Pause;
                Pause = new TimeSpan(0);
            }
            else
            {
                LiedSlider.Value = 0;
            }
            BtnPlay.Visibility = Visibility.Hidden;
            BtnPause.Visibility = Visibility.Visible;
        }

        private void LiedSlider_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            int value = Convert.ToInt16(LiedSlider.Value);
            TimeSpan time = new TimeSpan(0, 0, value);
            Player.Position = time;
        }

        private void VolumeSlider_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed && mouseCaptured)
            {
                var x = e.GetPosition(VolumeSlider).X;
                var ratio = x / VolumeSlider.ActualWidth;
                Volume = ratio * VolumeSlider.Maximum;
                Player.Volume = Volume / 100;
            }
        }

        private void VolumeSlider_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mouseCaptured = true;
        }

        private void VolumeSlider_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            mouseCaptured = false;
        }

        private void BtnStumm_Click(object sender, RoutedEventArgs e)
        {
            Volume = Player.Volume * 100;
            Player.Volume = 0;
            VolumeSlider.Value = Player.Volume;
            BtnStumm.Visibility = Visibility.Hidden;
            BtnLaut.Visibility = Visibility.Visible;
        }

        private void BtnLaut_Click(object sender, RoutedEventArgs e)
        {
            VolumeSlider.Value = Volume;
            Player.Volume = VolumeSlider.Value / 100;            
            BtnLaut.Visibility = Visibility.Hidden;
            BtnStumm.Visibility = Visibility.Visible;
        }

        private void BtnLiedWiederholen_Click(object sender, RoutedEventArgs e)
        {
            displayTitel item = new displayTitel();
            item = (displayTitel)ListViewTitel.SelectedItem;
            foreach (Lied l in Biblio.getAllLieder())
            {
                if (l.getWiederholen())
                {
                    BtnLiedWiederholen.BorderBrush = new SolidColorBrush(Colors.White); 
                    l.setWiederholen(false);
                }
                else
                {
                    if (l.getPfad() == item.Pfad)
                    {
                        BtnLiedWiederholen.BorderBrush = new SolidColorBrush(Colors.Orange);
                        l.setWiederholen(true);
                    }
                }
            }
        }

        private void BtnPlaylistWiederholen_Click(object sender, RoutedEventArgs e)
        {
            foreach (Playlist p in Playlists)
            {
                if (p.getSpielend())
                {
                    if (p.getWiederholen())
                    {
                        BtnPlaylistWiederholen.BorderBrush = new SolidColorBrush(Colors.White);
                        p.setWiederholen(false);
                    }
                    else
                    {
                        BtnPlaylistWiederholen.BorderBrush = new SolidColorBrush(Colors.Orange);
                        p.setWiederholen(true);
                    }
                }
            }
        }

        private void MenuAddLieder_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Filter = "MP3 Dateien (*.mp3)|*.mp3";
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            ofd.Title = "Lied öffenen";
            bool result = false;
            result = Convert.ToBoolean(ofd.ShowDialog());
            if (result)
            {
                Biblio.addLied(ofd.FileName);
                displayData(MyType.Titel);
            }
        }

        private void MenuAddOrdner_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog ofd = new FolderBrowserDialog();
            ofd.Description = "Ordner öffnen";
            ofd.ShowNewFolderButton = false;
            bool result = false;
            result = Convert.ToBoolean(ofd.ShowDialog());
            if (ofd.SelectedPath != "") 
                Biblio.addOrdner(ofd.SelectedPath);
            displayData(MyType.Titel);
        }

        private void MenuWartelisteLeeren_Click(object sender, RoutedEventArgs e)
        {
            foreach (Lied l in Biblio.getAllLieder())
                l.setWartend(false);
        }

        private void ListViewInterpreten_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            displayInterpret item = new displayInterpret();
            item = (displayInterpret)ListViewInterpreten.SelectedItem;
            ListViewTitel.Visibility = Visibility.Visible;
            ListViewPlaylists.Visibility = Visibility.Hidden;
            ListViewAlben.Visibility = Visibility.Hidden;
            ListViewInterpreten.Visibility = Visibility.Hidden;
            BtnNeuePlaylist.Visibility = Visibility.Hidden;
            BtnPlaylistLoeschen.Visibility = Visibility.Hidden;
            BtnZuWarteliste.Visibility = Visibility.Visible;
            BtnPlaylistWiederholen.Visibility = Visibility.Hidden;
            ListViewTitel.Items.Clear();
            foreach (Lied l in Biblio.getAllLieder()) 
            {
                if (l.getInterpret().Equals(item.Interpret))
                    ListViewTitel.Items.Add(new displayTitel { Titel = l.getTitel(), Album = l.getAlbum(), Interpret = l.getInterpret(), Dauer = l.getLaenge(), Pfad = l.getPfad() });
                
            }
        }

        private void ListViewAlben_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            displayAlbum item = new displayAlbum();
            item = (displayAlbum)ListViewAlben.SelectedItem;
            ListViewTitel.Visibility = Visibility.Visible;
            ListViewPlaylists.Visibility = Visibility.Hidden;
            ListViewAlben.Visibility = Visibility.Hidden;
            ListViewInterpreten.Visibility = Visibility.Hidden;
            BtnNeuePlaylist.Visibility = Visibility.Hidden;
            BtnPlaylistLoeschen.Visibility = Visibility.Hidden;
            BtnZuWarteliste.Visibility = Visibility.Visible;
            BtnPlaylistWiederholen.Visibility = Visibility.Hidden;
            ListViewTitel.Items.Clear();
            foreach (Lied l in Biblio.getAllLieder())
            {
                if (l.getInterpret().Equals(item.Interpret) && l.getAlbum().Equals(item.Album))
                    ListViewTitel.Items.Add(new displayTitel { Titel = l.getTitel(), Album = l.getAlbum(), Interpret = l.getInterpret(), Dauer = l.getLaenge(), Pfad = l.getPfad() });
            }
        }
    }
}