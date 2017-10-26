using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication29
{
    class Trasa
    {
        public static Random liczbamiast = new Random();
        private int[] tablicamiast = new int[0]; //przez jakie miasta przechodzi trasa [0,3,2,6,4]
        public int[] pobierz_tablice_miast { get { return this.tablicamiast; } set { tablicamiast = value; } }
        //zrobic ja stayczna
        public static int[,] tablica = new int[0, 0];
        public int ocena { get; set; }
        public int wspolczynnik_procentowy { get; set; }
        public static int wielkosc_tablicy;
        static Trasa()
        {
           
        }
        public Trasa(int i)
        {
            tablicamiast = new int[i];
            uzupełnienie_odległosci_miast();
            losowanie_trasy();
            ocena = this.obliczenie_odleglosci();
        }
        public Trasa(Trasa t1)
        {
            this.ocena = t1.ocena;
            this.wspolczynnik_procentowy = t1.wspolczynnik_procentowy;
            this.tablicamiast = t1.tablicamiast;
        }

        public void losowanie_trasy()
        {
            for (int j = 0; j < tablicamiast.Length; j++)
            {
                if (j == 0)
                    tablicamiast[j] = Trasa.liczbamiast.Next(0, Trasa.tablica.GetLength(0));
                else
                {
                    int liczba = 0;
                    bool pombool = true;
                    while (pombool)
                    {
                        liczba = Trasa.liczbamiast.Next(0, Trasa.tablica.GetLength(0));
                        pombool = false;
                        for (int z = 0; z < j; z++)
                        {
                            if (tablicamiast[z] == liczba)
                            {
                                pombool = true;
                                break;
                            }

                        }
                    }
                    tablicamiast[j] = liczba;
                }
            }
        }
        public void pokaz_tablice_Miast()
        {

            foreach (int f in tablicamiast)
                Console.Write(f + " ");
            Console.WriteLine();
        }
        //tez ja zrobic statyczna
        public static void uzupełnienie_odległosci_miast()
        {
            StreamReader sr = new StreamReader(@"C:\Users\Mar\Desktop\berlin52.txt");
            String tekst = sr.ReadLine();
            tekst=tekst.Trim();
            Trasa.wielkosc_tablicy = int.Parse(tekst);
            Trasa.tablica = new int[Int16.Parse(tekst), Int16.Parse(tekst)];
            String teskt2 = sr.ReadToEnd();
            sr.Close();
            String[] tekst3 = teskt2.Split(' ');
            int pom = 0;
            for (int i = 0; i < Trasa.tablica.GetLength(0); i++)
            {
                for (int y = 0; y <= i; y++)
                {
                    Trasa.tablica[i, y] = Int16.Parse(tekst3[pom]);
                    Trasa.tablica[y, i] = Trasa.tablica[i, y];
                    pom++;
                }
            }
                
            
        }
        
        public int obliczenie_odleglosci()
        {
            int pierwsze = 0;
            int drugie = 0;
            int odleglosc = 0;
            for (int i = 0; i + 1 < tablicamiast.Length; i++)
            {
                pierwsze = tablicamiast[i];
                drugie = tablicamiast[i + 1];
                odleglosc += Trasa.tablica[pierwsze, drugie];

            }
            odleglosc += Trasa.tablica[tablicamiast[0], tablicamiast[tablicamiast.Length - 1]];
            return odleglosc;
        }
        public override string ToString()
        {
            string tekst="";
            foreach (int x in tablicamiast)
                tekst += x + "-";
            tekst += " " + this.ocena;
            return tekst;
        }

    }
}
