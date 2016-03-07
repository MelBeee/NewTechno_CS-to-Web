using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PoneyLover3._0.Models
{
    public class ClassLiaisonBD
    {
        private static int ancienchiffre = 1;


        public static int GetIDCheval(SqlConnection conn, string name)
        {
            int id = 1;

            SqlCommand sql = new SqlCommand("select Id from cheval where nom = '" + name + "'");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();

            if (sqlDR.Read())
            {
                id = sqlDR.GetInt32(0);
            }
            sqlDR.Close();
            conn.Close();
            return id;
        }

        public static string GetNomUsager(SqlConnection conn, int id)
        {
            string nomusager = "";
            try
            {
                SqlCommand sql = new SqlCommand("select nomcomplet from usager where id = " + id);
                sql.Connection = conn;

                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Open();

                SqlDataReader sqlDR = sql.ExecuteReader();

                if (sqlDR.Read())
                {
                    nomusager = sqlDR.GetString(0);
                }

                conn.Close();
                sqlDR.Close();
            }
            catch (SqlException ex)
            {
                string la = ex.Message;
            }

            return nomusager;
        }



        public static string[,] GetListChevaux(SqlConnection conn)
        {
            SqlCommand sql = new SqlCommand("select id, nom, description, emplacement, race, discipline, idusager from cheval ");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();
            int nombre = 0;
            while (sqlDR.Read())
            {
                nombre++;
            }
            string[,] Tab = new string[nombre, 7];
            sqlDR.Close();
            if (nombre > 0)
            {
                SqlDataReader sqlDR2 = sql.ExecuteReader();
                int cpt = 0;
                while (sqlDR2.Read())
                {
                    Tab[cpt, 0] = sqlDR2.GetInt32(0).ToString();
                    Tab[cpt, 1] = sqlDR2.GetString(1);
                    Tab[cpt, 2] = sqlDR2.GetString(2);
                    Tab[cpt, 3] = sqlDR2.GetString(3);
                    Tab[cpt, 4] = sqlDR2.GetString(4);
                    Tab[cpt, 5] = sqlDR2.GetString(5);
                    Tab[cpt, 6] = sqlDR2.GetInt32(6).ToString();
                    cpt++;
                }
                sqlDR2.Close();
            }
            conn.Close();

            return Tab;
        }

        public static string[] GetImageChevaux(SqlConnection conn, int IDCheval)
        {
            SqlCommand sql = new SqlCommand("select guidphoto from photo where idcheval = " + IDCheval + "");
            sql.Connection = conn;

            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();
            int nombre = 0;
            while (sqlDR.Read())
            {
                nombre++;
            }
            sqlDR.Close();
            string[] Tab = new string[nombre];
            if (nombre > 0)
            {
                SqlDataReader sqlDR2 = sql.ExecuteReader();
                int cpt = 0;

                while (sqlDR2.Read())
                {
                    Tab[cpt] = sqlDR2.GetString(0);
                    cpt++;
                }
                sqlDR2.Close();
            }
            conn.Close();

            return Tab;
        }

        public static int TrouverIDHasard(SqlConnection conn, int ID)
        {
            int chiffre = 1;

            SqlCommand sql = new SqlCommand("select id from cheval");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();
            int nombre = 0;
            while (sqlDR.Read())
            {
                nombre++;
            }
            int[] tab = new int[nombre];
            sqlDR.Close();
            if (nombre > 0)
            {
                SqlDataReader sqlDR2 = sql.ExecuteReader();

                int cpt = 0;
                while (sqlDR2.Read())
                {
                    tab[cpt] = sqlDR2.GetInt32(0);
                    cpt++;
                }
                sqlDR2.Close();
            }

            do
            {
                Random chiffrealatoire = new Random();
                int aleatoire = chiffrealatoire.Next(1, nombre);
                aleatoire = aleatoire - 1;

                chiffre = tab[aleatoire];

            } while (chiffre == ancienchiffre);
            ancienchiffre = chiffre;
            conn.Close();

            return chiffre;
        }

        public static bool NomUsagerExiste(string nomuser, SqlConnection conn)
        {
            bool resultat = false;

            SqlCommand sql = new SqlCommand("select nomusager from usager where nomusager = '" + nomuser + "'");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();

            if (sqlDR.Read())
            {
                resultat = true;
            }

            sqlDR.Close();
            conn.Close();

            return resultat;
        }

        public static string[] GetChevaux(string Username, SqlConnection conn)
        {
            SqlCommand sql = new SqlCommand("select nom from cheval where idusager = " + GetIDUsager(conn, Username));
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();
            int nombreresultat = 0;
            while (sqlDR.Read())
            {
                nombreresultat++;
            }
            string[] Tab = new string[nombreresultat];
            sqlDR.Close();

            if (nombreresultat > 0)
            {
                SqlDataReader sqlDR2 = sql.ExecuteReader();
                int cpt = 0;
                while (sqlDR2.Read())
                {
                    Tab[cpt] = sqlDR2.GetString(0);
                    cpt++;
                }
                sqlDR2.Close();
            }

            conn.Close();

            return Tab;
        }

        public static void InsertionImageCheval(int id, String GuidCheval, String IDCheval, SqlConnection conn)
        {
            GuidCheval = ReplaceRegex(GuidCheval);

            SqlCommand sql = new SqlCommand("insert into photo(ID,GuidPhoto,IDCheval) values (" + id + ",'" + GuidCheval + "'," + IDCheval + ")");
            sql.Connection = conn;
            conn.Open();

            int ligne = sql.ExecuteNonQuery();

            conn.Close();
        }

        public static void UpdateImageCheval(String NewGuidCheval, int IdCheval, SqlConnection conn, int Pos)
        {
            NewGuidCheval = ReplaceRegex(NewGuidCheval);

            SqlCommand sql = new SqlCommand("Update Photo set GuidPhoto ='" + NewGuidCheval + "' where IDCheval =" + IdCheval + " And ID =" + Pos);
            sql.Connection = conn;
            conn.Open();
            sql.ExecuteNonQuery();
            conn.Close();
        }

        public static int TrouverDernierID(SqlConnection conn, string table)
        {
            int id = 0;

            SqlCommand sql = new SqlCommand("select max(ID) from " + table + "");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlRD = sql.ExecuteReader();

            if (sqlRD.Read())
            {
                id = sqlRD.GetInt32(0);
            }
            conn.Close();
            sqlRD.Close();

            return id;
        }

        public static string[] GetInfoCheval(SqlConnection conn, int Id)
        {
            SqlCommand sql = new SqlCommand("select id, nom, description, emplacement, race, discipline, idusager from cheval where id = " + Id);
            sql.Connection = conn;
            conn.Open();
            string[] unTableau = new string[7];

            SqlDataReader sqlDRget = sql.ExecuteReader();
            if (sqlDRget.Read())
            {
                unTableau[0] = sqlDRget.GetInt32(0).ToString();
                unTableau[1] = sqlDRget.GetString(1);
                unTableau[2] = sqlDRget.GetString(2);
                unTableau[3] = sqlDRget.GetString(3);
                unTableau[4] = sqlDRget.GetString(4);
                unTableau[5] = sqlDRget.GetString(5);
                unTableau[6] = sqlDRget.GetInt32(6).ToString();
            }
            conn.Close();
            sqlDRget.Close();

            return unTableau;
        }

        public static int GetPremierID(SqlConnection conn)
        {
            int id = 0;

            SqlCommand sql = new SqlCommand("select min(ID) from cheval");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlRD = sql.ExecuteReader();

            if (sqlRD.Read())
            {
                id = sqlRD.GetInt32(0) + 1;
            }
            conn.Close();
            sqlRD.Close();

            return id;
        }

        public static bool InsertionUtilisateur(string nomuser, string nomcomplet, string motdepasse, string courriel, SqlConnection conn)
        {
            nomuser = ReplaceRegex(nomuser);
            nomcomplet = ReplaceRegex(nomcomplet);
            motdepasse = ReplaceRegex(motdepasse);
            courriel = ReplaceRegex(courriel);
            bool resultat = false;
            int dernierid = TrouverDernierID(conn, "usager") + 1;
            SqlCommand sql = new SqlCommand("insert into usager(ID, NomUsager, NomComplet, MotDePasse, AdresseCourriel)" +
                                            "values (" + dernierid + ",'" + nomuser + "', '" + nomcomplet + "', '" + motdepasse + "', '" + courriel + "')");
            sql.Connection = conn;
            conn.Open();

            int ligne = sql.ExecuteNonQuery();

            if (ligne == 1)
            {
                resultat = true;
            }

            conn.Close();

            return resultat;
        }

        public static bool VerifierConnexion(string nomuser, string motdepasse, SqlConnection conn)
        {
            bool resultat = false;

            SqlCommand sql = new SqlCommand("select NomUsager from usager where NomUsager = '" + nomuser + "' and MotDePasse = '" + motdepasse + "'");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();

            if (sqlDR.Read())
            {
                resultat = true;
            }

            sqlDR.Close();
            conn.Close();

            return resultat;
        }

        public static string ReplaceRegex(string unString)
        {
            unString = unString.Replace("'", "''");

            return unString;
        }

        public static bool InsertionCheval(string Nom, string Description, string Emplacement, string Race, string Discipline, string NomUsager, SqlConnection conn)
        {
            Nom = ReplaceRegex(Nom);
            Description = ReplaceRegex(Description);
            Emplacement = ReplaceRegex(Emplacement);
            Race = ReplaceRegex(Race);
            Discipline = ReplaceRegex(Discipline);
            bool resultat = false;
            int ID = GetIDUsager(conn, NomUsager);
            int IDDernier = TrouverDernierID(conn, "Cheval") + 1;
            if (ID != -1)
            {
                SqlCommand sql = new SqlCommand("insert into cheval (id, nom, description, emplacement, race, discipline, idusager) " +
                                                " values (" + IDDernier + ",'" + Nom + "', '" + Description + "', '" + Emplacement + "', '" + Race + "', '" + Discipline + "', " + ID + ")");
                sql.Connection = conn;
                conn.Open();

                int ligne = sql.ExecuteNonQuery();

                if (ligne == 1)
                {
                    resultat = true;
                }

                conn.Close();
            }

            return resultat;
        }

        public static int GetIDUsager(SqlConnection conn, string NomUsager)
        {
            int id = -1;

            SqlCommand sql = new SqlCommand("select Id from usager where NomUsager = '" + NomUsager + "'");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();

            if (sqlDR.Read())
            {
                id = sqlDR.GetInt32(0);
            }
            sqlDR.Close();
            conn.Close();
            return id;
        }

        public static string GetPassword(string Username, SqlConnection conn)
        {
            string mdp = "";

            SqlCommand sql = new SqlCommand("select motdepasse from Usager where NomUsager = '" + Username + "'");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();

            if (sqlDR.Read())
            {
                mdp = sqlDR.GetString(0);
            }
            sqlDR.Close();
            conn.Close();

            return mdp;
        }

        public static string GetEmail(string Username, SqlConnection conn)
        {
            string email = "";

            SqlCommand sql = new SqlCommand("select adressecourriel from Usager where NomUsager = '" + Username + "'");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();

            if (sqlDR.Read())
            {
                email = sqlDR.GetString(0);
            }
            sqlDR.Close();
            conn.Close();

            return email;
        }

        public static bool UpdateCheval(int ID, string Nom, string Description, string Emplacement, string Race, string Discipline, string NomUsager, SqlConnection conn)
        {
            Nom = ReplaceRegex(Nom);
            Description = ReplaceRegex(Description);
            Emplacement = ReplaceRegex(Emplacement);
            Race = ReplaceRegex(Race);
            Discipline = ReplaceRegex(Discipline);
            bool resultat = false;
            int IDUsager = GetIDUsager(conn, NomUsager);

            if (IDUsager != -1)
            {
                SqlCommand sql = new SqlCommand(" update cheval set nom = '" + Nom + "', "
                                                                + " description = '" + Description + "', "
                                                                + " emplacement = '" + Emplacement + "', "
                                                                + " race = '" + Race + "', "
                                                                + " discipline = '" + Discipline + "'"
                                                                + " where ID = " + ID);
                sql.Connection = conn;
                conn.Open();

                if (1 == sql.ExecuteNonQuery())
                {
                    resultat = true;
                }

                conn.Close();
            }

            return resultat;
        }

        public static bool SupprimerUnCheval (SqlConnection conn, int IDCheval)
        {
            bool supprimer = false;

            SqlCommand sql2 = new SqlCommand("delete from photo where idcheval =" + IDCheval);
            SqlCommand sql = new SqlCommand("delete from cheval where id =" + IDCheval);
            sql2.Connection = conn;
            sql.Connection = conn;

            conn.Open();

            int j = sql2.ExecuteNonQuery();
            int i = sql.ExecuteNonQuery();

            if(i > 0 && j > 0)
            {
                supprimer = true; 
            }

            conn.Close();

            return supprimer; 
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PoneyLover3._0.Models
{
    public class ClassLiaisonBD
    {
        private static int ancienchiffre = 1;


        public static int GetIDCheval(SqlConnection conn, string name)
        {
            int id = 1;

            SqlCommand sql = new SqlCommand("select Id from cheval where nom = '" + name + "'");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();

            if (sqlDR.Read())
            {
                id = sqlDR.GetInt32(0);
            }
            sqlDR.Close();
            conn.Close();
            return id;
        }

        public static string GetNomUsager(SqlConnection conn, int id)
        {
            string nomusager = "";
            try
            {
                SqlCommand sql = new SqlCommand("select nomcomplet from usager where id = " + id);
                sql.Connection = conn;

                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Open();

                SqlDataReader sqlDR = sql.ExecuteReader();

                if (sqlDR.Read())
                {
                    nomusager = sqlDR.GetString(0);
                }

                conn.Close();
                sqlDR.Close();
            }
            catch (SqlException ex)
            {
                string la = ex.Message;
            }

            return nomusager;
        }



        public static string[,] GetListChevaux(SqlConnection conn)
        {
            SqlCommand sql = new SqlCommand("select id, nom, description, emplacement, race, discipline, idusager from cheval ");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();
            int nombre = 0;
            while (sqlDR.Read())
            {
                nombre++;
            }
            string[,] Tab = new string[nombre, 7];
            sqlDR.Close();
            if (nombre > 0)
            {
                SqlDataReader sqlDR2 = sql.ExecuteReader();
                int cpt = 0;
                while (sqlDR2.Read())
                {
                    Tab[cpt, 0] = sqlDR2.GetInt32(0).ToString();
                    Tab[cpt, 1] = sqlDR2.GetString(1);
                    Tab[cpt, 2] = sqlDR2.GetString(2);
                    Tab[cpt, 3] = sqlDR2.GetString(3);
                    Tab[cpt, 4] = sqlDR2.GetString(4);
                    Tab[cpt, 5] = sqlDR2.GetString(5);
                    Tab[cpt, 6] = sqlDR2.GetInt32(6).ToString();
                    cpt++;
                }
                sqlDR2.Close();
            }
            conn.Close();

            return Tab;
        }

        public static string[] GetImageChevaux(SqlConnection conn, int IDCheval)
        {
            SqlCommand sql = new SqlCommand("select guidphoto from photo where idcheval = " + IDCheval + "");
            sql.Connection = conn;

            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();
            int nombre = 0;
            while (sqlDR.Read())
            {
                nombre++;
            }
            sqlDR.Close();
            string[] Tab = new string[nombre];
            if (nombre > 0)
            {
                SqlDataReader sqlDR2 = sql.ExecuteReader();
                int cpt = 0;

                while (sqlDR2.Read())
                {
                    Tab[cpt] = sqlDR2.GetString(0);
                    cpt++;
                }
                sqlDR2.Close();
            }
            conn.Close();

            return Tab;
        }

        public static int TrouverIDHasard(SqlConnection conn, int ID)
        {
            int chiffre = 1;

            SqlCommand sql = new SqlCommand("select id from cheval");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();
            int nombre = 0;
            while (sqlDR.Read())
            {
                nombre++;
            }
            int[] tab = new int[nombre];
            sqlDR.Close();
            if (nombre > 0)
            {
                SqlDataReader sqlDR2 = sql.ExecuteReader();

                int cpt = 0;
                while (sqlDR2.Read())
                {
                    tab[cpt] = sqlDR2.GetInt32(0);
                    cpt++;
                }
                sqlDR2.Close();
            }

            do
            {
                Random chiffrealatoire = new Random();
                int aleatoire = chiffrealatoire.Next(1, nombre);
                aleatoire = aleatoire - 1;

                chiffre = tab[aleatoire];

            } while (chiffre == ancienchiffre);
            ancienchiffre = chiffre;
            conn.Close();

            return chiffre;
        }

        public static bool NomUsagerExiste(string nomuser, SqlConnection conn)
        {
            bool resultat = false;

            SqlCommand sql = new SqlCommand("select nomusager from usager where nomusager = '" + nomuser + "'");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();

            if (sqlDR.Read())
            {
                resultat = true;
            }

            sqlDR.Close();
            conn.Close();

            return resultat;
        }

        public static string[] GetChevaux(string Username, SqlConnection conn)
        {
            SqlCommand sql = new SqlCommand("select nom from cheval where idusager = " + GetIDUsager(conn, Username));
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();
            int nombreresultat = 0;
            while (sqlDR.Read())
            {
                nombreresultat++;
            }
            string[] Tab = new string[nombreresultat];
            sqlDR.Close();

            if (nombreresultat > 0)
            {
                SqlDataReader sqlDR2 = sql.ExecuteReader();
                int cpt = 0;
                while (sqlDR2.Read())
                {
                    Tab[cpt] = sqlDR2.GetString(0);
                    cpt++;
                }
                sqlDR2.Close();
            }

            conn.Close();

            return Tab;
        }

        public static void InsertionImageCheval(int id, String GuidCheval, String IDCheval, SqlConnection conn)
        {
            GuidCheval = ReplaceRegex(GuidCheval);

            SqlCommand sql = new SqlCommand("insert into photo(ID,GuidPhoto,IDCheval) values (" + id + ",'" + GuidCheval + "'," + IDCheval + ")");
            sql.Connection = conn;
            conn.Open();

            int ligne = sql.ExecuteNonQuery();

            conn.Close();
        }

        public static void UpdateImageCheval(String NewGuidCheval, int IdCheval, SqlConnection conn, int Pos)
        {
            NewGuidCheval = ReplaceRegex(NewGuidCheval);

            SqlCommand sql = new SqlCommand("Update Photo set GuidPhoto ='" + NewGuidCheval + "' where IDCheval =" + IdCheval + " And ID =" + Pos);
            sql.Connection = conn;
            conn.Open();
            sql.ExecuteNonQuery();
            conn.Close();
        }

        public static int TrouverDernierID(SqlConnection conn, string table)
        {
            int id = 0;

            SqlCommand sql = new SqlCommand("select max(ID) from " + table + "");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlRD = sql.ExecuteReader();

            if (sqlRD.Read())
            {
                id = sqlRD.GetInt32(0);
            }
            conn.Close();
            sqlRD.Close();

            return id;
        }

        public static string[] GetInfoCheval(SqlConnection conn, int Id)
        {
            SqlCommand sql = new SqlCommand("select id, nom, description, emplacement, race, discipline, idusager from cheval where id = " + Id);
            sql.Connection = conn;
            conn.Open();
            string[] unTableau = new string[7];

            SqlDataReader sqlDRget = sql.ExecuteReader();
            if (sqlDRget.Read())
            {
                unTableau[0] = sqlDRget.GetInt32(0).ToString();
                unTableau[1] = sqlDRget.GetString(1);
                unTableau[2] = sqlDRget.GetString(2);
                unTableau[3] = sqlDRget.GetString(3);
                unTableau[4] = sqlDRget.GetString(4);
                unTableau[5] = sqlDRget.GetString(5);
                unTableau[6] = sqlDRget.GetInt32(6).ToString();
            }
            conn.Close();
            sqlDRget.Close();

            return unTableau;
        }

        public static int GetPremierID(SqlConnection conn)
        {
            int id = 0;

            SqlCommand sql = new SqlCommand("select min(ID) from cheval");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlRD = sql.ExecuteReader();

            if (sqlRD.Read())
            {
                id = sqlRD.GetInt32(0) + 1;
            }
            conn.Close();
            sqlRD.Close();

            return id;
        }

        public static bool InsertionUtilisateur(string nomuser, string nomcomplet, string motdepasse, string courriel, SqlConnection conn)
        {
            nomuser = ReplaceRegex(nomuser);
            nomcomplet = ReplaceRegex(nomcomplet);
            motdepasse = ReplaceRegex(motdepasse);
            courriel = ReplaceRegex(courriel);
            bool resultat = false;
            int dernierid = TrouverDernierID(conn, "usager") + 1;
            SqlCommand sql = new SqlCommand("insert into usager(ID, NomUsager, NomComplet, MotDePasse, AdresseCourriel)" +
                                            "values (" + dernierid + ",'" + nomuser + "', '" + nomcomplet + "', '" + motdepasse + "', '" + courriel + "')");
            sql.Connection = conn;
            conn.Open();

            int ligne = sql.ExecuteNonQuery();

            if (ligne == 1)
            {
                resultat = true;
            }

            conn.Close();

            return resultat;
        }

        public static bool VerifierConnexion(string nomuser, string motdepasse, SqlConnection conn)
        {
            bool resultat = false;

            SqlCommand sql = new SqlCommand("select NomUsager from usager where NomUsager = '" + nomuser + "' and MotDePasse = '" + motdepasse + "'");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();

            if (sqlDR.Read())
            {
                resultat = true;
            }

            sqlDR.Close();
            conn.Close();

            return resultat;
        }

        public static string ReplaceRegex(string unString)
        {
            unString = unString.Replace("'", "''");

            return unString;
        }

        public static bool InsertionCheval(string Nom, string Description, string Emplacement, string Race, string Discipline, string NomUsager, SqlConnection conn)
        {
            Nom = ReplaceRegex(Nom);
            Description = ReplaceRegex(Description);
            Emplacement = ReplaceRegex(Emplacement);
            Race = ReplaceRegex(Race);
            Discipline = ReplaceRegex(Discipline);
            bool resultat = false;
            int ID = GetIDUsager(conn, NomUsager);
            int IDDernier = TrouverDernierID(conn, "Cheval") + 1;
            if (ID != -1)
            {
                SqlCommand sql = new SqlCommand("insert into cheval (id, nom, description, emplacement, race, discipline, idusager) " +
                                                " values (" + IDDernier + ",'" + Nom + "', '" + Description + "', '" + Emplacement + "', '" + Race + "', '" + Discipline + "', " + ID + ")");
                sql.Connection = conn;
                conn.Open();

                int ligne = sql.ExecuteNonQuery();

                if (ligne == 1)
                {
                    resultat = true;
                }

                conn.Close();
            }

            return resultat;
        }

        public static int GetIDUsager(SqlConnection conn, string NomUsager)
        {
            int id = -1;

            SqlCommand sql = new SqlCommand("select Id from usager where NomUsager = '" + NomUsager + "'");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();

            if (sqlDR.Read())
            {
                id = sqlDR.GetInt32(0);
            }
            sqlDR.Close();
            conn.Close();
            return id;
        }

        public static string GetPassword(string Username, SqlConnection conn)
        {
            string mdp = "";

            SqlCommand sql = new SqlCommand("select motdepasse from Usager where NomUsager = '" + Username + "'");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();

            if (sqlDR.Read())
            {
                mdp = sqlDR.GetString(0);
            }
            sqlDR.Close();
            conn.Close();

            return mdp;
        }

        public static string GetEmail(string Username, SqlConnection conn)
        {
            string email = "";

            SqlCommand sql = new SqlCommand("select adressecourriel from Usager where NomUsager = '" + Username + "'");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();

            if (sqlDR.Read())
            {
                email = sqlDR.GetString(0);
            }
            sqlDR.Close();
            conn.Close();

            return email;
        }

        public static bool UpdateCheval(int ID, string Nom, string Description, string Emplacement, string Race, string Discipline, string NomUsager, SqlConnection conn)
        {
            Nom = ReplaceRegex(Nom);
            Description = ReplaceRegex(Description);
            Emplacement = ReplaceRegex(Emplacement);
            Race = ReplaceRegex(Race);
            Discipline = ReplaceRegex(Discipline);
            bool resultat = false;
            int IDUsager = GetIDUsager(conn, NomUsager);

            if (IDUsager != -1)
            {
                SqlCommand sql = new SqlCommand(" update cheval set nom = '" + Nom + "', "
                                                                + " description = '" + Description + "', "
                                                                + " emplacement = '" + Emplacement + "', "
                                                                + " race = '" + Race + "', "
                                                                + " discipline = '" + Discipline + "'"
                                                                + " where ID = " + ID);
                sql.Connection = conn;
                conn.Open();

                if (1 == sql.ExecuteNonQuery())
                {
                    resultat = true;
                }

                conn.Close();
            }

            return resultat;
        }

        public static bool SupprimerUnCheval (SqlConnection conn, int IDCheval)
        {
            bool supprimer = false;

            SqlCommand sql2 = new SqlCommand("delete from photo where idcheval =" + IDCheval);
            SqlCommand sql = new SqlCommand("delete from cheval where id =" + IDCheval);
            sql2.Connection = conn;
            sql.Connection = conn;

            conn.Open();

            int j = sql2.ExecuteNonQuery();
            int i = sql.ExecuteNonQuery();

            if(i > 0 && j > 0)
            {
                supprimer = true; 
            }

            conn.Close();

            return supprimer; 
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PoneyLover3._0.Models
{
    public class ClassLiaisonBD
    {
        private static int ancienchiffre = 1;


        public static int GetIDCheval(SqlConnection conn, string name)
        {
            int id = 1;

            SqlCommand sql = new SqlCommand("select Id from cheval where nom = '" + name + "'");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();

            if (sqlDR.Read())
            {
                id = sqlDR.GetInt32(0);
            }
            sqlDR.Close();
            conn.Close();
            return id;
        }

        public static string GetNomUsager(SqlConnection conn, int id)
        {
            string nomusager = "";
            try
            {
                SqlCommand sql = new SqlCommand("select nomcomplet from usager where id = " + id);
                sql.Connection = conn;

                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Open();

                SqlDataReader sqlDR = sql.ExecuteReader();

                if (sqlDR.Read())
                {
                    nomusager = sqlDR.GetString(0);
                }

                conn.Close();
                sqlDR.Close();
            }
            catch (SqlException ex)
            {
                string la = ex.Message;
            }

            return nomusager;
        }



        public static string[,] GetListChevaux(SqlConnection conn)
        {
            SqlCommand sql = new SqlCommand("select id, nom, description, emplacement, race, discipline, idusager from cheval ");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();
            int nombre = 0;
            while (sqlDR.Read())
            {
                nombre++;
            }
            string[,] Tab = new string[nombre, 7];
            sqlDR.Close();
            if (nombre > 0)
            {
                SqlDataReader sqlDR2 = sql.ExecuteReader();
                int cpt = 0;
                while (sqlDR2.Read())
                {
                    Tab[cpt, 0] = sqlDR2.GetInt32(0).ToString();
                    Tab[cpt, 1] = sqlDR2.GetString(1);
                    Tab[cpt, 2] = sqlDR2.GetString(2);
                    Tab[cpt, 3] = sqlDR2.GetString(3);
                    Tab[cpt, 4] = sqlDR2.GetString(4);
                    Tab[cpt, 5] = sqlDR2.GetString(5);
                    Tab[cpt, 6] = sqlDR2.GetInt32(6).ToString();
                    cpt++;
                }
                sqlDR2.Close();
            }
            conn.Close();

            return Tab;
        }

        public static string[] GetImageChevaux(SqlConnection conn, int IDCheval)
        {
            SqlCommand sql = new SqlCommand("select guidphoto from photo where idcheval = " + IDCheval + "");
            sql.Connection = conn;

            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();
            int nombre = 0;
            while (sqlDR.Read())
            {
                nombre++;
            }
            sqlDR.Close();
            string[] Tab = new string[nombre];
            if (nombre > 0)
            {
                SqlDataReader sqlDR2 = sql.ExecuteReader();
                int cpt = 0;

                while (sqlDR2.Read())
                {
                    Tab[cpt] = sqlDR2.GetString(0);
                    cpt++;
                }
                sqlDR2.Close();
            }
            conn.Close();

            return Tab;
        }

        public static int TrouverIDHasard(SqlConnection conn, int ID)
        {
            int chiffre = 1;

            SqlCommand sql = new SqlCommand("select id from cheval");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();
            int nombre = 0;
            while (sqlDR.Read())
            {
                nombre++;
            }
            int[] tab = new int[nombre];
            sqlDR.Close();
            if (nombre > 0)
            {
                SqlDataReader sqlDR2 = sql.ExecuteReader();

                int cpt = 0;
                while (sqlDR2.Read())
                {
                    tab[cpt] = sqlDR2.GetInt32(0);
                    cpt++;
                }
                sqlDR2.Close();
            }

            do
            {
                Random chiffrealatoire = new Random();
                int aleatoire = chiffrealatoire.Next(1, nombre);
                aleatoire = aleatoire - 1;

                chiffre = tab[aleatoire];

            } while (chiffre == ancienchiffre);
            ancienchiffre = chiffre;
            conn.Close();

            return chiffre;
        }

        public static bool NomUsagerExiste(string nomuser, SqlConnection conn)
        {
            bool resultat = false;

            SqlCommand sql = new SqlCommand("select nomusager from usager where nomusager = '" + nomuser + "'");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();

            if (sqlDR.Read())
            {
                resultat = true;
            }

            sqlDR.Close();
            conn.Close();

            return resultat;
        }

        public static string[] GetChevaux(string Username, SqlConnection conn)
        {
            SqlCommand sql = new SqlCommand("select nom from cheval where idusager = " + GetIDUsager(conn, Username));
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();
            int nombreresultat = 0;
            while (sqlDR.Read())
            {
                nombreresultat++;
            }
            string[] Tab = new string[nombreresultat];
            sqlDR.Close();

            if (nombreresultat > 0)
            {
                SqlDataReader sqlDR2 = sql.ExecuteReader();
                int cpt = 0;
                while (sqlDR2.Read())
                {
                    Tab[cpt] = sqlDR2.GetString(0);
                    cpt++;
                }
                sqlDR2.Close();
            }

            conn.Close();

            return Tab;
        }

        public static void InsertionImageCheval(int id, String GuidCheval, String IDCheval, SqlConnection conn)
        {
            GuidCheval = ReplaceRegex(GuidCheval);

            SqlCommand sql = new SqlCommand("insert into photo(ID,GuidPhoto,IDCheval) values (" + id + ",'" + GuidCheval + "'," + IDCheval + ")");
            sql.Connection = conn;
            conn.Open();

            int ligne = sql.ExecuteNonQuery();

            conn.Close();
        }

        public static void UpdateImageCheval(String NewGuidCheval, int IdCheval, SqlConnection conn, int Pos)
        {
            NewGuidCheval = ReplaceRegex(NewGuidCheval);

            SqlCommand sql = new SqlCommand("Update Photo set GuidPhoto ='" + NewGuidCheval + "' where IDCheval =" + IdCheval + " And ID =" + Pos);
            sql.Connection = conn;
            conn.Open();
            sql.ExecuteNonQuery();
            conn.Close();
        }

        public static int TrouverDernierID(SqlConnection conn, string table)
        {
            int id = 0;

            SqlCommand sql = new SqlCommand("select max(ID) from " + table + "");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlRD = sql.ExecuteReader();

            if (sqlRD.Read())
            {
                id = sqlRD.GetInt32(0);
            }
            conn.Close();
            sqlRD.Close();

            return id;
        }

        public static string[] GetInfoCheval(SqlConnection conn, int Id)
        {
            SqlCommand sql = new SqlCommand("select id, nom, description, emplacement, race, discipline, idusager from cheval where id = " + Id);
            sql.Connection = conn;
            conn.Open();
            string[] unTableau = new string[7];

            SqlDataReader sqlDRget = sql.ExecuteReader();
            if (sqlDRget.Read())
            {
                unTableau[0] = sqlDRget.GetInt32(0).ToString();
                unTableau[1] = sqlDRget.GetString(1);
                unTableau[2] = sqlDRget.GetString(2);
                unTableau[3] = sqlDRget.GetString(3);
                unTableau[4] = sqlDRget.GetString(4);
                unTableau[5] = sqlDRget.GetString(5);
                unTableau[6] = sqlDRget.GetInt32(6).ToString();
            }
            conn.Close();
            sqlDRget.Close();

            return unTableau;
        }

        public static int GetPremierID(SqlConnection conn)
        {
            int id = 0;

            SqlCommand sql = new SqlCommand("select min(ID) from cheval");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlRD = sql.ExecuteReader();

            if (sqlRD.Read())
            {
                id = sqlRD.GetInt32(0) + 1;
            }
            conn.Close();
            sqlRD.Close();

            return id;
        }

        public static bool InsertionUtilisateur(string nomuser, string nomcomplet, string motdepasse, string courriel, SqlConnection conn)
        {
            nomuser = ReplaceRegex(nomuser);
            nomcomplet = ReplaceRegex(nomcomplet);
            motdepasse = ReplaceRegex(motdepasse);
            courriel = ReplaceRegex(courriel);
            bool resultat = false;
            int dernierid = TrouverDernierID(conn, "usager") + 1;
            SqlCommand sql = new SqlCommand("insert into usager(ID, NomUsager, NomComplet, MotDePasse, AdresseCourriel)" +
                                            "values (" + dernierid + ",'" + nomuser + "', '" + nomcomplet + "', '" + motdepasse + "', '" + courriel + "')");
            sql.Connection = conn;
            conn.Open();

            int ligne = sql.ExecuteNonQuery();

            if (ligne == 1)
            {
                resultat = true;
            }

            conn.Close();

            return resultat;
        }

        public static bool VerifierConnexion(string nomuser, string motdepasse, SqlConnection conn)
        {
            bool resultat = false;

            SqlCommand sql = new SqlCommand("select NomUsager from usager where NomUsager = '" + nomuser + "' and MotDePasse = '" + motdepasse + "'");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();

            if (sqlDR.Read())
            {
                resultat = true;
            }

            sqlDR.Close();
            conn.Close();

            return resultat;
        }

        public static string ReplaceRegex(string unString)
        {
            unString = unString.Replace("'", "''");

            return unString;
        }

        public static bool InsertionCheval(string Nom, string Description, string Emplacement, string Race, string Discipline, string NomUsager, SqlConnection conn)
        {
            Nom = ReplaceRegex(Nom);
            Description = ReplaceRegex(Description);
            Emplacement = ReplaceRegex(Emplacement);
            Race = ReplaceRegex(Race);
            Discipline = ReplaceRegex(Discipline);
            bool resultat = false;
            int ID = GetIDUsager(conn, NomUsager);
            int IDDernier = TrouverDernierID(conn, "Cheval") + 1;
            if (ID != -1)
            {
                SqlCommand sql = new SqlCommand("insert into cheval (id, nom, description, emplacement, race, discipline, idusager) " +
                                                " values (" + IDDernier + ",'" + Nom + "', '" + Description + "', '" + Emplacement + "', '" + Race + "', '" + Discipline + "', " + ID + ")");
                sql.Connection = conn;
                conn.Open();

                int ligne = sql.ExecuteNonQuery();

                if (ligne == 1)
                {
                    resultat = true;
                }

                conn.Close();
            }

            return resultat;
        }

        public static int GetIDUsager(SqlConnection conn, string NomUsager)
        {
            int id = -1;

            SqlCommand sql = new SqlCommand("select Id from usager where NomUsager = '" + NomUsager + "'");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();

            if (sqlDR.Read())
            {
                id = sqlDR.GetInt32(0);
            }
            sqlDR.Close();
            conn.Close();
            return id;
        }

        public static string GetPassword(string Username, SqlConnection conn)
        {
            string mdp = "";

            SqlCommand sql = new SqlCommand("select motdepasse from Usager where NomUsager = '" + Username + "'");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();

            if (sqlDR.Read())
            {
                mdp = sqlDR.GetString(0);
            }
            sqlDR.Close();
            conn.Close();

            return mdp;
        }

        public static string GetEmail(string Username, SqlConnection conn)
        {
            string email = "";

            SqlCommand sql = new SqlCommand("select adressecourriel from Usager where NomUsager = '" + Username + "'");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();

            if (sqlDR.Read())
            {
                email = sqlDR.GetString(0);
            }
            sqlDR.Close();
            conn.Close();

            return email;
        }

        public static bool UpdateCheval(int ID, string Nom, string Description, string Emplacement, string Race, string Discipline, string NomUsager, SqlConnection conn)
        {
            Nom = ReplaceRegex(Nom);
            Description = ReplaceRegex(Description);
            Emplacement = ReplaceRegex(Emplacement);
            Race = ReplaceRegex(Race);
            Discipline = ReplaceRegex(Discipline);
            bool resultat = false;
            int IDUsager = GetIDUsager(conn, NomUsager);

            if (IDUsager != -1)
            {
                SqlCommand sql = new SqlCommand(" update cheval set nom = '" + Nom + "', "
                                                                + " description = '" + Description + "', "
                                                                + " emplacement = '" + Emplacement + "', "
                                                                + " race = '" + Race + "', "
                                                                + " discipline = '" + Discipline + "'"
                                                                + " where ID = " + ID);
                sql.Connection = conn;
                conn.Open();

                if (1 == sql.ExecuteNonQuery())
                {
                    resultat = true;
                }

                conn.Close();
            }

            return resultat;
        }

        public static bool SupprimerUnCheval (SqlConnection conn, int IDCheval)
        {
            bool supprimer = false;

            SqlCommand sql2 = new SqlCommand("delete from photo where idcheval =" + IDCheval);
            SqlCommand sql = new SqlCommand("delete from cheval where id =" + IDCheval);
            sql2.Connection = conn;
            sql.Connection = conn;

            conn.Open();

            int j = sql2.ExecuteNonQuery();
            int i = sql.ExecuteNonQuery();

            if(i > 0 && j > 0)
            {
                supprimer = true; 
            }

            conn.Close();

            return supprimer; 
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PoneyLover3._0.Models
{
    public class ClassLiaisonBD
    {
        private static int ancienchiffre = 1;


        public static int GetIDCheval(SqlConnection conn, string name)
        {
            int id = 1;

            SqlCommand sql = new SqlCommand("select Id from cheval where nom = '" + name + "'");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();

            if (sqlDR.Read())
            {
                id = sqlDR.GetInt32(0);
            }
            sqlDR.Close();
            conn.Close();
            return id;
        }

        public static string GetNomUsager(SqlConnection conn, int id)
        {
            string nomusager = "";
            try
            {
                SqlCommand sql = new SqlCommand("select nomcomplet from usager where id = " + id);
                sql.Connection = conn;

                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Open();

                SqlDataReader sqlDR = sql.ExecuteReader();

                if (sqlDR.Read())
                {
                    nomusager = sqlDR.GetString(0);
                }

                conn.Close();
                sqlDR.Close();
            }
            catch (SqlException ex)
            {
                string la = ex.Message;
            }

            return nomusager;
        }



        public static string[,] GetListChevaux(SqlConnection conn)
        {
            SqlCommand sql = new SqlCommand("select id, nom, description, emplacement, race, discipline, idusager from cheval ");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();
            int nombre = 0;
            while (sqlDR.Read())
            {
                nombre++;
            }
            string[,] Tab = new string[nombre, 7];
            sqlDR.Close();
            if (nombre > 0)
            {
                SqlDataReader sqlDR2 = sql.ExecuteReader();
                int cpt = 0;
                while (sqlDR2.Read())
                {
                    Tab[cpt, 0] = sqlDR2.GetInt32(0).ToString();
                    Tab[cpt, 1] = sqlDR2.GetString(1);
                    Tab[cpt, 2] = sqlDR2.GetString(2);
                    Tab[cpt, 3] = sqlDR2.GetString(3);
                    Tab[cpt, 4] = sqlDR2.GetString(4);
                    Tab[cpt, 5] = sqlDR2.GetString(5);
                    Tab[cpt, 6] = sqlDR2.GetInt32(6).ToString();
                    cpt++;
                }
                sqlDR2.Close();
            }
            conn.Close();

            return Tab;
        }

        public static string[] GetImageChevaux(SqlConnection conn, int IDCheval)
        {
            SqlCommand sql = new SqlCommand("select guidphoto from photo where idcheval = " + IDCheval + "");
            sql.Connection = conn;

            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();
            int nombre = 0;
            while (sqlDR.Read())
            {
                nombre++;
            }
            sqlDR.Close();
            string[] Tab = new string[nombre];
            if (nombre > 0)
            {
                SqlDataReader sqlDR2 = sql.ExecuteReader();
                int cpt = 0;

                while (sqlDR2.Read())
                {
                    Tab[cpt] = sqlDR2.GetString(0);
                    cpt++;
                }
                sqlDR2.Close();
            }
            conn.Close();

            return Tab;
        }

        public static int TrouverIDHasard(SqlConnection conn, int ID)
        {
            int chiffre = 1;

            SqlCommand sql = new SqlCommand("select id from cheval");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();
            int nombre = 0;
            while (sqlDR.Read())
            {
                nombre++;
            }
            int[] tab = new int[nombre];
            sqlDR.Close();
            if (nombre > 0)
            {
                SqlDataReader sqlDR2 = sql.ExecuteReader();

                int cpt = 0;
                while (sqlDR2.Read())
                {
                    tab[cpt] = sqlDR2.GetInt32(0);
                    cpt++;
                }
                sqlDR2.Close();
            }

            do
            {
                Random chiffrealatoire = new Random();
                int aleatoire = chiffrealatoire.Next(1, nombre);
                aleatoire = aleatoire - 1;

                chiffre = tab[aleatoire];

            } while (chiffre == ancienchiffre);
            ancienchiffre = chiffre;
            conn.Close();

            return chiffre;
        }

        public static bool NomUsagerExiste(string nomuser, SqlConnection conn)
        {
            bool resultat = false;

            SqlCommand sql = new SqlCommand("select nomusager from usager where nomusager = '" + nomuser + "'");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();

            if (sqlDR.Read())
            {
                resultat = true;
            }

            sqlDR.Close();
            conn.Close();

            return resultat;
        }

        public static string[] GetChevaux(string Username, SqlConnection conn)
        {
            SqlCommand sql = new SqlCommand("select nom from cheval where idusager = " + GetIDUsager(conn, Username));
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();
            int nombreresultat = 0;
            while (sqlDR.Read())
            {
                nombreresultat++;
            }
            string[] Tab = new string[nombreresultat];
            sqlDR.Close();

            if (nombreresultat > 0)
            {
                SqlDataReader sqlDR2 = sql.ExecuteReader();
                int cpt = 0;
                while (sqlDR2.Read())
                {
                    Tab[cpt] = sqlDR2.GetString(0);
                    cpt++;
                }
                sqlDR2.Close();
            }

            conn.Close();

            return Tab;
        }

        public static void InsertionImageCheval(int id, String GuidCheval, String IDCheval, SqlConnection conn)
        {
            GuidCheval = ReplaceRegex(GuidCheval);

            SqlCommand sql = new SqlCommand("insert into photo(ID,GuidPhoto,IDCheval) values (" + id + ",'" + GuidCheval + "'," + IDCheval + ")");
            sql.Connection = conn;
            conn.Open();

            int ligne = sql.ExecuteNonQuery();

            conn.Close();
        }

        public static void UpdateImageCheval(String NewGuidCheval, int IdCheval, SqlConnection conn, int Pos)
        {
            NewGuidCheval = ReplaceRegex(NewGuidCheval);

            SqlCommand sql = new SqlCommand("Update Photo set GuidPhoto ='" + NewGuidCheval + "' where IDCheval =" + IdCheval + " And ID =" + Pos);
            sql.Connection = conn;
            conn.Open();
            sql.ExecuteNonQuery();
            conn.Close();
        }

        public static int TrouverDernierID(SqlConnection conn, string table)
        {
            int id = 0;

            SqlCommand sql = new SqlCommand("select max(ID) from " + table + "");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlRD = sql.ExecuteReader();

            if (sqlRD.Read())
            {
                id = sqlRD.GetInt32(0);
            }
            conn.Close();
            sqlRD.Close();

            return id;
        }

        public static string[] GetInfoCheval(SqlConnection conn, int Id)
        {
            SqlCommand sql = new SqlCommand("select id, nom, description, emplacement, race, discipline, idusager from cheval where id = " + Id);
            sql.Connection = conn;
            conn.Open();
            string[] unTableau = new string[7];

            SqlDataReader sqlDRget = sql.ExecuteReader();
            if (sqlDRget.Read())
            {
                unTableau[0] = sqlDRget.GetInt32(0).ToString();
                unTableau[1] = sqlDRget.GetString(1);
                unTableau[2] = sqlDRget.GetString(2);
                unTableau[3] = sqlDRget.GetString(3);
                unTableau[4] = sqlDRget.GetString(4);
                unTableau[5] = sqlDRget.GetString(5);
                unTableau[6] = sqlDRget.GetInt32(6).ToString();
            }
            conn.Close();
            sqlDRget.Close();

            return unTableau;
        }

        public static int GetPremierID(SqlConnection conn)
        {
            int id = 0;

            SqlCommand sql = new SqlCommand("select min(ID) from cheval");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlRD = sql.ExecuteReader();

            if (sqlRD.Read())
            {
                id = sqlRD.GetInt32(0) + 1;
            }
            conn.Close();
            sqlRD.Close();

            return id;
        }

        public static bool InsertionUtilisateur(string nomuser, string nomcomplet, string motdepasse, string courriel, SqlConnection conn)
        {
            nomuser = ReplaceRegex(nomuser);
            nomcomplet = ReplaceRegex(nomcomplet);
            motdepasse = ReplaceRegex(motdepasse);
            courriel = ReplaceRegex(courriel);
            bool resultat = false;
            int dernierid = TrouverDernierID(conn, "usager") + 1;
            SqlCommand sql = new SqlCommand("insert into usager(ID, NomUsager, NomComplet, MotDePasse, AdresseCourriel)" +
                                            "values (" + dernierid + ",'" + nomuser + "', '" + nomcomplet + "', '" + motdepasse + "', '" + courriel + "')");
            sql.Connection = conn;
            conn.Open();

            int ligne = sql.ExecuteNonQuery();

            if (ligne == 1)
            {
                resultat = true;
            }

            conn.Close();

            return resultat;
        }

        public static bool VerifierConnexion(string nomuser, string motdepasse, SqlConnection conn)
        {
            bool resultat = false;

            SqlCommand sql = new SqlCommand("select NomUsager from usager where NomUsager = '" + nomuser + "' and MotDePasse = '" + motdepasse + "'");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();

            if (sqlDR.Read())
            {
                resultat = true;
            }

            sqlDR.Close();
            conn.Close();

            return resultat;
        }

        public static string ReplaceRegex(string unString)
        {
            unString = unString.Replace("'", "''");

            return unString;
        }

        public static bool InsertionCheval(string Nom, string Description, string Emplacement, string Race, string Discipline, string NomUsager, SqlConnection conn)
        {
            Nom = ReplaceRegex(Nom);
            Description = ReplaceRegex(Description);
            Emplacement = ReplaceRegex(Emplacement);
            Race = ReplaceRegex(Race);
            Discipline = ReplaceRegex(Discipline);
            bool resultat = false;
            int ID = GetIDUsager(conn, NomUsager);
            int IDDernier = TrouverDernierID(conn, "Cheval") + 1;
            if (ID != -1)
            {
                SqlCommand sql = new SqlCommand("insert into cheval (id, nom, description, emplacement, race, discipline, idusager) " +
                                                " values (" + IDDernier + ",'" + Nom + "', '" + Description + "', '" + Emplacement + "', '" + Race + "', '" + Discipline + "', " + ID + ")");
                sql.Connection = conn;
                conn.Open();

                int ligne = sql.ExecuteNonQuery();

                if (ligne == 1)
                {
                    resultat = true;
                }

                conn.Close();
            }

            return resultat;
        }

        public static int GetIDUsager(SqlConnection conn, string NomUsager)
        {
            int id = -1;

            SqlCommand sql = new SqlCommand("select Id from usager where NomUsager = '" + NomUsager + "'");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();

            if (sqlDR.Read())
            {
                id = sqlDR.GetInt32(0);
            }
            sqlDR.Close();
            conn.Close();
            return id;
        }

        public static string GetPassword(string Username, SqlConnection conn)
        {
            string mdp = "";

            SqlCommand sql = new SqlCommand("select motdepasse from Usager where NomUsager = '" + Username + "'");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();

            if (sqlDR.Read())
            {
                mdp = sqlDR.GetString(0);
            }
            sqlDR.Close();
            conn.Close();

            return mdp;
        }

        public static string GetEmail(string Username, SqlConnection conn)
        {
            string email = "";

            SqlCommand sql = new SqlCommand("select adressecourriel from Usager where NomUsager = '" + Username + "'");
            sql.Connection = conn;
            conn.Open();

            SqlDataReader sqlDR = sql.ExecuteReader();

            if (sqlDR.Read())
            {
                email = sqlDR.GetString(0);
            }
            sqlDR.Close();
            conn.Close();

            return email;
        }

        public static bool UpdateCheval(int ID, string Nom, string Description, string Emplacement, string Race, string Discipline, string NomUsager, SqlConnection conn)
        {
            Nom = ReplaceRegex(Nom);
            Description = ReplaceRegex(Description);
            Emplacement = ReplaceRegex(Emplacement);
            Race = ReplaceRegex(Race);
            Discipline = ReplaceRegex(Discipline);
            bool resultat = false;
            int IDUsager = GetIDUsager(conn, NomUsager);

            if (IDUsager != -1)
            {
                SqlCommand sql = new SqlCommand(" update cheval set nom = '" + Nom + "', "
