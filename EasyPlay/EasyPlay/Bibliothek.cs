﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EasyPlay
{
    class Bibliothek
    {
        private List<Lied> Lieder;
        private bool Spielt;

        public Bibliothek(string pfad)
        {
            Lieder = new List<Lied>();
            addOrdner(pfad);
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
            foreach (DirectoryInfo f in directory.GetDirectories())
            {
                subDirectories(f);
            }

            foreach (FileInfo f in directory.GetFiles())
            {
                if (f.Extension.Equals("mp3"))
                {
                    addLied(directory.FullName + '/' + f.FullName);
                }
            }
        }

        public List<Lied> getAlllLieder()
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
    }
}
