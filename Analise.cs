using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication29
{
    class Analise
    {
        
        public Individual[] VariableTable { get; set; }
        private double min = 0;
        private double max = 0;
        private double _percentCount = 0;
        public int MutationChance { get; set; }
        public int CrossChange { get; set; }

        public Individual BestValue;
        private readonly Random _random;
        public Analise(int numbersOfIndividuals)
        {
            MutationChance = 2;
            CrossChange = 85;
            _random = new Random();
            VariableTable = new Individual[numbersOfIndividuals];
            for (int i = 0; i < numbersOfIndividuals; i++)
                VariableTable[i] = new Individual(2);
            BestValue = new Individual(VariableTable[0]);

        }
        public void RecalculateFitness()
        {
            foreach (Individual t in VariableTable)
                t.Fitness = t.CalculateFitness();
        }
        public string BestInviduals()
        {
            Console.WriteLine(BestValue.ToString() + " jej ocena " + BestValue.Fitness);
            return BestValue.ToString();
        }
        public void CalcuateSunMinAndMax()
        {
            RecalculateFitness();
            NormaliseFitness();

            max = VariableTable.Select(a => a.NormalizedFitness).Max();
            min = VariableTable.Select(a => a.NormalizedFitness).Min();
            var actualBest = VariableTable.First(a => Math.Abs(a.NormalizedFitness - max) < 0.0001);
            if(BestValue.Fitness > actualBest.Fitness)
            {
                BestValue = new Individual(actualBest);
            }

        }

        public void NormaliseFitness()
        {
            var max = VariableTable.Select(a => a.Fitness).Max();
            var min = VariableTable.Select(a => a.Fitness).Min();
            foreach (var invidual in VariableTable)
            {
                if (Math.Abs(min - max) < 0.000000001)
                    invidual.NormalizedFitness = 0;
                else
                {
                    invidual.NormalizedFitness = Math.Abs(1-(invidual.Fitness - min) / (max - min));
                }
            }
        }
        public void CalculatePercent()
        {
            // zmien sume zamienic na zmienna statyczna w trasie
            double suma = VariableTable.Select(a => a.NormalizedFitness).Sum();
            _percentCount = 0;
            foreach (var ind in VariableTable)
            {
                ind.FitnessPercent = _percentCount;
                _percentCount +=  ind.NormalizedFitness;
                if (ind.NormalizedFitness  == 0)
                    _percentCount+=0.0001;
            }

        }
        public void RouletteSelection()
        {
            Individual[] NewIndividualsTable = new Individual[VariableTable.Length];
            for (int y = 0; y < VariableTable.Length; y++)
            {
                ;
                double liczba = _random.NextDouble() * _percentCount;

                for (int i = VariableTable.Length - 1; i > -1; i--)
                {
                    if (liczba >= VariableTable[i].FitnessPercent)
                    {
                        NewIndividualsTable[y] = VariableTable[i];
                        break;
                    }
                }
            }
            for (int i = 0; i < VariableTable.Length; i++)
                VariableTable[i] = new Individual(NewIndividualsTable[i]);
        }
        public void TournamentSelection()
        {
            Individual[] newIndividualTable = new Individual[VariableTable.Length];
            double min = 0;
            int when = 0;
            for (int i = 0; i < VariableTable.Length; i++)
            {
                int[] tournamentIndividualsIdTable = RandomizeVariables(); 
                Individual[] tempTable = new Individual[5];
                min = VariableTable[tournamentIndividualsIdTable[0]].NormalizedFitness;
                for (int y = 0; y < tempTable.Length; y++)
                {
                    tempTable[y] = new Individual(VariableTable[tournamentIndividualsIdTable[y]]);

                    if (min < tempTable[y].NormalizedFitness)
                    {
                        min = tempTable[y].NormalizedFitness;
                        when = y;
                    }
                }
                newIndividualTable[i] = tempTable[when];
            }
            for (int i = 0; i < VariableTable.Length; i++)
                VariableTable[i] = new Individual(newIndividualTable[i]);
        }
        private int[] RandomizeVariables()
        {
            int[] randomizeVariables = new int[5];
            for (int j = 0; j < 5; j++)
            {
                if (j == 0)
                    randomizeVariables[j] = Individual.Randomizer.Next(0, VariableTable.Length - 1);
                else
                {
                    int liczba = 0;
                    bool pombool = true;
                    while (pombool)
                    {
                        liczba = Individual.Randomizer.Next(0, VariableTable.Length - 1);
                        pombool = false;
                        for (int z = 0; z < j; z++)
                        {
                            if (randomizeVariables[z] == liczba)
                                pombool = true;
                        }
                    }
                    randomizeVariables[j] = liczba;
                }
            }
            return randomizeVariables;
        }
        public void PMXCross(int liczba)
        {
            if (liczba <= CrossChange)
            {
                Cross();
            }
        }
        public void Cross()
        {
            List<int> queue = new List<int>();
            for (int i = 0; i < VariableTable.Length; i++)
            {
                queue.Add(i);
            }
            for (int j = 0; j < VariableTable.Length; j += 2)
            {
                int length = VariableTable[j].VariableTable.Length;
                int possition1 = _random.Next(0, length - 2);
                int posittion2 = _random.Next(possition1, length - 1);
                int crossLength = posittion2 - possition1;
                int[] middleOfFirstInvidual = new int[crossLength];
                int[] middleOfSecondInvidual = new int[crossLength];
                Array.Copy(VariableTable[j].VariableTable, possition1, middleOfFirstInvidual, 0, crossLength);
                Array.Copy(VariableTable[j + 1].VariableTable, possition1, middleOfSecondInvidual, 0, crossLength);
                int[] dziecko1 = new int[length];
                int[] dziecko2 = new int[length];
                int nowygen;
                for (int i = 0; i < length; i++)
                {
                    nowygen = VariableTable[j].VariableTable[i];
                    if (Array.IndexOf(middleOfFirstInvidual, nowygen) > -1)
                    {
                        nowygen = middleOfSecondInvidual[i - possition1];
                    }
                    else if (Array.IndexOf(middleOfSecondInvidual, nowygen) > -1)
                    {
                        while (Array.IndexOf(middleOfSecondInvidual, nowygen) > -1)
                        {
                            nowygen = middleOfFirstInvidual[Array.IndexOf(middleOfSecondInvidual, nowygen)];
                        }
                    }
                    dziecko1[i] = nowygen;
                    nowygen = VariableTable[j + 1].VariableTable[i];
                    if (Array.IndexOf(middleOfSecondInvidual, nowygen) > -1)
                    {
                        nowygen = middleOfFirstInvidual[Array.IndexOf(middleOfSecondInvidual, nowygen)];
                    }
                    else if (Array.IndexOf(middleOfFirstInvidual, nowygen) > -1)
                    {
                        while (Array.IndexOf(middleOfFirstInvidual, nowygen) > -1)
                        {
                            nowygen = middleOfSecondInvidual[Array.IndexOf(middleOfFirstInvidual, nowygen)];
                        }
                    }

                    dziecko2[i] = nowygen;
                }
                VariableTable[j].VariableTable = dziecko1;
                VariableTable[j + 1].VariableTable = dziecko2;
            }
        }
        private int[][] podzial_tablicy(Individual individual, int secondPratLength)
        {
            int[][] podzielona_tablica = new int[3][];
            int j = 0;
            int koniec = 0;
            int wielkosc_czesci = individual.VariableTable.Length;
            podzielona_tablica[0] = new int[((wielkosc_czesci - secondPratLength) / 2)];
            for (int i = 0; i < ((wielkosc_czesci - secondPratLength) / 2); i++)
            {

                podzielona_tablica[0][i] = individual.VariableTable[j];
                j++;
            }
            podzielona_tablica[1] = new int[secondPratLength];
            for (int i = 0; i < secondPratLength; i++)
            {

                podzielona_tablica[1][i] = individual.VariableTable[j];
                j++;
            }
            podzielona_tablica[2] = new int[individual.VariableTable.Length - podzielona_tablica[0].Length - podzielona_tablica[1].Length];
            for (int i = 0; j < individual.VariableTable.Length; i++)
            {
                podzielona_tablica[2][i] = individual.VariableTable[j];
                j++;
            }
            return podzielona_tablica;
        }

        public void SwitchValuesMutation()
        {


            for (int i = 0; i < VariableTable.Length; i++)
            {
                if (_random.Next(100) <= MutationChance)
                {
                    SwitchValuesMutation(i);
                }
                if (_random.Next(100) <= MutationChance)
                {
                    ChangeValueMigration(i);
                }
            }
        }
        public void SwitchValuesMutation(int which)
        {
            var indInvidual = VariableTable[which];
            var temp = indInvidual.VariableTable[0];
            indInvidual.VariableTable[0] = indInvidual.VariableTable[1];
            indInvidual.VariableTable[1] = temp;
        }

        public void ChangeValueMigration(int which)
        {
            for (var i = 0; i < VariableTable[which].VariableTable.Length; i++)
            {
                var random = _random.Next(25);
                var change = _random.Next(6);
                if (random < 1)
                {
                    VariableTable[which].VariableTable[i]+= change;
                    if (VariableTable[which].VariableTable[i] > Individual.Range.Max)
                        VariableTable[which].VariableTable[i] = Individual.Range.Max;
                }

                if (random > 23)
                {
                    VariableTable[which].VariableTable[i]-= change;
                    if (VariableTable[which].VariableTable[i] < Individual.Range.Min)
                        VariableTable[which].VariableTable[i] = Individual.Range.Min;
                }
            }
        }
        public void pokaz_tablice_miast()
        {
            for (int i = 0; i < VariableTable.Length; i++)
            {
                Console.WriteLine(VariableTable[i].ToString());
            }
        }
    }
}
