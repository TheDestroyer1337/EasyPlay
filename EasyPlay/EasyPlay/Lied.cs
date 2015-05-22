using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Media;
using System.Windows.Media;
using System.Windows;
using ID3TagLib;

namespace EasyPlay
{
    [Serializable]
    class Lied
    {
        //Klassenvariabeln
        private string Pfad;
        private string Laenge;
        private bool Spielt;
        private string Titel;
        private string Interpret;
        private string Album;
        private bool Wiederholen;
        private bool Wartend;

        //Konstruktor
        public Lied(string pfad)
        {
            string [] split = pfad.Split('.');
            int length = split.Length - 1;
            //Prüfen ob es eine .mp3 Datei ist
            if (split[length].Equals("mp3"))
            {
                Pfad = pfad;
                
                ID3File tag = new ID3File(Pfad);
                ID3v2Tag v2Tag = tag.ID3v2Tag;
                TextFrame f = v2Tag.Frames[FrameFactory.AlbumFrameId] as TextFrame;
                if (f != null)
                    Album = f.Text;
                f = v2Tag.Frames[FrameFactory.TitleFrameId] as TextFrame;
                if (f != null)
                    Titel = f.Text;
                f = v2Tag.Frames[FrameFactory.BandFrameId] as TextFrame;
                if(f != null)
                    Interpret = f.Text;

                Spielt = false;
                Wiederholen = false;
                Wartend = false;
                
                MediaPlayer player = new MediaPlayer();
                player.Open(new Uri(Pfad));
                //Laenge = player.NaturalDuration.TimeSpan.ToString();
                while (!player.NaturalDuration.HasTimeSpan)
                {

                }
                Duration d = player.NaturalDuration;
                string Splittit = d.ToString();
                string[] splitD = Splittit.Split(new char[] { ':', '.' });
                int std = Convert.ToInt16(splitD[0]);
                int min = Convert.ToInt16(splitD[1]);
                int sec = Convert.ToInt16(splitD[2]);
                Laenge = std + ":" + min + ":" + sec;
            }
        }
        //Klassenmethoden
        public void setSpielt(bool value)
        {
            Spielt = value;
        }

        public bool getSpielt()
        {
            return Spielt;
        }

        public void setWiederholen(bool value)
        {
            Wiederholen = value;
        }

        public bool getWiederholen()
        {
            return Wiederholen;
        }

        public void setWartend(bool value)
        {
            Wartend = value;
        }

        public bool getWartend()
        {
            return Wartend;
        }

        public string getPfad()
        {
            return Pfad;
        }

        public string getLaenge()
        {
            return Laenge;
        }

        public string getTitel()
        {
            return Titel;
        }

        public string getInterpret()
        {
            return Interpret;
        }

        public string getAlbum()
        {
            return Album;
        }
    }
}
