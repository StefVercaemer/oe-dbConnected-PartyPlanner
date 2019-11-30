using PartyPlanning.Lib.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyPlanning.Lib
{
    public class ShiftBeheer
    {
        const string TabelNaam = "Shift";
        const string CnId = "Id";
        const string CnMedewerker_id = "Medewerker_id";
        const string CnTaak_id = "Taak_id";
        const string CnUur = "Uur";
        const string CnOpmerking = "Opmerking";

        public DataView DvShifts { get; set; }

        public ShiftBeheer()
        {
            VulDvShifts();
        }

        void VulDvShifts()
        {
            DvShifts = new DataView
            {
                Table = GeefAlleRecords(),
                Sort = "Uur ASC, Taak_id ASC"
            };
        }

        public List<Shift> ShiftLijst
        {
            get 
            {
                VulDvShifts();
                List<Shift> shiftLijst = new List<Shift>();
                foreach (DataRowView drMedewerker in DvShifts)
                {
                    int id = (int)drMedewerker[CnId];
                    int medewerkerId = (int)drMedewerker[CnMedewerker_id];
                    int taakId = (int)drMedewerker[CnTaak_id];
                    int uur = (int)drMedewerker[CnUur];
                    string opmerking = drMedewerker[CnOpmerking].ToString();
                    Shift shift = new Shift
                    {
                        Id = id,
                        ToegewezenTaak = TaakBeheer.GeefTaakObject(taakId),
                        Uur = uur, 
                        Verantwoordelijke = MedewerkerBeheer.GeefMedewerkerObject(medewerkerId),
                        Opmerkingen = opmerking
                    };
                    Console.WriteLine("ShiftId: " + id);
                    shiftLijst.Add(shift);
                }

                return shiftLijst; 
            }

        }

        public DataTable GeefAlleRecords()
        {
            string sql;
            sql = "select * from " + TabelNaam;
            return DBConnector.ExecuteSelect(sql);
        }

        public static int GeefNieuwId()
        {
            string sql;
            sql = $"select max({CnId}) from {TabelNaam}";
            return int.Parse(DBConnector.ExecuteSelect(sql).Rows[0][0].ToString()) + 1;
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

        public static bool SlaOp(Shift shift)
        {
            string sql;
            int shiftId = shift.Id;
            int medewerkerId = shift.Verantwoordelijke.Id;
            int taakId = shift.ToegewezenTaak.Id;
            int uur = shift.Uur;
            string opmerkingen = shift.Opmerkingen;

            try
            {
                bool nieuwRecord = GeefRecord(shiftId) == null ? true : false;
                if (nieuwRecord)
                {
                    sql = $"insert into {TabelNaam} ({CnMedewerker_id},{CnOpmerking},{CnTaak_id},{CnUur}) values " +
                                                  $"({medewerkerId},'{opmerkingen}',{taakId}, {uur})";
                }
                else
                {
                    sql = $"update {TabelNaam} set " +
                        $"{CnMedewerker_id} = {medewerkerId}, " +
                        $"{CnOpmerking} = '{opmerkingen}', " +
                        $"{CnTaak_id} = {taakId}, " +
                        $"{CnUur} = {uur} " +
                        $"where {CnId} = {shiftId}";
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
