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

        public Requisicao(Medicamento medicamento, Paciente paciente, int qtdMedicamento, DateTime data, Funcionario funcionario)
        {
            Medicamento = medicamento;
            Paciente = paciente;
            QtdMedicamento = qtdMedicamento;
            Data = data;
            Funcionario = funcionario;
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

        public override bool Equals(object obj)
        {
            return obj is Requisicao requisicao &&
                   Id == requisicao.Id &&
                   Medicamento == requisicao.Medicamento &&
                   Paciente == requisicao.Paciente &&
                   QtdMedicamento == requisicao.QtdMedicamento &&
                   Data == requisicao.Data &&
                   Funcionario == requisicao.Funcionario;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Medicamento, Paciente, QtdMedicamento, Data, Funcionario);
        }
    }
}
