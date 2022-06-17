using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloPaciente;
using ControleMedicamentos.Dominio.ModuloRequisicao;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ControleMedicamentos.Infra.BancoDados.ModuloRequisicao
{
    public class RepositorioRequisicaoEmBancoDados
    {
        private const string enderecoBanco = @"Data Source=(LOCALDB)\MSSQLLOCALDB;Initial Catalog=ControleMedicamentosDB;Integrated Security=True";

        private const string sqlInserir =
          @"INSERT INTO [TBRequisicao]
                (
                    [FUNCIONARIO_ID],                    
                    [PACIENTE_ID],
                    [MEDICAMENTO_ID],
                    [QUANTIDADEMEDICAMENTO],
                    [DATA]
	            )
	            VALUES
                (
                    @FUNCIONARIO_ID,
                    @PACIENTE_ID,
                    @MEDICAMENTO_ID,
                    @QUANTIDADEMEDICAMENTO,
                    @DATA
                );SELECT SCOPE_IDENTITY();";

        private const string sqlEditar =
           @"UPDATE [TBRequisicao]	
		        SET
                    [FUNCIONARIO_ID] = @FUNCIONARIO_ID,            
                    [PACIENTE_ID] = @PACIENTE_ID,
                    [MEDICAMENTO_ID] = @MEDICAMENTO_ID,
                    [QUANTIDADEMEDICAMENTO] = @QUANTIDADEMEDICAMENTO,
                    [DATA] = @DATA
		        WHERE
			        [ID] = @ID";

        private const string sqlExcluir =
           @"DELETE FROM [TBRequisicao]
		        WHERE
			        [ID] = @ID";


        private const string sqlSelecionarTodos =
             @"SELECT TBR.ID
                     ,TBR.MEDICAMENTO_ID
                     ,TBR.QUANTIDADEMEDICAMENTO
                     ,TBR.[DATA]
                     ,TBF.ID FUNCIONARIO_Id
                     ,TBF.NOME FUNCIONARIO_NOME
                     ,TBP.ID PACIENTE_Id
                     ,TBP.NOME PACIENTE_NOME
                     ,TBP.CARTAOSUS
                     ,TBM.ID MEDICAMENTO_Id
                     ,TBM.NOME MEDICAMENTO_NOME
                     ,TBM.LOTE
                     ,TBM.VALIDADE
                 
                 FROM 
                    TBREQUISICAO TBR INNER JOIN TBFUNCIONARIO TBF 
                 ON
                    TBF.ID = TBR.FUNCIONARIO_ID INNER JOIN TBPACIENTE TBP
                 ON
                    TBP.ID = TBR.PACIENTE_ID INNER JOIN TBMEDICAMENTO TBM
                 ON
                   TBM.ID = TBR.MEDICAMENTO_ID";

        private const string sqlSelecionarPorId =
            @"SELECT TBR.ID
                     ,TBR.QUANTIDADEMEDICAMENTO
                     ,TBR.[DATA]
                     ,TBF.ID FUNCIONARIO_Id
                     ,TBF.NOME FUNCIONARIO_NOME
                     ,TBP.ID PACIENTE_Id
                     ,TBP.NOME PACIENTE_NOME
                     ,TBP.CARTAOSUS
                     ,TBM.ID MEDICAMENTO_Id
                     ,TBM.NOME MEDICAMENTO_NOME
                     ,TBM.LOTE
                     ,TBM.VALIDADE
                 
                 FROM 
                    TBREQUISICAO TBR INNER JOIN TBFUNCIONARIO TBF 
                 ON
                    TBF.ID = TBR.FUNCIONARIO_ID INNER JOIN TBPACIENTE TBP
                 ON
                    TBP.ID = TBR.PACIENTE_ID INNER JOIN TBMEDICAMENTO TBM
                 ON
                   TBM.ID = TBR.MEDICAMENTO_ID
                WHERE 
	                TBR.ID = @ID";

        public ValidationResult Inserir(Requisicao requisicao)
        {
            RequisicaoValidator validador = new RequisicaoValidator();

            var resultadoValidacao = validador.Validate(requisicao);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoInsercao = new SqlCommand(sqlInserir, conexaoComBanco);

            ConfigurarParametrosRequisicao(requisicao, comandoInsercao);

            conexaoComBanco.Open();
            var id = comandoInsercao.ExecuteScalar();
            requisicao.Id = Convert.ToInt32(id);

            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public ValidationResult Excluir(Requisicao requisicao)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoExclusao = new SqlCommand(sqlExcluir, conexaoComBanco);

            comandoExclusao.Parameters.AddWithValue("ID", requisicao.Id);

            conexaoComBanco.Open();
            int IdRegistrosExcluidos = comandoExclusao.ExecuteNonQuery();

            var resultadoValidacao = new ValidationResult();

            if (IdRegistrosExcluidos == 0)
                resultadoValidacao.Errors.Add(new ValidationFailure("", "Não foi possível remover o registro"));

            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public List<Requisicao> SelecionarTodos()
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);

            conexaoComBanco.Open();
            SqlDataReader leitorRequisicao = comandoSelecao.ExecuteReader();

            List<Requisicao> requisicoes = new List<Requisicao>();

            while (leitorRequisicao.Read())
            {
                Requisicao requisicao = ConverterParaRiquisicao(leitorRequisicao);

                requisicoes.Add(requisicao);
            }

            conexaoComBanco.Close();

            return requisicoes;
        }

        public Requisicao SelecionarPorId(int Id)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorId, conexaoComBanco);

            comandoSelecao.Parameters.AddWithValue("ID", Id);

            conexaoComBanco.Open();
            SqlDataReader leitorRequisicao = comandoSelecao.ExecuteReader();

            Requisicao requisicao = null;

            if (leitorRequisicao.Read())
            {
                requisicao = ConverterParaRiquisicao(leitorRequisicao);
            }

            conexaoComBanco.Close();

            return requisicao;
        }

        private void ConfigurarParametrosRequisicao(Requisicao riquisicao, SqlCommand comando)
        {
            comando.Parameters.AddWithValue("ID", riquisicao.Id);
            comando.Parameters.AddWithValue("QUANTIDADEMEDICAMENTO", riquisicao.QtdMedicamento);
            comando.Parameters.AddWithValue("DATA", riquisicao.Data);
            comando.Parameters.AddWithValue("FUNCIONARIO_ID", riquisicao.Funcionario.Id);
            comando.Parameters.AddWithValue("PACIENTE_ID", riquisicao.Paciente.Id);
            comando.Parameters.AddWithValue("MEDICAMENTO_ID", riquisicao.Medicamento.Id);
        }

        private Requisicao ConverterParaRiquisicao(SqlDataReader leitorRequisicao)
        {
            int Id = Convert.ToInt32(leitorRequisicao["ID"]);
            int qtdMedicamento = Convert.ToInt32(leitorRequisicao["QUANTIDADEMEDICAMENTO"]);
            DateTime data = Convert.ToDateTime(leitorRequisicao["DATA"]);

            int IdFuncionario = Convert.ToInt32(leitorRequisicao["FUNCIONARIO_Id"]);
            string nomeFuncionario = Convert.ToString(leitorRequisicao["FUNCIONARIO_NOME"]);

            int IdPaciente = Convert.ToInt32(leitorRequisicao["PACIENTE_Id"]);
            string nomePaciente = Convert.ToString(leitorRequisicao["PACIENTE_NOME"]);
            string cartaoSUS = Convert.ToString(leitorRequisicao["CARTAOSUS"]);

            int IdMedicamento = Convert.ToInt32(leitorRequisicao["MEDICAMENTO_Id"]);
            string nomeMedicamento = Convert.ToString(leitorRequisicao["MEDICAMENTO_NOME"]);
            string lote = Convert.ToString(leitorRequisicao["LOTE"]);
            DateTime validade = Convert.ToDateTime(leitorRequisicao["VALIDADE"]);

            var funcionario = new Funcionario
            {
                Id = IdFuncionario,
                Nome = nomeFuncionario
            };

            var paciente = new Paciente
            {
                Id = IdPaciente,
                Nome = nomePaciente,
                CartaoSUS = cartaoSUS
            };

            var requisicao = new Requisicao
            {
                Id = Id,
                QtdMedicamento = qtdMedicamento,
                Data = data
            };

            var medicamento = new Medicamento
            {
                Id = IdMedicamento,
                Nome = nomeMedicamento,
                Lote = lote,
                Validade = validade
            };

            requisicao.ConfigurarMedicamento(medicamento);
            requisicao.ConfigurarFuncionario(funcionario);
            requisicao.ConfigurarPaciente(paciente);

            return requisicao;
        }
    }
}
