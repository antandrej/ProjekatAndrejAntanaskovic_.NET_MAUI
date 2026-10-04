using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjekatAndrejAntanaskovic.Models
{
    public class Usluga
    {
        public string Id { get; set; }
        public string Naziv { get; set; }
        public int TrajanjeMinuti { get; set; }
        public decimal Cena { get; set; }
        public string Opis { get; set; }
    }
}
