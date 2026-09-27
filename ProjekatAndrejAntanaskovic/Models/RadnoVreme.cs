using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjekatAndrejAntanaskovic.Models
{
    public class RadnoVreme
    {
        public TimeSpan PocetakRadnogVremena { get; set; } = new TimeSpan(9, 0, 0);
        public TimeSpan KrajRadnogVremena { get; set; } = new TimeSpan(17, 0, 0);
        public int IntervalMinuti { get; set; } = 30;
    }
}
