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
        static List<string> keywordsList = new List<string>();
        static bool    stats    = false,        // option qui crée un fichier de statistiques
                       keywords = false;        // option qui met les keywords du langage en relief
        static List<string> csFilesToConvert  = new List<string>();   // tableau contenant les fichiers CS a convertir

        static void Main(string[] args)
        {
            RemplirTableauKeyWord();
            AnalyseArguments(args);
            ReadEachLine();
        }

        private static void AnalyseArguments(string[] arguments)
        {
            string extension = ""; 
            foreach (string s in arguments)
            {
                if (s == "-stats" || s == "/stats")
                    stats = true; 
                else if (s == "-keywords" || s == "/keywords")
                    keywords = true;
                else if(s[0] != '-' && s[0] != '/')
                    extension = Path.GetExtension(s);
                    if (extension == ".cs")
                        csFilesToConvert.Add(s);
                        
            }
        }

        private static void RemplirTableauKeyWord()
        {
            string line;
            StreamReader file = new StreamReader("KeyWords.txt");
            while ((line = file.ReadLine()) != null)
            {
                if (line != "") keywordsList.Add(line);
            }
            file.Close();
        }
        private static void ReadEachLine()
        {
            string Input;
            for (int i = 0; i < csFilesToConvert.Count ; ++i)
            {
                StreamReader file = new StreamReader(csFilesToConvert.ElementAt(i));
                StreamWriter fileEnd = new StreamWriter("End.txt");
                while ((Input = file.ReadLine()) != null)
                {
                    string v =  Input.Replace("&", "&gt;");
                    v = v.Replace(">", "&gt;");
                    v = v.Replace("<", "&lt;");
                    if (keywords)
                        v = AddColor(v);
                    fileEnd.WriteLine(v);
                }
                file.Close();
                fileEnd.Close();
            }
        }

        private static string AddColor(string line)
        {
            for (int i = 0; i < keywordsList.Count ; ++i)
            {
                line = line.Replace(keywordsList.ElementAt(i) + " ", "<span class='color'>" + keywordsList.ElementAt(i) + "</span>");
            }
            
            return line;
        }
    }
}
