using System;

namespace ControleMedicamentos.Dominio.ModuloPaciente
{
    public class Paciente : EntidadeBase<Paciente>
    {
        private int v;

        public Paciente()
        {
        }

        public Paciente(int v)
        {
            this.v = v;
        }

        public Paciente(string nome, string cartaoSUS)
        {
            Nome = nome;
            CartaoSUS = cartaoSUS;
        }

        public string Nome { get; set; }
        public string CartaoSUS { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Paciente paciente &&
                   Id == paciente.Id &&
                   Nome == paciente.Nome &&
                   CartaoSUS == paciente.CartaoSUS;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nome, CartaoSUS);
        }
    }
}
