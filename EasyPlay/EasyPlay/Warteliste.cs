using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyPlay
{
    [Serializable]
    class Warteliste : Bibliothek
    {
        public Warteliste(List<Lied> lieder) : base(lieder)
        {
            if (lieder == null)
                Lieder = new List<Lied>();
            else
                Lieder = lieder;
        }

        //Methoden
        public void Leeren()
        {
            foreach (Lied l in Lieder)
            {
                liedLoeschen(l);
            }
        }

        public void Gespielt(Lied lied)
        {
            if(lied != null)
                liedLoeschen(lied);
        }

        public new void addLied(Lied l)
        {
            Lieder.Add(l);
        }
    }
}
