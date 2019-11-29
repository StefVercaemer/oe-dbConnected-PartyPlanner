using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyPlanning.Lib.Entities
{
    public class Shift
    {
        int[] InTeVullenUren = { 20, 21, 22, 23 };

        public int Id { get; set; }

        public Medewerker Verantwoordelijke { get; set; }

        public Taak ToegewezenTaak { get; set; }

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
    }
}
