using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiAvonale.Models
{
    public class ParamAdicionar
    {
        public string nome { get; set; }
        public decimal valor_unitario { get; set; }
        public int qtde_estoque { get; set; }

        #region Validação Dados
        public void ValidarParam(ParamAdicionar param)
        {
            if (param == null)
                throw new Exception("0;Favor informar o nome, valor unitário e a quantidade de estoque do produto");

            if (String.IsNullOrEmpty(param.nome))
                throw new Exception("0;É obrigatório informar o nome do produto");

            if (param.valor_unitario <= 0)
                throw new Exception("0;Valor do produto menor ou igual a zero");

            if (param.qtde_estoque <= 0)
                throw new Exception("0;Quantidade de estoque do produto menor ou igual a zero");
        }
        #endregion
    }
}
