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
            StreamWriter fileEnd = new StreamWriter("End.html");
            string TemplateStart = "<!DOCTYPE html>" + '\n' +
                                      "<html>" + '\n' +
                                      "<head>" + '\n' +
                                      "<meta charset = 'utf-8' />" + '\n' +
                                      "<title> Titre </title>" + '\n' +
                                      "</head><body>" + '\n' ; 
            string TemplateEnd = "</body>" + '\n' +
                                 "</html >";
            fileEnd.WriteLine(TemplateStart);
            for (int i = 0; i < csFilesToConvert.Count ; ++i)
            {
                StreamReader file = new StreamReader(csFilesToConvert.ElementAt(i));
                
               

                
                while ((Input = file.ReadLine()) != null)
                {
                    string v =  Input.Replace("&", "&gt;");
                    v = v.Replace(">", "&gt;");
                    v = v.Replace("<", "&lt;");
                    if (keywords)
                        v = AddColor(v);
                    
                    fileEnd.WriteLine(v);
                }
                fileEnd.WriteLine(TemplateEnd);
                file.Close();
                fileEnd.Close();
            }
        }

        private static string AddColor(string line)
        {
            for (int i = 0; i < keywordsList.Count ; ++i)
            {
                line = line.Replace(keywordsList.ElementAt(i) + " ", "<span style='color: blue;'>" + keywordsList.ElementAt(i) + "</span>" + " ");
            }
            
            return line;
        }
    }
}
