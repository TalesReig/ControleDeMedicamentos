﻿using ControleMedicamentos.Dominio.ModuloFuncionario;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.ModuloFuncionario
{
    internal class RepositorioFuncionarioEmBancoDados
    {
        private const string enderecoBanco = "Data Source=(localdb)\\MSSQLLocalDB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False";

        #region Querry's
        private const string sqlInserir =
            @"INSERT INTO [TBFUNCIONARIO]
                       (
                        [NOME],
                        [LOGIN],
                        [SENHA]
                       )
                 VALUES
                       (
		                @NOME,
		                @LOGIN,
		                @SENHA
		               );SELECT SCOPE_IDENTITY();";

        private const string sqlEditar =
            @"UPDATE [TBFUNCIONARIO]
                SET 
                    [NOME] = @NOME,
                    [LOGIN] = @LOGIN,
                    [SENHA] = @SENHA
                WHERE 
                    [ID] = @ID";

        private const string sqlExcluir =
            @"DELETE FROM [TBFUNCIONARIO]
		                WHERE
			                [ID] = @ID";

        private const string sqlSelecionarTodos =
            @"SELECT 
                    [ID],
                    [NOME],
                    [LOGIN],
                    [SENHA]
                FROM 
                    [TBFUNCIONARIO];";

        private const string sqlSelecionarPorId =
            @"SELECT 
		            [ID],
                    [NOME],
                    [LOGIN],
                    [SENHA]
	            FROM 
		            [TBPACIENTE]
		        WHERE
                    [ID] = @ID";

        #endregion

        public ValidationResult Insert(Funcionario novoFuncionario)
        {
            var validador = new FuncionarioValidator();
            var resultadoValidacao = validador.Validate(novoFuncionario);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            SqlConnection conexao = new SqlConnection(enderecoBanco);
            conexao.Open();

            SqlCommand comandoInsercao = new SqlCommand(sqlInserir, conexao);

            ConfigurarParametros(novoFuncionario, comandoInsercao);

            var id = comandoInsercao.ExecuteScalar();//retorna o valor da coluna id
            novoFuncionario.Id = Convert.ToInt32(id);

            //fecha a conexão
            conexao.Close();

            return resultadoValidacao;
        }

        public ValidationResult Editar(Funcionario novoFuncionario)
        {
            var validador = new FuncionarioValidator();

            var resultadoValidacao = validador.Validate(novoFuncionario);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoEdicao = new SqlCommand(sqlEditar, conexaoComBanco);

            ConfigurarParametros(novoFuncionario, comandoEdicao);

            conexaoComBanco.Open();
            comandoEdicao.ExecuteNonQuery();
            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public void Excluir(Funcionario novoFuncionario)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoExclusao = new SqlCommand(sqlExcluir, conexaoComBanco);

            comandoExclusao.Parameters.AddWithValue("ID", novoFuncionario.Id);

            conexaoComBanco.Open();
            comandoExclusao.ExecuteNonQuery();
            conexaoComBanco.Close();
        }

        public List<Funcionario> SelecionarTodos()
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);
            conexaoComBanco.Open();

            SqlDataReader leitorPaciente = comandoSelecao.ExecuteReader();

            List<Funcionario> pacientes = new List<Funcionario>();

            while (leitorPaciente.Read())
                pacientes.Add(ConverterParFuncionario(leitorPaciente));

            conexaoComBanco.Close();

            return pacientes;
        }

        public Funcionario SelecionarPorId(int id)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorId, conexaoComBanco);

            comandoSelecao.Parameters.AddWithValue("ID", id);

            conexaoComBanco.Open();
            SqlDataReader leitorFuncionario = comandoSelecao.ExecuteReader();

            Funcionario funcionario = null;
            if (leitorFuncionario.Read())
                funcionario = ConverterParFuncionario(leitorFuncionario);

            conexaoComBanco.Close();

            return funcionario;
        }

        private void ConfigurarParametros(Funcionario novoFuncionario, SqlCommand comandoInsercao)
        {
            comandoInsercao.Parameters.AddWithValue("ID", novoFuncionario.Id);
            comandoInsercao.Parameters.AddWithValue("NOME", novoFuncionario.Nome);
            comandoInsercao.Parameters.AddWithValue("LOGIN", novoFuncionario.Login);
            comandoInsercao.Parameters.AddWithValue("SENHA", novoFuncionario.Senha);
        }

        private Funcionario ConverterParFuncionario(SqlDataReader leitorPaciente)
        {
            int id = Convert.ToInt32(leitorPaciente["ID"]);
            string nome = Convert.ToString(leitorPaciente["NOME"]);
            string login = Convert.ToString(leitorPaciente["LOGIN"]);
            string senha = Convert.ToString(leitorPaciente["SENHA"]);

            return new Funcionario()
            {
                Id = id,
                Nome = nome,
                Login = login,
                Senha = senha
            };
        }

    }
}
