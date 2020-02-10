using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using Semantic;
using ConsoleApp31;

namespace ICG
{
    class Intermediate_code_generation
    {
        int Tindex=1,Lindex=1;
        public string CreateTemp()
        {
            return "T" + Tindex++;
        }
        public string CreateLabel()
        {
            return "L" + Lindex++;
        }
        public void Output(string result)
        {
            File.AppendAllText("C:/Users/Anthony/Desktop/outputFile.txt", result);
            File.AppendAllText("C:/Users/Anthony/Desktop/outputFile.txt", '\r'.ToString());
            File.AppendAllText("C:/Users/Anthony/Desktop/outputFile.txt", '\n'.ToString());
        }
    }
}