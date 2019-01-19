using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace ConsoleApplication29
{
    class Individual
    {
        public static Random Randomizer;
        public static int UsedFitnessFunctionNumber { get; set; }

        public static Range<int,int> Range { get; set; }
        private int[] _variableTable;
        public int[] VariableTable
        {
            get => this._variableTable;
            set => _variableTable = value;
        }
        public double Fitness { get; set; }
        public double NormalizedFitness { get; set; }
        public double FitnessPercent { get; set; }
        static Individual()
        {
            Range = new Range<int, int>() { Min = -100, Max = 100 };
            Randomizer = new Random();
            UsedFitnessFunctionNumber = 3;
        }
        public Individual(int i)
        {
            _variableTable = new int[i];
            RandomizeVariables();
            Fitness = this.CalculateFitness();
        }
        public Individual(Individual t1)
        {
            this.NormalizedFitness = t1.NormalizedFitness;
            this.FitnessPercent = t1.FitnessPercent;
            this._variableTable = new int[t1._variableTable.Length];
            for (int i = 0; i < t1._variableTable.Length; i++)
            {
                _variableTable[i] = t1._variableTable[i];
            }
            this.Fitness = t1.Fitness;
        }

        public void RandomizeVariables()
        {
            for (int j = 0; j < _variableTable.Length; j++)
            {
                _variableTable[j] = Randomizer.Next(Range.Min, Range.Max);
            }
        }

        public void ShowVariables()
        {
            foreach (int f in _variableTable)
                Console.Write(f + " ");
            Console.WriteLine();
        }
        //tez ja zrobic statyczna
        public double CalculateFitness()
        {
            var x = VariableTable[0];
            var y = VariableTable[1];
            switch (UsedFitnessFunctionNumber)
            {
                case 1: return Math.Sin(x)*Math.Pow(x, 3) + 2 * x * Math.Pow(y, 2) - x + Math.Pow(y, 2)* Math.Sin(y*x);
                case 2: return Math.Pow((x-29), 2) + Math.Pow((y+12), 2) -2-x*y;
                case 3: return Math.Pow(x, 2) + 2 * x * Math.Pow(y, 2) - x + Math.Pow(y, 2);
                default: return Math.Pow(x, 2)* Math.Pow(y, 3) - 2 * Math.Pow(y, 2) - Math.Pow(y, 5);
            }
        }

        public string GetPlotFunction()
        {
            var x = VariableTable[0];
            var y = VariableTable[1];
            switch (UsedFitnessFunctionNumber)
            {
                case 1: return "sin(x) * x**3 + 2 * x * y ** 2 - x + y ** 2 * sin(y * x)";
                case 2: return "(x - 29) ** 2 + (y + 12) ** 2 - 2 - x * y";
                case 3: return "x ** 2 + 2 * x * y ** 2 - x + y ** 2";
                default: return "x** 2 * y** 3 - 2 * y** 2 - y ** 5";
            }
        }
        public override string ToString()
        {
            string tekst = "";
            foreach (int x in _variableTable)
                tekst += x + "-";
            tekst += " " + this.NormalizedFitness;
            return tekst;
        }

    }
}
