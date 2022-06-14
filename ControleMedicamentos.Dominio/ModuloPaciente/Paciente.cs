namespace ControleMedicamentos.Dominio.ModuloPaciente
{
    public class Paciente : EntidadeBase<Paciente>
    {
        public Paciente()
        {
        }

        public Paciente(string nome, string cartaoSUS)
        {
            Nome = nome;
            CartaoSUS = cartaoSUS;
        }

        public string Nome { get; set; }
        public string CartaoSUS { get; set; }

    }
}
