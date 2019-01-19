using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication29
{
    class GnuplotCommandCreation
    {
        public List<string> Commands { get; set; }

        public GnuplotCommandCreation()
        {
               Commands= new List<string>() { "set ticslevel 0", "set isosample 20,40." };
        }

        public void AddPoint(double x, double y, double z)
        {
            Commands.Add($"set object circle at first {x}, {y}, 2{z} radius char 0.3 fillstyle empty border lc rgb '#aa1100' lw 2");
        }

        public void SetRange(Range<int,int> xRange, Range<int,int> yRange)
        {
            Commands.Add($"set xrange [{xRange.Min}:{xRange.Max}]");
            Commands.Add($"set yrange [{yRange.Min}:{yRange.Max}]");
        }

        public void SetTerminalToPngWithSize(int x, int y)
        {
            Commands.Add($"set term png size {x}, {y}");
        }

        public void SetOutputFilePath(string path)
        {
            Commands.Add($"set output '{path}'");
        }
        public void SetSplot(string splot)
        {
            Commands.Add("splot "+splot);
        }

        public void CreatepPlot(
            string filePath, string functionPlot,
            Range<int, int> xRange,
            Range<int, int> yRange,
            int xCanvasSize = 800, int yCanvasSize = 400 )
        {
            this.SetRange(xRange,yRange);
            this.SetTerminalToPngWithSize(xCanvasSize,yCanvasSize);
            this.SetOutputFilePath(filePath);
            this.SetSplot(functionPlot);
        }
    }
}
