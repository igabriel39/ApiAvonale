using ApiAvonale.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiAvonale.Negocios
{
    public class GatewayProdutos
    {
        public static void AddProduto(ParamAdicionar param, string connectionString)
        {
            SqlConnection conn = new SqlConnection(connectionString);

        }
    }
}
