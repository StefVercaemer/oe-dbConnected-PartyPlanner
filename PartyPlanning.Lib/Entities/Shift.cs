using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyPlanning.Lib.Entities
{
    public class Shift
    {
        public static List<int> InTeVullenUren{ get; set; } = new List<int>() { 20, 21, 22, 23 };

        public int Id { get; set; }

        private Medewerker verantwoordelijke;

        public Medewerker Verantwoordelijke
        {
            get { return verantwoordelijke; }
            set 
            {
                if (value == null)
                {
                    throw new Exception("Duid een verantwoordelijke aan");
                }
                else
                {
                    verantwoordelijke = value; 
                }
            }
        }

        public Taak ToegewezenTaak { get; set; }

        public string Opmerkingen { get; set; }

        private int uur;

        public int Uur
        {
            get { return uur; }
            set
            {
                if (InTeVullenUren.Contains(value))
                {
                    uur = value;
                }
                else
                {
                    throw new Exception($"{value} is geen geldig uur\nKies een uur tussen 20 en 23 uur");
                }
            }
        }

        public override string ToString()
        {
            return $"{Uur} u.: {ToegewezenTaak} - { Verantwoordelijke }";
        }
    }
}
