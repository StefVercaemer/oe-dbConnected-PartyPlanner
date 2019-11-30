using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PartyPlanning.Lib.Entities;

namespace PartyPlanning.Lib
{
    public class MedewerkerBeheer
    {
        const string TabelNaam = "Medewerker";
        const string CnId = "Id";
        const string CnOmschrijving = "Medewerker";
        const string CnGeboortedatum= "Geboortedatum";

        public static DataView DvMedeWerkers { get; set; }

        public static List<Medewerker> Medewerkers { get; set; }

        public static List<string> NamenLijst
        {
            get
            {
                List<string> namenLijst = new List<string>();
                foreach (DataRowView drMedewerker in DvMedeWerkers)
                {
                    namenLijst.Add(drMedewerker[CnOmschrijving].ToString());
                    namenLijst.Sort();
                }
                return namenLijst;
            } 
            //set; 
        }



        public static void LaadDvMedeWerkers()
        {
            Medewerkers = new List<Medewerker>();
            DataView gesorteerdeTabel = new DataView
            {
                Table = GeefAlleRecords(),
                Sort = "Medewerker ASC"
            };
            DvMedeWerkers = gesorteerdeTabel;

            foreach (DataRow  drMedewerker in GeefAlleRecords().Rows)
            {
                Medewerker medewerker = new Medewerker
                    (
                        (int)drMedewerker[CnId],
                        drMedewerker[CnOmschrijving].ToString(),
                        (DateTime)drMedewerker[CnGeboortedatum]
                    );
                Medewerkers.Add(medewerker);
            }

        }

        public static DataTable GeefAlleRecords()
        {
            string sql;
            sql = "select * from " + TabelNaam;
            return DBConnector.ExecuteSelect(sql);
        }
        public static DataTable GeefAlleRecords(string sorteerOpVeld, bool opklimmend = true)
        {
            string sql;
            string sorteerVolgorde = opklimmend ? "ASC" : "DESC";

            sql = "select * from " + TabelNaam;
            sql += " order by " + sorteerOpVeld + " " + sorteerVolgorde;
            return DBConnector.ExecuteSelect(sql);
        }
        public static string GeefVeldInfo(int id, string veldnaam)
        {
            string sql;
            sql = $"select * from {TabelNaam} where {CnId} = " + id.ToString();
            DataTable tabel = DBConnector.ExecuteSelect(sql);
            if (tabel.Rows.Count > 0)
            {
                return tabel.Rows[0][veldnaam].ToString();
            }
            else
                return null;
        }

        public static DataRow GeefRecord(int id)
        {
            string sql;
            sql = $"select * from {TabelNaam} where {CnId} = " + id.ToString();
            DataTable tabel = DBConnector.ExecuteSelect(sql);
            if (tabel.Rows.Count > 0)
            {
                return tabel.Rows[0];
            }
            else
                return null;
        }

        public static Medewerker GeefMedewerkerObject(int id)
        {
            DataRow record = GeefRecord(id);
            Medewerker medewerker = new Medewerker
            (id, record[CnOmschrijving].ToString(), (DateTime)record[CnGeboortedatum]

            );
            return medewerker;
        }

        public static int GeefNieuwId()
        {
            string sql;
            sql = $"select max({CnId}) from {TabelNaam}";
            return int.Parse(DBConnector.ExecuteSelect(sql).Rows[0][0].ToString()) + 1;
        }

        public static bool SlaOp(Medewerker medewerker)
        {
            string sql;
            int medewerkerId = medewerker.Id;
            string naam = Helper.HandleQuotes(medewerker.Naam);
            DateTime geboorteDatum = medewerker.GeboorteDatum;

            try
            {
                bool nieuwRecord = GeefRecord(medewerkerId) == null ? true : false;
                if (nieuwRecord)
                {
                    sql = $"insert into {TabelNaam} ({CnOmschrijving},{CnGeboortedatum}) values " +
                                                  $"('{naam}',{Helper.DatumInSqlNotatie(geboorteDatum)})";
                }
                else
                {
                    sql = $"update {TabelNaam} set " +
                        $"{CnOmschrijving} = '{naam}', " +
                        $"{CnGeboortedatum} = {Helper.DatumInSqlNotatie(medewerker.GeboorteDatum)} " +
                        $"where {CnId} = {medewerkerId}";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return DBConnector.ExecuteCommand(sql);
        }

        public static bool VerwijderRecord(int id)
        {
             string sql = $"delete from {TabelNaam} where {CnId} = {id}";
            return DBConnector.ExecuteCommand(sql);
        }

    }
}
