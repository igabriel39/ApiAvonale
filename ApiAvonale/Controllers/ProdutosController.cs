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
        #region Conexão
        // Conexão com o banco que será usada em todas as rotas
        Conexao conexao = new Conexao();
        #endregion

        #region Campos
        public enum Campos
        {
            ID_PRODUTO = 100 // Número do id do produto passado na requisição para rota de buscar produtos ou deletar produtos
        }
        #endregion

        // Rota para inserir produto
        public HttpResponseMessage Post( ParamAdicionar param )
        {
            try
            {
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

        // Rota para consultar os produtos
        public HttpResponseMessage Get()
        {
            try
            {
                Dictionary<string, string> queryString = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
                GatewayProdutos dados = new GatewayProdutos();
                List<Retorno> retorno = new List<Retorno>();

                // Se for passado o parametro 100 do id do produto, a consulta será por um produto específico
                // Se não for passado o parametro 100 do ido do produto, a consulta será por todos os produtos
                retorno = dados.Get(queryString, conexao.connectionString);
                return Request.CreateResponse(HttpStatusCode.OK, retorno);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }

        // Rota para excluir um produto
        public HttpResponseMessage Delete()
        {
            try
            {
                Dictionary<string, string> queryString = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);

                GatewayProdutos.DeleteProduto(queryString, conexao.connectionString);
                return Request.CreateResponse(HttpStatusCode.OK, "Produto excluído com sucesso");
            }
            catch (Exception e)
            {
                if (e.Message.Split(':')[0].Contains("Consulta"))
                    return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message.Split(':')[1]);
                else
                    return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }
    }
}
