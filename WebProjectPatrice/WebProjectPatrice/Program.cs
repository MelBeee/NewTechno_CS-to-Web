using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebProjectPatrice
{
    class Program
    {
        static void Main(string[] args)
        {
            string line;
            List<string> data = new List<string>();
            StreamReader file = new StreamReader("KeyWords.txt");
            while ((line = file.ReadLine()) != null)
            { 
                if (line != "") data.Add(line); 
            }
            file.Close();

        }
    }
}
