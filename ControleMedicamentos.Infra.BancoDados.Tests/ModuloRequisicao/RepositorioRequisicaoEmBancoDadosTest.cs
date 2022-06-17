using ControleMedicamento.Infra.BancoDados.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloPaciente;
using ControleMedicamentos.Dominio.ModuloRequisicao;
using ControleMedicamentos.Infra.BancoDados.Compartilhado;
using ControleMedicamentos.Infra.BancoDados.ModuloFornecedor;
using ControleMedicamentos.Infra.BancoDados.ModuloFuncionario;
using ControleMedicamentos.Infra.BancoDados.ModuloPaciente;
using ControleMedicamentos.Infra.BancoDados.ModuloRequisicao;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.Tests.ModuloRequisicao
{
    [TestClass]
    public class RepositorioRequisicaoEmBancoDadosTest
    {
        private Medicamento medicamento;
        private Fornecedor fornecedor;
        private Paciente paciente;
        private Funcionario funcionario;
        private Requisicao requisicao;

        private RepositorioMedicamentoEmBancoDados repositorioMedicamento;
        private RepositorioFuncionarioEmBancoDados repositorioFuncionario;
        private RepositorioPacienteEmBancoDados repositorioPaciente;
        private RepositorioFornecedorEmBancoDados repositorioFornecedor;
        private RepositorioRequisicaoEmBancoDados repositorioRequisicao;

        private DateTime validade = new DateTime(2022, 10, 04);
        public RepositorioRequisicaoEmBancoDadosTest()
        {
            Db.ExecutarSql("DELETE FROM TBREQUISICAO; DBCC CHECKIDENT (TBREQUISICAO, RESEED, 0)");

            fornecedor = new Fornecedor("Neo Quimica", "32333040", "neoquimica@gmail.com", "São Paulo", "SP");
            funcionario = new Funcionario("Eduardo", "Eduardo", "0077");
            paciente = new Paciente("Pedro lopes", "000000");
            medicamento = new Medicamento("Doril", "Para dores musculares", "1234", new(2023, 8, 9), 12, fornecedor);
            requisicao = new Requisicao(medicamento, paciente, 10, DateTime.Now, funcionario);

            repositorioMedicamento = new RepositorioMedicamentoEmBancoDados();
            repositorioFornecedor = new RepositorioFornecedorEmBancoDados();
            repositorioFuncionario = new RepositorioFuncionarioEmBancoDados();
            repositorioPaciente = new RepositorioPacienteEmBancoDados();
            repositorioRequisicao = new RepositorioRequisicaoEmBancoDados();
        }

        [TestMethod]
        public void Deve_inserir_novo_medicamento()
        {
            //action
            repositorioFornecedor.Inserir(fornecedor);
            repositorioFuncionario.Inserir(funcionario);
            repositorioPaciente.Inserir(paciente);
            repositorioMedicamento.Inserir(medicamento);
            repositorioRequisicao.Inserir(requisicao);

            //assert
            var requisicaoEncontrado = repositorioRequisicao.SelecionarPorId(requisicao.Id);

            Assert.IsNotNull(requisicaoEncontrado);
            Assert.AreEqual(requisicao.Id, requisicaoEncontrado.Id);
        }
        
        [TestMethod]
        public void Deve_editar_informacoes_medicamento()
        {
            //arrange                      
            repositorioFornecedor.Inserir(fornecedor);
            repositorioFuncionario.Inserir(funcionario);
            repositorioPaciente.Inserir(paciente);
            repositorioMedicamento.Inserir(medicamento);
            repositorioRequisicao.Inserir(requisicao);

            //action
            requisicao.Medicamento.Id = 1;
            requisicao.Paciente.Id = 2;
            requisicao.QtdMedicamento = 5;
            requisicao.Data = DateTime.Now;
            requisicao.Funcionario.Id = 1;

            //assert
            var requisicaoEncontrado = repositorioRequisicao.SelecionarPorId(requisicao.Id);

            Assert.IsNotNull(requisicaoEncontrado);
            Assert.AreEqual(requisicao.Id, requisicaoEncontrado.Id);
        }

        [TestMethod]
        public void Deve_excluir_medicamento()
        {
            //arrange           
            repositorioFornecedor.Inserir(fornecedor);
            repositorioFuncionario.Inserir(funcionario);
            repositorioPaciente.Inserir(paciente);
            repositorioMedicamento.Inserir(medicamento);
            repositorioRequisicao.Inserir(requisicao);

            //action           
            repositorioRequisicao.Excluir(requisicao);

            //assert
            var requisicaoEncontrado = repositorioRequisicao.SelecionarPorId(requisicao.Id);
            Assert.IsNull(requisicaoEncontrado);
        }

        [TestMethod]
        public void Deve_selecionar_apenas_um_medicamento()
        {
            //arrange          
            repositorioFornecedor.Inserir(fornecedor);
            repositorioFuncionario.Inserir(funcionario);
            repositorioPaciente.Inserir(paciente);
            repositorioMedicamento.Inserir(medicamento);
            repositorioRequisicao.Inserir(requisicao);

            //action
            var requisicaoEncontrado = repositorioRequisicao.SelecionarPorId(requisicao.Id);

            //assert
            Assert.IsNotNull(requisicaoEncontrado);
            Assert.AreEqual(requisicao.Id, requisicaoEncontrado.Id);
        }

        [TestMethod]
        public void Deve_selecionar_todos_um_medicamento()
        {
            //arrange
            paciente = new Paciente(1);
            funcionario = new Funcionario(1);
            medicamento = new Medicamento(1);
            var r01 = new Requisicao(medicamento, paciente, 1, DateTime.Now, funcionario);

            paciente = new Paciente(2);
            funcionario = new Funcionario(2);
            medicamento = new Medicamento(2);
            var r02 = new Requisicao(medicamento, paciente, 2, DateTime.Now, funcionario);

            paciente = new Paciente(3);
            funcionario = new Funcionario(3);
            medicamento = new Medicamento(3);
            var r03 = new Requisicao(medicamento, paciente, 3, DateTime.Now, funcionario);

            paciente = new Paciente(1);
            funcionario = new Funcionario(4);
            medicamento = new Medicamento(4);
            var r04 = new Requisicao(medicamento, paciente, 4, DateTime.Now, funcionario);

            var repositorio = new RepositorioRequisicaoEmBancoDados();
            repositorio.Inserir(r01);
            repositorio.Inserir(r02);
            repositorio.Inserir(r03);
            repositorio.Inserir(r04);

            //action
            var requisicoes = repositorio.SelecionarTodos();

            //assert

            Assert.AreEqual(4, requisicoes.Count);

            Assert.AreEqual(r01.Id, requisicoes[0].Id);
            Assert.AreEqual(r02.Id, requisicoes[1].Id);
            Assert.AreEqual(r03.Id, requisicoes[2].Id);
            Assert.AreEqual(r04.Id, requisicoes[3].Id);
        }
    }
}
   