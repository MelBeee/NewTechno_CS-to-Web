using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebProjectPatrice
{
    class Program
    {
        static List<string> keywordsList = new List<string>();
        static bool    stats    = false,        // option qui crée un fichier de statistiques
                       keywords = false;        // option qui met les keywords du langage en relief
        static List<string> csFilesToConvert  = new List<string>();   // tableau contenant les fichiers CS a convertir
        static Dictionary<string, int> statistiques = new Dictionary<string, int>();  // contient les stats
        static int NumberKeyWord = 0;
        static int NumberOfNumber = 0;

        static void Main(string[] args)
        {
            RemplirTableauKeyWord();
            AnalyseArguments(args);
            ReadEachLine();
            if (stats)
                Statistiques();

        }

        private static void Statistiques()
        {
            StreamWriter file = new StreamWriter("Statistiques.txt");
            var items = from pair in statistiques
                        orderby pair.Value descending,
                        pair.Key ascending
                        select pair;
            foreach (KeyValuePair<string, int> pair in items)
            {
                file.WriteLine("{0}: {1}", pair.Key, pair.Value);
            }
            file.WriteLine("Nombre de KeyWord : " + NumberKeyWord);
            file.WriteLine("Nombre de Nombre : " + NumberOfNumber);
            file.Close();
        }

        private static void FillStatsFile(string line)
        {
            string pattern = @"[^\w_]|(\d+\.\d+)";
            string[] words = Regex.Split(line, pattern);
            for (int i = 0; i < words.Length; ++i)
            {
                if(words[i] != "")
                {
                    if ((Regex.IsMatch(words[i], @"^\d+\.\d+")) || (Regex.IsMatch(words[i], @"^\d+")))
                        NumberOfNumber++;
                    if (!statistiques.ContainsKey(words[i]))
                        statistiques.Add(words[i], 1);
                    else
                        statistiques[words[i]]++;
                }
            }
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
            StreamWriter fileEnd = new StreamWriter("End.html");
            string TemplateStart = "<!DOCTYPE html>" + '\n' +
                                      "<html>" + '\n' +
                                      "<head>" + '\n' +
                                      "<meta charset = 'utf-8' />" + '\n' +
                                      "<style> span {color:blue;} </style>" +
                                      "</head><body><pre>" + '\n' ; 
            string TemplateEnd = "</pre></body>" + '\n' +
                                 "</html >";
            fileEnd.WriteLine(TemplateStart);
            for (int i = 0; i < csFilesToConvert.Count ; ++i)
            {
                StreamReader file = new StreamReader(csFilesToConvert.ElementAt(i));
                
                while ((Input = file.ReadLine()) != null)
                {

                    string v = Input;
                    if (stats)
                        FillStatsFile(v);
                    v =  Input.Replace("&", "&amp;");
                    v = v.Replace(">", "&gt;");
                    v = v.Replace("<", "&lt;");
                    if (keywords)
                        v = AddColor(v);
                        NumberKeyWord++;

                    fileEnd.WriteLine(v);
                }

                fileEnd.WriteLine(TemplateEnd);
                file.Close();
                fileEnd.Close();
            }
        }

        private static string AddColor(string line)
        {
            string pattern = @"([^a-zA-Z])";
            string sentence = "";
            
            string[] words = Regex.Split(line, pattern);
            for(int i = 0 ; i < words.Length ; ++i)
            {
                words[i] = ChangeColor(words[i]);
                sentence += words[i];
                
            }
            return sentence;
        }

        private static string ChangeColor(string word)
        {
            if (keywordsList.Contains(word, StringComparer.OrdinalIgnoreCase))
                word = "<span>" + word + "</span>";
                
            return word; 
        }
    }
}