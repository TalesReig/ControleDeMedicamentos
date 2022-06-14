using ControleMedicamentos.Dominio.ModuloPaciente;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Dominio.Tests.ModuloPaciente
{
    [TestClass]
    public class PacienteTest
    {
        [TestMethod]
        public void Nome_Do_Cliente_Deve_Ser_Obrigatorio()
        {
            var t = new Paciente();
            t.Nome = null;

            PacienteValidator validador = new PacienteValidator();

            var resultado = validador.Validate(t);

            Assert.AreEqual("Campo 'Nome' é obrigatório.", resultado.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void CartaoSUS_Do_Cliente_Deve_Ser_Obrigatorio()
        {
            var t = new Paciente();
            t.CartaoSUS = null;

            PacienteValidator validador = new PacienteValidator();

            var resultado = validador.Validate(t);

            Assert.AreEqual("Campo 'Cartão SUS' é obrigatório.", resultado.Errors[1].ErrorMessage);
        }
    }
}
