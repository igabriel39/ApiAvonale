using ApiAvonale.Models;
using ApiAvonale.Negocios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ApiAvonale.Controllers
{
    public class ProdutosController : ApiController
    {
        // Rota para inserir produto
        public HttpResponseMessage Post( ParamAdicionar param )
        {
            try
            {
                Conexao conexao = new Conexao();
                GatewayProdutos.AddProduto(param, conexao.connectionString);
                return Request.CreateResponse(HttpStatusCode.OK, "Produto Cadastrado");
            }
            catch (Exception e)
            {
                if (e.Message.Split(';')[0] == "0")
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, (e.Message.Split(';')[1]));
                else
                    return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }
    }
}
