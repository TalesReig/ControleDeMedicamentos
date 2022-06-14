namespace ControleMedicamentos.Dominio.ModuloFornecedor
{
    public class Fornecedor : EntidadeBase<Fornecedor>
    {
        public Fornecedor()
        {
        }

        public Fornecedor(string nome, string telefone, string email, string cidade, string estado)
        {
            Nome = nome;
            Telefone = telefone;
            Email = email;
            Cidade = cidade;
            Estado = estado;
        }

        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }

    }
}
