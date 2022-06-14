using ControleMedicamentos.Dominio.ModuloPaciente;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.ModuloPaciente
{
    public class RepositorioPacienteEmBancoDados
    {
        private const string enderecoBanco = "Data Source=(localdb)\\MSSQLLocalDB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False";

        #region Querry's

        private const string sqlInserir =
            @"INSERT INTO [TBPACIENTE]
                (
                    [NOME],
                    [CARTAOSUS]
                )
            VALUES
                (
                    @NOME,
		            @CARTAOSUS
                );SELECT SCOPE_IDENTITY();";

        private const string sqlEditar =
            @"UPDATE [TBPACIENTE]
                SET 
                    [NOME] = @NOME,
                    [CARTAOSUS] = @CARTAOSUS
                WHERE 
                    [ID] = @ID";

        private const string sqlExcluir =
            @"DELETE FROM [TBPACIENTE]
		        WHERE
			        [ID] = @ID";

        private const string sqlSelecionarTodos =
            @"SELECT 
                    [ID],
                    [NOME],
                    [CARTAOSUS]
                FROM 
                    [TBPACIENTE];";

        private const string sqlSelecionarPorNumero =
           @"SELECT 
		            [ID], 
		            [NOME],
                    [CARTAOSUS]
	            FROM 
		            [TBPACIENTE]
		        WHERE
                    [ID] = @ID";
        #endregion

        public ValidationResult Inserir(Paciente novoPaciente)
        {
            var validador = new PacienteValidator();
            var resultadoValidacao = validador.Validate(novoPaciente);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            SqlConnection conexao = new SqlConnection(enderecoBanco);
            conexao.Open();

            SqlCommand comandoInsercao = new SqlCommand(sqlInserir, conexao);

            ConfigurarParametros(novoPaciente, comandoInsercao);

            var id = comandoInsercao.ExecuteScalar();//retorna o valor da coluna id
            novoPaciente.Id = Convert.ToInt32(id);

            //fecha a conexão
            conexao.Close();

            return resultadoValidacao;
        }

        public ValidationResult Editar(Paciente paciente)
        {
            var validador = new PacienteValidator();

            var resultadoValidacao = validador.Validate(paciente);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoEdicao = new SqlCommand(sqlEditar, conexaoComBanco);

            ConfigurarParametros(paciente, comandoEdicao);

            conexaoComBanco.Open();
            comandoEdicao.ExecuteNonQuery();
            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public void Excluir(Paciente paciente)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoExclusao = new SqlCommand(sqlExcluir, conexaoComBanco);

            //PASSANDO O PARAMETRO NECESSÁRIO PARA EXECUTAR O COMANDO
            comandoExclusao.Parameters.AddWithValue("ID", paciente.Id);

            conexaoComBanco.Open();
            comandoExclusao.ExecuteNonQuery();
            conexaoComBanco.Close();
        }

        public List<Paciente> SelecionarTodos()
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);
            conexaoComBanco.Open();

            SqlDataReader leitorPaciente = comandoSelecao.ExecuteReader();

            List<Paciente> pacientes = new List<Paciente>();

            while (leitorPaciente.Read())
                pacientes.Add(ConverterParaPaciente(leitorPaciente));

            conexaoComBanco.Close();

            return pacientes;
        }

        public Paciente SelecionarPorId(int id)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorNumero, conexaoComBanco);

            comandoSelecao.Parameters.AddWithValue("ID", id);

            conexaoComBanco.Open();
            SqlDataReader leitorPaciente = comandoSelecao.ExecuteReader();

            Paciente paciente = null;
            if (leitorPaciente.Read())
                paciente = ConverterParaPaciente(leitorPaciente);

            conexaoComBanco.Close();

            return paciente;
        }

        private void ConfigurarParametros(Paciente novoPaciente, SqlCommand comandoInsercao)
        {
            comandoInsercao.Parameters.AddWithValue("ID", novoPaciente.Id);
            comandoInsercao.Parameters.AddWithValue("NOME", novoPaciente.Nome);
            comandoInsercao.Parameters.AddWithValue("CARTAOSUS", novoPaciente.CartaoSUS);

        }

        private Paciente ConverterParaPaciente(SqlDataReader leitorPaciente)
        {
            int id = Convert.ToInt32(leitorPaciente["ID"]);
            string nome = Convert.ToString(leitorPaciente["NOME"]);
            string cartaoSus = Convert.ToString(leitorPaciente["CARTAOSUS"]);

            return new Paciente()
            {
                Id = id,
                Nome = nome,
                CartaoSUS = cartaoSus
            };
        }
    }
}
