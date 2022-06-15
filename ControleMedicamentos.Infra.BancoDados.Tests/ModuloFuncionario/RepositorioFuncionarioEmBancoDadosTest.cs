using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Infra.BancoDados.Compartilhado;
using ControleMedicamentos.Infra.BancoDados.ModuloFuncionario;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.Tests.ModuloFuncionamento
{
    [TestClass]
    public class RepositorioFuncionarioEmBancoDadosTest
    {
        private Funcionario funcionario;
        private RepositorioFuncionarioEmBancoDados repositorio;

        public RepositorioFuncionarioEmBancoDadosTest()
        {
            Db.ExecutarSql("DELETE FROM TBFUNCIONARIO; DBCC CHECKIDENT (TBFUNCIONARIO, RESEED, 0)");

            funcionario = new Funcionario("José da Silva", "teste", "teste");
            repositorio = new RepositorioFuncionarioEmBancoDados();
        }

        [TestMethod]
        public void Deve_inserir_novo_funcionario()
        {
            //action
            repositorio.Inserir(funcionario);

            //assert
            var funcionarioEncontrado = repositorio.SelecionarPorId(funcionario.Id);

            Assert.IsNotNull(funcionarioEncontrado);
            Assert.AreEqual(funcionario.Nome, funcionarioEncontrado.Nome);
        }

        [TestMethod]
        public void Deve_editar_informacoes_funcionario()
        {
            //arrange                      
            repositorio.Inserir(funcionario);

            //action
            funcionario.Nome = "João de Moraes";
            repositorio.Editar(funcionario);

            //assert
            var funcionarioEncontrado = repositorio.SelecionarPorId(funcionario.Id);

            Assert.IsNotNull(funcionarioEncontrado);
            Assert.AreEqual(funcionario.Nome, funcionarioEncontrado.Nome);
        }

        [TestMethod]
        public void Deve_excluir_funcionario()
        {
            //arrange           
            repositorio.Inserir(funcionario);

            //action           
            repositorio.Excluir(funcionario);

            //assert
            var funcionarioEncontrado = repositorio.SelecionarPorId(funcionario.Id);
            Assert.IsNull(funcionarioEncontrado);
        }
        [TestMethod]
        public void Deve_selecionar_apenas_um_funcionario()
        {
            //arrange          
            repositorio.Inserir(funcionario);

            //action
            var funcionarioEncontrado = repositorio.SelecionarPorId(funcionario.Id);

            //assert
            Assert.IsNotNull(funcionarioEncontrado);
            Assert.AreEqual(funcionario.Nome, funcionarioEncontrado.Nome);
        }

        [TestMethod]
        public void Deve_selecionar_todos_um_funcionarios()
        {
            //arrange
            var p01 = new Funcionario("Teste1", "teste", "teste");
            var p02 = new Funcionario("Teste2", "teste", "teste");
            var p03 = new Funcionario("Teste3", "teste", "teste");

            var repositorio = new RepositorioFuncionarioEmBancoDados();
            repositorio.Inserir(p01);
            repositorio.Inserir(p02);
            repositorio.Inserir(p03);

            //action
            var funcionarios = repositorio.SelecionarTodos();

            //assert

            Assert.AreEqual(3, funcionarios.Count);

            Assert.AreEqual(p01.Nome, funcionarios[0].Nome);
            Assert.AreEqual(p02.Nome, funcionarios[1].Nome);
            Assert.AreEqual(p03.Nome, funcionarios[2].Nome);
        }
    }
}
