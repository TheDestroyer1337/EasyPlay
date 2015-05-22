using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EasyPlay
{
    [Serializable]
    class Bibliothek
    {
        protected List<Lied> Lieder;
        protected bool Spielt;

        public Bibliothek(string pfad)
        {
            Lieder = new List<Lied>();
            addOrdner(pfad);
            Spielt = false;
        }

        public Bibliothek(List<Lied> lieder)
        {
            if (lieder == null)
                Lieder = new List<Lied>();
            else
                Lieder = lieder;

            Spielt = false;
        }

        public void addLied(string pfad)
        {
            Lied lied = new Lied(pfad);
            Lieder.Add(lied);
        }

        public void addOrdner(string pfad)
        {
            DirectoryInfo parentDirectory = new DirectoryInfo(pfad);
            subDirectories(parentDirectory);
        }

        private void subDirectories(DirectoryInfo directory)
        {
            foreach (FileInfo f in directory.GetFiles())
            {
                if (f.Extension.Equals(".mp3"))
                {
                    addLied(f.FullName);
                }
            }

            foreach (DirectoryInfo f in directory.GetDirectories())
            {
                subDirectories(f);
            }
        }

        public List<Lied> getAllLieder()
        {
            return Lieder;
        }

        public void liedLoeschen(Lied lied)
        {
            Lieder.Remove(lied);
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

        public bool getSpielend()
        {
            return Spielt;
        }

        public void setSpielend(bool value)
        {
            Spielt = value;
        }
    }
}
