using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ControleMedicamento.Infra.BancoDados.ModuloMedicamento
{
    internal class RepositorioMedicamentoEmBancoDados
    {

        private const string enderecoBanco = @"Data Source=(LOCALDB)\MSSQLLOCALDB;Initial Catalog=ControleMedicamentosDB;Integrated Security=True";

        private const string sqlInserir =
         @"INSERT INTO [TBMedicamento]
                (
                    [NOME],                    
                    [DESCRICAO],
                    [LOTE],
                    [VALIDADE],
                    [QUANTIDADEDISPONIVEL],
                    [FORNCEDOR_ID]
	            )
	            VALUES
                (
                    @NOME,                    
                    @DESCRICAO,
                    @LOTE,
                    @VALIDADE,
                    @QUANTIDADEDISPONIVEL,
                    @FORNCEDOR_ID
                );SELECT SCOPE_IDENTITY();";

        private const string sqlEditar =
            @"UPDATE [TBMedicamento]	
		            SET
                        [NOME] = @NOME,                 
                        [DESCRICAO] = @DESCRICAO,
                        [LOTE] = @LOTE,
                        [VALIDADE] = @VALIDADE,
                        [QUANTIDADEDISPONIVEL] = @QUANTIDADEDISPONIVEL,
                        [FORNCEDOR_ID] = @FORNCEDOR_ID
		            WHERE
			            [ID] = @ID";

        private const string sqlExcluir =
            @"DELETE FROM [TBMedicamento]
		            WHERE
			            [ID] = @ID";

        private const string sqlSelecionarTodos =
            @"SELECT 
                    TBM.ID,
                    TBM.NOME,
                    TBM.DESCRICAO,
                    TBM.LOTE,
                    TBM.VALIDADE,
                    TBM.QUANTIDADEDISPONIVEL,
	                TBF.ID AS FORNECEDOR_ID,
	                TBF.NOME AS FORNECEDOR_NOME
                  FROM 
                	TBMEDICAMENTO TBM INNER JOIN TBFORNECEDOR TBF 
                  ON
                	TBM.FORNCEDOR_ID = TBF.ID";

        private const string sqlSelecionarPorNumero =
           @"SELECT 
                    TBM.ID,
                    TBM.NOME,
                    TBM.DESCRICAO,
                    TBM.LOTE,
                    TBM.VALIDADE,
                    TBM.QUANTIDADEDISPONIVEL,
	                TBF.ID AS FORNECEDOR_ID,
	                TBF.NOME AS FORNECEDOR_NOME
                  FROM 
                	TBMEDICAMENTO TBM INNER JOIN TBFORNECEDOR TBF 
                  ON
                	TBM.FORNCEDOR_ID = TBF.ID
                WHERE 
                    TBM.ID = @ID";

        public ValidationResult Insert(Medicamento novoMedicamente)
        {
            var validador = new MedicamentoValidator();
            var resultadoValidacao = validador.Validate(novoMedicamente);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            SqlConnection conexao = new SqlConnection(enderecoBanco);
            conexao.Open();

            SqlCommand comandoInsercao = new SqlCommand(sqlInserir, conexao);

            ConfigurarParametros(novoMedicamente, comandoInsercao);

            var id = comandoInsercao.ExecuteScalar();//retorna o valor da coluna id
            novoMedicamente.Id = Convert.ToInt32(id);

            //fecha a conexão
            conexao.Close();

            return resultadoValidacao;
        }

        public ValidationResult Editar(Medicamento novoMedicamento)
        {
            var validador = new MedicamentoValidator();

            var resultadoValidacao = validador.Validate(novoMedicamento);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoEdicao = new SqlCommand(sqlEditar, conexaoComBanco);

            ConfigurarParametros(novoMedicamento, comandoEdicao);

            conexaoComBanco.Open();
            comandoEdicao.ExecuteNonQuery();
            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public void Excluir(Medicamento novoMedicamento)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoExclusao = new SqlCommand(sqlExcluir, conexaoComBanco);

            comandoExclusao.Parameters.AddWithValue("ID", novoMedicamento.Id);

            conexaoComBanco.Open();
            comandoExclusao.ExecuteNonQuery();
            conexaoComBanco.Close();
        }

        public List<Medicamento> SelecionarTodos()
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);
            conexaoComBanco.Open();

            SqlDataReader leitorPaciente = comandoSelecao.ExecuteReader();

            List<Medicamento> pacientes = new List<Medicamento>();

            while (leitorPaciente.Read())
                pacientes.Add(ConverterParaMedicamento(leitorPaciente));

            conexaoComBanco.Close();

            return pacientes;
        }

        public Medicamento SelecionarPorId(int id)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorNumero, conexaoComBanco);

            comandoSelecao.Parameters.AddWithValue("ID", id);

            conexaoComBanco.Open();
            SqlDataReader leitorFuncionario = comandoSelecao.ExecuteReader();

            Medicamento medicamento = null;
            if (leitorFuncionario.Read())
                medicamento = ConverterParaMedicamento(leitorFuncionario);

            conexaoComBanco.Close();

            return medicamento;
        }

        private Medicamento ConverterParaMedicamento(SqlDataReader leitorMedicamento)
        {
            int numero = Convert.ToInt32(leitorMedicamento["ID"]);
            string nome = Convert.ToString(leitorMedicamento["NOME"]);
            string descricao = Convert.ToString(leitorMedicamento["DESCRICAO"]);
            string lote = Convert.ToString(leitorMedicamento["LOTE"]);
            DateTime validade = Convert.ToDateTime(leitorMedicamento["VALIDADE"]);
            int quantidadeDisponivel = Convert.ToInt32(leitorMedicamento["QUANTIDADEDISPONIVEL"]);

            int numeroFornecedor = Convert.ToInt32(leitorMedicamento["FORNCEDOR_ID"]);
            string nomeFornecedor = Convert.ToString(leitorMedicamento["FORNECEDOR_NOME"]);

            var fornecedor = new Fornecedor
            {
                Id = numeroFornecedor,
                Nome = nomeFornecedor
            };

            var medicamento = new Medicamento
            {
                Id = numero,
                Nome = nome,
                Descricao = descricao,
                Lote = lote,
                Validade = validade,
                QuantidadeDisponivel = quantidadeDisponivel
            };

            medicamento.ConfigurarFornecedor(fornecedor);

            return medicamento;
        }

        private void ConfigurarParametros(Medicamento novoMedicamente, SqlCommand comandoInsercao)
        {
            comandoInsercao.Parameters.AddWithValue("ID", novoMedicamente.Id);
            comandoInsercao.Parameters.AddWithValue("NOME", novoMedicamente.Nome);
            comandoInsercao.Parameters.AddWithValue("DESCRICAO", novoMedicamente.Descricao);
            comandoInsercao.Parameters.AddWithValue("LOTE", novoMedicamente.Lote);
            comandoInsercao.Parameters.AddWithValue("VALIDADE", novoMedicamente.Validade);
            comandoInsercao.Parameters.AddWithValue("QUANTIDADEDISPONIVEL", novoMedicamente.QuantidadeDisponivel);
            comandoInsercao.Parameters.AddWithValue("FORNCEDOR_ID", novoMedicamente.Fornecedor.Id);
        }
    }
}
