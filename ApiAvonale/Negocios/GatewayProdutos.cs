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
    }
}
