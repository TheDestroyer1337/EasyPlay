using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyPlay
{
    class Bibliothek
    {
        private List<Lied> Lieder;
        private bool Spielt;

        public Bibliothek(List<Lied> lieder, bool spielt)
        {
            Lieder = lieder;
            Spielt = spielt;
        }

        public void addLied(string pfad)
        {
            Lied Lied = null;



            Lieder.Add(Lied);
        }

        public void addOrdner(string pfad)
        {

        }

        public List<Lied> getAlllLieder()
        {
            return Lieder;
        }

        public void liedLoeschen(string pfad)
        {

        }

        public Lied getSpielendesLied()
        {
            foreach (Lied lied in Lieder)
            {
                if (lied.getSpielt())
                    return lied;
            }
            return null;
        }
    }
}
