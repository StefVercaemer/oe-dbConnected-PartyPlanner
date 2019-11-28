using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyPlanning.Lib
{
    public class MedewerkerBeheer
    {
        const string TabelNaam = "Medewerker";
        const string IdNaam = "Id";
        const string OmschrijvingsVeldNaam = "Medewerker";

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

        public static bool VoegRecordToe(string naam, bool autoNummering = true)
        {
            string sql;
            naam = Helper.HandleQuotes(naam);
            if (naam.Length == 0)
                return false;

            if (!autoNummering)
            {
                sql = $"select max({IdNaam}) from {TabelNaam}";
                int nieuwe_id = int.Parse(DBConnector.ExecuteSelect(sql).Rows[0][0].ToString()) + 1;
                sql = $"insert into {TabelNaam} ({IdNaam}, naam) values ({nieuwe_id},'{naam}')";
            }
            else
            {
                sql = $"insert into {TabelNaam} ({OmschrijvingsVeldNaam}) values ('{naam}')";
            }

            return DBConnector.ExecuteCommand(sql);
        }

        public static bool WijzigOmschrijving(int id, string nieuweWaarde)
        {
            nieuweWaarde = Helper.HandleQuotes(nieuweWaarde);
            if (nieuweWaarde.Length == 0)
                return false;
            string sql = $"update {TabelNaam} set {OmschrijvingsVeldNaam} = '{nieuweWaarde}' where {IdNaam} = {id}";
            return DBConnector.ExecuteCommand(sql);
        }

        public static bool VerwijderAuteur(int id)
        {
            string sql;
            sql = $"select count(*) from Rooster where medewerker_id = {id}";
            if ((int)DBConnector.ExecuteSelect(sql).Rows[0][0] != 0)
                throw new Exception("Er zijn nog taken toegewezen aan deze medewerker");

            sql = $"delete from {TabelNaam} where {IdNaam} = {id}";
            return DBConnector.ExecuteCommand(sql);
        }

    }
}
