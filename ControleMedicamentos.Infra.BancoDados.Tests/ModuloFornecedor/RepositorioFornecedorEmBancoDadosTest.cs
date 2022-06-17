using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Infra.BancoDados.Compartilhado;
using ControleMedicamentos.Infra.BancoDados.ModuloFornecedor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.Tests.ModuloFornecedor
{
    [TestClass]
    public class RepositorioFornecedorEmBancoDadosTest
    {
        private Fornecedor fornecedor;
        private RepositorioFornecedorEmBancoDados repositorio;

        public RepositorioFornecedorEmBancoDadosTest()
        {
            Db.ExecutarSql("DELETE FROM TBMEDICAMENTO; DBCC CHECKIDENT (TBMEDICAMENTO, RESEED, 0)");
            Db.ExecutarSql("DELETE FROM TBFORNECEDOR; DBCC CHECKIDENT (TBFORNECEDOR, RESEED, 0)");

            fornecedor = new Fornecedor("José da Silva","(49) 99999-9999", "teste@gmail.com", "Lajaika", "SC");
            repositorio = new RepositorioFornecedorEmBancoDados();
        }

        [TestMethod]
        public void Deve_inserir_novo_fornecedor()
        {
            //action
            repositorio.Inserir(fornecedor);

            //assert
            var fornecedorEncontrado = repositorio.SelecionarPorId(fornecedor.Id);

            Assert.IsNotNull(fornecedorEncontrado);
            Assert.AreEqual(fornecedor.Nome, fornecedorEncontrado.Nome);
        }

        [TestMethod]
        public void Deve_editar_informacoes_fornecedor()
        {
            //arrange                      
            repositorio.Inserir(fornecedor);

            //action
            fornecedor.Nome = "João de Moraes";
            repositorio.Editar(fornecedor);

            //assert
            var fornecedorEncontrado = repositorio.SelecionarPorId(fornecedor.Id);

            Assert.IsNotNull(fornecedorEncontrado);
            Assert.AreEqual(fornecedor.Nome, fornecedorEncontrado.Nome);
        }

        [TestMethod]
        public void Deve_excluir_fornecedor()
        {
            //arrange           
            repositorio.Inserir(fornecedor);

            //action           
            repositorio.Excluir(fornecedor);

            //assert
            var fornecedorEncontrado = repositorio.SelecionarPorId(fornecedor.Id);
            Assert.IsNull(fornecedorEncontrado);
        }
        [TestMethod]
        public void Deve_selecionar_apenas_um_fornecedor()
        {
            //arrange          
            repositorio.Inserir(fornecedor);

            //action
            var fornecedorEncontrado = repositorio.SelecionarPorId(fornecedor.Id);

            //assert
            Assert.IsNotNull(fornecedorEncontrado);
            Assert.AreEqual(fornecedor.Nome, fornecedorEncontrado.Nome);
        }

        [TestMethod]
        public void Deve_selecionar_todos_um_fornecedors()
        {
            //arrange
            var p01 = new Fornecedor("Teste1", "(49) 99999-9999", "teste@gmail.com", "Lajaika", "SC");
            var p02 = new Fornecedor("Teste2", "(49) 99999-9999", "teste@gmail.com", "Lajaika", "SC");
            var p03 = new Fornecedor("Teste3", "(49) 99999-9999", "teste@gmail.com", "Lajaika", "SC");

            var repositorio = new RepositorioFornecedorEmBancoDados();
            repositorio.Inserir(p01);
            repositorio.Inserir(p02);
            repositorio.Inserir(p03);

            //action
            var fornecedors = repositorio.SelecionarTodos();

            //assert

            Assert.AreEqual(3, fornecedors.Count);

            Assert.AreEqual(p01.Nome, fornecedors[0].Nome);
            Assert.AreEqual(p02.Nome, fornecedors[1].Nome);
            Assert.AreEqual(p03.Nome, fornecedors[2].Nome);
        }
    }
}
