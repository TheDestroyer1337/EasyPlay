using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Media;
using System.Windows.Media;
using System.Windows;

namespace EasyPlay
{
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
                FileInfo file = new FileInfo(Pfad);
                Stream s = file.OpenRead();

                byte [] bytes = new byte[128];
                s.Seek(-128, SeekOrigin.End);
                int numBytesToRead = 128;
                int numBytesRead = 0;
                while (numBytesToRead > 0)
                {
                    int n = s.Read(bytes, numBytesRead, numBytesToRead);

                    if (n == 0)
                        break;
                    numBytesRead += n;
                    numBytesToRead -= n;
                }
                Titel = ConvertByteToString(bytes, 3, 32);
                Interpret = ConvertByteToString(bytes, 33, 62);
                Album = ConvertByteToString(bytes, 63, 92);

                Spielt = false;
                Wiederholen = false;
                Wartend = false;
                
                MediaPlayer player = new MediaPlayer();
                player.Open(new Uri(Pfad));
                //Laenge = player.NaturalDuration.TimeSpan.ToString();
                Duration d = player.NaturalDuration;
                string Splittit = d.ToString();
                string[] splitD = Splittit.Split(new char[] { ':', '.' });
                int std = Convert.ToInt16(splitD[0]);
                int min = Convert.ToInt16(splitD[1]);
                int sec = Convert.ToInt16(splitD[2]);
                Laenge = std + ":" + min + ":" + sec;
            }
        }
        //Methode um Bytes in Strings zu konvertieren
        private static String ConvertByteToString(byte[] bytes, int pos1, int pos2)
        {
            if ((pos1 > pos2) || (pos2 > bytes.Length - 1))
                throw new ArgumentException("Aruments out of range");

            int length = pos2 - pos1 + 1;

            char[] chars = new char[length];

            for (int i = 0; i < length; i++)
                chars[i] = Convert.ToChar(bytes[i + pos1]);

            return new String(chars);
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
