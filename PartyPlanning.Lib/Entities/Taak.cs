using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyPlanning.Lib.Entities
{
    public struct Taak
    {
        public int Id { get; set; }

        public string Naam { get; set; }


        public override string ToString()
        {
            return Naam;
        }
    }
}
