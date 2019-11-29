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
        const string IdNaam = "Id";
        const string OmschrijvingsVeldNaam = "Medewerker";
        const string GeboortedatumVeldNaam = "Geboortedatum";

        public static DataView dvMedeWerkers;

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
            sql = $"select * from {TabelNaam} where {IdNaam} = " + id.ToString();
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
            sql = $"select * from {TabelNaam} where {IdNaam} = " + id.ToString();
            DataTable tabel = DBConnector.ExecuteSelect(sql);
            if (tabel.Rows.Count > 0)
            {
                return tabel.Rows[0];
            }
            else
                return null;
        }

        public static int GeefNieuwId()
        {
            string sql;
            sql = $"select max({IdNaam}) from {TabelNaam}";
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
                    sql = $"insert into {TabelNaam} ({OmschrijvingsVeldNaam},{GeboortedatumVeldNaam}) values " +
                                                  $"('{naam}',{Helper.DatumInSqlNotatie(geboorteDatum)})";
                }
                else
                {
                    sql = $"update {TabelNaam} set " +
                        $"{OmschrijvingsVeldNaam} = '{naam}', " +
                        $"{GeboortedatumVeldNaam} = {Helper.DatumInSqlNotatie(medewerker.GeboorteDatum)} " +
                        $"where {IdNaam} = {medewerkerId}";
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
            string sql;
            //sql = $"select count(*) from Rooster where medewerker_id = {id}";
            //if ((int)DBConnector.ExecuteSelect(sql).Rows[0][0] != 0)
            //    throw new Exception("Er zijn nog taken toegewezen aan deze medewerker");

            sql = $"delete from {TabelNaam} where {IdNaam} = {id}";
            return DBConnector.ExecuteCommand(sql);
        }

    }
}
