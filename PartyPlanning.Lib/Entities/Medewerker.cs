using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyPlanning.Lib.Entities
{
    public class Medewerker
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string naam;

        public string Naam
        {
            get { return naam; }
            set
            {
                if (value.Trim().Length < 4)
                {
                    throw new Exception("De naam moet minimum 4 tekens lang zijn");
                }
                else
                {
                    naam = value;
                }

            }
        }

        private DateTime geboorteDatum;

        public DateTime GeboorteDatum
        {
            get { return geboorteDatum; }
            set
            {
                if (value > DateTime.Now)
                {
                    throw new Exception("De geboortedatum kan niet in de toekomst liggen");
                }
                else if (Leeftijd < 18)
                {
                    throw new Exception("De minimumleeftijd is 18");
                }
                else
                {
                    geboorteDatum = value;
                }
            }
        }

        public float Leeftijd
        {
            get
            {
                int leeftijd;
                DateTime vandaag = DateTime.Today;
                int vandaagInt = int.Parse($"{vandaag.Year}{vandaag.Month.ToString("00")}{vandaag.Day.ToString("00")}");
                int geboortedatumInt = int.Parse($"{GeboorteDatum.Year}{GeboorteDatum.Month.ToString("00")}{GeboorteDatum.Day.ToString("00")}");
                leeftijd = (vandaagInt - geboortedatumInt) / 10000;
                return leeftijd;
            }
        }

        public Medewerker(int id, string naam, DateTime geboortedatum)
        {
            Id = id;
            Naam = naam;
            GeboorteDatum = geboortedatum;
        }
    }
}
