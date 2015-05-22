using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyPlay
{
    class Playlist : Bibliothek
    {
        //Klassenvariabeln
        private bool Wiederholen;
        private string Name;

        //Konstruktor
        public Playlist(List<Lied> lieder, string name) : base(lieder)
        {
            Name = name;
            Wiederholen = false;
        }

        //Methoden
        public void Loeschen()
        {
            foreach (Lied l in Lieder)
            {
                liedLoeschen(l);
            }
        }

        public string getName()
        {
            return Name;
        }
    }
}
