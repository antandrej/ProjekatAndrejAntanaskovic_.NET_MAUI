using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjekatAndrejAntanaskovic.Models
{
    public enum StatusTermina
    {
        Zakazan,
        Završen,
        Otkazan
    }

    public class Termin
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UslugaId { get; set; }
        public string NazivUsluge { get; set; }
        public string ImeKlijenta { get; set; }
        public string TelefonKlijenta { get; set; }
        public string UredjajId { get; set; }
        public DateTime DatumVreme { get; set; }
        public StatusTermina Status { get; set; } = StatusTermina.Zakazan;
        public DateTime KreiranDatum { get; set; } = DateTime.Now;
    }
}
