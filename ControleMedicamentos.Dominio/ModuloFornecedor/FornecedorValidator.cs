using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Dominio.ModuloFornecedor
{
    public class FornecedorValidator : AbstractValidator<Fornecedor>
    {
        public FornecedorValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("Campo 'Nome' é obrigatório.");
        }
    }
}
