using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Threading;
using System;

namespace WebProjectPatrice
{
    class Program
    {
        static bool stats = false,
                    keywords = false;
        static List<string> keywordsList = new List<string>();

        static void Main(string[] args)
        { 
            List<string> csFilesToConvert = new List<string>();
            List<CSFile> ListFiles = new List<CSFile>();

            keywordsList = RemplirTableauKeyWord();
            csFilesToConvert = AnalyseArguments(args);
            for (int i = 0; i < csFilesToConvert.Count(); i++)
            {
                CSFile unFile = new CSFile(csFilesToConvert[i]);
                ListFiles.Add(unFile);
            }

            TimeSpan ts = Threadpool(ListFiles);
            // ICI ON ATTEND QUE LE THREAD POOL FINISSE POUR STARTER LAUTRE THREAD. ET ON COMPILE LES DONNÉES DE STATS
        }

        static TimeSpan Threadpool(List<CSFile> ListFiles)
        {
            Stopwatch unwatch = new Stopwatch();
            unwatch.Start();

            ThreadPool.SetMaxThreads(Environment.ProcessorCount, Environment.ProcessorCount);

            for (int i = 0; i < ListFiles.Count(); i++)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(task), ListFiles[i]);
            }
            // attendre d'avoir toute fini les thread
            if (stats)
                Statistiques(ListFiles[0].getStatsDictionary(), ListFiles[0].getNumberofNumber(), keywordsList);

  
            unwatch.Stop();
            TimeSpan ts = unwatch.Elapsed;

            return ts;
        }

        private static void task(Object unObjet)
        {
            CSFile unFichier = (CSFile)unObjet;
            unFichier.CreateHTML(stats, keywords, keywordsList);
        }

        // CRÉE LE FICHIER DE STATISTIQUES
        private static void Statistiques(Dictionary<string, int> statistiques, int NumberOfNumber, List<string> keywordsList)
        {
            StreamWriter file = new StreamWriter("Statistiques.txt");
            var items = from pair in statistiques
                        orderby pair.Value descending,
                        pair.Key ascending
                        select pair;
            int nbreKeyWords = 0;
            foreach (KeyValuePair<string, int> pair in items)
            {
                file.WriteLine("{0}: {1}", pair.Key, pair.Value);
                if (keywordsList.Contains(pair.Key))
                    nbreKeyWords += pair.Value;
            }
            file.WriteLine("Il y a " + nbreKeyWords + " mot clés dans les fichiers");
            file.WriteLine("Il y a " + NumberOfNumber + " nombre dans les fichiers");
            file.Close();
        }

        // ANALYSE LES ARGUMENTS RECU DE L'UTILISATEUR
        private static List<string> AnalyseArguments(string[] arguments)
        {
            List<string> csFilesToConvert = new List<string>();
            string extension = "";
            foreach (string s in arguments)
            {
                if (s.ToLower() == "-stats" || s.ToLower() == "/stats")
                    stats = true;
                else if (s.ToLower() == "-keywords" || s.ToLower() == "/keywords")
                    keywords = true;
                else if (s[0] != '-' && s[0] != '/')
                    extension = Path.GetExtension(s);
                if (extension == ".cs")
                    csFilesToConvert.Add(s);
            }
            return csFilesToConvert;
        }

        // REMPLI LA LIST DES KEYWORDS
        private static List<string> RemplirTableauKeyWord()
        {
            List<string> keywordsList = new List<string>();
            string line;
            StreamReader file = new StreamReader("KeyWords.txt");
            while ((line = file.ReadLine()) != null)
            {
                if (line != "") keywordsList.Add(line);
            }
            file.Close();
            return keywordsList;
        }
    }
    // CLASSE POUR GERER LES TRANSLATION DE FILES
    class CSFile
    {
        private Dictionary<string, int> statistiques = new Dictionary<string, int>();
        private int NumberOfNumber = 0;
        private string FileName;

        public CSFile(string name)
        {
            FileName = name;
        }

        public int getNumberofNumber()
        {
            return NumberOfNumber;
        }

        public Dictionary<string, int> getStatsDictionary()
        {
            return statistiques;
        }

        public void CreateHTML(bool stats, bool keywords, List<string> keywordsList)
        {
            Dictionary<string, int> statistiques = new Dictionary<string, int>();
            string Input;
            string TemplateStart = "<!DOCTYPE html>\n" +
                                   "<html>\n" +
                                   "<head>\n" +
                                   "<meta charset = 'utf-8' />\n" +
                                   "<style> span {color:blue;} </style>" +
                                   "</head><body><pre>\n";
            string TemplateEnd = "</pre></body>\n" +
                                 "</html >";
            StreamWriter fileEnd = new StreamWriter(FileName + ".html");
            fileEnd.WriteLine(TemplateStart);
            StreamReader file = new StreamReader(FileName);
            while ((Input = file.ReadLine()) != null)
            {
                string v = Input;
                if (stats)
                    FillStatsDictionnary(v);
                v = Input.Replace("&", "&amp;");
                v = v.Replace(">", "&gt;");
                v = v.Replace("<", "&lt;");
                if (keywords)
                    v = AddColor(v, keywordsList);
                fileEnd.WriteLine(v);
            }
            fileEnd.WriteLine(TemplateEnd);
            file.Close();
            fileEnd.Close();
        }

        private void FillStatsDictionnary(string line)
        {
            string pattern = @"[^\w_]|(\d+\.\d+)";
            string[] words = Regex.Split(line, pattern);
            for (int i = 0; i < words.Length; ++i)
            {
                if (words[i] != "")
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

        private string AddColor(string line, List<string> keywordsList)
        {
            string pattern = @"([^a-zA-Z])";
            string sentence = "";
            string[] words = Regex.Split(line, pattern);
            for (int i = 0; i < words.Length; ++i)
            {
                words[i] = ChangeColor(words[i], keywordsList);
                sentence += words[i];
            }
            return sentence;
        }

        private string ChangeColor(string word, List<string> keywordsList)
        {
            if (keywordsList.Contains(word))
                word = "<span>" + word + "</span>";
            return word;
        }
    }
}