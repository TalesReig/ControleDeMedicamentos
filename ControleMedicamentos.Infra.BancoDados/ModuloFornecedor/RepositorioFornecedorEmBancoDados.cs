using ControleMedicamentos.Dominio.ModuloFornecedor;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.ModuloFornecedor
{
    public class RepositorioFornecedorEmBancoDados
    {
        private const string enderecoBanco = @"Data Source=(LOCALDB)\MSSQLLOCALDB;Initial Catalog=ControleMedicamentosDB;Integrated Security=True";
        #region Querry's
        private const string sqlInserir =
            @"INSERT INTO [TBFORNECEDOR]
                       (
                        [NOME],
                        [TELEFONE],
                        [EMAIL],
                        [CIDADE],
                        [ESTADO]
                       )
                 VALUES
                       (
		                @NOME,
                        @TELEFONE,
                        @EMAIL,
                        @CIDADE,
                        @ESTADO
		               );SELECT SCOPE_IDENTITY();";

        private const string sqlEditar =
            @"UPDATE [TBFORNECEDOR]
                SET 
                    [NOME] = @NOME,
                    [TELEFONE] = @TELEFONE,
                    [EMAIL] = @EMAIL,
                    [CIDADE] = @CIDADE,
                    [ESTADO] = @ESTADO
                WHERE 
                    [ID] = @ID";

        private const string sqlExcluir =
            @"DELETE FROM [TBFORNECEDOR]
		                WHERE
			                [ID] = @ID";

        private const string sqlSelecionarTodos =
            @"SELECT
                    [ID],
                    [NOME],
                    [TELEFONE],
                    [EMAIL],
                    [CIDADE],
                    [ESTADO]
                FROM 
                    [TBFORNECEDOR];";

        private const string sqlSelecionarPorId =
            @"SELECT 
                    [ID],
                    [NOME],
                    [TELEFONE],
                    [EMAIL],
                    [CIDADE],
                    [ESTADO]
	            FROM 
		            [TBFORNECEDOR]
		        WHERE
                    [ID] = @ID";

        #endregion

        public ValidationResult Inserir(Fornecedor novoFornecedor)
        {
            var validador = new FornecedorValidator();
            var resultadoValidacao = validador.Validate(novoFornecedor);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            SqlConnection conexao = new SqlConnection(enderecoBanco);
            conexao.Open();

            SqlCommand comandoInsercao = new SqlCommand(sqlInserir, conexao);

            ConfigurarParametros(novoFornecedor, comandoInsercao);

            var id = comandoInsercao.ExecuteScalar();//retorna o valor da coluna id
            novoFornecedor.Id = Convert.ToInt32(id);

            //fecha a conexão
            conexao.Close();

            return resultadoValidacao;
        }

        public ValidationResult Editar(Fornecedor novoFornecedor)
        {
            var validador = new FornecedorValidator();

            var resultadoValidacao = validador.Validate(novoFornecedor);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoEdicao = new SqlCommand(sqlEditar, conexaoComBanco);

            ConfigurarParametros(novoFornecedor, comandoEdicao);

            conexaoComBanco.Open();
            comandoEdicao.ExecuteNonQuery();
            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public void Excluir(Fornecedor novoFornecedor)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoExclusao = new SqlCommand(sqlExcluir, conexaoComBanco);

            comandoExclusao.Parameters.AddWithValue("ID", novoFornecedor.Id);

            conexaoComBanco.Open();
            comandoExclusao.ExecuteNonQuery();
            conexaoComBanco.Close();
        }

        public List<Fornecedor> SelecionarTodos()
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);
            conexaoComBanco.Open();

            SqlDataReader leitorPaciente = comandoSelecao.ExecuteReader();

            List<Fornecedor> pacientes = new List<Fornecedor>();

            while (leitorPaciente.Read())
                pacientes.Add(ConverterParFornecedor(leitorPaciente));

            conexaoComBanco.Close();

            return pacientes;
        }

        public Fornecedor SelecionarPorId(int id)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorId, conexaoComBanco);

            comandoSelecao.Parameters.AddWithValue("ID", id);

            conexaoComBanco.Open();
            SqlDataReader leitorFornecedor = comandoSelecao.ExecuteReader();

            Fornecedor fornecedor = null;
            if (leitorFornecedor.Read())
                fornecedor = ConverterParFornecedor(leitorFornecedor);

            conexaoComBanco.Close();

            return fornecedor;
        }

        private void ConfigurarParametros(Fornecedor novoFornecedor, SqlCommand comandoInsercao)
        {
            comandoInsercao.Parameters.AddWithValue("ID", novoFornecedor.Id);
            comandoInsercao.Parameters.AddWithValue("NOME", novoFornecedor.Nome);
            comandoInsercao.Parameters.AddWithValue("EMAIL", novoFornecedor.Email);
            comandoInsercao.Parameters.AddWithValue("TELEFONE", novoFornecedor.Telefone);
            comandoInsercao.Parameters.AddWithValue("CIDADE", novoFornecedor.Cidade);
            comandoInsercao.Parameters.AddWithValue("ESTADO", novoFornecedor.Estado);
        }

        private Fornecedor ConverterParFornecedor(SqlDataReader leitorFornecedor)
        {
            int id = Convert.ToInt32(leitorFornecedor["ID"]);
            string nome = Convert.ToString(leitorFornecedor["NOME"]);
            string login = Convert.ToString(leitorFornecedor["EMAIL"]);
            string fone = Convert.ToString(leitorFornecedor["TELEFONE"]);
            string cidade = Convert.ToString(leitorFornecedor["CIDADE"]);
            string estado = Convert.ToString(leitorFornecedor["CIDADE"]);

            return new Fornecedor()
            {
                Id = id,
                Nome = nome,
                Email = login,
                Telefone = fone,
                Cidade = cidade,
                Estado = estado
            };
        }
    }
}
