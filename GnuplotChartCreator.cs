using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication29
{
    class GnuplotChartCreator
    {
        private const string Path = @"D:\gnuplot\bin\gnuplot.exe";

        public void RunComangs(GnuplotCommandCreation commands)
        {
            Process gnuplotProcess = new Process();
            gnuplotProcess.StartInfo.FileName = Path;
            gnuplotProcess.StartInfo.UseShellExecute = false;
            gnuplotProcess.StartInfo.RedirectStandardInput = true;
            gnuplotProcess.Start();
            StreamWriter sw = gnuplotProcess.StandardInput;
            foreach (var command in commands.Commands)
            {
                sw.WriteLine(command);
            }
            sw.WriteLine("exit");
            sw.Flush();
        }
    }
}
