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
            ID_PRODUTO = 100 // Número do id do produto passado na requisição para rota de buscar produtos ou deletar produtos
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
            SqlConnection conn = new SqlConnection(connectionString);

            string outValue = null;
            int idProduto = 0;

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

        public static void DeleteProduto(Dictionary<string, string> queryString, string connectionString)
        {
            SqlConnection conn = new SqlConnection(connectionString);

            string outValue = null;
            int idProduto = 0;

            if (!queryString.TryGetValue("" + (int)Campos.ID_PRODUTO, out outValue))
                throw new Exception("Id do produto que será deletado deve ser informado!");

            idProduto = idProduto = Int32.Parse(queryString["" + (int)Campos.ID_PRODUTO]);

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
                // Verifica se o existe um produto com o id passado na requisição
                List<Retorno> produto = ListarProdutos(idProduto, conn);

                ExcluirProduto(idProduto, conn);
            }
            catch (Exception e)
            {
                conn.Close();
                throw new Exception(e.Message);
            }
        }

        #region Inserir Produto
        private static void InserirProduto(ParamAdicionar param, SqlConnection conn)
        {
            try
            {
                #region script
                string scriptInsert = @"INSERT INTO dbo.Produtos (nmProduto, vlUnitario, qtEstoque, dtUltimaVenda, vlUltimaVenda) VALUES (@nmProduto, @vlProduto, @qtProduto, NULL, NULL)";
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

        #region Excluir Produto
        private static void ExcluirProduto(int idProduto, SqlConnection conn)
        {
            try
            {
                #region Script
                string scriptDelete = @"DELETE FROM dbo.Produtos WHERE idProduto = @idProduto";
                #endregion

                SqlCommand command = new SqlCommand(scriptDelete, conn);

                command.Parameters.AddWithValue("@idProduto", idProduto);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception("Falha ao excluir produto: " + e.Message);
            }
        }
        #endregion
    }
}
