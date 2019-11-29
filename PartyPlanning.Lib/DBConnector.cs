using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace PartyPlanning.Lib
{
    public class DBConnector
    {
        public static DataTable ExecuteSelect(string sqlInstructie, string dbNaam = "Party")
        {
            string constring = ConfigurationManager.ConnectionStrings[dbNaam].ToString();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(sqlInstructie, constring);
            try
            {
                da.Fill(ds);
            }
            catch (Exception fout)
            {
                throw new Exception(fout.Message);
            }
            return ds.Tables[0];
        }

        public static bool ExecuteCommand(string sqlInstructie, string dbNaam = "Party")
        {
            bool opgeslagen = true;
            string constring = ConfigurationManager.ConnectionStrings[dbNaam].ToString();
            SqlConnection mijnVerbinding = new SqlConnection(constring);
            SqlCommand mijnOpdracht = new SqlCommand(sqlInstructie, mijnVerbinding);
            try
            {
                mijnOpdracht.Connection.Open();
                mijnOpdracht.ExecuteNonQuery();
                mijnVerbinding.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Opslaan mislukt\n" + ex.Message);
            }
            return opgeslagen;
        }

    }
}
