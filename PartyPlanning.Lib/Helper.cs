﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyPlanning.Lib
{
    public class Helper
    {
        public static string HandleQuotes(string waarde)
        {
            return waarde.Trim().Replace("'", "''");
        }

        public static string DatumInSqlNotatie(DateTime datum)
        {
            return $"'{datum.Year}-{datum.Month}-{datum.Day}'";
        }
    }
}
