using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication29
{
    class Analiza
    {
        private Trasa[] tablica_tras = new Trasa[0];
        private int min = 0;
        private int max = 0;
        private int suma_procentow = 0;
        public int szansa_mutacji = 2;
        public int szansa_krzyzowania = 85;
        public Trasa najlepsza;
        Random losowa;
        public Analiza(int liczba_tras)
        {
            losowa = new Random();
            Trasa.uzupełnienie_odległosci_miast();
            tablica_tras = new Trasa[liczba_tras];
            for (int i = 0; i < liczba_tras; i++)
                tablica_tras[i] = new Trasa(Trasa.wielkosc_tablicy);
            najlepsza = new Trasa(tablica_tras[0]);

        }
        public void ponowne_obliczanie_odleglosci()
        {
            foreach (Trasa t in tablica_tras)
                t.ocena = t.obliczenie_odleglosci();
        }
        public string Najlepsza_Trasa()
        {
            Console.WriteLine(najlepsza.ToString() + " jej ocena " + najlepsza.ocena);
            return najlepsza.ToString();
        }
        public void policz_sume_min_max()
        {
            max = 0;
            min = najlepsza.ocena;
            for (int i = 0; i < tablica_tras.Length; i++)
            {

                if (max < tablica_tras[i].ocena)
                    max = tablica_tras[i].ocena;
                if (min > tablica_tras[i].ocena)
                {
                    najlepsza = new Trasa(tablica_tras[i]);
                    min = tablica_tras[i].ocena;
                }
            }
        }
        //zmienic wzor na max z ocen - ocena / suma
        public void oblicz_procenty()
        {
            // zmien sume zamienic na zmienna statyczna w trasie
            int suma = 0;
            for (int i = 0; i < tablica_tras.Length; i++)
            {
                suma += tablica_tras[i].ocena;
            }

            for (int i = 0; i < tablica_tras.Length; i++)
            {
                tablica_tras[i].wspolczynnik_procentowy = suma_procentow;
                suma_procentow += (int)((((max + 1) - tablica_tras[i].ocena) / (double)suma) * 1000000);
                if ((int)((((max + 1) - tablica_tras[i].ocena) / (double)suma) * 1000000) == 0)
                    suma_procentow++;
            }

        }
        public void selekcja_ruletka()
        {
            Trasa[] tablica_tras2 = new Trasa[tablica_tras.Length];
            for (int y = 0; y < tablica_tras.Length; y++)
            {
                int liczba = losowa.Next(suma_procentow);

                for (int i = tablica_tras.Length - 1; i > -1; i--)
                {
                    if (liczba >= tablica_tras[i].wspolczynnik_procentowy)
                    {
                        tablica_tras2[y] = tablica_tras[i];
                        break;
                    }
                }
            }
            for (int i = 0; i < tablica_tras.Length; i++)
                tablica_tras[i] = new Trasa(tablica_tras2[i]);
            suma_procentow = 0;
        }
        public void selekcja_turniej()
        {
            Trasa[] nowa_tablica = new Trasa[tablica_tras.Length];
            int min = 0;
            int kiedy = 0; //element z jakim indeksem wygral
            for (int i = 0; i < tablica_tras.Length; i++)
            {
                int[] tablica = losowanie_liczb(); //losowanie 5 losowych liczb
                Trasa[] tablica_pomocnicza = new Trasa[5];
                min = tablica_tras[tablica[0]].ocena;
                for (int y = 0; y < tablica_pomocnicza.Length; y++)
                {
                    tablica_pomocnicza[y] = new Trasa(tablica_tras[tablica[y]]);

                    if (min > tablica_pomocnicza[y].ocena)
                    {
                        min = tablica_pomocnicza[y].ocena;
                        kiedy = y;
                    }
                }
                nowa_tablica[i] = tablica_pomocnicza[kiedy];
            }
            for (int i = 0; i < tablica_tras.Length; i++)
                tablica_tras[i] = new Trasa(nowa_tablica[i]);
            suma_procentow = 0;
        }
        private int[] losowanie_liczb() // losowanie osobnikow do selekcji turnieju
        {
            int[] tablica_liczb = new int[5];
            for (int j = 0; j < 5; j++)
            {
                if (j == 0)
                    tablica_liczb[j] = Trasa.liczbamiast.Next(0, tablica_tras.Length - 1);
                else
                {
                    int liczba = 0;
                    bool pombool = true;
                    while (pombool)
                    {
                        liczba = Trasa.liczbamiast.Next(0, tablica_tras.Length - 1);
                        pombool = false;
                        for (int z = 0; z < j; z++)
                        {


                            if (tablica_liczb[z] == liczba)
                                pombool = true;

                        }
                    }
                    tablica_liczb[j] = liczba;
                }
            }
            return tablica_liczb;
        }
        public void krzyzowanie_pmx(int liczba)
        {
            if (liczba <= szansa_krzyzowania)
            {    
                krzyzowanie();
            }
        }
        /*public void krzyzowanie_pmx()
        {
            for (int h = 0; h < tablica_tras.Length - 1; h += 2)
            {
                int ktory = losowa.Next(tablica_tras.Length-1);
                int ktory2 = losowa.Next(tablica_tras.Length - 1); 
                while(ktory2==ktory)
                    ktory2 = losowa.Next(tablica_tras.Length - 1);
                int pom2 = losowa.Next(2, tablica_tras[ktory].pobierz_tablice_miast.Length - 4);
                int[][] podzielona = podzial_tablicy(tablica_tras[ktory], pom2);
                int[][] podzielona2 = podzial_tablicy(tablica_tras[ktory2], pom2);
                List<int> Lista_podzielona_1 = new List<int>();
                List<int> Lista_podzielona_2 = new List<int>();
                List<List<int>> Lista_zamian = new List<List<int>>(); // lista w ktorej mamy listy odpowiednikow ( 1<->2 3<->4<->7)
                #region srodek i uzupelnienie listy zmian
                for (int y = 0; y < podzielona[1].Length; y++) // srodek (ten co zamieniamy)
                {
                    Lista_podzielona_1.Add(podzielona2[1][y]);
                    Lista_podzielona_2.Add(podzielona[1][y]);
                    if (y == 0)
                    {
                        Lista_zamian.Add(new List<int>());
                        Lista_zamian.ElementAt(0).Add(podzielona2[1][y]);
                        Lista_zamian.ElementAt(0).Add(podzielona[1][y]);
                    }
                    else
                    {
                        bool pom = false; // czy taki element znajduje sie w liscie zamian
                        for (int i = 0; i < Lista_zamian.Count; i++)
                        {
                            if (Lista_zamian.ElementAt(i).Contains(podzielona2[1][y]))
                            {
                                if (!Lista_zamian.ElementAt(i).Contains(podzielona[1][y])) // jezeli lista zmian zawiera juz w sobie pierwsza liczbe a nie zawiera 2
                                {
                                    Lista_zamian.ElementAt(i).Add(podzielona[1][y]);
                                    pom = true;
                                    break;
                                }
                                else
                                {
                                    pom = true;
                                    break;
                                }
                            }

                            if (Lista_zamian.ElementAt(i).Contains(podzielona[1][y]))
                            {
                                if (!Lista_zamian.ElementAt(i).Contains(podzielona2[1][y])) // jezeli lista zmian zawiera juz w sobie pierwsza liczbe a nie zawiera 2
                                {
                                    Lista_zamian.ElementAt(i).Add(podzielona2[1][y]);
                                    pom = true;
                                    break;
                                }
                                else
                                {
                                    pom = true;
                                    break;
                                }
                            }

                        }

                        if (!pom) // jezeli nie ma takich liczb w liscie zamienionych
                        {
                            Lista_zamian.Add(new List<int>());
                            Lista_zamian.Last().Add(podzielona2[1][y]);
                            Lista_zamian.Last().Add(podzielona[1][y]);
                        }

                    }
                }
                while (true)
                {
                    bool pom = false;
                    for (int i = 0; i < Lista_zamian.Count; i++)
                    {

                        for (int y = 0; y < Lista_zamian.ElementAt(i).Count; y++)
                        {
                            int pobrana = Lista_zamian.ElementAt(i).ElementAt(y);
                            for (int n = i + 1; n < Lista_zamian.Count; n++)
                            {
                                if (Lista_zamian.ElementAt(n).Contains(pobrana))
                                {
                                    Lista_zamian.ElementAt(i).AddRange(Lista_zamian.ElementAt(n));
                                    Lista_zamian.RemoveAt(n);
                                    pom = true;
                                }
                            }
                        }
                    }
                    if (!pom)
                        break;
                }
                #endregion
                #region pierwsza czesc
                for (int y = podzielona[0].Length - 1; y >= 0; y--) // pierwsza czesc
                {
                    if (Lista_podzielona_1.Contains(podzielona[0][y]))
                    {
                        for (int n = 0; n < Lista_zamian.Count; n++)
                        {
                            int pom = -1;
                            for (int i = 0; i < Lista_zamian.ElementAt(n).Count; i++)
                            {
                                if (Lista_zamian.ElementAt(n).Contains(podzielona[0][y]))
                                {
                                    pom = Lista_zamian.ElementAt(n).ElementAt(i);
                                    if (Lista_podzielona_1.Contains(pom)) continue;
                                    else break;
                                }
                                else
                                    break;
                            }
                            if (pom != -1)
                                Lista_podzielona_1.Insert(0, pom);
                        }
                    }
                    else
                    {
                        Lista_podzielona_1.Insert(0, podzielona[0][y]);
                    }

                    if (Lista_podzielona_2.Contains(podzielona2[0][y]))
                    {
                        for (int n = 0; n < Lista_zamian.Count; n++)
                        {
                            int pom = -1;
                            for (int i = 0; i < Lista_zamian.ElementAt(n).Count; i++)
                            {
                                if (Lista_zamian.ElementAt(n).Contains(podzielona2[0][y]))
                                {
                                    pom = Lista_zamian.ElementAt(n).ElementAt(i);
                                    if (Lista_podzielona_2.Contains(pom)) continue;
                                    else break;
                                }
                                else
                                    break;
                            }
                            if (pom != -1)
                                Lista_podzielona_2.Insert(0, pom);
                        }
                    }
                    else
                    {
                        Lista_podzielona_2.Insert(0, podzielona2[0][y]);
                    }
                }
                #endregion
                #region trzecia czesc
                for (int y = 0; y < podzielona[2].Length; y++) // 3 czesc
                {
                    #region lista1
                    if (Lista_podzielona_1.Contains(podzielona[2][y]))
                    {
                        for (int n = 0; n < Lista_zamian.Count; n++)
                        {
                            int pom = -1;
                            for (int i = 0; i < Lista_zamian.ElementAt(n).Count; i++)
                            {
                                if (Lista_zamian.ElementAt(n).Contains(podzielona[2][y]))
                                {
                                    pom = Lista_zamian.ElementAt(n).ElementAt(i);
                                    if (Lista_podzielona_1.Contains(pom)) continue;
                                    else
                                    {

                                        break;
                                    }
                                }
                                else
                                    break;
                            }
                            if (pom != -1)
                                Lista_podzielona_1.Add(pom);
                        }
                    }
                    else
                    {
                        Lista_podzielona_1.Add(podzielona[2][y]);
                    }
                    #endregion
                    if (Lista_podzielona_2.Contains(podzielona2[2][y]))
                    {
                        for (int n = 0; n < Lista_zamian.Count; n++)
                        {
                            int pom = -1;
                            for (int i = 0; i < Lista_zamian.ElementAt(n).Count; i++)
                            {
                                if (Lista_zamian.ElementAt(n).Contains(podzielona2[2][y]))
                                {
                                    pom = Lista_zamian.ElementAt(n).ElementAt(i);
                                    if (Lista_podzielona_2.Contains(pom)) continue;
                                    else break;
                                }
                                else
                                    break;
                            }
                            if (pom != -1)
                                Lista_podzielona_2.Add(pom);
                        }
                    }
                    else
                    {
                        Lista_podzielona_2.Add(podzielona2[2][y]);
                    }
                }
                #endregion

                tablica_tras[ktory].pobierz_tablice_miast = Lista_podzielona_1.ToArray<int>();
                tablica_tras[ktory2].pobierz_tablice_miast = Lista_podzielona_2.ToArray<int>();
            }

        }
        */
        public void krzyzowanie()
        {
            List<int> kolejka = new List<int>();
            for(int i=0;i<tablica_tras.Length;i++)
            {
                kolejka.Add(i);
            }
            for (int j = 0; j < tablica_tras.Length; j += 2)
            {
                int dlugosc = tablica_tras[j].pobierz_tablice_miast.Length;
                int pozycja1 = losowa.Next(0, dlugosc - 2);
                int pozycja2 = losowa.Next(pozycja1, dlugosc - 1);
                int dlugoscKrzyzowania = pozycja2 - pozycja1;
                int[] srodekOsobnika1 = new int[dlugoscKrzyzowania];
                int[] srodekOsobnika2 = new int[dlugoscKrzyzowania];
                Array.Copy(tablica_tras[j].pobierz_tablice_miast, pozycja1, srodekOsobnika1, 0, dlugoscKrzyzowania);
                Array.Copy(tablica_tras[j+1].pobierz_tablice_miast, pozycja1, srodekOsobnika2, 0, dlugoscKrzyzowania);
                int[] dziecko1 = new int[dlugosc];
                int[] dziecko2 = new int[dlugosc];
                int nowygen;
                for (int i = 0; i < dlugosc; i++)
                {
                    nowygen = tablica_tras[j].pobierz_tablice_miast[i];
                    if (Array.IndexOf(srodekOsobnika1, nowygen) > -1)
                    {
                        nowygen = srodekOsobnika2[i - pozycja1];
                    }
                    else if (Array.IndexOf(srodekOsobnika2, nowygen) > -1)
                    {
                        while (Array.IndexOf(srodekOsobnika2, nowygen) > -1)
                        {
                            nowygen = srodekOsobnika1[Array.IndexOf(srodekOsobnika2, nowygen)];
                        }
                    }
                    dziecko1[i] = nowygen;
                    nowygen = tablica_tras[j+1].pobierz_tablice_miast[i];
                    if (Array.IndexOf(srodekOsobnika2, nowygen) > -1)
                    {
                        nowygen = srodekOsobnika1[Array.IndexOf(srodekOsobnika2, nowygen)];
                    }
                    else if (Array.IndexOf(srodekOsobnika1, nowygen) > -1)
                    {
                        while (Array.IndexOf(srodekOsobnika1, nowygen) > -1)
                        {
                            nowygen = srodekOsobnika2[Array.IndexOf(srodekOsobnika1, nowygen)];
                        }
                    }

                    dziecko2[i] = nowygen;
                }
                tablica_tras[j].pobierz_tablice_miast  = dziecko1;
                tablica_tras[j+1].pobierz_tablice_miast  = dziecko2;
            }
        }
        private int[][] podzial_tablicy(Trasa trasa, int dlugośc_2czesci)
        {
            int[][] podzielona_tablica = new int[3][];
            int j = 0;
            int koniec = 0;
            int wielkosc_czesci = trasa.pobierz_tablice_miast.Length;
            podzielona_tablica[0] = new int[((wielkosc_czesci - dlugośc_2czesci) / 2)];
            for (int i = 0; i < ((wielkosc_czesci - dlugośc_2czesci) / 2); i++)
            {

                podzielona_tablica[0][i] = trasa.pobierz_tablice_miast[j];
                j++;
            }
            podzielona_tablica[1] = new int[dlugośc_2czesci];
            for (int i = 0; i < dlugośc_2czesci; i++)
            {

                podzielona_tablica[1][i] = trasa.pobierz_tablice_miast[j];
                j++;
            }
            podzielona_tablica[2] = new int[trasa.pobierz_tablice_miast.Length - podzielona_tablica[0].Length - podzielona_tablica[1].Length];
            for (int i = 0; j < trasa.pobierz_tablice_miast.Length; i++)
            {
                podzielona_tablica[2][i] = trasa.pobierz_tablice_miast[j];
                j++;
            }
            return podzielona_tablica;
        }

        public void mutacja()
        {
           
            
            for (int i = 0; i < tablica_tras.Length; i++)
            {
                if (losowa.Next(100) <= szansa_mutacji)
                {
                    mutacja(i);
                }
                if (losowa.Next(100) <= szansa_mutacji)
                {
                        mutacja2(i);
                }
            }
        }
        public void mutacja(int ktory)
        {
            
            int[][] podzielona_tablica = podzial_tablicy(tablica_tras[ktory], losowa.Next(tablica_tras[ktory].pobierz_tablice_miast.Length - 2));
            int[] nowa_tablica = new int[tablica_tras[0].pobierz_tablice_miast.Length];
            int i;
            for (i = 0; i < podzielona_tablica[0].Length; i++)
            {
                nowa_tablica[i] = podzielona_tablica[0][i];
            }
            int y;
            for (y = 0; y < podzielona_tablica[1].Length; y++)
            {
                nowa_tablica[y + i] = podzielona_tablica[1][podzielona_tablica[1].Length - 1 - y];
            }
            for (int z = 0; z < podzielona_tablica[2].Length; z++)
            {
                nowa_tablica[z + y + i] = podzielona_tablica[2][z];
            }
            tablica_tras[ktory].pobierz_tablice_miast = nowa_tablica;


        }

        public void mutacja2(int ktory)
        {
            int losowa2 = 0;
            for (int i = 0; i < tablica_tras[ktory].pobierz_tablice_miast.Length; i++)
            {
                if (losowa.Next(25) < 1)
                {

                    int temp;
                    temp = tablica_tras[ktory].pobierz_tablice_miast[i];
                    losowa2 = losowa.Next(tablica_tras[ktory].pobierz_tablice_miast.Length);
                    for (int y = 0; y < tablica_tras[ktory].pobierz_tablice_miast.Length; y++)
                    {
                        if(tablica_tras[ktory].pobierz_tablice_miast[y]==losowa2)
                        {
                            tablica_tras[ktory].pobierz_tablice_miast[y] = temp;
                            break;
                        }
                    }
                    tablica_tras[ktory].pobierz_tablice_miast[i] = losowa2;


                }
            }
        }
        public void pokaz_tablice_miast()
        {
            for (int i = 0; i < tablica_tras.Length; i++)
            {
                Console.WriteLine(tablica_tras[i].ToString());
            }
        }
    }
}
