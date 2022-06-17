using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloRequisicao;
using System;
using System.Collections.Generic;

namespace ControleMedicamentos.Dominio.ModuloMedicamento
{
    public class Medicamento : EntidadeBase<Medicamento>
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Lote { get; set; }
        public DateTime Validade { get; set; }
        public int QuantidadeDisponivel { get; set; }

        public List<Requisicao> Requisicoes { get; set; }

        public Fornecedor Fornecedor { get; set; }

        public int QuantidadeRequisicoes { get { return Requisicoes.Count; } }

        public Medicamento()
        {

        }
        public Medicamento(int numero) : this()
        {
            Id = numero;
        }
        public Medicamento(string nome, string descricao, string lote, DateTime validade) : this()
        {
            Nome = nome;
            Descricao = descricao;
            Lote = lote;
            Validade = validade;
            Requisicoes = new List<Requisicao>();
        }

        public Medicamento(string nome, string descricao, string lote, DateTime validade, int quantidadeDisponivel, Fornecedor fornecedor) : this()
        {
            Nome = nome;
            Descricao = descricao;
            Lote = lote;
            Validade = validade;
            QuantidadeDisponivel = quantidadeDisponivel;
            Fornecedor = fornecedor;
        }

        public void ConfigurarFornecedor(Fornecedor fornecedor)
        {
            if (fornecedor == null)
                return;

            Fornecedor = fornecedor;
        }

        public override bool Equals(object obj)
        {
            return obj is Medicamento medicamento &&
                   Id == medicamento.Id &&
                   Nome == medicamento.Nome &&
                   Descricao == medicamento.Descricao &&
                   Lote == medicamento.Lote &&
                   Validade == medicamento.Validade &&
                   QuantidadeDisponivel == medicamento.QuantidadeDisponivel &&
                   EqualityComparer<List<Requisicao>>.Default.Equals(Requisicoes, medicamento.Requisicoes) &&
                   EqualityComparer<Fornecedor>.Default.Equals(Fornecedor, medicamento.Fornecedor) &&
                   QuantidadeRequisicoes == medicamento.QuantidadeRequisicoes;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Id);
            hash.Add(Nome);
            hash.Add(Descricao);
            hash.Add(Lote);
            hash.Add(Validade);
            hash.Add(QuantidadeDisponivel);
            hash.Add(Requisicoes);
            hash.Add(Fornecedor);
            hash.Add(QuantidadeRequisicoes);
            return hash.ToHashCode();
        }
    }
}
