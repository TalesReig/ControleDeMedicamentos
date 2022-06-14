using ControleMedicamentos.Dominio.ModuloFornecedor;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Dominio.ModuloFuncionario
{
    public class FuncionarioValidator : AbstractValidator<Funcionario>
    {
        public FuncionarioValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("Campo 'Nome' é obrigatório.");
        }
    }
}
