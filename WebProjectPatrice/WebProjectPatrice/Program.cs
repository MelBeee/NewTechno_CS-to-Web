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


    }
}
