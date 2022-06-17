using ControleMedicamentos.Dominio.ModuloPaciente;
using ControleMedicamentos.Infra.BancoDados.Compartilhado;
using ControleMedicamentos.Infra.BancoDados.ModuloPaciente;
using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.Tests.ModuloPaciente
{
    [TestClass]
    public class RepositorioPacienteEmBancoDadosTest
    {
        private Paciente paciente;
        private RepositorioPacienteEmBancoDados repositorio;

        public RepositorioPacienteEmBancoDadosTest()
        {
            Db.ExecutarSql("DELETE FROM TBPACIENTE; DBCC CHECKIDENT (TBPACIENTE, RESEED, 0)");

            paciente = new Paciente("José da Silva", "321654987");
            repositorio = new RepositorioPacienteEmBancoDados();
        }

        [TestMethod]
        public void Deve_inserir_novo_paciente()
        {
            //action
            repositorio.Inserir(paciente);

            //assert
            var pacienteEncontrado = repositorio.SelecionarPorId(paciente.Id);

            Assert.IsNotNull(pacienteEncontrado);
            Assert.AreEqual(paciente, pacienteEncontrado);
        }

        [TestMethod]
        public void Deve_editar_informacoes_paciente()
        {
            //arrange                      
            repositorio.Inserir(paciente);

            //action
            paciente.Nome = "João de Moraes";
            paciente.CartaoSUS = "987654321";
            repositorio.Editar(paciente);

            //assert
            var pacienteEncontrado = repositorio.SelecionarPorId(paciente.Id);

            Assert.IsNotNull(pacienteEncontrado);
            Assert.AreEqual(paciente.Nome, pacienteEncontrado.Nome);
        }

        [TestMethod]
        public void Deve_excluir_paciente()
        {
            //arrange           
            repositorio.Inserir(paciente);

            //action           
            repositorio.Excluir(paciente);

            //assert
            var pacienteEncontrado = repositorio.SelecionarPorId(paciente.Id);
            Assert.IsNull(pacienteEncontrado);
        }
        [TestMethod]
        public void Deve_selecionar_apenas_um_paciente()
        {
            //arrange          
            repositorio.Inserir(paciente);

            //action
            var pacienteEncontrado = repositorio.SelecionarPorId(paciente.Id);

            //assert
            Assert.IsNotNull(pacienteEncontrado);
            Assert.AreEqual(paciente, pacienteEncontrado);
        }

        [TestMethod]
        public void Deve_selecionar_todos_um_pacientes()
        {
            //arrange
            var p01 = new Paciente("Alberto da Silva", "321654987");
            var p02 = new Paciente("Maria do Carmo", "321654987");
            var p03 = new Paciente("Patricia Amorim", "321654987");

            var repositorio = new RepositorioPacienteEmBancoDados();
            repositorio.Inserir(p01);
            repositorio.Inserir(p02);
            repositorio.Inserir(p03);

            //action
            var pacientes = repositorio.SelecionarTodos();

            //assert

            Assert.AreEqual(3, pacientes.Count);

            Assert.AreEqual(p01, pacientes[0]);
            Assert.AreEqual(p02, pacientes[1]);
            Assert.AreEqual(p03, pacientes[2]);
        }
    }
}
