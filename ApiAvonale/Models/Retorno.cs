using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiAvonale.Models
{
    public class Retorno
    {
        public int id { get; set; }
        public string nome { get; set; }
        public decimal valor_unitario { get; set; }
        public int qtde_estoque { get; set; }
        public DateTime? dtUltimaVenda { get; set; }
        public decimal? vlUltimaVenda { get; set; }
    }
}
