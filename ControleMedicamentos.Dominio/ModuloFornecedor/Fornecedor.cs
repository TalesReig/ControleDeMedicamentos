using System;

namespace ControleMedicamentos.Dominio.ModuloFornecedor
{
    public class Fornecedor : EntidadeBase<Fornecedor>
    {
        private int v;

        public Fornecedor()
        {
        }

        public Fornecedor(int id)
        {
            this.Id = id;
        }

        public Fornecedor(string nome, string telefone, string email, string cidade, string estado) : this()
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

        public override bool Equals(object obj)
        {
            return obj is Fornecedor fornecedor &&
                   Id == fornecedor.Id &&
                   Nome == fornecedor.Nome &&
                   Telefone == fornecedor.Telefone &&
                   Email == fornecedor.Email &&
                   Cidade == fornecedor.Cidade &&
                   Estado == fornecedor.Estado;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nome, Telefone, Email, Cidade, Estado);
        }
    }
}
