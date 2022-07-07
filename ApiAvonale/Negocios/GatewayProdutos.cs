using ApiAvonale.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiAvonale.Negocios
{
    public class GatewayProdutos
    {
        #region Campos
        public enum Campos
        {
            ID_PRODUTO = 100 // Número do id do produto para fazer a busca detalhada pelo produto, se o número não for passado na requisição então a busca será feita por todos os produtos
        }
        #endregion

        public static void AddProduto(ParamAdicionar param, string connectionString)
        {
            SqlConnection conn = new SqlConnection(connectionString);

            param.ValidarParam(param);
            param.valor_unitario = Math.Round(param.valor_unitario, 2);

            try
            {
                conn.Open();
            }
            catch (Exception e)
            {
                throw new Exception("Falha ao criar conexão com o Banco de Dados: " + e.Message);
            }

            try
            {
                InserirProduto(param, conn);
            }
            catch (Exception ex)
            {
                conn.Close();
                throw new Exception(ex.Message);
            }
        }

        public List<Retorno> Get(Dictionary<string, string> queryString, string connectionString)
        {
            string outValue = null;
            int idProduto = 0;

            SqlConnection conn = new SqlConnection(connectionString);

            if (queryString.TryGetValue("" + (int)Campos.ID_PRODUTO, out outValue))
                idProduto = Int32.Parse(queryString["" + (int)Campos.ID_PRODUTO]);

            try
            {
                conn.Open();
            }
            catch (Exception e)
            {
                throw new Exception("Falha ao criar conexão com o Banco de Dados: " + e.Message);
            }

            try
            {
                List<Retorno> produtos = ListarProdutos(idProduto, conn);
                return produtos;
            }
            catch (Exception ex)
            {
                conn.Close();
                throw new Exception(ex.Message);
            }
        }

        #region Inserir Produto
        private static void InserirProduto(ParamAdicionar param, SqlConnection conn)
        {
            try
            {
                #region script
                string scriptInsert = @"INSERT INTO dbo.Produtos (nmProduto, vlUnitario, qtEstoque) VALUES (@nmProduto, @vlProduto, @qtProduto)";
                #endregion

                SqlCommand command = new SqlCommand(scriptInsert, conn);

                command.Parameters.AddWithValue("@nmProduto", param.nome);
                command.Parameters.AddWithValue("@vlProduto", param.valor_unitario);
                command.Parameters.AddWithValue("@qtProduto", param.qtde_estoque);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception("Falha ao inserir produto: " + e.Message);
            }
        }
        #endregion

        #region Listar Produtos
        private static List<Retorno> ListarProdutos(int idProduto, SqlConnection conn)
        {
            try
            {
                #region Script
                string script = @"SELECT * FROM dbo.Produtos (NOLOCK)" +
                                 (idProduto == 0 ? "" : "WHERE idProduto = " + idProduto);
                #endregion

                List<Retorno> produtosERP = new List<Retorno>();

                DataTable dtDados = new DataTable();
                SqlCommand cmd = new SqlCommand(script, conn);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtDados);

                if (dtDados.Rows.Count > 0)
                {
                    for (int i = 0; i < dtDados.Rows.Count; i++)
                    {
                        produtosERP.Add(new Retorno
                        {
                            id = Convert.ToInt32(dtDados.Rows[i][0]),
                            nome = Convert.ToString(dtDados.Rows[i][1]),
                            valor_unitario = Convert.ToDecimal(dtDados.Rows[i][2]),
                            qtde_estoque = Convert.ToInt32(dtDados.Rows[i][3]),
                            dtUltimaVenda = dtDados.Rows[i][4].Equals(DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dtDados.Rows[i][4]),
                            vlUltimaVenda = dtDados.Rows[i][5].Equals(DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dtDados.Rows[i][5])
                        });
                    }
                }
                else if (dtDados.Rows.Count == 0 && idProduto == 0)
                    throw new Exception("Não existem produtos cadastrados");
                else
                    throw new Exception("Nenhum produto encontrado para o id " + idProduto);

                return produtosERP;

            }
            catch (Exception e)
            {
                throw new Exception("Falha ao Consultar Produtos: " + e.Message);
            }


        }
        #endregion
    }
}
