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
            DateTime d = DateTime.Now;
            Analise a = new Analise(40);
            Random crossChange = new Random();
            Individual tempolaryBest= new Individual(2) {Fitness = -5000000};
            for (int i = 0; i < 200000; i++)
            {
                a.PMXCross(crossChange.Next(100));
                if (i % 100000 == 0 & i != 0)
                {
                    a.CrossChange += 5;

                }
                if (i == 50000 | i == 150000)
                    a.MutationChance += 1;
                if (i < 50000)
                {
                    a.CalcuateSunMinAndMax();
                    a.CalculatePercent();
                    a.RouletteSelection();
                }
                else
                {
                    a.CalcuateSunMinAndMax();
                    a.TournamentSelection();
                }

                if (i == 1)
                {
                    tempolaryBest = new Individual(a.BestValue);
                }
                a.SwitchValuesMutation();
                if (i % 10 == 0 && tempolaryBest.Fitness - a.BestValue.Fitness < 0.01)
                {
                    GnuplotCommandCreation operations = new GnuplotCommandCreation();
                    foreach (var individual in a.VariableTable)
                    {
                        operations.AddPoint(individual.VariableTable[0],individual.VariableTable[1],(int)individual.Fitness);
                    }
                    operations.CreatepPlot($@"D:\Charts\function{Individual.UsedFitnessFunctionNumber}Iteration{i}.png",a.BestValue.GetPlotFunction(),Individual.Range,Individual.Range);
                    new  GnuplotChartCreator().RunComangs(operations);
                }
            }

            DateTime Db = DateTime.Now;
            Console.WriteLine(Db.TimeOfDay - d.TimeOfDay);
            Console.ReadKey();

        }
    }
}
