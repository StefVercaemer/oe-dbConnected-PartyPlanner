using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using PartyPlanning.Lib.Entities;

namespace PartyPlanning.Lib
{
    public class TaakBeheer
    {
        const string TabelNaam = "Taak";

        const string CnId = "Id";
        const string CnTaak = "Taak";

        public static List<Taak> Taken { get; private set; }

        public static void LaadDvTaken()
        {
            Taken = new List<Taak>();
            DataTable taken;
            string sql;

            sql = $"select * from {TabelNaam} order by {CnTaak} ASC";

            taken = DBConnector.ExecuteSelect(sql);

            foreach (DataRow drTaak in taken.Rows)
            {
                Taak taak = new Taak
                {
                    Id = (int)drTaak[CnId],
                    Naam = drTaak[CnTaak].ToString()
                };
                Taken.Add(taak);
            }
        }

        public static bool VoegRecordToe(string naam)
        {
            string sql;
            try
            {
                sql = $"insert into {TabelNaam} ({CnTaak}) values " +
                              $"('{naam}')";

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            return DBConnector.ExecuteCommand(sql); ;
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

        public static Taak GeefTaakObject(int id)
        {
            DataRow record = GeefRecord(id);
            Taak taak = new Taak
            {
                Id = id,
                Naam = record[CnTaak].ToString()
            };
            return taak;
        }

    }
}
