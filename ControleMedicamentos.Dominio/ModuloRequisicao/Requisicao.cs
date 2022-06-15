using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloPaciente;
using System;
using System.Collections.Generic;

namespace ControleMedicamentos.Dominio.ModuloRequisicao
{
    public class Requisicao : EntidadeBase<Requisicao>
    {

        public Requisicao()
        {

        }

        public Medicamento Medicamento { get; set; }
        public Paciente Paciente { get; set; }
        public int QtdMedicamento { get; set; }
        public DateTime Data { get; set; }
        public Funcionario Funcionario { get; set; }

        public void ConfigurarPaciente(Paciente paciente)
        {
            if (paciente == null)
                return;

            Paciente = paciente;
        }

        public void ConfigurarFuncionario(Funcionario funcionario)
        {
            if (funcionario == null)
                return;

            Funcionario = funcionario;
        }

        public void ConfigurarMedicamento(Medicamento medicamento)
        {
            if (medicamento == null)
                return;

            Medicamento = medicamento;
        }
    }
}
