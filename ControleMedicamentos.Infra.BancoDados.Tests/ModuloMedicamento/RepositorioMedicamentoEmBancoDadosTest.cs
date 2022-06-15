using ControleMedicamento.Infra.BancoDados.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Infra.BancoDados.Compartilhado;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ControleMedicamento.Infra.BancoDados.Tests.ModuloMedicamento
{
    [TestClass]
    internal class RepositorioMedicamentoEmBancoDadosTest
    {
        private Medicamento medicamento;
        private RepositorioMedicamentoEmBancoDados repositorio;

        public RepositorioMedicamentoEmBancoDadosTest()
        {
            Db.ExecutarSql("DELETE FROM TBmedicamento; DBCC CHECKIDENT (TBmedicamento, RESEED, 0)");

           //prepara a instância de medicamento
            repositorio = new RepositorioMedicamentoEmBancoDados();
        }

        [TestMethod]
        public void Deve_inserir_novo_medicamento()
        {
            //action
            repositorio.Inserir(medicamento);

            //assert
            var medicamentoEncontrado = repositorio.SelecionarPorId(medicamento.Id);

            Assert.IsNotNull(medicamentoEncontrado);
            Assert.AreEqual(medicamento.Nome, medicamentoEncontrado.Nome);
        }

        [TestMethod]
        public void Deve_editar_informacoes_medicamento()
        {
            //arrange                      
            repositorio.Inserir(medicamento);

            //action
            medicamento.Nome = "Ciclobenzaprina";
            repositorio.Editar(medicamento);

            //assert
            var medicamentoEncontrado = repositorio.SelecionarPorId(medicamento.Id);

            Assert.IsNotNull(medicamentoEncontrado);
            Assert.AreEqual(medicamento.Nome, medicamentoEncontrado.Nome);
        }

        [TestMethod]
        public void Deve_excluir_medicamento()
        {
            //arrange           
            repositorio.Inserir(medicamento);

            //action           
            repositorio.Excluir(medicamento);

            //assert
            var medicamentoEncontrado = repositorio.SelecionarPorId(medicamento.Id);
            Assert.IsNull(medicamentoEncontrado);
        }
        [TestMethod]
        public void Deve_selecionar_apenas_um_medicamento()
        {
            //arrange          
            repositorio.Inserir(medicamento);

            //action
            var medicamentoEncontrado = repositorio.SelecionarPorId(medicamento.Id);

            //assert
            Assert.IsNotNull(medicamentoEncontrado);
            Assert.AreEqual(medicamento.Nome, medicamentoEncontrado.Nome);
        }

        [TestMethod]
        public void Deve_selecionar_todos_um_medicamentos()
        {
            //arrange
            var p01 = new Medicamento("Teste1", "teste", "teste");
            var p02 = new Medicamento("Teste2", "teste", "teste");
            var p03 = new Medicamento("Teste3", "teste", "teste");

            var repositorio = new RepositorioMedicamentoEmBancoDados();
            repositorio.Inserir(p01);
            repositorio.Inserir(p02);
            repositorio.Inserir(p03);

            //action
            var medicamentos = repositorio.SelecionarTodos();

            //assert

            Assert.AreEqual(3, medicamentos.Count);

            Assert.AreEqual(p01.Nome, medicamentos[0].Nome);
            Assert.AreEqual(p02.Nome, medicamentos[1].Nome);
            Assert.AreEqual(p03.Nome, medicamentos[2].Nome);
        }
    }
}
