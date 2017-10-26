using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace ConsoleApplication29
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime D = DateTime.Now;
            List<int> srednia = new List<int>();
            StreamWriter sw = File.AppendText("Wyniki.txt");
            for (int y=0;y<10;y++)
            { 
            
            Analiza A = new Analiza(40);
            
            Random szansa_krzyzowania = new Random();
            for (int i = 0; i < 200000; i++)
            {
                    A.krzyzowanie_pmx(szansa_krzyzowania.Next(100));
                    if (i%100000==0 & i!=0)
                    {
                        A.szansa_krzyzowania += 5;
                        
                    }
                if(i==50000 | i == 150000)
                        A.szansa_mutacji += 1;
                    if (i < 50000)
                {
                        A.policz_sume_min_max();
                        A.oblicz_procenty();
                        A.selekcja_ruletka();
                        
                }
                else
                {
                        A.policz_sume_min_max();
                        A.selekcja_turniej();

                    }
                A.mutacja();
                    
                A.ponowne_obliczanie_odleglosci();
                    if(i%10000==0)
                    {
                        A.Najlepsza_Trasa();
                        
                    }
            }
                sw.WriteLine(A.Najlepsza_Trasa());
                srednia.Add(A.najlepsza.ocena);
            }
            sw.WriteLine(srednia.Average() + " Srednia");
            sw.WriteLine("---------------------------------------------------------");
            sw.Close();
            //A.pokaz_tablice_miast();
            //for (int i = 0; i < 5; i++)
            //{
            //    A.policz_sume_min_max();
            //    A.oblicz_procenty();
            //    A.mutacja2(0);
            //    A.ponowne_obliczanie_odleglosci();
            //}
            //Console.WriteLine();
            //A.pokaz_tablice_miast();

            DateTime Db = DateTime.Now;
            Console.WriteLine(Db.TimeOfDay - D.TimeOfDay);
            Console.ReadKey();

        }
    }
}
