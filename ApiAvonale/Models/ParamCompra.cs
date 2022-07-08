using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiAvonale.Models
{
    public class ParamCompra
    {
        public int produto_id { get; set; }
        public int qtde_comprada { get; set; }
        public Cartao cartao { get; set; }
    }

    public class Cartao
    {
        public string titular { get; set; }
        public string numero { get; set; }
        public DateTime data_expiracao { get; set; }
        public string bandeira { get; set; }
        public string cvv { get; set; }
    }
}
